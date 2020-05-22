using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace CatchTheCovid19.Serial
{
    public class SerialCommunicator
    {
        /// <summary>
        /// Private variables
        /// </summary>
        private SerialDevice serialPort = null;
        DataWriter dataWriteObject = null;
        DataReader dataReaderObject = null;

        private CancellationTokenSource ReadCancellationTokenSource;

        public delegate void ListenComplete(string data);
        public event ListenComplete ListenCompleteEvent;

        /// <summary>
        /// 연결된 디바이스의 리스트를 수집해서 리턴해주는 메소드
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<DeviceInformation>> ListAvailablePorts()
        {
            ObservableCollection<DeviceInformation> listOfDevices = new ObservableCollection<DeviceInformation>();

            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);


                for (int i = 0; i < dis.Count; i++)
                {
                    listOfDevices.Add(dis[i]);
                }
                return listOfDevices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 컴포트를 인자로주면 Serial을 Connect하는 메소드
        /// </summary>
        /// <param name="comport"></param>
        public async void ConnectSerial(DeviceInformation comport)
        {

            try
            {
                serialPort = await SerialDevice.FromIdAsync(comport.Id);
                if (serialPort == null) return;

                // Disable the 'Connect' button 
               // comPortInput.IsEnabled = false;

                // Configure serial settings
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.BaudRate = 9600;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.None;

                // Display configured settings
                //status.Text = "Serial port configured successfully: ";
                //status.Text += serialPort.BaudRate + "-";
                //status.Text += serialPort.DataBits + "-";
                //status.Text += serialPort.Parity.ToString() + "-";
                //status.Text += serialPort.StopBits;

                // Set the RcvdText field to invoke the TextChanged callback
                // The callback launches an async Read task to wait for data
                //rcvdText.Text = "Waiting for data...";

                // Create cancellation token object to close I/O operations when closing the device
                ReadCancellationTokenSource = new CancellationTokenSource();

                // Enable 'WRITE' button to allow sending data
                //sendTextButton.IsEnabled = true;

                //Listen();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// 데이터를 보낼때 쓰는 메소드
        /// </summary>
        /// <param name="sendData"></param>
        public async void SendSerial(string sendData)
        {
            try
            {
                if (serialPort != null)
                {
                    // Create the DataWriter object and attach to OutputStream
                    dataWriteObject = new DataWriter(serialPort.OutputStream);

                    //Launch the WriteAsync task to perform the write
                    await WriteAsync(sendData);
                }
                else
                {
                    Debug.WriteLine("디바이스 선택과정에서 문제발생");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                // Cleanup once complete
                if (dataWriteObject != null)
                {
                    dataWriteObject.DetachStream();
                    dataWriteObject = null;
                }
            }
        }

        /// <summary>
        /// WriteAsync: Task that asynchronously writes data from the input text box 'sendText' to the OutputStream 
        /// </summary>
        /// <returns></returns>
        private async Task WriteAsync(string sendData)
        {
            Task<UInt32> storeAsyncTask;

            if (sendData.Length != 0)
            {
                // Load the text from the sendText input text box to the dataWriter object
                dataWriteObject.WriteString(sendData);

                // Launch an async task to complete the write operation
                storeAsyncTask = dataWriteObject.StoreAsync().AsTask();

                UInt32 bytesWritten = await storeAsyncTask;
                if (bytesWritten > 0)
                {
                    Debug.WriteLine("보내짐");
                }
                //sendText.Text = "";
            }
            else
            {
                Debug.WriteLine("공백");
                //status.Text = "Enter the text you want to write and then click on 'WRITE'";
            }
        }

        /// <summary>
        /// 시리얼을 통해 데이터를 듣는 메소드
        /// </summary>
        public async void Listen()
        {
            try
            {
                if (serialPort != null)
                {
                    dataReaderObject = new DataReader(serialPort.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token);
                    }
                }
            }
            catch (TaskCanceledException tce)
            {
                //status.Text = "Reading task was cancelled, closing device and cleaning up";
                CloseDevice();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                // Cleanup once complete
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }

        /// <summary>
        /// 버퍼에 있는 데이터를 읽어들이고 받은 데이터를 이벤트를 통해 전달하는 메소드
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;

            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            using (var childCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                // Create a task object to wait for data on the serialPort.InputStream
                loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(childCancellationTokenSource.Token);

                // Launch the task and wait
                UInt32 bytesRead = await loadAsyncTask;
                if (bytesRead > 0)
                {
                    ListenCompleteEvent?.Invoke(dataReaderObject.ReadString(bytesRead));
                }
            }
        }


        /// <summary>
        /// 쓰레드를 종료시키는 메소드
        /// </summary>
        private void CancelReadTask()
        {
            if (ReadCancellationTokenSource != null)
            {
                if (!ReadCancellationTokenSource.IsCancellationRequested)
                {
                    ReadCancellationTokenSource.Cancel();
                }
            }
        }


        /// <summary>
        /// 시리얼 커넥트를 종료하는 메소드
        /// </summary>
        public void CloseDevice()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
            }
            serialPort = null;
        }

        /// <summary>
        /// 디바이스와 연결을 종료하고 연결된 디바이스 리스트를 조회하는 메소드
        /// </summary>
        public async void CloseDeviceAndFindDevice()
        {
            try
            {
                //status.Text = "";
                CancelReadTask();
                CloseDevice();
                await ListAvailablePorts();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}

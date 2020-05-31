using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers;


namespace CatchTheCovid19.Serial
{
    public class SerialRTU
    {
        string comport = "COM1";
        int baudrate = 9600;
        string dataType = "Decimal";
        string bitCnt = "8";
        //string Rate = "1000";
        string slaveID = "1";
        string startAddr = "0";
        bool isPolling = false;
        int pollCount;
        List<double> datas = new List<double>();

        Timer.Timer timer = new Timer.Timer();
        public delegate void TeamperatureReadComplete(List<double> success);
        public event TeamperatureReadComplete TeamperatureReadCompleteEvent;
        public SerialRTU(string comport, int baudrate, string datatype, string bitCnt, string slaveID, string startAddr, string rate)
        {
            this.comport = comport;
            this.baudrate = baudrate;
            this.dataType = datatype;
            this.bitCnt = bitCnt;
            this.slaveID = slaveID;
            this.startAddr = startAddr;
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = Convert.ToDouble(rate);
        }

        private void Timer_Elapsed(object sender, Timer.ElapsedEventArgs e)
        {
            PollFunction();
        }

        #region Start and Stop Procedures

        modbus mb = new modbus();

        //DispatcherTimer timer = new DispatcherTimer();




        public void StartPoll()
        {
            pollCount = 0;
            //Open COM port using provided settings:
            if (mb.Open(comport.ToString(), Convert.ToInt32(baudrate.ToString()),
                8, Parity.None, StopBits.One))
            {
                //Disable double starts:
                //btnStart.Enabled = false;
                dataType = dataType.ToString();

                //Set polling flag:
                isPolling = true;
                PollFunction();
                timer.Start();
                //Start timer using provided values:
                // timer.Interval = TimeSpan.FromMilliseconds(Convert.ToDouble(Rate));
                // timer.Start();
                //timer.AutoReset = true;
                //if (Rate != "")
                //    timer.Interval = Convert.ToDouble(Rate);
                //else
                //    timer.Interval = 1000;
                //timer.Start();
            }

            Debug.WriteLine(mb.modbusStatus);

        }
        public void StopPoll()
        {
            //Stop timer and close COM port:
            isPolling = false;
            pollCount = 0;
            datas.Clear();
            timer.Stop();
            mb.Close();

            //btnStart.Enabled = true;

            Debug.WriteLine(mb.modbusStatus);

        }

        #endregion

        #region Poll Function
        private void PollFunction()
        {
            //Update GUI:
            //DoGUIClear();
            pollCount++;
            Debug.WriteLine("Poll count: " + pollCount.ToString());

            //Create array to accept read values:
            short[] values = new short[Convert.ToInt32(bitCnt)];
            ushort pollStart;
            ushort pollLength;

            if (startAddr != "")
                pollStart = Convert.ToUInt16(startAddr);
            else
                pollStart = 0;
            if (bitCnt != "")
                pollLength = Convert.ToUInt16(bitCnt);
            else
                pollLength = 20;

            //Read registers and display data in desired format:
            try
            {
                while (!mb.SendFc3(Convert.ToByte(slaveID), pollStart, pollLength, ref values)) ;
            }
            catch (Exception err)
            {
                Debug.WriteLine("Error in modbus read: " + err.Message);
                //DoGUIStatus();
            }

            //string itemString;

            switch (dataType)
            {
                case "Decimal":
                    //for (int i = 0; i < pollLength; i++)
                    //{

                    //}//60, 136
                    short data1 = (short)(values[2] >> 8);
                    short data2 = ((short)(values[2] & 0x00FF));
                    int res = data1 * 256 + data2;
                    double res2 = ((double)((res * 0.02) - 273.15));
                    //values[5], value[6]
                    //itemString = "[" + Convert.ToString(pollStart + i + 40001) + "] , MB[" +
                    //    Convert.ToString(pollStart + i) + "] = " + values[i].ToString();
                    datas.Add(res2);
                    if (datas.Count == 5)
                    {
                        TeamperatureReadCompleteEvent?.Invoke(datas);
                    }
                    break;
            }
        }
        #endregion
    }
    public class modbus
    {
        private SerialPort sp = new SerialPort();
        public string modbusStatus;

        #region Constructor / Deconstructor
        public modbus()
        {
        }
        ~modbus()
        {
        }
        #endregion

        #region Open / Close Procedures
        public bool Open(string portName, int baudRate, int databits, Parity parity, StopBits stopBits)
        {
            //Ensure port isn't already opened:
            if (!sp.IsOpen)
            {
                //Assign desired settings to the serial port:
                sp.PortName = portName;
                sp.BaudRate = baudRate;
                sp.DataBits = databits;
                sp.Parity = parity;
                sp.StopBits = stopBits;
                //These timeouts are default and cannot be editted through the class at this point:
                sp.ReadTimeout = 1000;
                sp.WriteTimeout = 1000;

                try
                {
                    sp.Open();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error opening " + portName + ": " + err.Message;
                    return false;
                }
                modbusStatus = portName + " opened successfully";
                return true;
            }
            else
            {
                modbusStatus = portName + " already opened";
                return false;
            }
        }
        public bool Close()
        {
            //Ensure port is opened before attempting to close:
            if (sp.IsOpen)
            {
                try
                {
                    sp.Close();
                }
                catch (Exception err)
                {
                    modbusStatus = "Error closing " + sp.PortName + ": " + err.Message;
                    return false;
                }
                modbusStatus = sp.PortName + " closed successfully";
                return true;
            }
            else
            {
                modbusStatus = sp.PortName + " is not open";
                return false;
            }
        }
        #endregion

        #region CRC Computation
        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }
        #endregion

        #region Build Message
        private void BuildMessage(byte address, byte type, ushort start, ushort registers, ref byte[] message)
        {
            //Array to receive CRC bytes:
            byte[] CRC = new byte[2];

            message[0] = address;
            message[1] = type;
            message[2] = (byte)(start >> 8);
            message[3] = (byte)start;
            message[4] = (byte)(registers >> 8);
            message[5] = (byte)registers;

            GetCRC(message, ref CRC);
            message[message.Length - 2] = CRC[0];
            message[message.Length - 1] = CRC[1];
        }
        #endregion

        #region Check Response
        private bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            GetCRC(response, ref CRC);
            if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                return true;
            else
                return false;
        }
        #endregion

        #region Get Response
        private void GetResponse(ref byte[] response)
        {
            //There is a bug in .Net 2.0 DataReceived Event that prevents people from using this
            //event as an interrupt to handle data (it doesn't fire all of the time).  Therefore
            //we have to use the ReadByte command for a fixed length as it's been shown to be reliable.
            for (int i = 0; i < response.Length; i++)
            {
                response[i] = (byte)(sp.ReadByte());
            }
        }
        #endregion

        #region Function 16 - Write Multiple Registers
        public bool SendFc16(byte address, ushort start, ushort registers, short[] values)
        {
            //Ensure port is open:
            if (sp.IsOpen)
            {
                //Clear in/out buffers:
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
                //Message is 1 addr + 1 fcn + 2 start + 2 reg + 1 count + 2 * reg vals + 2 CRC
                byte[] message = new byte[9 + 2 * registers];
                //Function 16 response is fixed at 8 bytes
                byte[] response = new byte[8];

                //Add bytecount to message:
                message[6] = (byte)(registers * 2);
                //Put write values into message prior to sending:
                for (int i = 0; i < registers; i++)
                {
                    message[7 + 2 * i] = (byte)(values[i] >> 8);
                    message[8 + 2 * i] = (byte)(values[i]);
                }
                //Build outgoing message:
                BuildMessage(address, (byte)16, start, registers, ref message);

                //Send Modbus message to Serial Port:
                try
                {
                    sp.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in write event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    modbusStatus = "Write successful";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }
        }
        #endregion

        #region Function 3 - Read Registers
        public bool SendFc3(byte address, ushort start, ushort registers, ref short[] values)
        {
            //Ensure port is open:
            if (sp.IsOpen)
            {
                //Clear in/out buffers:
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
                //Function 3 request is always 8 bytes:
                byte[] message = new byte[8];
                //Function 3 response buffer:
                byte[] response = new byte[5 + 2 * registers];
                //Build outgoing modbus message:
                BuildMessage(address, (byte)3, start, registers, ref message);
                //Send modbus message to Serial Port:
                try
                {
                    sp.Write(message, 0, message.Length);
                    GetResponse(ref response);
                }
                catch (Exception err)
                {
                    modbusStatus = "Error in read event: " + err.Message;
                    return false;
                }
                //Evaluate message:
                if (CheckResponse(response))
                {
                    //Return requested register values:
                    for (int i = 0; i < (response.Length - 5) / 2; i++)
                    {
                        values[i] = response[2 * i + 3];
                        values[i] <<= 8;
                        values[i] += response[2 * i + 4];
                    }
                    modbusStatus = "Read successful";
                    return true;
                }
                else
                {
                    modbusStatus = "CRC error";
                    return false;
                }
            }
            else
            {
                modbusStatus = "Serial port not open";
                return false;
            }

        }
        #endregion

    }
}

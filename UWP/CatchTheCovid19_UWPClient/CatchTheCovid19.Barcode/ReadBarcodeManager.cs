using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CatchTheCovid19.Serial;

namespace CatchTheCovid19.Barcode
{
    public class ReadBarcodeManager
    {
        SerialCommunicator serialCommunicator = new SerialCommunicator("COM9");

        public delegate void ReadComplete(string data);
        public event ReadComplete ReadCompleteEvent;



        public ReadBarcodeManager()
        {

        }
        public async void ConnectBarcodeRaspi()
        {
            //await serialCommunicator.ShowComList();
            //정확한 디바이스 이름 파악되면 바꿀것
            if ((await serialCommunicator.FindDevicebyName("USB")) == true)
            {
                Debug.WriteLine("포트찾음");
                if((await serialCommunicator.ConnectSerial(115200)) == true)
                {
                    Debug.WriteLine("성공적으로 연결됨");
                    serialCommunicator.BUFFSIZE = 10;
                    serialCommunicator.ListenCompleteEvent += SerialCommunicator_ListenCompleteEvent;
                    GetSerialData();

                }
                else
                {
                    Debug.WriteLine("연결실패");
                }
            }
            else
            {
                Debug.WriteLine("포트검색불가");
            }
        }

        private void SerialCommunicator_ListenCompleteEvent(string data)
        {
            Debug.WriteLine(data + "받음");

            ReadCompleteEvent?.Invoke(data);
        }

        public void GetSerialData()
        {
            serialCommunicator.Listen();
        }

        public async Task SendSerialData(string data)
        {
            await serialCommunicator.SendSerial(data);
        }
    }
}

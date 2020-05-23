using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CatchTheCovid19.Serial;

namespace CatchTheCovid19.Barcode
{
    public class ReadBarcodeManager
    {
        SerialCommunicator serialCommunicator = new SerialCommunicator("COM6");

        public delegate void ReadComplete(string data);
        public event ReadComplete ReadCompleteEvent;

        public ReadBarcodeManager()
        {
            ConnectBarcodeRaspi();
        }
        public async void ConnectBarcodeRaspi()
        {
            //정확한 디바이스 이름 파악되면 바꿀것
            if((await serialCommunicator.FindPortsDevice()) == true)
            {
                Debug.WriteLine("포트찾음");
                if((await serialCommunicator.ConnectSerial(9600)) == true)
                {
                    Debug.WriteLine("성공적으로 연결됨");
                    serialCommunicator.ListenCompleteEvent += SerialCommunicator_ListenCompleteEvent;
                    await GetSerialData();

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

        public async Task GetSerialData()
        {
            await serialCommunicator.Listen();
        }

        public async Task SendSerialData(string data)
        {
            await serialCommunicator.SendSerial(data);
        }
    }
}

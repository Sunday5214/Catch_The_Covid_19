using System;
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
            ConnectBarcodeRasp();
        }
        public async void ConnectBarcodeRasp()
        {
            //정확한 디바이스 이름 파악되면 바꿀것
           await serialCommunicator.FindPortsDevice();

        }
    }
}

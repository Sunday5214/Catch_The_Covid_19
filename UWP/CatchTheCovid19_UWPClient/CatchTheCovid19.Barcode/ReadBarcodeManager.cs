using System;
using CatchTheCovid19.Serial;

namespace CatchTheCovid19.Barcode
{
    public class ReadBarcodeManager
    {
        SerialCommunicator serialCommunicator = new SerialCommunicator();

        public delegate void ReadComplete(string data);
        public event ReadComplete ReadCompleteEvent;

        public ReadBarcodeManager()
        {
            ConnectBarcodeRasp();
        }
        public async void ConnectBarcodeRasp()
        {
           var deviceList =  await serialCommunicator.ListAvailablePorts();
           //deviceList.
        }
    }
}

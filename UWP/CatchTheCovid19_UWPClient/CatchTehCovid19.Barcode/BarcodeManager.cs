using System;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.PointOfService;
using Windows.Media.Capture;

namespace CatchTheCovid19.Barcode
{
    public class BarcodeManager
    {
        private BarcodeScanner scanner { get; set; }
        private ClaimedBarcodeScanner claimedScanner { get; set; }
        private MediaCapture captureManager { get; set; }

        public DeviceInformationCollection deviceInformation = null;
        public BarcodeManager()
        {

            InitBarcode();
        }

        private async Task InitBarcode()
        {
            scanner = null;
            // Acquire the barcode scanner.
            string aqs = BarcodeScanner.GetDeviceSelector();
            deviceInformation = await DeviceInformation.FindAllAsync(aqs);
            //scanner = await DeviceHelpers.GetFirstBarcodeScannerAsync();

        }
    }
}

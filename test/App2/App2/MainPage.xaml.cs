using CatchTheCovid19.I2C;
using CatchTheCovid19.Serial;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace App2
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // SerialRTU serialRTU = new SerialRTU("UART0", 19200, "Decimal", "8", "1", "0", "1000");
        I2CManager I2CManager;

        public MainPage()
        {
            this.InitializeComponent();

            //serialRTU.TeamperatureReadCompleteEvent += SerialRTU_TeamperatureReadCompleteEvent;
        }
        public async void SetI2C()
        {
            string aqs = I2cDevice.GetDeviceSelector();
            var dis = await DeviceInformation.FindAllAsync(aqs).AsTask();
            var settings = new I2cConnectionSettings(0x04);
            var device = await I2cDevice.FromIdAsync(dis[0].Id, settings);
            I2CManager = new I2CManager(LoadData, ref device);
           // AirInfoService service = new AirInfoService(LoadData, ref device);
            await I2CManager.StartI2C();


        }
        private async void LoadData(object sender, string data)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                tbData.Text = data;
            });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetI2C();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
         //   serialRTU.GetSerialPorts();
        }
    }
}

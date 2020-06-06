using CatchTheCovid19.I2C;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            VL53L0XSensor sensor = new VL53L0XSensor();
            await sensor.InitializeAsync();

            while (true)
            {
                var dis = sensor.ReadDistance();
                Debug.WriteLine("distance : " + dis + " mm");

                //var res = sensor.Read();
                //Debug.WriteLine("ambient count : " + res.Ambient);
                //Debug.WriteLine("signal count : " + res.Signal);
                //Debug.WriteLine("distance : " + res.Distance + " mm");
                //Debug.WriteLine("status : " + res.Status);

                await Task.Delay(2000);
            }
        }
    }
}

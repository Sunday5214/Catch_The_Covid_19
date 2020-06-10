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

        private int _distanceData;
        public int DistanceData
        {
            get => _distanceData;
            set => _distanceData = value;
        }

        private int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private int constrain(int amt, int low, int high)
        {
            return amt < low ? low : (amt > high ? high : amt);
        }
        private async Task Start()
        {
            VL53L0XSensor sensor = new VL53L0XSensor();
            await sensor.InitializeAsync();

            bool IsSixCm = false;
            int data = -1;

            while (!IsSixCm)
            {
                data = sensor.ReadDistance();
                data = map(constrain(data, 10, 100), 100, 10, 10, 100);
                Debug.WriteLine(data);
                pbdata.Value = data;

                
                if (pbdata.Value >= 80)
                {
                    IsSixCm = true;
                    break;    
                    //GetTemperatureData();
                }
                await Task.Delay(200);
            }
            Debug.WriteLine("만땅");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Start();
        }
    }
}

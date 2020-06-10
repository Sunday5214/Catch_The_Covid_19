using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace CatchTheCovid19_UWPClient.View
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class CheckTemperatureView : Page
    {
        public delegate void ChangeScreen();
        public event ChangeScreen ChangeScreenEvent;
        public CheckTemperatureView()
        {
            this.InitializeComponent();
            Loaded += CheckTemperatureView_Loaded;
        }

        private void CheckTemperatureView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = App.checkTemperatureViewModel;
            App.checkTemperatureViewModel.TeamperatureReadCompleteEvent += CheckTemperatureViewModel_TeamperatureReadCompleteEvent;
        }

        private async void CheckTemperatureViewModel_TeamperatureReadCompleteEvent(bool success)
        {
            if (success)
            {
                tbDesc.Visibility = Visibility.Collapsed;
                tbName.Visibility = Visibility.Visible;
                tbTemp.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                ChangeScreenEvent?.Invoke();
            }
            else
            {
                tbDesc.Text = "네트워크 문제가 발생했습니다.\n네트워크 상태를 체크후 마지막 사람부터 다시 측정해주세요";
            }
        }
        public void BarCodeReadOff()
        {
            GpioController gpio = GpioController.GetDefault();
            if (gpio == null) return;
            using (GpioPin pin = gpio.OpenPin(4))
            {
                pin.Write(GpioPinValue.High);
                pin.SetDriveMode(GpioPinDriveMode.Output);
            }
        }
        public async void Init()
        {
            //BarCodeReadOff();
            
            App.checkTemperatureViewModel.Member = null;
            App.checkTemperatureViewModel.Temperature = 0;
            await App.checkTemperatureViewModel.StartI2C();
            tbDesc.Visibility = Visibility.Visible;
            tbName.Visibility = Visibility.Collapsed;
            tbTemp.Visibility = Visibility.Collapsed;

        }
    }
}

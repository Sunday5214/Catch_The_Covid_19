using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CatchTheCovid19.RestClient.Option;
using CatchTheCovid10.InitData;
using Windows.Devices.Gpio;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace CatchTheCovid19_UWPClient.View
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class SelectTimeView : Page
    {
        public delegate void ChangeScreen();
        public event ChangeScreen ChangeScreenEvent;
        public SelectTimeView()
        {
            this.InitializeComponent();
            Loaded += SelectTimeView_Loaded;
        }

        private async void SelectTimeView_Loaded(object sender, RoutedEventArgs e)
        {
            BarCodeReadOff();
            await App.infoManager.GetInfoData();

            //DataContext = App.infoManager;
            foreach(var item in InfoManager.Infos.Codes)
            {
                lvTime.Items.Add(item);
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
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            switch ((string)list.SelectedItem)
            {
                case "아침":
                    NetworkOptions.nowTime = TimeEnum.BREAKFAST;
                    break;
                case "점심":
                    NetworkOptions.nowTime = TimeEnum.BREAKFAST;
                    break;
                case "저녁":
                    NetworkOptions.nowTime = TimeEnum.BREAKFAST;
                    break;
                case "입실":
                    NetworkOptions.nowTime = TimeEnum.BREAKFAST;
                    break;
                case "퇴실":
                    NetworkOptions.nowTime = TimeEnum.BREAKFAST;
                    break;
            }
            ChangeScreenEvent?.Invoke();
        }
    }
}

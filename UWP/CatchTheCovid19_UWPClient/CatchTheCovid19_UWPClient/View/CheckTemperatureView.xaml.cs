using CatchTheCovid19_UWPClient.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
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
   
        MediaPlayer mediaPlayerTemperature = new MediaPlayer();

        public CheckTemperatureView()
        {
            this.InitializeComponent();
            Loaded += CheckTemperatureView_Loaded;
        }

        private void CheckTemperatureView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = App.checkTemperatureViewModel;
            mediaPlayerTemperature.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Voice/측정되었습니다.mp3"));
            App.checkTemperatureViewModel.TeamperatureReadCompleteEvent += CheckTemperatureViewModel_TeamperatureReadCompleteEvent;
        }
       
        private async void CheckTemperatureViewModel_TeamperatureReadCompleteEvent(bool success)
        {
            if (success)
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    tbDesc.Visibility = Visibility.Collapsed;
                  //  pbdata.Visibility = Visibility.Collapsed;
                  //  tbMark.Visibility = Visibility.Collapsed;
                    tbName.Visibility = Visibility.Visible;
                    tbTemp.Visibility = Visibility.Visible;

                });
                TemperaturePlayMedia();
                if (App.checkTemperatureViewModel.Temperature > 37.5)
                {
                    gdGreen.Visibility = Visibility.Collapsed;
                    gdBlue.Visibility = Visibility.Collapsed;
                    gdRed.Visibility = Visibility.Visible; 
                    //PlayMedia("Red");
                }
                else if(App.checkTemperatureViewModel.Temperature < 35)
                {
                    gdRed.Visibility = Visibility.Collapsed;
                    gdGreen.Visibility = Visibility.Collapsed;
                    gdBlue.Visibility = Visibility.Visible;

                }
                else 
                {
                    gdBlue.Visibility = Visibility.Collapsed;
                    gdRed.Visibility = Visibility.Collapsed;
                    gdGreen.Visibility = Visibility.Visible;
                   // PlayMedia("Green");
                }
                await Task.Delay(2000);
                ChangeScreenEvent?.Invoke();
            }
            else
            {
                tbDesc.Text = "네트워크 문제가 발생했습니다.\n네트워크 상태를 체크후 마지막 사람부터 다시 측정해주세요";
            }
        }

        private void TemperaturePlayMedia()
        {

            mediaPlayerTemperature.Play();
        }
        public void Init()
        {
            //BarCodeReadOff();
            App.checkTemperatureViewModel.Member = null;
            App.checkTemperatureViewModel.Temperature = 0;
            //App.checkTemperatureViewModel.GetDistanceData();
            tbDesc.Visibility = Visibility.Visible;
            //pbdata.Visibility = Visibility.Visible;
            //tbMark.Visibility = Visibility.Visible;
            tbName.Visibility = Visibility.Collapsed;
            tbTemp.Visibility = Visibility.Collapsed;


        }
    }
}

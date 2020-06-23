
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CatchTheCovid19.RestClient.Option;
using CatchTheCovid10.InitData;


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

        public delegate void InvalideIP();
        public event InvalideIP InvalideIPEvent;

        public delegate void ChangeScreenSetting();
        public event ChangeScreenSetting ChangeScreenSettingEvent;
        public SelectTimeView()
        {
            this.InitializeComponent();
            Loaded += SelectTimeView_Loaded;
        }

        private void SelectTimeView_Loaded(object sender, RoutedEventArgs e)
        {
            // BarCodeReadOff();
            LoadSelectTimeData();
            //DataContext = App.infoManager;

        }

        public async void LoadSelectTimeData()
        {
            await App.infoManager.GetInfoData();
            if (InfoManager.Infos == null)
            {
                InvalideIPEvent?.Invoke();
            }
            else
            {
                foreach (var item in InfoManager.Infos.Codes)
                {
                    lvTime.Items.Add(item);
                }
            }
        }
        //public void BarCodeReadOff()
        //{
        //    GpioController gpio = GpioController.GetDefault();
        //    if (gpio == null) return;
        //    GpioPin pin = gpio.OpenPin(4);
        //    pin.Write(GpioPinValue.High);
        //    pin.SetDriveMode(GpioPinDriveMode.Output);
        //}
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            switch ((string)list.SelectedItem)
            {
                case "아침":
                    NetworkOptions.nowTime = TimeEnum.BREAKFAST;
                    break;
                case "점심":
                    NetworkOptions.nowTime = TimeEnum.LUNCH;
                    break;
                case "저녁":
                    NetworkOptions.nowTime = TimeEnum.DINNDER;
                    break;
                case "입실":
                    NetworkOptions.nowTime = TimeEnum.IN;
                    break;
                case "퇴실":
                    NetworkOptions.nowTime = TimeEnum.OUT;
                    break;
            }
            ChangeScreenEvent?.Invoke();
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            ChangeScreenSettingEvent?.Invoke();
        }
    }
}

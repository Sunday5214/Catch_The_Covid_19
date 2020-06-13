using CatchTheCovid10.InitData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input.Preview.Injection;
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
    public sealed partial class CheckMemberCardView : Page
    {
        public delegate void ChangeScreen();
        public event ChangeScreen ChangeScreenEvent;

        bool IsReadComplete = false;
        //TextBox tbxBarInput = new TextBox();
        public CheckMemberCardView()
        {
            this.InitializeComponent();
            Loaded += CheckMemberCard_Loaded;
        }

        private void CheckMemberCard_Loaded(object sender, RoutedEventArgs e)
        {
            TabInput();
            DataContext = App.checkMemberCardViewModel;
            App.checkMemberCardViewModel.BarcodeReadCompleteEvent += CheckMemberCardViewModel_BarcodeReadCompleteEvent;
            //App.checkMemberCardViewModel.StartReadCard();
        }

        private async void CheckMemberCardViewModel_BarcodeReadCompleteEvent(Member member)
        {
            if (member != null)
            {
                await ShowData(member);
            }
            else
            {
                tbDesc.Text = "등록되지 않은 멤버입니다.";
            }
        }

        private async Task ShowData(Member member)
        {

            tbDesc.Visibility = Visibility.Collapsed;
            tbName.Visibility = Visibility.Visible;
            tbClassRoom.Visibility = Visibility.Visible;
            tbIsStudent.Visibility = Visibility.Visible;
         
            
            await Task.Delay(3000);
            ChangeScreenEvent?.Invoke();
            App.checkTemperatureViewModel.SetMemberData(member);
            //App.checkTemperatureViewModel.GetTemperatureData();
            

        }

        public async Task Init()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                IsReadComplete = false;
                App.checkMemberCardViewModel.CheckMemberCard = null;
                tbDesc.Visibility = Visibility.Visible;

                tbName.Visibility = Visibility.Collapsed;
                tbClassRoom.Visibility = Visibility.Collapsed;
                tbIsStudent.Visibility = Visibility.Collapsed;
                //BarCodeReadOn();
                TabInput();
                //MakeInputTbx();
                //tbxBarInput.IsFocusEngaged = true;
            });

        }
        private void TabInput()
        {
            InputInjector inputInjector = InputInjector.TryCreate();
            var tab = new InjectedInputKeyboardInfo();
            tab.VirtualKey = (ushort)(VirtualKey.Tab);
            tab.KeyOptions = InjectedInputKeyOptions.None;
            inputInjector.InjectKeyboardInput(new[] { tab });
        }

        public async Task BarCodeReadOn()
        {
            GpioController gpio = GpioController.GetDefault();
            if (gpio == null) return;
            using (GpioPin pin = gpio.OpenPin(4))
            {
                while (true)
                {
                    await Task.Run(() =>
                    {
                        pin.Write(GpioPinValue.High);
                        pin.SetDriveMode(GpioPinDriveMode.Output);
                    });
                    await Task.Run(() =>
                    {
                        pin.Write(GpioPinValue.Low);
                        pin.SetDriveMode(GpioPinDriveMode.Output);
                        Task.Delay(2000);
                    });

                    if (IsReadComplete)
                    {
                        break;
                    }

               
                }
                

            }
            BarCodeReadOff();

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
        private async void tbxBarInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                IsReadComplete = true;
                await App.checkMemberCardViewModel.SearchMember(tbxBarInput.Text);
                tbxBarInput.Text = "";

                //tbxBarInput.Focus(FocusState.Programmatic);
                //await ShowData(App.checkMemberCardViewModel.CheckMemberCard);
            }
        }
    }
}

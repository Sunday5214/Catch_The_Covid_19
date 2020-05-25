using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class CheckMemberCardView : Page
    {
        public delegate void ChangeScreen();
        public event ChangeScreen ChangeScreenEvent;

        public CheckMemberCardView()
        {
            this.InitializeComponent();
            Loaded += CheckMemberCard_Loaded;
        }

        private void CheckMemberCard_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = App.checkMemberCardViewModel;
            App.checkMemberCardViewModel.BarcodeReadCompleteEvent += CheckMemberCardViewModel_BarcodeReadCompleteEvent;
            //App.checkMemberCardViewModel.StartReadCard();
        }

        private async void CheckMemberCardViewModel_BarcodeReadCompleteEvent(Model.CheckMemberCard member)
        {
            if (member != null) 
            {
                await ShowData(member);
            }
            else
            {
                tbDesc.Text = "네트워크 오류가 발생했습니다. \n마지막 사람부터 다시 측정해주세요";
            }


        }

        private async Task ShowData(Model.CheckMemberCard member)
        {
            tbDesc.Visibility = Visibility.Collapsed;
            tbName.Visibility = Visibility.Visible;
            tbClassRoom.Visibility = Visibility.Visible;
            tbIsStudent.Visibility = Visibility.Visible;
            
            await Task.Delay(3000);
            App.checkTemperatureViewModel.SetMemberData(member);
            App.checkTemperatureViewModel.StartReadTemperature();
            ChangeScreenEvent?.Invoke();

        }

        public void Init()
        {
            App.checkMemberCardViewModel.CheckMemberCard = null;
            tbDesc.Visibility = Visibility.Visible;
            tbName.Visibility = Visibility.Collapsed;
            tbClassRoom.Visibility = Visibility.Collapsed;
            tbIsStudent.Visibility = Visibility.Collapsed;
        }
    }
}

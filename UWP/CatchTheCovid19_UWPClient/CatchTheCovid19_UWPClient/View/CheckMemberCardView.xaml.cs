using CatchTheCovid10.Member;
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
            tbBarInput.Focus(FocusState.Programmatic);
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
            App.checkTemperatureViewModel.GetTemperatureData();
            

        }

        public void Init()
        {
            tbBarInput.Focus(FocusState.Programmatic);
            App.checkMemberCardViewModel.CheckMemberCard = null;
            tbDesc.Visibility = Visibility.Visible;
            tbName.Visibility = Visibility.Collapsed;
            tbClassRoom.Visibility = Visibility.Collapsed;
            tbIsStudent.Visibility = Visibility.Collapsed;
        }

        private async void tbBarInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                await App.checkMemberCardViewModel.SearchMember(tbBarInput.Text);
                
                //await ShowData(App.checkMemberCardViewModel.CheckMemberCard);
            }
        }
    }
}

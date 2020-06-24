using CatchTheCovid19.RestClient.Option;
using CatchTheCovid19_UWPClient.Common;
using CatchTheCovid19_UWPClient.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace CatchTheCovid19_UWPClient
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        CheckMemberCardView ctrlCheckMember = new CheckMemberCardView();
        CheckTemperatureView ctrlTemperature = new CheckTemperatureView();
        SelectTimeView ctrlSelectTime = new SelectTimeView();
        SettingView ctrlSetting = new SettingView();
        bool IsSettingAgain = false;
        public MainPage()
        {
            InitializeComponent();
            
            Loaded += MainPage_Loaded;
        }

        private void CtrlSelectTime_InvalideIPEvent()
        {

            ShowMessage();
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
           
            //처음 시작일 경우
            if (!IsSettingAgain)
            {
                pivotMain.Items.Add(new PivotItem { Content = ctrlSetting });
                App.SettingViewModel.SettingCompleteEvent += SettingViewModel_SettingCompleteEvent;
            }
            
            string ip = App.SettingViewModel.GetSetting("ServerAddress");
            VoiceOption.VoiceName = App.SettingViewModel.GetSetting("SelectedVoice");

          //  string notSavedData = App.SettingViewModel.GetSetting("ReqSaveData");
          //  await App.checkTemperatureViewModel.AddDataNotSaved(notSavedData);

            if (ip == "" || ip == null || ip == "http://")
            {
                
                ShowMessage();

                return;
            }
            else
            {
                if (IsSettingAgain)
                {
                    pivotMain.SelectedItem = pivotMain.Items[1];
                    return;
                }
                NetworkOptions.serverUrl = ip;

                App.memberManager.GetMemberData();

                ctrlSelectTime.InvalideIPEvent += CtrlSelectTime_InvalideIPEvent;
                ctrlSelectTime.ChangeScreenEvent += CtrlSelectTime_ChangeScreenEvent;
                ctrlCheckMember.ChangeScreenEvent += CtrlCheckMember_ChangeScreenEvent;
                ctrlTemperature.ChangeScreenEvent += CtrlTemperature_ChangeScreenEvent;
                ctrlSelectTime.ChangeScreenSettingEvent += CtrlSelectTime_ChangeScreenSettingEvent;

                pivotMain.Items.Add(new PivotItem { Content = ctrlSelectTime });
                pivotMain.Items.Add(new PivotItem { Content = ctrlCheckMember });
                pivotMain.Items.Add(new PivotItem { Content = ctrlTemperature });
                pivotMain.SelectedItem = pivotMain.Items[1];
                IsSettingAgain = true;
            }           
        }

        private void SettingViewModel_SettingCompleteEvent(bool success)
        {
            MainPage_Loaded(null, null);
            ctrlSelectTime.LoadSelectTimeData();
        }



        private void CtrlSelectTime_ChangeScreenSettingEvent()
        {
            pivotMain.SelectedItem = pivotMain.Items[0];
        }

        private async void ShowMessage()
        {
            var contentDialog = new ContentDialog
            {
                Content = "지정된 세팅이 없습니다. 세팅페이지로 이동합니다.",
                Title = "저장된 세팅 없음!",
                CloseButtonText = "OK"
            };
            await contentDialog.ShowAsync();

            pivotMain.SelectedItem = pivotMain.Items[0];
        }

        private async void CtrlSelectTime_ChangeScreenEvent()
        {
            await ctrlCheckMember.Init();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                pivotMain.SelectedItem = pivotMain.Items[2];
            });
           // await ctrlCheckMember.BarCodeReadOn();
            
        }

        private async void CtrlTemperature_ChangeScreenEvent() 
        {
            await ctrlCheckMember.Init();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                pivotMain.SelectedItem = pivotMain.Items[2];
            });

           // await ctrlCheckMember.BarCodeReadOn();
            
        }

        private void CtrlCheckMember_ChangeScreenEvent()
        {
            ctrlTemperature.Init();

            pivotMain.SelectedItem = pivotMain.Items[3];
        }
    }
}

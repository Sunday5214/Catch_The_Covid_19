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

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace CatchTheCovid19_UWPClient
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.memberManager.GetMemberData();
            ctrlCheckMember.ChangeScreenEvent += CtrlCheckMember_ChangeScreenEvent;
            ctrlTemperature.ChangeScreenEvent += CtrlTemperature_ChangeScreenEvent;
        }

        private void CtrlTemperature_ChangeScreenEvent()
        {
            ctrlCheckMember.Init();
            ctrlCheckMember.Visibility = Visibility.Visible;
     
            ctrlTemperature.Visibility = Visibility.Collapsed;
        }

        private void CtrlCheckMember_ChangeScreenEvent()
        {
            ctrlCheckMember.Init();
            ctrlCheckMember.Visibility = Visibility.Collapsed;

            ctrlTemperature.Visibility = Visibility.Visible;
        }
    }
}

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
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            NetworkOptions.nowTime = TimeEnum.UP;
            ChangeScreenEvent?.Invoke();
        }

        private void btnBreakFast_Click(object sender, RoutedEventArgs e)
        {
            NetworkOptions.nowTime = TimeEnum.BREAKFAST;
            ChangeScreenEvent?.Invoke();
        }

        private void btnLunch_Click(object sender, RoutedEventArgs e)
        {
            NetworkOptions.nowTime = TimeEnum.LUNCH;
            ChangeScreenEvent?.Invoke();
        }

        private void btnDinner_Click(object sender, RoutedEventArgs e)
        {
            NetworkOptions.nowTime = TimeEnum.DINNDER;
            ChangeScreenEvent?.Invoke();
        }
    }
}

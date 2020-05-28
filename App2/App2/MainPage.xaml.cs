using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace App2
{

    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ushort vid = 44176;      // 0x046D
        ushort pid = 12290;     // 0xC52B

        ushort usageId = 1;         // 0x0001
        ushort usagePage = 6;   // 0xFF00

        HidDevice myHidDevice = null;
        DeviceInformationCollection myDevices = null;
        public MainPage()
        {
            this.InitializeComponent();
            Init();

        } 

        public async void Init() 
        {
            string selector = HidDevice.GetDeviceSelector(usagePage, usageId, vid, pid);
            await GetDevice(selector);
            if (myDevices.Any())
            {
               await GetHidDevice();
            }

            if (myHidDevice != null)
            {
                myHidDevice.InputReportReceived += (sender, args) =>
                {
                    // read report
                };
            }
        }
        public async Task GetHidDevice()
        {
            myHidDevice = await HidDevice.FromIdAsync(myDevices[1].Id, FileAccessMode.ReadWrite);
        }
        public async Task GetDevice(string selector)
        {
            myDevices = await DeviceInformation.FindAllAsync(selector);
        }
        // get selector string for the device (connection string)
        
    }
}

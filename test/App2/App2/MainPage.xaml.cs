using CatchTheCovid19.I2C;
using CatchTheCovid19.Serial;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;
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

namespace App2
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera;
        int isCameraRunning = 0;
        // SerialRTU serialRTU = new SerialRTU("UART0", 19200, "Decimal", "8", "1", "0", "1000");
        //I2CManager I2CManager;
        //SerialCommunicator serial = new SerialCommunicator();
        public MainPage()
        {
            this.InitializeComponent();
            // GG();
            //serial.ListenCompleteEvent += Serial_ListenCompleteEvent;
            //serial.BUFFSIZE = 5;
            //Connect();
            //serialRTU.TeamperatureReadCompleteEvent += SerialRTU_TeamperatureReadCompleteEvent;
        }
        private void CaptureCamera()
        {

            camera = new Thread(new ThreadStart(CaptureCameraCallback));
            camera.Start();
        }

        private void CaptureCameraCallback()
        {
            frame = new Mat();
            capture = new VideoCapture();
            capture.Open(1);

            while (isCameraRunning == 1)
            {
                capture.Read(frame);
                if (!frame.Empty())
                {
                    image = BitmapConverter.ToBitmap(frame);
                    //pictureBox1.Image = image;
                }
                image = null;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (btnData.Content.Equals("Start"))
            {
                CaptureCamera();
                btnData.Content = "Stop";
                isCameraRunning = 1;
            }
            else
            {
                if (capture.IsOpened())
                {
                    capture.Release();
                }

                btnData.Content = "Start";
                isCameraRunning = 0;
            }
        }
    }
}

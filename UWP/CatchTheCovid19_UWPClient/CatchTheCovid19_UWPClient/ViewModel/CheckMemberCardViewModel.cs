using CatchTheCovid10.Member;
using CatchTheCovid19.Barcode;
using CatchTheCovid19.RestManager;
using CatchTheCovid19_UWPClient.Model;
using Prism.Mvvm;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;

namespace CatchTheCovid19_UWPClient.ViewModel
{
    public class CheckMemberCardViewModel : BindableBase
    {
        ReadBarcodeManager barcodeManager = new ReadBarcodeManager();
        RestManager restManager = new RestManager();

        public delegate void BarcodeReadComplete(Member member);
        public event BarcodeReadComplete BarcodeReadCompleteEvent;

        private Member _checkMemberData = new Member();
        public Member CheckMemberCard
        {
            get => _checkMemberData;
            set => SetProperty(ref _checkMemberData, value);
        }
        

        public CheckMemberCardViewModel()
        {
            barcodeManager.ReadCompleteEvent += BarcodeManager_ReadCompleteEvent;
            barcodeManager.ConnectBarcodeRaspi();
        }

        private async Task SearchMember(string data)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, 
            ()=>
            {
                var respData = MemberManager.GetMember(data);
                if (respData != null)
                {
                    CheckMemberCard = respData;
                    BarcodeReadCompleteEvent?.Invoke(CheckMemberCard);
                }
                else
                {
                    BarcodeReadCompleteEvent?.Invoke(null);
                }
            });
        }

        private async void BarcodeManager_ReadCompleteEvent(string data)
        {
            await SearchMember(data);
        }

        public async void StartReadCard()
        {
            await barcodeManager.SendSerialData("1");
        }
    }
}

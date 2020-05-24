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

namespace CatchTheCovid19_UWPClient.ViewModel
{
    public class CheckMemberCardViewModel : BindableBase
    {
        ReadBarcodeManager barcodeManager = new ReadBarcodeManager();
        RestManager restManager = new RestManager();

        public delegate void BarcodeReadComplete(CheckMemberCard member);
        public event BarcodeReadComplete BarcodeReadCompleteEvent;

        private CheckMemberCard _checkMemberData = new CheckMemberCard();
        public CheckMemberCard CheckMemberCard
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
            var respData = await restManager.GetResponse<CheckMemberCard>("/searchCard?cardId="+data, Method.POST);
            if (respData.respStatus == HttpStatusCode.OK)
            {
                CheckMemberCard = respData.respData;
            }
            else
            {
                
            }
        }

        private async void BarcodeManager_ReadCompleteEvent(string data)
        {
            await SearchMember(data);
            BarcodeReadCompleteEvent?.Invoke(CheckMemberCard);
        }

        public async void StartReadCard()
        {
            await barcodeManager.SendSerialData("1");
        }
    }
}

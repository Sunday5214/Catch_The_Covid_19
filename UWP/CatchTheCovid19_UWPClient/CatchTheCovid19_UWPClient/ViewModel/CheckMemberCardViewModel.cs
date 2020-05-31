using CatchTheCovid10.Member;
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
        public delegate void BarcodeReadComplete(Member member);
        public event BarcodeReadComplete BarcodeReadCompleteEvent;

        private Member _checkMemberData = new Member();
        public Member CheckMemberCard
        {
            get => _checkMemberData;
            set => SetProperty(ref _checkMemberData, value);
        }
       

        public async Task SearchMember(string cardId)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, 
            ()=>
            {
                var respData = MemberManager.GetMember(cardId);
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
    }
}

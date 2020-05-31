using CatchTheCovid19.RestManager;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CatchTheCovid10.Member
{
    public class MemberManager
    {
        RestManager restManager = new RestManager();
        public static List<Member> Member
        {
            get;
            set;
        }
        public async void GetMemberData()
        {
            var resp = await restManager.GetResponse<List<Member>>("/allUser", Method.GET);
            if(resp.respStatus == System.Net.HttpStatusCode.OK)
            {
                Member = resp.respData;
            }
            else
            {
                Member = new List<Member>
                {
                    new Member{ Class=3, Grade=3, CardId="S000000289", ClassNumber=5, Idx=0, IsStudent=true, Name="김태오"}
                };
            }
        }

        public static Member GetMember(string cardId)
        {
            return (Member.Where(x => x.CardId == cardId) == null ? null : Member.Where(x => x.CardId == cardId).ToList()[0]);
        }
    }
}

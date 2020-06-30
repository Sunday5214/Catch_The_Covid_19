using CatchTheCovid19.Barcode;
using CatchTheCovid10.InitData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheCovid19_UWPClient.Model
{
    public class CheckMemberCard : Member
    {
        BarcodeManager barcodeManager = new BarcodeManager();
    }
}

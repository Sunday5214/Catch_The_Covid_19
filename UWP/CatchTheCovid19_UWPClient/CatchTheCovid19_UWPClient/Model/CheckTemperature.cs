using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheCovid19_UWPClient.Model
{
    public class CheckTemperature
    {
        public double Temp
        {
            get;
            set;
        }

        public int code
        {
            get;
            set;
        }

        public int memberIdx
        {
            get;
            set;
        }
    }
}

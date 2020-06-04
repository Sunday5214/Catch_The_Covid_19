using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace CatchTheCovid19.I2C
{
    public class I2CManager
    {
        public delegate void ReciveHandler(object sender, string data);
        ReciveHandler OnReciveHander = null;
        I2cDevice device = null;
        public I2CManager(ReciveHandler reciveHandler, ref I2cDevice device)
        {
            this.device = device;
            OnReciveHander = reciveHandler;
        }

        public async Task StartI2C()
        {
            string strdata = "";
            byte[] data = new byte[20];
            await Task.Run(async () =>
            {

                while (true)
                {
                    data = await GetDataByte();
                    strdata = Encoding.Default.GetString(data);
                    OnReciveHander?.Invoke(this, strdata);
                    Debug.WriteLine(strdata);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            });
        }

        private async Task<byte[]> GetDataByte()
        {
            byte[] bytedata = new byte[20];
            await Task.Run(() =>
            {
                device.Read(bytedata);
            });

            return bytedata;
        }
    }
}

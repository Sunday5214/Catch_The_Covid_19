using CatchTheCovid19.RestClient.Option;
using CatchTheCovid19_UWPClient.Model;
using CatchTheCovid19_UWPClient.View;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls.Primitives;

namespace CatchTheCovid19_UWPClient.ViewModel
{
    public class SettingViewModel : BindableBase
    {
        public delegate void SettingComplete(bool success);
        public event SettingComplete SettingCompleteEvent;

        private List<string> _voiceLists = new List<string>();
        public List<string> VoiceLists
        {
            get => _voiceLists;
            set => SetProperty(ref _voiceLists, value);
        }

        private string _serverAddress;
        public string ServerAddress
        {
            get => _serverAddress;
            set => SetProperty(ref _serverAddress, value);
        }

        private string _selectedVoice;
        public string SelectedVoice
        {
            get => _selectedVoice;
            set => SetProperty(ref _selectedVoice, value);
        }

        public ICommand SettingOKCommand
        {
            get;
            set;
        }

        public SettingViewModel()
        {
            _voiceLists.Add("Taeo");
            _voiceLists.Add("Chu");

            SettingOKCommand = new DelegateCommand(OnSaveSetting);
        }

        private void OnSaveSetting()
        {
            SaveSetting("SelectedVoice", SelectedVoice);
            SaveSetting("ServerAddress", "http://"+ServerAddress);
            SettingCompleteEvent?.Invoke(true);
        }

        private void SaveSetting(string settingName, string data)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[settingName] = data;
        }

        

        public string GetSetting(string key)
        {
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                return localSettings.Values[key] as string;
            }
            catch
            {
                return null;
            }

        }
    }
}

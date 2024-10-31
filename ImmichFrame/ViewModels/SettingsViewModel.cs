using CommunityToolkit.Mvvm.ComponentModel;
using ImmichFrame.Core.Exceptions;
using ImmichFrame.Helpers;
using ImmichFrame.Models;
using System;
using System.Windows.Input;

namespace ImmichFrame.ViewModels
{
    public partial class SettingsViewModel : NavigatableViewModelBase
    {
        [ObservableProperty]
        public Settings settings;
        public ICommand SaveCommand { get; set; }        
        
       
        public SettingsViewModel()
        {
            try
            {
                Settings = Settings.CurrentSettings;
            }
            catch (SettingsNotValidException)
            {
                Settings = new Settings();
            }

            SaveCommand = new RelayCommand(SaveAction);            
        }
        
        private void SaveAction()
        {
            Settings.SaveSettings(Settings);
            Settings = Settings.CurrentSettings;

            Navigate(new MainViewModel());
        }
    }
}

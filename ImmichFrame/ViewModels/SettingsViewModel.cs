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
        public ICommand CancelCommand { get; set; }

        // Parameterless constructor for XAML and design-time support
        public SettingsViewModel() : this(true) { }
        public SettingsViewModel(bool cancleEnabled = true)
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
            CancelCommand = new RelayCommand(CancelAction);
        }
        private void CancelAction()
        {
            try
            {
                Settings.ReloadFromJson();
                Navigate(new MainViewModel());
            }
            catch (SettingsNotValidException)
            {
                this.Navigate(new ErrorViewModel(new Exception("Please provide valid settings")));
            }
        }

        private void SaveAction()
        {
            try
            {
                Settings.SaveSettings(Settings);
                Settings = Settings.CurrentSettings;
            }
            catch (Exception ex)
            {
                // could not parse 
                this.Navigate(new ErrorViewModel(ex));
                return;
            }

            Navigate(new MainViewModel());
        }
    }
}

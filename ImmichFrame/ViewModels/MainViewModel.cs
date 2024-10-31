using CommunityToolkit.Mvvm.ComponentModel;
using ImmichFrame.Helpers;
using ImmichFrame.Models;
using System;
using System.Windows.Input;

namespace ImmichFrame.ViewModels;

public partial class MainViewModel : NavigatableViewModelBase
{
    public ICommand QuitCommand { get; set; }
    public ICommand NavigateSettingsPageCommand { get; set; }
    public Uri? ServerUri => !string.IsNullOrEmpty(Settings.ImmichServerUrl) ? new Uri(Settings.ImmichServerUrl) : null;
    public MainViewModel()
    {
        settings = Settings.CurrentSettings;
        QuitCommand = new RelayCommand(ExitApp);
        NavigateSettingsPageCommand = new RelayCommand(NavigateSettingsPageAction);
    }

    public void NavigateSettingsPageAction()
    {
        Navigate(new SettingsViewModel());
    }
    

    public void ExitApp()
    {
        Environment.Exit(0);
    }

    [ObservableProperty]
    public Settings settings;
}


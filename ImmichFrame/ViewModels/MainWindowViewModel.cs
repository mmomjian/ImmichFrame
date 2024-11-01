﻿using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using ImmichFrame.Core.Exceptions;
using ImmichFrame.Models;
using System.IO;

namespace ImmichFrame.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Thickness margin;

        public MainWindowViewModel()
        {
            try
            {
                if (!File.Exists(Settings.JsonSettingsPath))
                {
                    ContentViewModel = new WelcomeViewModel();
                }
                else
                {
                    var settings = Settings.CurrentSettings;


                    ContentViewModel = new MainViewModel();
                }
            }
            catch (SettingsNotValidException ex)
            {
                ContentViewModel = new ErrorViewModel(ex);
            }

            this.ContentViewModel.Navigated += Navigate;
            this.ContentViewModel.MarginUpdated += UpdateMargin;
        }

        [ObservableProperty]
        private NavigatableViewModelBase contentViewModel;

        private void UpdateMargin(object? sender, MarginUpdatedEventArgs e)
        {
            this.Margin = Thickness.Parse(e.Margin);
        }

        private void Navigate(object? sender, NavigatedEventArgs e)
        {
            this.ContentViewModel.Navigated -= Navigate;
            this.ContentViewModel.MarginUpdated -= UpdateMargin;
            ContentViewModel = e.ViewModel;
            this.ContentViewModel.Navigated += Navigate;
            this.ContentViewModel.MarginUpdated += UpdateMargin;
        }
    }
}

using Avalonia;
using Avalonia.Controls;
using ImmichFrame.Helpers;
using ImmichFrame.ViewModels;

namespace ImmichFrame.Views;

public partial class MainView : BaseView
{
    private MainViewModel? _viewModel;

    public MainView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += OnAttachedToVisualTree;
    }
    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (PlatformDetector.IsAndroid())
        {
            var insetsManager = TopLevel.GetTopLevel(this)?.InsetsManager;
            if (insetsManager != null)
            {
                insetsManager.DisplayEdgeToEdge = true;
                insetsManager.IsSystemBarVisible = false;
            }
        }

        _viewModel = (this.DataContext as MainViewModel)!;
    }

}

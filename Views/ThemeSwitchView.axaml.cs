using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace MyAvaloniaSamples.Views;

public partial class ThemeSwitchView : UserControl
{
    public ThemeSwitchView()
    {
        InitializeComponent();
        ThemeSwitch.IsChecked = Application.Current?.ActualThemeVariant == ThemeVariant.Dark;
    }

    private void ThemeSwitch_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var isChecked = ThemeSwitch.IsChecked ?? false;
        Application
            .Current
            ?.RequestedThemeVariant = isChecked ? ThemeVariant.Dark : ThemeVariant.Light;
    }
}

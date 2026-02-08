using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace MyAvaloniaSamples.Utilities;

public class GoToSiteCommand : IRelayCommand
{
    public static readonly GoToSiteCommand Default = new();

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) =>
        Process.Start(new ProcessStartInfo(parameter?.ToString() ?? "") { UseShellExecute = true });

    public void NotifyCanExecuteChanged() { }
}

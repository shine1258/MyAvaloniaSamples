using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;

namespace MyAvaloniaSamples.ViewModels;

public partial class ClockViewModel : ViewModelBase
{
    public ClockViewModel() =>
        DispatcherTimer.Run(
            () =>
            {
                OnPropertyChanged(nameof(HourHandAngle));
                OnPropertyChanged(nameof(MinuteHandAngle));
                OnPropertyChanged(nameof(SecondHandAngle));
                return true;
            },
            TimeSpan.FromMilliseconds(500)
        );

    public DateTime Now => DateTime.Now;
    public double HourHandAngle => Now.Hour * 30 + Now.Minute / 60.0 * 30;
    public double MinuteHandAngle => Now.Minute * 6 + Now.Second / 60.0 * 6;
    public double SecondHandAngle => Now.Second * 6;
    public TickViewModel[] Ticks { get; } =
        Enumerable
            .Range(1, 12)
            .Select(i =>
            {
                const int radius = 225;
                var radians = i * 30 * Math.PI / 180;
                return new TickViewModel(
                    i,
                    radius * Math.Sin(radians),
                    -radius * Math.Cos(radians)
                );
            })
            .ToArray();

    [RelayCommand]
    private void GoToReferenceImplementationSite(string url) =>
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
}

public class TickViewModel(int number, double x, double y) : ViewModelBase
{
    public int Number { get; } = number;
    public double X { get; } = x;
    public double Y { get; } = y;
}

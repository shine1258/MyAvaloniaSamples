namespace MyAvaloniaSamples.ViewModels;

public class MainViewModel : ViewModelBase
{
    public SectionNavigatorViewModel SectionNavigatorViewModel { get; } = new();
    public ClockViewModel ClockViewModel { get; } = new();
}

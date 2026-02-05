using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace StickyHeader.ViewModels;

public partial class SectionNavigatorViewModel : ViewModelBase
{
    public ObservableCollection<SectionViewModel> Sections { get; } =
    [new("A"), new("B"), new("C"), new("D"), new("E")];

    [ObservableProperty]
    public partial SectionViewModel? CurrentSection { get; set; }
}

public partial class SectionViewModel(string title) : ViewModelBase
{
    public string Title { get; } = title;

    [ObservableProperty]
    public partial bool IsExpanded { get; set; } = true;
}

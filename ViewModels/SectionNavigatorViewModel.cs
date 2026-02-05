using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace StickyHeader.ViewModels;

public partial class SectionNavigatorViewModel : ViewModelBase
{
    public event EventHandler<SectionViewModel>? ScrollToSectionRequested;
    public ObservableCollection<SectionViewModel> Sections { get; } =
    [new("A"), new("B"), new("C"), new("D"), new("E")];

    [ObservableProperty]
    public partial SectionViewModel? CurrentSection { get; set; }

    partial void OnCurrentSectionChanged(SectionViewModel? value)
    {
        if (value != null)
            ScrollToSectionRequested?.Invoke(this, value);
    }

    [RelayCommand]
    private void OnSectionTapped(SectionViewModel section) =>
        ScrollToSectionRequested?.Invoke(this, section);
}

public class SectionViewModel(string title) : ViewModelBase
{
    public string Title { get; } = title;
}

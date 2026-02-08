using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace StickyHeader.ViewModels;

public partial class SectionNavigatorViewModel : ViewModelBase
{
    public event EventHandler<int>? ScrollToSectionRequested;
    public ObservableCollection<string> Sections { get; } = ["A", "B", "C", "D", "E"];

    [ObservableProperty]
    public partial int SelectedSectionIndex { get; set; }

    partial void OnSelectedSectionIndexChanged(int value) =>
        ScrollToSectionRequested?.Invoke(this, value);

    [RelayCommand]
    private void OnSectionTapped() => ScrollToSectionRequested?.Invoke(this, SelectedSectionIndex);
}

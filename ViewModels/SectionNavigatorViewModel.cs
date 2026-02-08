using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace MyAvaloniaSamples.ViewModels;

public partial class SectionNavigatorViewModel : ViewModelBase, IRecipient<SelectSectionMessage>
{
    public SectionNavigatorViewModel() => WeakReferenceMessenger.Default.Register(this);

    public string[] Sections { get; } = ["A", "B", "C", "D", "E"];

    [ObservableProperty]
    public partial int SelectedSectionIndex { get; set; }

    partial void OnSelectedSectionIndexChanged(int value) =>
        WeakReferenceMessenger.Default.Send(new ScrollToSectionMessage(value));

    [RelayCommand]
    private void OnSectionTapped() =>
        WeakReferenceMessenger.Default.Send(new ScrollToSectionMessage(SelectedSectionIndex));

    public void Receive(SelectSectionMessage message) =>
        SelectedSectionIndex = message.SectionIndex;
}

public record ScrollToSectionMessage(int SectionIndex);

public record SelectSectionMessage(int SectionIndex);

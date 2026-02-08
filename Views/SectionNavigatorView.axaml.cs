using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;
using MyAvaloniaSamples.ViewModels;

namespace MyAvaloniaSamples.Views;

public partial class SectionNavigatorView : UserControl
{
    public SectionNavigatorView()
    {
        InitializeComponent();
        Loaded += (_, _) => Vm.ScrollToSectionRequested += Vm_OnScrollToSectionRequested;
    }

    // 避免循环触发事件
    private bool _scrollFromVm;
    private bool _selectionFromScroll;
    private SectionNavigatorViewModel Vm => (SectionNavigatorViewModel)DataContext!;

    private void Vm_OnScrollToSectionRequested(object? sender, int index)
    {
        if (_selectionFromScroll)
            return;

        ScrollToCurrentSection(index);
    }

    private void ScrollToCurrentSection(int index)
    {
        if (index < 0)
            return;

        _scrollFromVm = true;
        if (ItemsControl.ContainerFromIndex(index) is { } container)
            container.BringIntoView(ScrollViewer.Bounds);

        _scrollFromVm = false;
    }

    private void Scroll_OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (_scrollFromVm || e.Property != ScrollViewer.OffsetProperty)
            return;

        // 需要将 UpdateCurrentSection 方法 Post 到 UIThread 中
        // 如果此时直接调用, p.Value 的值将会是滚动结束前的值
        Dispatcher.UIThread.Post(UpdateCurrentSection);
    }

    private void UpdateCurrentSection()
    {
        var index = -1;
        for (var i = 0; i < ItemsControl.ItemCount; i++)
        {
            if (ItemsControl.ContainerFromIndex(i) is not { } container)
                continue;

            var expander = container.FindDescendantOfType<Expander>();
            var p = expander?.TranslatePoint(new Point(0, 0), ScrollViewer);
            if (p == null)
                continue;

            if (p.Value.Y <= 0)
                index = i;
            else
                break;
        }

        if (index >= 0 && Vm.SelectedSectionIndex != index)
        {
            _selectionFromScroll = true;
            Vm.SelectedSectionIndex = index;
            _selectionFromScroll = false;
        }
    }
}

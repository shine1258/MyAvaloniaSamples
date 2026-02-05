using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;
using StickyHeader.ViewModels;

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

    private void Vm_OnScrollToSectionRequested(object? sender, SectionViewModel section)
    {
        if (_selectionFromScroll)
            return;

        ScrollToCurrentSection(section);
    }

    private void ScrollToCurrentSection(SectionViewModel section)
    {
        _scrollFromVm = true;

        var index = Vm.Sections.IndexOf(section);
        if (index < 0)
            return;

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
        SectionViewModel? current = null;
        for (var i = 0; i < ItemsControl.ItemCount; i++)
        {
            if (ItemsControl.ContainerFromIndex(i) is not { } container)
                continue;

            var expander = container.FindDescendantOfType<Expander>();
            var p = expander?.TranslatePoint(new Point(0, 0), ScrollViewer);
            if (!p.HasValue)
                continue;

            if (p.Value.Y <= 0)
                current = (SectionViewModel)container.DataContext!;
            else
                break;
        }

        if (current != null && Vm.CurrentSection != current)
        {
            _selectionFromScroll = true;
            Vm.CurrentSection = current;
            _selectionFromScroll = false;
        }
    }
}

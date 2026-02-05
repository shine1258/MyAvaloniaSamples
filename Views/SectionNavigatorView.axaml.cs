using System.ComponentModel;
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
        Loaded += (_, _) => Vm.PropertyChanged += Vm_PropertyChanged;
    }

    // 避免循环触发事件
    private bool _scrollFromVm;
    private bool _selectionFromScroll;
    private SectionNavigatorViewModel Vm => (SectionNavigatorViewModel)DataContext!;

    private void Vm_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (
            e.PropertyName == nameof(SectionNavigatorViewModel.CurrentSection)
            && !_selectionFromScroll
        )
        {
            ScrollToCurrentSection();
        }
    }

    private void ScrollToCurrentSection()
    {
        if (_scrollFromVm || Vm.CurrentSection == null)
            return;

        _scrollFromVm = true;

        var index = Vm.Sections.IndexOf(Vm.CurrentSection);
        if (index < 0)
            return;

        if (Items.ContainerFromIndex(index) is { } container)
            container.BringIntoView(Scroll.Bounds);

        _scrollFromVm = false;
    }

    private void Scroll_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != ScrollViewer.OffsetProperty || _scrollFromVm)
            return;

        // 需要将 UpdateCurrentSection 方法 Post 到 UIThread 中
        // 如果此时直接调用, p.Value 的值将会是滚动结束前的值
        Dispatcher.UIThread.Post(UpdateCurrentSection);
    }

    private void UpdateCurrentSection()
    {
        SectionViewModel? current = null;
        for (var i = 0; i < Items.ItemCount; i++)
        {
            if (Items.ContainerFromIndex(i) is not { } container)
                continue;

            var expander = container.FindDescendantOfType<Expander>();
            var p = expander?.TranslatePoint(new Point(0, 0), Scroll);
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

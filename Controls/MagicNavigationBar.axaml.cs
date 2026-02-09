using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace MyAvaloniaSamples.Controls;

public class MagicNavigationBar : ListBox
{
    public static FuncValueConverter<int, double> CircleCanvasLeftConverter { get; } =
        new(i => 80 * i);
}

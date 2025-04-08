namespace TempestMonitor.Views.Converters;

public class RowIndexToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int rowIndex)
        {
            return rowIndex % 2 == 0 ? Colors.Gold : Colors.White;
        }
        if (value is Int64 rowIndex64)
        {
            return rowIndex64 % 2 == 0 ? Colors.Gold : Colors.White;
        }
        return Colors.White;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

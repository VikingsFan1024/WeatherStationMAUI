using DictionaryOfStringUnit = System.Collections.Generic.Dictionary<string, RedStar.Amounts.Unit>;

namespace TempestMonitor.Models;

public interface IPropertyUnit
{
    public static readonly DictionaryOfStringUnit? PropertyUnit;
}

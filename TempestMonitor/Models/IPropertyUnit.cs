// Aliases for types used in this file to keep the code cleaner
using DictionaryOfStringUnit = System.Collections.Generic.Dictionary<string, RedStar.Amounts.Unit>;

namespace TempestMonitor.Models;

public interface IPropertyUnit
{
    public static readonly DictionaryOfStringUnit? PropertyUnit;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedStar.Amounts;

namespace TempestMonitor.Models;
public interface IPropertyUnit
{
    public static readonly Dictionary<string, Unit>? PropertyUnit;
}

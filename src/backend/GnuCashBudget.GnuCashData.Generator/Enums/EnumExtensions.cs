using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace GnuCashBudget.GnuCashData.Generator.Enums;
// Thanks to cocowalla for this (method)[https://gist.github.com/cocowalla/f8002c14cbf5d35fd3e0b14e3db96c75]

public static class EnumExtensions
{
    // Note that we never need to expire these cache items, so we just use ConcurrentDictionary rather than MemoryCache
    private static readonly ConcurrentDictionary<string, string> DisplayNameCache = new();

    public static string DisplayName(this Enum value)
    {
        var key = $"{value.GetType().FullName}.{value}";

        var displayName = DisplayNameCache.GetOrAdd(key, x =>
        {
            var name = (DescriptionAttribute[])value
                .GetType()
                .GetTypeInfo()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return name.Length > 0 ? name[0].Description : value.ToString();
        });

        return displayName;
    }
    
    private static readonly Random Random = new();
    
    public static T PickAtRandom<T>() where T : struct, Enum
    {
        var values = Enum.GetValues<T>();
        
        return (T) values.GetValue(Random.Next(values.Length));
    }
}

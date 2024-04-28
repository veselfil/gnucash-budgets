namespace GnuCashBudget.GnuCashData.EntityFramework.Extensions;

public static class GuidExtensions
{

    public static string ToGnuCashFormat(this Guid value)
    {
        var result = value.ToString().Replace("-", "");
        
        return result;
    }
}
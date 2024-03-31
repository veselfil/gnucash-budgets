namespace GnuCashBudget.GnuCashData.EntityFramework.Helpers;

public static class SimpleHelper
{
    public static string GenerateGuid()
    {
        var guid = Guid.NewGuid().ToString();
        guid = guid.Replace("-", "");
        
        return guid;
    }
}
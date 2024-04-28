using System.ComponentModel;

namespace GnuCashBudget.GnuCashData.Generator.Enums;

public enum IncomeDescriptionType
{
    [Description("Work Salary")]
    Work,
    
    [Description("Side Hustle")]
    SideHustle,
    
    [Description("Business")]
    Business,
    
    [Description("Charity")]
    Charity,
}
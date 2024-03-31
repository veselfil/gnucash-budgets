using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace GnuCashBudget.GnuCashData.EntityFramework.Entities;

[Table("accounts")]
public class AccountEntity
{
    [Column("guid")] public string Id { get; set; }

    [Column("name")] public string Name { get; set; }

    [Column("account_type", TypeName = "text(2048)")]
    public AccountType Type { get; set; }

    [Column("commodity_guid")] public string? CommodityId { get; set; }

    [Column("commodity_scu")] public int CommodityScu { get; set; }

    [Column("non_std_scu")] public int NonStdScu { get; set; }

    [Column("parent_guid")] public string? ParentId { get; set; }

    [Column("code")] public string? Code { get; set; }

    [Column("description")] public string? Description { get; set; }

    [Column("hidden")] public bool Hidden { get; set; }

    [Column("placeholder")] public bool Placeholder { get; set; }
}

public enum AccountType
{
    [EnumMember(Value = "ROOT")] Root,
    [EnumMember(Value = "ASSET")] Asset,
    [EnumMember(Value = "STOCK")] Stock,
    [EnumMember(Value = "MUTUAL")] Mutual,
    [EnumMember(Value = "BANK")] Bank,
    [EnumMember(Value = "CASH")] Cash,
    [EnumMember(Value = "MUTUAL_FUND")] MutualFund,
    [EnumMember(Value = "OTHER_ASSETS")] OtherAssets,
    [EnumMember(Value = "INCOME")] Income,
    [EnumMember(Value = "EXPENSE")] Expense,
    [EnumMember(Value = "LIABILITY")] Liability,
    [EnumMember(Value = "CREDIT")] Credit,
    [EnumMember(Value = "EQUITY")] Equity
}
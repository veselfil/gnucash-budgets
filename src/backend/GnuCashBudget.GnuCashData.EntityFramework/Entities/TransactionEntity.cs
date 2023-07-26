using System.ComponentModel.DataAnnotations.Schema;

namespace GnuCashBudget.GnuCashData.EntityFramework.Entities;

[Table("transactions")]
public class TransactionEntity
{
    [Column("guid")]
    public string Id { get; set; }
    
    [Column("currency_guid")]
    public string CurrencyId { get; set; }
    
    [Column("num")]
    public string Num { get; set; }
    
    [Column("post_date")]
    public DateTime PostDate { get; set; }
    
    [Column("enter_date")]
    public DateTime EntryDate { get; set; }
    
    [Column("description")]
    public string Description { get; set; }
}
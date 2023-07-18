using System.ComponentModel.DataAnnotations.Schema;

namespace GnuCashBudget.GnuCashData.EntityFramework.Entities;

public class CommodityEntity
{
    [Column("guid")]
    public string Id { get; set; }
    
    [Column("namespace")]
    public string Namespace { get; set; }
    
    [Column("mnemonic")]
    public string Mnemonic { get; set; }
    
    [Column("full_name")]
    public string FullName { get; set; }
}
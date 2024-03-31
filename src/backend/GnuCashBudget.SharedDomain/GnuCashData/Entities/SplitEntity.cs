using System.ComponentModel.DataAnnotations.Schema;

namespace GnuCashBudget.GnuCashData.EntityFramework.Entities;

[Table("splits")]
public class SplitEntity
{
    [Column("guid")]
    public string Id { get; set; }
    
    [Column("tx_guid")]
    public string TxId { get; set; }
    
    [Column("account_guid")]
    public string AccountId { get; set; }
    
    [Column("memo")]
    public string Memo { get; set; }
    
    [Column("action")]
    public string Action { get; set; }
    
    [Column("reconcile_state")]
    public bool ReconcileState { get; set; }
    
    [Column("reconcile_date")]
    public string ReconcileDate { get; set; }
    
    [Column("value_num")]
    public int ValueNum { get; set; }
    
    [Column("value_denom")]
    public int ValueDenom { get; set; }
    
    [Column("quantity_num")]
    public int QuantityNum { get; set; }
    
    [Column("quantity_denom")]
    public int QuantityDenom { get; set; }
    
    [Column("lot_guid")]
    public string LotId { get; set; }
}
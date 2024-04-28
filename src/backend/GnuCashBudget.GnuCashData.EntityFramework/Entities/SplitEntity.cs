using System.ComponentModel.DataAnnotations.Schema;

namespace GnuCashBudget.GnuCashData.EntityFramework.Entities;

/**
 * CREATE TABLE splits (
    guid            CHAR(32) PRIMARY KEY NOT NULL,
    tx_guid         CHAR(32) NOT NULL,
    account_guid    CHAR(32) NOT NULL,
    memo            text(2048) NOT NULL,
    action          text(2048) NOT NULL,
    reconcile_state text(1) NOT NULL,
    reconcile_date  timestamp NOT NULL,
    value_num       integer NOT NULL,
    value_denom     integer NOT NULL,
    quantity_num    integer NOT NULL,
    quantity_denom  integer NOT NULL,
    lot_guid        CHAR(32)
);
 */

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
    public bool ReconcileState { get; set; } // TODO is the bool correct?
    
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
    public string? LotId { get; set; }
}
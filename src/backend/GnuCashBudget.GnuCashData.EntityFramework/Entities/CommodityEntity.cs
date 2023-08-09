using System.ComponentModel.DataAnnotations.Schema;

namespace GnuCashBudget.GnuCashData.EntityFramework.Entities;

/**
 * CREATE TABLE commodities (
    guid            CHAR(32) PRIMARY KEY NOT NULL,
    namespace       text(2048) NOT NULL,
    mnemonic        text(2048) NOT NULL,
    fullname        text(2048),
    cusip           text(2048),
    fraction        integer NOT NULL,
    quote_flag      integer NOT NULL,
    quote_source    text(2048),
    quote_tz        text(2048)
);*/


public class CommodityEntity
{
    [Column("guid")]
    public string Id { get; set; }
    
    [Column("namespace")]
    public string Namespace { get; set; }
    
    [Column("mnemonic")]
    public string Mnemonic { get; set; }
    
    [Column("fullname")]
    public string? FullName { get; set; }
    
    [Column("cusip")]
    public string? Cusip { get; set; }
    
    [Column("fraction")]
    public int? Fraction { get; set; }
    
    [Column("quote_flag")]
    public int? QuoteFlag { get; set; }
    
    [Column("quote_source")]
    public string? QuoteSource { get; set; }
    
    [Column("quote_tz")]
    public string? QuoteTz { get; set; }
}
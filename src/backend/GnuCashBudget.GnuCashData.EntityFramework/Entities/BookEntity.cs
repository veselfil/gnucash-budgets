using System.ComponentModel.DataAnnotations.Schema;

namespace GnuCashBudget.GnuCashData.EntityFramework.Entities;

public class BookEntity
{
    [Column("root_account")]
    public int RootAccount { get; set; }
    
    [Column("root_template")]
    public int RootTemplate { get; set; }
}
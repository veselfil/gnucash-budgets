using System.ComponentModel;

namespace GnuCashBudget.GnuCashData.Generator.Enums;

public enum ExpensesDescriptionType
{
    [Description("Cookies")]
    Cookies,
    
    [Description("Bike Parts")]
    BikeParts,
    
    [Description("Coffee")]
    Coffee,
    
    [Description("Keyboard")]
    Keyboard,
    
    [Description("Home Utils")]
    HomeUtils,
    
    [Description("Car Parts")]
    CarParts,
    
    [Description("Dog Food")]
    DogFood,
    
    [Description("Cat Food")]
    CatFood,
    
    [Description("Moleskine notebooks")]
    NotebooksMoleskine,
    
    [Description("Field Notes notebooks")]
    NotebooksFieldNotes,
    
    [Description("Book")]
    Book,
    
    [Description("Subscription")]
    Subscription,
    
    [Description("Electronic Device")]
    ElectronicDevice,
    
    [Description("Guinness Keg")]
    GuinnessKeg,
    
    [Description("Ale Keg")]
    AleKeg,
    
    [Description("Fitness centre subscription")]
    Fitness,
    
    [Description("Restaurant expense")]
    Restaurants,
}
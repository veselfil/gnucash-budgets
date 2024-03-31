namespace GnuCashBudget.GnuCashData.Generator.Exceptions;

public class CommodityNotFoundException(string accountId) : Exception($"Commodity for account with ID {accountId} was not found");
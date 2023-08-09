namespace GnuCashBudget.GnuCashData.Abstractions.Exceptions;

public class AccountNotFoundException: Exception
{
    public AccountNotFoundException(string accountGuid): 
        base($"Account with ID {accountGuid} was not found")
    {
    }
}
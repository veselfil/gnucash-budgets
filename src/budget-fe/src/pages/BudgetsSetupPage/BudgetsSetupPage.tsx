import React, {useEffect, useState} from 'react';
import Container from "react-bootstrap/Container";
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Form from "react-bootstrap/Form";
import DateBoundsInput from "../../components/DateBoundsInput/DateBoundsInput";
import {DateRange} from "../../models/DateRange";
import {
    Account,
    BudgetedAccountResponse,
    BudgetedAccountsService, BudgetResponse,
    BudgetsService,
    ExpenseAccountsService
} from "../../gc-client";
import BudgetsSetupDataGrid from "../../components/BudgetsSetupDataGrid/BudgetsSetupDataGrid";
import AccountPicker from "../../components/AccountPicker/AccountPicker";
import moment from "moment";
import {budgetKeyToString, BudgetsKey, BudgetsMap} from "../../models/BudgetsMap";

const BudgetsSetupPage: React.FC = () => {
    const [dateRange, setDateRange] = useState<DateRange>({ 
        startDate: new Date("2023-01-01"),
        endDate: new Date("2023-12-31")});
    
    const [allAccounts, setAllAccounts] = useState<Account[]>([]);
    const [budgetedAccounts, setBudgetedAccounts] = useState<BudgetedAccountResponse[]>([]);
    const [budgets, setBudgets] = useState<BudgetsMap>({});

    async function fetchAllAccounts() {
        const accounts = await ExpenseAccountsService.getExpenseAccounts(true);
        setAllAccounts(accounts);
    }

    async function fetchBudgetedAccounts () {
        const budgeted = await BudgetedAccountsService.getBudgetedAccounts();
        setBudgetedAccounts(budgeted.accounts!);
    }
    
    async function fetchBudgets () {
        const response = await BudgetsService.getBudgets(
            moment(dateRange.startDate).format("yyyy-MM-DD"), 
            moment(dateRange.endDate).format("yyyy-MM-DD"));
        
        let budgetsObject = {} as BudgetsMap;
        response.budgets?.forEach(budget => {
            const key = { budgetedAccountId: budget.budgetedAccountId, dateString: moment(budget.startDate).format("yyyy-MM") } as BudgetsKey;
            budgetsObject = { ...budgetsObject,  [budgetKeyToString(key)]: budget.amount! };
        })
        
        setBudgets(budgetsObject);
    }
    
    useEffect(() => {
        fetchAllAccounts().catch(console.error);
        fetchBudgetedAccounts().catch(console.error);
        fetchBudgets().catch(console.error);
    }, [])
    
    async function handleAccountSelected (account: Account) {
        await BudgetedAccountsService.postBudgetedAccounts({ accountGuid: account.id });
        await fetchBudgetedAccounts();
    }
    
    function budgetHasChanged (account: BudgetedAccountResponse, amount: number, dateString: string) {
        BudgetsService.putBudgets({ accountId: account.budgetedAccountId, budgetId: 0, amount: amount, date: dateString + "-01" })
        setBudgets({ ...budgets, [budgetKeyToString({ budgetedAccountId: account.budgetedAccountId!, dateString: dateString })]: amount })
    }
    
    return (
        <Container fluid>
            <h2>Setup budgets</h2>
            <DateBoundsInput dateRange={dateRange} 
                             onRangeChanged={setDateRange} />
            
            <AccountPicker accounts={allAccounts} onAccountSelected={handleAccountSelected} />
            <BudgetsSetupDataGrid accounts={budgetedAccounts}
                                  budgets={budgets}
                                  onBudgetChanged={budgetHasChanged}
                                  dateRange={dateRange} />
        </Container>
    )
}

export default BudgetsSetupPage;
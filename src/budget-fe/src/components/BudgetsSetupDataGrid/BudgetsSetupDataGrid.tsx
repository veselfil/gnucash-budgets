import 'react-data-grid/lib/styles.css';

import React from 'react';
import DataGrid, {Column, RowsChangeData, textEditor} from 'react-data-grid';
import {Account, BudgetedAccountResponse} from "../../gc-client";
import {DateRange} from "../../models/DateRange";
import moment from "moment";
import {budgetKeyToString, BudgetsMap} from "../../models/BudgetsMap";
import {formatCurrency} from "../../helpers/dateFormatting";

interface BudgetsSetupDataGridProps {
    accounts: BudgetedAccountResponse[]
    budgets: BudgetsMap
    dateRange: DateRange   
    onBudgetChanged: (account: BudgetedAccountResponse, budgetValue: number, dateString: string) => void;
}

interface Row {
    accountName: string
    accountGuid: string
    [key: string]: string
}

interface YearMonth { 
    year: number,
    month: number 
}

function* iterateDateRange(range: DateRange): Generator<YearMonth> { 
    let cursor = moment(range.startDate);
    const endMoment = moment(range.endDate);
    while (cursor.isBefore(endMoment) || cursor.isSame(endMoment)) {
        yield { year: cursor.year(), month: cursor.month() + 1 };
        cursor.add(1, 'M');
    }
}

const BudgetsSetupDataGrid: React.FC<BudgetsSetupDataGridProps> = props => {
    const yearMonthToString = (yearMonth: YearMonth) => 
        `${yearMonth.year}-${yearMonth.month.toLocaleString("cs-CZ", { minimumIntegerDigits: 2 })}`;

    const expensesString = "Expenses:";
    const buildAccountName = (accountName: string, currencyCode: string) => 
        `${accountName!.substring(expensesString.length)} (${currencyCode})`;
    
    const monthColumns = [...iterateDateRange(props.dateRange)]
        .map(x => {
            const yearMonth = yearMonthToString(x)
            return { 
                key: yearMonth,
                name: yearMonth,
                editor: textEditor,
                resizable: true,
            } as Column<Row>
        });

    const columns: Column<Row>[] = [
        { key: "accountName", name: "Account", width: 200, resizable: true, },
        ...monthColumns
    ];
    
    const rows: Row[] = props.accounts
        .map(account => ({ budgetedAccountId: account.budgetedAccountId, row: { 
            accountGuid: account.accountId, 
            accountName: buildAccountName(account.fullName!, account.currencyCode!),
            currency: account.currencyCode!    
        } as Row })).map(x => {
            const { budgetedAccountId, row } = x;
            const currency = props.accounts.filter(x => x.budgetedAccountId == budgetedAccountId)[0].currencyCode;
            return [...iterateDateRange(props.dateRange)].reduce((r, m) => {
                const key = budgetKeyToString({ dateString: yearMonthToString(m), budgetedAccountId: budgetedAccountId!})
                return {
                    ...r, [yearMonthToString(m)]: Object.hasOwn(props.budgets, key) ? props.budgets[key] : ""
                } as Row;
            }, row);     
        });
    function handleRowsChange(rows: Row[], data: RowsChangeData<Row>) {
        data.indexes.forEach(index => {
            const row = rows[index];
            let newValue = row[data.column.key];
            if (newValue === "")
               newValue = "0"; 
            
            const account = props.accounts.filter(x => x.accountId == row.accountGuid)[0];
            props.onBudgetChanged(account, parseFloat(newValue), data.column.key);
        })
    }

    return (
        <DataGrid rows={rows} columns={columns} onRowsChange={handleRowsChange} />
    )
} 

export default BudgetsSetupDataGrid;
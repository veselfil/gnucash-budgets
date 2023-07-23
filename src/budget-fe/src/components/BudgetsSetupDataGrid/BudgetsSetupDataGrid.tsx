import React from 'react';
// import 'react-data-grid/lib/styles.css';
import DataGrid, {Column, TextEditor} from 'react-data-grid';
import {Account} from "../../gc-client";
import {DateRange} from "../../models/DateRange";
import moment from "moment";

interface BudgetsSetupDataGridProps {
    accounts: Account[]
    dateRange: DateRange    
}

interface Row {
    accountName: string
    [key: string]: string
}

interface YearMonth { 
    year: number,
    month: number 
}

function* iterateDateRange(range: DateRange): Generator<YearMonth> { 
    let cursor = moment(range.startDate);
    const endMoment = moment(range.endDate);
    while (cursor.isBefore(endMoment) || cursor.isSame(endMoment))
    {
        yield { year: cursor.year(), month: cursor.month() + 1 };
        cursor.add(1, 'M');
    }
}

const BudgetsSetupDataGrid: React.FC<BudgetsSetupDataGridProps> = props => {
    const yearMonthToString = (yearMonth: YearMonth) => 
        `${yearMonth.year}-${yearMonth.month.toLocaleString("cs-CZ", { minimumIntegerDigits: 2 })}`;
    
    const monthColumns = [...iterateDateRange(props.dateRange)]
        .map(x => {
            const yearMonth = yearMonthToString(x)
            return { 
                key: yearMonth,
                name: yearMonth,
                editor: TextEditor,
                resizable: true
            } as Column<Row>
        });

    const columns: Column<Row>[] = [
        { key: "accountName", name: "accountName", editable: true, width: 200, resizable: true, },
        ...monthColumns
    ];
    
    const expensesString = "Expenses:";
    const rows: Row[] = props.accounts
        .map(account => ({ accountName: account.fullName!.substring(expensesString.length) }))
        .map(row => {
            [...iterateDateRange(props.dateRange)].forEach(m => {
                row = { ...row, [yearMonthToString(m)]: "200" }
            })
            
            return row;
        });
    
    return (
        <DataGrid rows={rows} columns={columns} />
    )
} 

export default BudgetsSetupDataGrid;
import React, {useEffect, useState} from 'react';
import Container from "react-bootstrap/Container";
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Form from "react-bootstrap/Form";
import DateBoundsInput from "../../components/DateBoundsInput/DateBoundsInput";
import {DateRange} from "../../models/DateRange";
import {Button} from "react-bootstrap";
import {Account, ExpenseAccountsService} from "../../gc-client";
import BudgetsSetupDataGrid from "../../components/BudgetsSetupDataGrid/BudgetsSetupDataGrid";

const BudgetsSetupPage: React.FC = () => {
    const [dateRange, setDateRange] = useState<DateRange>({ 
        startDate: new Date("2023-01-01"),
        endDate: new Date("2023-12-31")});
    
    const [accounts, setAccounts] = useState<Account[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            const accounts = await ExpenseAccountsService.getExpenseAccounts(true);
            setAccounts(accounts);
        }

        fetchData().catch(console.error);
    }, [])
    
    return (
        <Container fluid>
            <h2>Setup budgets</h2>
            <DateBoundsInput dateRange={dateRange} 
                             onRangeChanged={setDateRange} />
            
            
            <BudgetsSetupDataGrid accounts={accounts} dateRange={dateRange} />
        </Container>
    )
}

export default BudgetsSetupPage;
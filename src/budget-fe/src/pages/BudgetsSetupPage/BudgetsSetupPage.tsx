import React, {useState} from 'react';
import Container from "react-bootstrap/Container";
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Form from "react-bootstrap/Form";
import DateBoundsInput from "../../components/DateBoundsInput/DateBoundsInput";
import {DateRange} from "../../models/DateRange";
import {Button} from "react-bootstrap";

const BudgetsSetupPage: React.FC = () => {
    const [dateRange, setDateRange] = useState<DateRange>({ 
        startDate: new Date("2023-01-01"),
        endDate: new Date("2023-12-31")});
    
    return (
        <Container>
            <h2>Setup budgets</h2>
            <DateBoundsInput dateRange={dateRange} 
                             onRangeChanged={setDateRange} />
            
            <Button onClick={e => console.log(dateRange)}>Print shit</Button>
        </Container>
    )
}

export default BudgetsSetupPage;
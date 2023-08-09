import React, {useEffect} from 'react';
import Container from "react-bootstrap/Container";
import Card from 'react-bootstrap/Card';
import {BalanceResponse, BalanceService} from "../../gc-client";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import {formatCurrency} from "../../helpers/dateFormatting";


const BudgetsBalancePage: React.FC = () => {
    const [balances, setBalances] = React.useState<BalanceResponse[]>([]);

    async function init() {
        const response = await BalanceService.getBalances();
        setBalances(response.balances!);
    }

    useEffect(() => {
        init().catch(console.error);
    }, []);


    return (
        <Container fluid>
            <br/>
            <h2>Budget balances</h2>
            <hr/>
            <Row md={4}>
                {balances.map((b, index) => <BudgetStatusCard key={index} 
                                                              accountName={b.accountName!} 
                                                              balance={b.balance!}
                                                              currency={b.currencyCode!} />)}
            </Row>
        </Container>);
};

interface BudgetStatusCardProps {
    accountName: string
    balance: number
    currency: string
}

const BudgetStatusCard: React.FC<BudgetStatusCardProps> = props => {
    const getBackground = (balance: number) => {
        if (balance < -250) return "danger";
        if (balance < 0) return "warning";
        if (balance < 100) return "info";
        return "success";
    }
    const removeExpensesPrefix = (input: string) =>
        input.substring("Expenses:".length);

    return <Col>
        <Card className="mb-5" bg={getBackground(props.balance)} text="white">
            <Card.Header>{removeExpensesPrefix(props.accountName)}</Card.Header>
            <Card.Body>
                <Card.Title>{formatCurrency(props.balance, props.currency)}</Card.Title>
            </Card.Body>
        </Card>
    </Col>;
}

export default BudgetsBalancePage;
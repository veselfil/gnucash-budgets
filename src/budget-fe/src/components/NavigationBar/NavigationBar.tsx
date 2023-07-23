import React from 'react';
import {Navbar, Nav, NavDropdown, Container} from 'react-bootstrap';

const NavigationBar: React.FC = () => {
    return (<Navbar expand="lg" className="bg-body-tertiary">
        <Container>
            <Navbar.Brand href="#home">GNU Cash budgets</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="me-auto">
                    <Nav.Link href="#setup">Setup</Nav.Link>
                    <Nav.Link href="#report">Report</Nav.Link>
                </Nav>
            </Navbar.Collapse>
        </Container>
    </Navbar>)
}

export default NavigationBar;
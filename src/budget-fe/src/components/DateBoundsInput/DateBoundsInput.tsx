import React, {useState} from 'react';
import Col from "react-bootstrap/Col";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import {DateRange} from "../../models/DateRange";

interface DateBoundsInputProps {
    dateRange: DateRange
    onRangeChanged: (range: DateRange) => void;
}

const DateBoundsInput: React.FC<DateBoundsInputProps> = props => {
    return (
        <Row>
            <Col>
                <Form>
                    <Form.Group>
                        <Form.Label>Start date</Form.Label>
                        <Form.Control type="date" 
                                      value={props.dateRange.startDate.toISOString().split('T')[0]} 
                                      onChange={event => props.onRangeChanged({ 
                                          ...props.dateRange, startDate: new Date(event.currentTarget.value) })} />
                    </Form.Group>
                </Form>
            </Col>
            <Col>
                <Form>
                    <Form.Group>
                        <Form.Label>Start date</Form.Label>
                        <Form.Control type="date" value={props.dateRange.endDate.toISOString().split('T')[0]}
                                      onChange={event => props.onRangeChanged({ 
                                          ...props.dateRange, endDate: new Date(event.currentTarget.value) })}/>
                    </Form.Group>
                </Form>
            </Col>
        </Row>
    )
}

export default DateBoundsInput;

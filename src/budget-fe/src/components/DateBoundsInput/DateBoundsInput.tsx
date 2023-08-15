import React, {useState} from 'react';
import Col from "react-bootstrap/Col";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import {DateRange} from "../../models/DateRange";
import moment from 'moment';
import {Button, FormControl} from "react-bootstrap";

interface DateBoundsInputProps {
    dateRange: DateRange
    onRangeChanged: (range: DateRange) => void;
}

interface ValidationErrors {
    startDate: boolean
    endDate: boolean
}

interface DateRangeString {
    startDate: string
    endDate: string
}

const DateBoundsInput: React.FC<DateBoundsInputProps> = props => {
    const [dateRange, setDateRange] = useState<DateRangeString>({
        startDate: props.dateRange.startDate.toISOString().split('T')[0],
        endDate : props.dateRange.endDate.toISOString().split('T')[0]
    });
    
    const [validationErrors, setValidationErrors] = useState<ValidationErrors>({ 
        startDate: false, endDate: false 
    });
    const startChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
        setValidationErrors({ ...validationErrors, startDate: !moment(event.currentTarget.value).isValid() })
        setDateRange({
            ...dateRange, startDate: event.currentTarget.value
        });
    }

    const endChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
        const isValid = !moment(event.currentTarget.value).isValid();
        setValidationErrors({ ...validationErrors, endDate: isValid })
        setDateRange({
            ...dateRange, endDate: event.currentTarget.value
        });
    }
    
    const onSetButtonClicked = (event: React.UIEvent) => {
        if (validationErrors.endDate || validationErrors.startDate)
            return;
        
        props.onRangeChanged({ startDate: new Date(dateRange.startDate), endDate: new Date(dateRange.endDate) });
    }

    return (
        <div>
            <Row>
                <Col>Start date</Col>
                <Col>End date</Col>
                <Col></Col>
            </Row>
            <Row>
                <Col>
                    <Form>
                        <Form.Group>
                            <Form.Control type="date"
                                          isInvalid={validationErrors.startDate}
                                          value={dateRange.startDate}
                                          onChange={startChanged}
                            />
                        </Form.Group>
                    </Form>
                </Col>
                <Col>
                    <Form>
                        <Form.Group>
                            <Form.Control type="date"
                                          isInvalid={validationErrors.endDate}
                                          value={dateRange.endDate}
                                          onChange={endChanged}/>
                        </Form.Group>
                    </Form>
                </Col>
                <Col>
                    <Button onClick={onSetButtonClicked}>Set</Button>
                </Col>
            </Row>
        </div>
    )
}

export default DateBoundsInput;

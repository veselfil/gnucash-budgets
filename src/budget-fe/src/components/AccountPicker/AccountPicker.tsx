import React from 'react';
import FormSelect from "react-bootstrap/FormSelect";
import {ExpenseAccount} from "../../gc-client";
import {FormControl} from "react-bootstrap";

interface AccountPickerProps {
    accounts: ExpenseAccount[]
    onAccountSelected: (account: ExpenseAccount) => void;
}

const AccountPicker: React.FC<AccountPickerProps> = (props: AccountPickerProps) => {
    function onChange(event: React.ChangeEvent<HTMLSelectElement>) {
        const selectedId = event.currentTarget.value;
        props.onAccountSelected(props.accounts.filter(x => x.id == selectedId)[0]);
    }
    
    return (
        <FormSelect aria-label="Default select example" onChange={onChange}>
            <option value="">Select account</option>
            {props.accounts.map(x => (
                <option value={x.id!} key={x.id!}>{x.fullName} ({x.currencyCode})</option>
            ))}
        </FormSelect>
    )
}

export default AccountPicker;
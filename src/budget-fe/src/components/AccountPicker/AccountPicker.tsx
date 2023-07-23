import React from 'react';
import Form from "react-bootstrap/Form";
import {Account} from "../../gc-client";

interface AccountPickerProps {
    accounts: Account[]
}

const AccountPicker: React.FC<AccountPickerProps> = (props: AccountPickerProps) => {
    return (
        <Form.Select aria-label="Default select example">
            <option value="">Select account</option>
            {props.accounts.map(x => (
                <option value={x.id!} key={x.id!}>{x.fullName}</option>
            ))}
        </Form.Select>
    )
}

export default AccountPicker;
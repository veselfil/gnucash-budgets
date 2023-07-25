import React, {useContext, useEffect, useState} from 'react';
import {Account, ExpenseAccountsService} from "../../gc-client";
import AccountPicker from "../AccountPicker/AccountPicker";

export const AccountsList: React.FC = () => {
    const [accounts, setAccounts] = useState<Account[]>([])
    useEffect(() => {
        const fetchData = async () => {
            const accounts = await ExpenseAccountsService.getExpenseAccounts(true);
            setAccounts(accounts);
        }
        
        fetchData().catch(console.error);
    }, [])

    return <div>Loading</div>;
}

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
    
    if (accounts != null)
    {
        return (
            <AccountPicker accounts={accounts} />
        )    
    }
    else return <div>Loading</div>;
}
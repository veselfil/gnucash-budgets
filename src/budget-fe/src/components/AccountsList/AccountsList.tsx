import React, {useContext, useEffect, useState} from 'react';
import {useContextNullGuarded} from "../../contextHelpers";
import {GnuCashContext} from "../../context/GnuCashContext";
import {ExpenseAccountsService} from "../../gc-client";


export const AccountsList: React.FC = () => {
    const [accounts, setAccounts] = useState<string[]>([])
    useEffect(() => {
        (async () => {
            const accounts = await ExpenseAccountsService.getExpenseAccounts();
            setAccounts(accounts.map(x => x.name!));
        })();
    }, [])
    
    return (
        <ul>
            {accounts.map((name, index) => (<li key={index}>{name}</li>))}
        </ul>
    )
}
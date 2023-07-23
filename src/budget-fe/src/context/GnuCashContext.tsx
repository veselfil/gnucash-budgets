import React, {createContext, PropsWithChildren, useState} from 'react';
import {ExpenseAccountsService} from "../gc-client";


interface GnuCashContextProps {
    expenseAccountsService: ExpenseAccountsService    
}

export const GnuCashContext = createContext<GnuCashContextProps | null>(null);

const GnuCashContextProvider = (props: PropsWithChildren) => {
    const context: GnuCashContextProps = {
        expenseAccountsService: new ExpenseAccountsService()
    }
    
    return (
        <GnuCashContext.Provider value={context}>
            {props.children}  
        </GnuCashContext.Provider>
    )
}

export { GnuCashContextProvider }
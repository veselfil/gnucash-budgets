/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { AccountType } from './AccountType';

export type ExpenseAccount = {
    id?: string;
    name?: string;
    fullName?: string;
    commodity?: string;
    accountType?: AccountType;
    childAccounts?: Array<ExpenseAccount>;
    currencyCode?: string;
};


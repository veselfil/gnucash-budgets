/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { AccountType } from './AccountType';

export type Account = {
    id?: string | null;
    name?: string | null;
    fullName?: string | null;
    commodity?: string | null;
    accountType?: AccountType;
    childAccounts?: Array<Account> | null;
};


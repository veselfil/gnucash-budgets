/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ExpenseAccount } from '../models/ExpenseAccount';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class ExpenseAccountsService {

    /**
     * @param bottomLevelOnly
     * @returns ExpenseAccount Success
     * @throws ApiError
     */
    public static getExpenseAccounts(
        bottomLevelOnly: boolean = false,
    ): CancelablePromise<Array<ExpenseAccount>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/expense-accounts',
            query: {
                'bottomLevelOnly': bottomLevelOnly,
            },
        });
    }

}

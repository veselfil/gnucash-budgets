/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { AddBudgetedAccountRequest } from '../models/AddBudgetedAccountRequest';
import type { ListBudgetedAccountsResponse } from '../models/ListBudgetedAccountsResponse';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class BudgetedAccountsService {

    /**
     * @returns ListBudgetedAccountsResponse Success
     * @throws ApiError
     */
    public static getBudgetedAccounts(): CancelablePromise<ListBudgetedAccountsResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/budgeted-accounts',
        });
    }

    /**
     * @param requestBody
     * @returns any Success
     * @throws ApiError
     */
    public static postBudgetedAccounts(
        requestBody?: AddBudgetedAccountRequest,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/budgeted-accounts',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}

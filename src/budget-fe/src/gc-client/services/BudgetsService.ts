/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { GetBudgetsInRangeResponse } from '../models/GetBudgetsInRangeResponse';
import type { SetBudgetRequest } from '../models/SetBudgetRequest';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class BudgetsService {

    /**
     * @param requestBody
     * @returns any Success
     * @throws ApiError
     */
    public static putBudgets(
        requestBody?: SetBudgetRequest,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/budgets',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * @param fromDate
     * @param toDate
     * @returns GetBudgetsInRangeResponse Success
     * @throws ApiError
     */
    public static getBudgets(
        fromDate?: string,
        toDate?: string,
    ): CancelablePromise<GetBudgetsInRangeResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/budgets',
            query: {
                'fromDate': fromDate,
                'toDate': toDate,
            },
        });
    }

}

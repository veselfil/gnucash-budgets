/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { GetBalancesResponse } from '../models/GetBalancesResponse';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class BalanceService {

    /**
     * @returns GetBalancesResponse Success
     * @throws ApiError
     */
    public static getBalances(): CancelablePromise<GetBalancesResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/balances',
        });
    }

}

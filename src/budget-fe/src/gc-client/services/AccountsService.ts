/* generated using openapi-typescript-codegen -- do no edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Account } from '../models/Account';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class AccountsService {

    /**
     * @returns Account Success
     * @throws ApiError
     */
    public static getAccounts(): CancelablePromise<Array<Account>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/accounts',
        });
    }

}

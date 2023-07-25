export interface BudgetsKey {
    dateString: string
    budgetedAccountId: number
}

export interface BudgetsMap {
    [key: string]: number
}

export const budgetKeyToString = (key: BudgetsKey) => `${key.budgetedAccountId}_${key.dateString}`; 



export enum TransactionType {
  Income = 1,
  Expense = 2
}

export interface Category {
  id: string;
  name: string;
  type: TransactionType;
  isDefault: boolean;
}

export interface CreateCategoryRequest {
  name: string;
  type: TransactionType;
}

export interface UpdateCategoryRequest {
  id: string;
  name: string;
  type: TransactionType;
}
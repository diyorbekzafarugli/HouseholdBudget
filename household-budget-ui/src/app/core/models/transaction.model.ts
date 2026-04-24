
import { TransactionType } from './category.model';

export interface Transaction {
  id: string;
  type: TransactionType;
  categoryName: string;
  transactionDate: string;
  amount: number;
  formattedAmount: string;
  formattedDate: string;
  comment?: string;
}

export interface CategoryChartItem {
  categoryName: string;
  total: number;
  formattedTotal: string;
  percentage: number;
}

export interface TransactionPagedResponse {
  items: Transaction[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  chartData: CategoryChartItem[];
  totalIncome: number;
  totalExpense: number;
  balance: number;
  formattedTotalIncome: string;
  formattedTotalExpense: string;
  formattedBalance: string;
}

export interface CreateTransactionRequest {
  categoryId: string;
  type: TransactionType;
  amount: number;
  transactionDate: string;
  comment?: string;
}

export interface UpdateTransactionRequest {
  categoryId: string;
  amount: number;
  transactionDate: string;
  comment?: string;
}

export interface TransactionFilter {
  type?: TransactionType;
  categoryIds?: string[];
  dateFrom?: string;
  dateTo?: string;
  pageNumber: number;
  pageSize: number;
}
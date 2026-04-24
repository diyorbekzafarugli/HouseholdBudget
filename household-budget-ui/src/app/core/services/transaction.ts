
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Transaction,
  TransactionPagedResponse,
  CreateTransactionRequest,
  UpdateTransactionRequest,
  TransactionFilter
} from '../models/transaction.model';
import { ApiResponse } from '../models/api.model';

@Injectable({ providedIn: 'root' })
export class TransactionService {
  private readonly apiUrl = 'https://localhost:7099/api/transactions';

  constructor(private http: HttpClient) {}

  getAll(filter: TransactionFilter): Observable<TransactionPagedResponse> {
    let params = new HttpParams()
      .set('pageNumber', filter.pageNumber.toString())
      .set('pageSize', filter.pageSize.toString());

    if (filter.type !== undefined)
      params = params.set('type', filter.type.toString());

    if (filter.dateFrom)
      params = params.set('dateFrom', filter.dateFrom);

    if (filter.dateTo)
      params = params.set('dateTo', filter.dateTo);

    if (filter.categoryIds?.length)
      filter.categoryIds.forEach(id => params = params.append('categoryIds', id));

    return this.http.get<TransactionPagedResponse>(this.apiUrl, { params });
  }

  getById(id: string): Observable<ApiResponse<Transaction>> {
    return this.http.get<ApiResponse<Transaction>>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateTransactionRequest): Observable<ApiResponse<{ id: string }>> {
    return this.http.post<ApiResponse<{ id: string }>>(this.apiUrl, request);
  }

  update(id: string, request: UpdateTransactionRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
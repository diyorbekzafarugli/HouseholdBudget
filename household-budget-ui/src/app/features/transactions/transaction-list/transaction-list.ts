// src/app/features/transactions/transaction-list/transaction-list.ts
import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDividerModule } from '@angular/material/divider';
import { TransactionService } from '../../../core/services/transaction';
import { CategoryService } from '../../../core/services/category';
import { Transaction, TransactionPagedResponse, TransactionFilter } from '../../../core/models/transaction.model';
import { Category, TransactionType } from '../../../core/models/category.model';
import { TransactionForm } from '../transaction-form/transaction-form';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,
    MatCardModule, MatButtonModule, MatIconModule,
    MatTableModule, MatPaginatorModule, MatSelectModule,
    MatDatepickerModule, MatNativeDateModule, MatFormFieldModule,
    MatInputModule, MatChipsModule, MatProgressSpinnerModule,
    MatDialogModule, MatTooltipModule, MatDividerModule
  ],
  templateUrl: './transaction-list.html',
  styleUrls: ['./transaction-list.scss']
})
export class TransactionList implements OnInit {
  private transactionService = inject(TransactionService);
  private categoryService = inject(CategoryService);
  private dialog = inject(MatDialog);
  private fb = inject(FormBuilder);

  loading = signal(true);
  data = signal<TransactionPagedResponse | null>(null);
  categories = signal<Category[]>([]);

  columns = ['type', 'category', 'date', 'amount', 'comment', 'actions'];
  pageSize = 20;
  pageNumber = 1;

  filterForm = this.fb.group({
    type: [null as number | null],
    categoryIds: [[] as string[]],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  ngOnInit(): void {
    this.loadCategories();
    this.loadTransactions();
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (categories) => {
        this.categories.set(categories);
      },
      error: (err) => console.error('Categories error:', err)
    });
  }

  loadTransactions(): void {
    this.loading.set(true);
    const f = this.filterForm.value;
    const categoryIds = f.categoryIds as string[];

    const filter: TransactionFilter = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      type: f.type ?? undefined,
      categoryIds: categoryIds?.length ? categoryIds : undefined,
      dateFrom: f.dateFrom ? new Date(f.dateFrom).toISOString() : undefined,
      dateTo: f.dateTo ? new Date(f.dateTo).toISOString() : undefined,
    };

    this.transactionService.getAll(filter).subscribe({
      next: res => { this.data.set(res); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  applyFilter(): void {
    this.pageNumber = 1;
    this.loadTransactions();
  }

  resetFilter(): void {
    this.filterForm.reset({
      type: null,
      categoryIds: [] as string[],
      dateFrom: null,
      dateTo: null
    });
    this.pageNumber = 1;
    this.loadTransactions();
  }

  onPageChange(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadTransactions();
  }

  openForm(transaction?: Transaction): void {
    if (this.categories().length === 0) {
      this.loadCategories();
    }

    const dialogRef = this.dialog.open(TransactionForm, {
      width: '520px',
      disableClose: false,
      data: { transaction, categories: this.categories() }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) this.loadTransactions();
    });
  }

  delete(id: string): void {
    if (!confirm('Удалить транзакцию?')) return;
    this.transactionService.delete(id).subscribe({
      next: () => this.loadTransactions()
    });
  }
}
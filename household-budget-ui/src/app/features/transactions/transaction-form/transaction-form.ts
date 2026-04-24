// src/app/features/transactions/transaction-form/transaction-form.ts
import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TransactionService } from '../../../core/services/transaction';
import { Category, TransactionType } from '../../../core/models/category.model';
import { Transaction } from '../../../core/models/transaction.model';

@Component({
  selector: 'app-transaction-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,
    MatDialogModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatIconModule,
    MatDatepickerModule, MatNativeDateModule, MatProgressSpinnerModule
  ],
  templateUrl: './transaction-form.html',
  styleUrls: ['./transaction-form.scss']
})
export class TransactionForm implements OnInit {
  private fb = inject(FormBuilder);
  private transactionService = inject(TransactionService);
  private dialogRef = inject(MatDialogRef<TransactionForm>);
  data: { transaction?: Transaction; categories: Category[] } = inject(MAT_DIALOG_DATA);

  loading = false;
  filteredCategories: Category[] = [];

  form = this.fb.group({
    type: [1, Validators.required],
    categoryId: ['', Validators.required],
    amount: [null, [Validators.required, Validators.min(0.01)]],
    transactionDate: [new Date(), Validators.required],
    comment: ['']
  });

  ngOnInit(): void {
      this.onTypeChange();

      if (this.data.transaction) {
        const t = this.data.transaction;
        const cat = this.data.categories.find(c => c.name === t.categoryName);
        this.form.patchValue({
          type: t.type,
          categoryId: cat?.id ?? '',
          amount: t.amount as any,
          transactionDate: new Date(t.transactionDate),
          comment: t.comment ?? ''
        });
        this.onTypeChange();
      }
    }

    onTypeChange(): void {
      const type = this.form.get('type')?.value as TransactionType;
      this.filteredCategories = this.data.categories.filter(c => c.type === type);
      this.form.get('categoryId')?.setValue('');
    }

    // transaction-form.ts — obs ni aniq type bilan
  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading = true;

    const value = this.form.value;
    const request = {
      categoryId: value.categoryId!,
      type: value.type!,
      amount: Number(value.amount),
      transactionDate: new Date(value.transactionDate!).toISOString(),
      comment: value.comment || undefined
    };

    if (this.data.transaction) {
      this.transactionService.update(this.data.transaction.id, request).subscribe({
        next: () => this.dialogRef.close(true),
        error: () => { this.loading = false; }
      });
    } else {
      this.transactionService.create(request).subscribe({
        next: () => this.dialogRef.close(true),
        error: () => { this.loading = false; }
      });
    }
  }
}
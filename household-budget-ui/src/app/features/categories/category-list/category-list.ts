
import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CategoryService } from '../../../core/services/category';
import { Category, TransactionType, CreateCategoryRequest } from '../../../core/models/category.model';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,
    MatCardModule, MatButtonModule, MatIconModule,
    MatTableModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatDialogModule, MatProgressSpinnerModule,
    MatTooltipModule, MatChipsModule, MatSnackBarModule
  ],
  templateUrl: './category-list.html',
  styleUrls: ['./category-list.scss']
})
export class CategoryList implements OnInit {
  private categoryService = inject(CategoryService);
  private snackBar = inject(MatSnackBar);
  private fb = inject(FormBuilder);

  loadingList = signal(true);
  loading = false;
  categories = signal<Category[]>([]);
  editingCategory: Category | null = null;

  incomeCategories = signal<Category[]>([]);
  expenseCategories = signal<Category[]>([]);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    type: [2, Validators.required]
  });

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loadingList.set(true);
    this.categoryService.getAll().subscribe({
      next: (categories) => {
        this.categories.set(categories);
        this.incomeCategories.set(categories.filter(c => c.type === TransactionType.Income));
        this.expenseCategories.set(categories.filter(c => c.type === TransactionType.Expense));
        this.loadingList.set(false);
      },
      error: () => this.loadingList.set(false)
    });
  }

  startEdit(cat: Category): void {
    this.editingCategory = cat;
    this.form.patchValue({ name: cat.name, type: cat.type });
  }

  cancelEdit(): void {
    this.editingCategory = null;
    this.form.reset({ name: '', type: 2 });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading = true;

    const req = this.form.value as CreateCategoryRequest;

    if (this.editingCategory) {
      this.categoryService.update(this.editingCategory.id, { ...req, id: this.editingCategory.id }).subscribe({
        next: () => {
          this.snackBar.open('Категория обновлена', 'OK', { duration: 3000 });
          this.cancelEdit();
          this.loadCategories();
          this.loading = false;
        },
        error: () => { this.loading = false; }
      });
    } else {
      this.categoryService.create(req).subscribe({
        next: () => {
          this.snackBar.open('Категория добавлена', 'OK', { duration: 3000 });
          this.cancelEdit();
          this.loadCategories();
          this.loading = false;
        },
        error: () => { this.loading = false; }
      });
    }
  }

  delete(id: string): void {
    if (!confirm('Удалить категорию?')) return;
    this.categoryService.delete(id).subscribe({
      next: () => {
        this.snackBar.open('Категория удалена', 'OK', { duration: 3000 });
        this.loadCategories();
      }
    });
  }
}
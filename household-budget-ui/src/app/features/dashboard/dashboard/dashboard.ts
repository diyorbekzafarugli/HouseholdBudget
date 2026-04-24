import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { BaseChartDirective } from 'ng2-charts';
import { ChartData, ChartOptions } from 'chart.js';
import { Chart, ArcElement, Tooltip, Legend, PieController } from 'chart.js';
import { TransactionService } from '../../../core/services/transaction';
import { TransactionPagedResponse } from '../../../core/models/transaction.model';
import { AuthService } from '../../../core/services/auth';

Chart.register(ArcElement, Tooltip, Legend, PieController);

interface ExpenseChartItem {
  categoryName: string;
  total: number;
  formattedTotal: string;
  percentage: number;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule, RouterLink,
    MatCardModule, MatButtonModule, MatIconModule,
    MatProgressSpinnerModule, MatDividerModule,
    BaseChartDirective
  ],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class Dashboard implements OnInit {
  private transactionService = inject(TransactionService);
  authService = inject(AuthService);

  loading = signal(true);
  data = signal<TransactionPagedResponse | null>(null);
  expenseChartData = signal<ExpenseChartItem[]>([]);

  pieChartData: ChartData<'pie'> = { labels: [], datasets: [{ data: [] }] };
  pieChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: { display: false },
      tooltip: {
        callbacks: {
          label: (ctx) => ` ${ctx.label}: ${this.formatAmount(ctx.raw as number)}`
        }
      }
    }
  };

  ngOnInit(): void {
    this.transactionService.getAll({
      pageNumber: 1,
      pageSize: 100
    }).subscribe({
      next: (res) => {
        this.data.set(res);
        this.buildChart(res);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  private buildChart(res: TransactionPagedResponse): void {
    const totalIncome = res.totalIncome;
    const totalExpense = res.totalExpense;
    const remaining = totalIncome - totalExpense; // qolgan mablag'

    if (totalIncome === 0) {
      this.expenseChartData.set([]);
      return;
    }

    const expensePercent = Math.round(totalExpense / totalIncome * 10000) / 100;
    const remainingPercent = Math.round(remaining / totalIncome * 10000) / 100;

    this.expenseChartData.set([
      {
        categoryName: 'Расходы',
        total: totalExpense,
        formattedTotal: this.formatAmount(totalExpense),
        percentage: expensePercent
      },
      {
        categoryName: 'Остаток',
        total: remaining > 0 ? remaining : 0,
        formattedTotal: this.formatAmount(remaining > 0 ? remaining : 0),
        percentage: remaining > 0 ? remainingPercent : 0
      }
    ]);

    this.pieChartData = {
      labels: ['Расходы', 'Остаток'],
      datasets: [{
        data: [totalExpense, remaining > 0 ? remaining : 0],
        backgroundColor: ['#f44336', '#4caf50'],
        hoverBackgroundColor: ['#d32f2f', '#388e3c']
      }]
    };
  }

  private formatAmount(amount: number): string {
    return new Intl.NumberFormat('ru-RU', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    }).format(amount).replace(',', '.');
  }
}
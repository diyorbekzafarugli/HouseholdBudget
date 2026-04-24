// src/app/features/transactions/transactions-module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TransactionsRoutingModule } from './transactions-routing-module';

@NgModule({
  imports: [CommonModule, TransactionsRoutingModule]
})
export class TransactionsModule {}
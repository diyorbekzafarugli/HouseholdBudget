using HouseholdBudget.Domain.Entities;
using HouseholdBudget.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HouseholdBudget.Infrastructure.Persistence;

public static class DataSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedCategories(modelBuilder);
    }

    private static void SeedCategories(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category(new Guid("a1b2c3d4-0001-0000-0000-000000000000"), "Заработная плата", TransactionType.Income, true),
            new Category(new Guid("a1b2c3d4-0002-0000-0000-000000000000"), "Иные доходы", TransactionType.Income, true),
            new Category(new Guid("a1b2c3d4-0003-0000-0000-000000000000"), "Продукты питания", TransactionType.Expense, true),
            new Category(new Guid("a1b2c3d4-0004-0000-0000-000000000000"), "Транспорт", TransactionType.Expense, true),
            new Category(new Guid("a1b2c3d4-0005-0000-0000-000000000000"), "Мобильная связь", TransactionType.Expense, true),
            new Category(new Guid("a1b2c3d4-0006-0000-0000-000000000000"), "Интернет", TransactionType.Expense, true),
            new Category(new Guid("a1b2c3d4-0007-0000-0000-000000000000"), "Развлечения", TransactionType.Expense, true)
        );
    }
}
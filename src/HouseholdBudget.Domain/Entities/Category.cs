using HouseholdBudget.Domain.Common;
using HouseholdBudget.Domain.Enums;

namespace HouseholdBudget.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public TransactionType Type { get; private set; }
    public bool IsDefault { get; private set; }

    private Category() { }

    internal Category(Guid id, string name, TransactionType type, bool isDefault)
    {
        Id = id;
        Name = name;
        Type = type;
        IsDefault = isDefault;
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }

    public static Category Create(string name, TransactionType type, bool isDefault = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Type = type,
            IsDefault = isDefault,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, TransactionType type)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Type = type;
        SetUpdatedAt();
    }
}
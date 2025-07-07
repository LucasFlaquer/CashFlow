using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

internal class CashFlowDbContext: DbContext
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost;port=3306;Database=ignite-cashflow;user=docker;password=docker;";
        var serverVersion = new MySqlServerVersion(new Version(9,3,0));
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }
}

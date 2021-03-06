// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.6
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    using Mapping;

    public interface IAccountingContext : System.IDisposable
    {
        System.Data.Entity.DbSet<Account> Accounts { get; set; } // Account
        System.Data.Entity.DbSet<DebtDemand> DebtDemands { get; set; } // DebtDemand
        System.Data.Entity.DbSet<DebtDemandLabel> DebtDemandLabels { get; set; } // DebtDemandLabel
        System.Data.Entity.DbSet<Installment> Installments { get; set; } // Installment
        System.Data.Entity.DbSet<Label> Labels { get; set; } // Label
        System.Data.Entity.DbSet<Loan> Loans { get; set; } // Loan
        System.Data.Entity.DbSet<LoanLabel> LoanLabels { get; set; } // LoanLabel
        System.Data.Entity.DbSet<Log> Logs { get; set; } // Logs
        System.Data.Entity.DbSet<Note> Notes { get; set; } // Note
        System.Data.Entity.DbSet<Notification> Notifications { get; set; } // Notification
        System.Data.Entity.DbSet<Permission> Permissions { get; set; } // Permission
        System.Data.Entity.DbSet<Person> People { get; set; } // Person
        System.Data.Entity.DbSet<Role> Roles { get; set; } // Role
        System.Data.Entity.DbSet<RolePermission> RolePermissions { get; set; } // RolePermission
        System.Data.Entity.DbSet<SettleDebtDemand> SettleDebtDemands { get; set; } // SettleDebtDemand
        System.Data.Entity.DbSet<Transaction> Transactions { get; set; } // Transaction
        System.Data.Entity.DbSet<TransactionGroup> TransactionGroups { get; set; } // TransactionGroup
        System.Data.Entity.DbSet<TransactionLabel> TransactionLabels { get; set; } // TransactionLabel
        System.Data.Entity.DbSet<User> Users { get; set; } // User
        System.Data.Entity.DbSet<UserRole> UserRoles { get; set; } // UserRole
        System.Data.Entity.DbSet<V_UserRole> V_UserRoles { get; set; } // V_UserRole
        System.Data.Entity.DbSet<V_UserRolePermission> V_UserRolePermissions { get; set; } // V_UserRolePermission

        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
        System.Threading.Tasks.Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken);
        System.Data.Entity.Infrastructure.DbChangeTracker ChangeTracker { get; }
        System.Data.Entity.Infrastructure.DbContextConfiguration Configuration { get; }
        System.Data.Entity.Database Database { get; }
        System.Data.Entity.Infrastructure.DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity);
        System.Collections.Generic.IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> GetValidationErrors();
        System.Data.Entity.DbSet Set(System.Type entityType);
        System.Data.Entity.DbSet<TEntity> Set<TEntity>() where TEntity : class;
        string ToString();

        // Stored Procedures
        int sp_SetDisplayName(string schema, string table, string column, string displayname);
        // sp_SetDisplayNameAsync cannot be created due to having out parameters, or is relying on the procedure result (int)

    }

}
// </auto-generated>

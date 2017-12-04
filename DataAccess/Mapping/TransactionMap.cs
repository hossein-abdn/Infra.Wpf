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

namespace DataAccess.Mapping
{
    using Models;

    // Transaction
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class TransactionMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Transaction>
    {
        public TransactionMap()
            : this("dbo")
        {
        }

        public TransactionMap(string schema)
        {
            ToTable("Transaction", schema);
            Property(x => x.TransactionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Amount).HasColumnName(@"Amount").HasColumnType("int");
            Property(x => x.TransactionDate).HasColumnName(@"TransactionDate").HasColumnType("datetime");
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").IsOptional();
            Property(x => x.AccountId).HasColumnName(@"AccountId").HasColumnType("int");
            Property(x => x.TypeId).HasColumnName(@"TypeId").HasColumnType("int");
            Property(x => x.TransferTransactionId).HasColumnName(@"TransferTransactionId").HasColumnType("int").IsOptional();
            Property(x => x.TransactionGroupId).HasColumnName(@"TransactionGroupId").HasColumnType("int");
            Property(x => x.TransactionGroupParentId).HasColumnName(@"TransactionGroupParentId").HasColumnType("int").IsOptional();
            Property(x => x.PersonId).HasColumnName(@"PersonId").HasColumnType("int").IsOptional();
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime");
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int");
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int");

            // Foreign keys
            HasOptional(a => a.Person).WithMany(b => b.Transactions).HasForeignKey(c => c.PersonId).WillCascadeOnDelete(false); // FK_Transaction_Person
            HasOptional(a => a.TransactionGroupParent).WithMany(b => b.TransactionGroupParent).HasForeignKey(c => c.TransactionGroupParentId).WillCascadeOnDelete(false); // FK_Transaction_TransactionGroup_Parent
            HasOptional(a => a.TransferTransaction).WithMany(b => b.Transactions).HasForeignKey(c => c.TransferTransactionId).WillCascadeOnDelete(false); // FK_TransferTransaction_Transaction
            HasRequired(a => a.Account).WithMany(b => b.Transactions).HasForeignKey(c => c.AccountId).WillCascadeOnDelete(false); // FK_Transaction_Account
            HasRequired(a => a.TransactionGroup_TransactionGroupId).WithMany(b => b.TransactionGroupId).HasForeignKey(c => c.TransactionGroupId).WillCascadeOnDelete(false); // FK_Transaction_TransactionGroup
            HasRequired(a => a.User).WithMany(b => b.Transactions).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_Transaction_User
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
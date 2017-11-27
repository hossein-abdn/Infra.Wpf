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

    // DebtDemand
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class DebtDemandMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<DebtDemand>
    {
        public DebtDemandMap()
            : this("dbo")
        {
        }

        public DebtDemandMap(string schema)
        {
            ToTable("DebtDemand", schema);
            Property(x => x.DebtDemandId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName(@"Title").HasColumnType("nvarchar");
            Property(x => x.Amount).HasColumnName(@"Amount").HasColumnType("int");
            Property(x => x.TypeId).HasColumnName(@"TypeId").HasColumnType("int");
            Property(x => x.DebtDemandDate).HasColumnName(@"DebtDemandDate").HasColumnType("datetime");
            Property(x => x.DueDate).HasColumnName(@"DueDate").HasColumnType("datetime").IsOptional();
            Property(x => x.TransactionId).HasColumnName(@"TransactionId").HasColumnType("int").IsOptional();
            Property(x => x.PersonId).HasColumnName(@"PersonId").HasColumnType("int").IsOptional();
            Property(x => x.NotificationId).HasColumnName(@"NotificationId").HasColumnType("int").IsOptional();
            Property(x => x.SettledStatusId).HasColumnName(@"SettledStatusId").HasColumnType("int");
            Property(x => x.PaidAmount).HasColumnName(@"PaidAmount").HasColumnType("int");
            Property(x => x.UnpaidAmount).HasColumnName(@"UnpaidAmount").HasColumnType("int");
            Property(x => x.UserId).HasColumnName(@"UserId").HasColumnType("int");
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int");
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime");

            // Foreign keys
            HasOptional(a => a.Notification).WithMany(b => b.DebtDemands).HasForeignKey(c => c.NotificationId).WillCascadeOnDelete(false); // FK_DebtDemand_Notification
            HasOptional(a => a.Person).WithMany(b => b.DebtDemands).HasForeignKey(c => c.PersonId).WillCascadeOnDelete(false); // FK_DebtDemand_Person
            HasOptional(a => a.Transaction).WithMany(b => b.DebtDemands).HasForeignKey(c => c.TransactionId).WillCascadeOnDelete(false); // FK_DebtDemand_Transaction
            HasRequired(a => a.User).WithMany(b => b.DebtDemands).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_DebtDemand_User
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>

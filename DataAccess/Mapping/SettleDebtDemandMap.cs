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

    // SettleDebtDemand
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class SettleDebtDemandMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<SettleDebtDemand>
    {
        public SettleDebtDemandMap()
            : this("dbo")
        {
        }

        public SettleDebtDemandMap(string schema)
        {
            ToTable("SettleDebtDemand", schema);
            Property(x => x.SettleDebtDemandId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TransactionId).HasColumnName(@"TransactionId").HasColumnType("int");
            Property(x => x.DebtDemandId).HasColumnName(@"DebtDemandId").HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.DebtDemand).WithMany(b => b.SettleDebtDemands).HasForeignKey(c => c.DebtDemandId).WillCascadeOnDelete(false); // FK_SettleDebtDemand_DebtDemand
            HasRequired(a => a.Transaction).WithMany(b => b.SettleDebtDemands).HasForeignKey(c => c.TransactionId).WillCascadeOnDelete(false); // FK_SettleDebtDemand_Transaction
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>

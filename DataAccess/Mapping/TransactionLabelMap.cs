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

    // TransactionLabel
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class TransactionLabelMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<TransactionLabel>
    {
        public TransactionLabelMap()
            : this("dbo")
        {
        }

        public TransactionLabelMap(string schema)
        {
            ToTable("TransactionLabel", schema);
            Property(x => x.TransactionLabelId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TransactionId).HasColumnName(@"TransactionId").HasColumnType("int");
            Property(x => x.LabelId).HasColumnName(@"LabelId").HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.Label).WithMany(b => b.TransactionLabels).HasForeignKey(c => c.LabelId).WillCascadeOnDelete(false); // FK_TransactionLabel_Label
            HasRequired(a => a.Transaction).WithMany(b => b.TransactionLabels).HasForeignKey(c => c.TransactionId).WillCascadeOnDelete(false); // FK_TransactionLabel_Transaction
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>

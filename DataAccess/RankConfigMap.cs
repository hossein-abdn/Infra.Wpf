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

namespace DataAccess
{

    // RankConfig
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class RankConfigMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RankConfig>
    {
        public RankConfigMap()
            : this("dbo")
        {
        }

        public RankConfigMap(string schema)
        {
            ToTable("RankConfig", schema);
            Property(x => x.RankConfigId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ScoreItemId).HasColumnName(@"ScoreItemId").HasColumnType("int");
            Property(x => x.TermId).HasColumnName(@"TermId").HasColumnType("int");
            Property(x => x.Score).HasColumnName(@"Score").HasColumnType("decimal").HasPrecision(4,2);
            Property(x => x.Delay).HasColumnName(@"Delay").HasColumnType("int").IsOptional();
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.GeneralBaseType).WithMany(b => b.RankConfigs).HasForeignKey(c => c.RecordStatusId).WillCascadeOnDelete(false); // FK_RankConfig_GeneralBaseType
            HasRequired(a => a.ScoreItem).WithMany(b => b.RankConfigs).HasForeignKey(c => c.ScoreItemId).WillCascadeOnDelete(false); // FK_RankConfig_ScoreItem
            HasRequired(a => a.Term).WithMany(b => b.RankConfigs).HasForeignKey(c => c.TermId).WillCascadeOnDelete(false); // FK_RankConfig_Term
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>

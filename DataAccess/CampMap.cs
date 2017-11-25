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

    // Camp
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class CampMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Camp>
    {
        public CampMap()
            : this("dbo")
        {
        }

        public CampMap(string schema)
        {
            ToTable("Camp", schema);
            Property(x => x.CampId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ClassSessionId).HasColumnName(@"ClassSessionId").HasColumnType("int");
            Property(x => x.Domestic).HasColumnName(@"Domestic").HasColumnType("bit");
            Property(x => x.PlaceId).HasColumnName(@"PlaceId").HasColumnType("int");
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.ClassSession).WithMany(b => b.Camps).HasForeignKey(c => c.ClassSessionId).WillCascadeOnDelete(false); // FK_Camp_ClassSession
            HasRequired(a => a.GeneralBaseType).WithMany(b => b.Camps).HasForeignKey(c => c.RecordStatusId).WillCascadeOnDelete(false); // FK_Camp_GeneralBaseType
            HasRequired(a => a.Place).WithMany(b => b.Camps).HasForeignKey(c => c.PlaceId).WillCascadeOnDelete(false); // FK_Camp_Place
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>

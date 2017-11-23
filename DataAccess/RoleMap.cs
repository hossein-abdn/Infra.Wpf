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

namespace DebugMode
{

    // Role
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class RoleMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Role>
    {
        public RoleMap()
            : this("dbo")
        {
        }

        public RoleMap(string schema)
        {
            ToTable("Role", schema);
            Property(x => x.RoleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar");
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.GeneralBaseType).WithMany(b => b.Roles).HasForeignKey(c => c.RecordStatusId).WillCascadeOnDelete(false); // FK_Role_GeneralBaseType
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
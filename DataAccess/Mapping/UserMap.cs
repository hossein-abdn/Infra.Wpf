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

    // User
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class UserMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<User>
    {
        public UserMap()
            : this("dbo")
        {
        }

        public UserMap(string schema)
        {
            ToTable("User", schema);
            Property(x => x.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UserName).HasColumnName(@"UserName").HasColumnType("nvarchar");
            Property(x => x.Password).HasColumnName(@"Password").HasColumnType("nvarchar");
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").HasColumnType("datetime");
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int");
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>

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

    // Place
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class PlaceMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Place>
    {
        public PlaceMap()
            : this("dbo")
        {
        }

        public PlaceMap(string schema)
        {
            ToTable("Place", schema);
            Property(x => x.PlaceId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar");
            Property(x => x.PlaceTypeId).HasColumnName(@"PlaceTypeId").HasColumnType("int");
            Property(x => x.TermId).HasColumnName(@"TermId").HasColumnType("int").IsOptional();
            Property(x => x.PlaceGroupId).HasColumnName(@"PlaceGroupId").HasColumnType("int");
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int");

            // Foreign keys
            HasOptional(a => a.Term).WithMany(b => b.Places).HasForeignKey(c => c.TermId).WillCascadeOnDelete(false); // FK_Place_Term
            HasRequired(a => a.PlaceGroup).WithMany(b => b.PlaceGroup).HasForeignKey(c => c.PlaceGroupId).WillCascadeOnDelete(false); // FK_Place_GeneralBaseType1
            HasRequired(a => a.PlaceType).WithMany(b => b.PlaceType).HasForeignKey(c => c.PlaceTypeId).WillCascadeOnDelete(false); // FK_Place_GeneralBaseType
            HasRequired(a => a.RecordStatus).WithMany(b => b.Places_RecordStatusId).HasForeignKey(c => c.RecordStatusId).WillCascadeOnDelete(false); // FK_Place_GeneralBaseType2
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
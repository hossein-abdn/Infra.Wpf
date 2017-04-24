// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.6
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace DataAccess.Mappings
{
    using DataAccess.Models;

    // ClassSession
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.29.1.0")]
    public class ClassSessionMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ClassSession>
    {
        public ClassSessionMap()
            : this("dbo")
        {
        }

        public ClassSessionMap(string schema)
        {
            ToTable("ClassSession", schema);
            HasKey(x => x.ClassSessionId);

            Property(x => x.ClassSessionId).HasColumnName(@"ClassSessionId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ClassScaduleId).HasColumnName(@"ClassScaduleId").HasColumnType("int").IsOptional();
            Property(x => x.Date).HasColumnName(@"Date").HasColumnType("date").IsRequired();
            Property(x => x.StatTime).HasColumnName(@"StatTime").HasColumnType("time").IsRequired();
            Property(x => x.EndTime).HasColumnName(@"EndTime").HasColumnType("time").IsRequired();
            Property(x => x.Holding).HasColumnName(@"Holding").HasColumnType("bit").IsRequired();
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int").IsRequired();

            // Foreign keys
            HasOptional(a => a.ClassScadule).WithMany(b => b.ClassSessions).HasForeignKey(c => c.ClassScaduleId).WillCascadeOnDelete(false); // FK_ClassSession_ClassScadule
            HasRequired(a => a.GeneralBaseType).WithMany(b => b.ClassSessions).HasForeignKey(c => c.RecordStatusId).WillCascadeOnDelete(false); // FK_ClassSession_GeneralBaseType
        }
    }

}
// </auto-generated>

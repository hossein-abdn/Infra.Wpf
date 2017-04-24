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

    // RollCall
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.29.1.0")]
    public class RollCallMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RollCall>
    {
        public RollCallMap()
            : this("dbo")
        {
        }

        public RollCallMap(string schema)
        {
            ToTable("RollCall", schema);
            HasKey(x => x.RollCallId);

            Property(x => x.RollCallId).HasColumnName(@"RollCallId").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ClassSessionId).HasColumnName(@"ClassSessionId").HasColumnType("int").IsRequired();
            Property(x => x.StudentId).HasColumnName(@"StudentId").HasColumnType("int").IsOptional();
            Property(x => x.ParentId).HasColumnName(@"ParentId").HasColumnType("int").IsOptional();
            Property(x => x.PresenceId).HasColumnName(@"PresenceId").HasColumnType("int").IsRequired();
            Property(x => x.Comment).HasColumnName(@"Comment").HasColumnType("nvarchar").IsOptional().HasMaxLength(50);
            Property(x => x.Delay).HasColumnName(@"Delay").HasColumnType("int").IsOptional();
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int").IsRequired();

            // Foreign keys
            HasOptional(a => a.Parent).WithMany(b => b.RollCalls).HasForeignKey(c => c.ParentId).WillCascadeOnDelete(false); // FK_RollCall_Parent
            HasOptional(a => a.Student).WithMany(b => b.RollCalls).HasForeignKey(c => c.StudentId).WillCascadeOnDelete(false); // FK_RollCall_Student
            HasRequired(a => a.ClassSession).WithMany(b => b.RollCalls).HasForeignKey(c => c.ClassSessionId).WillCascadeOnDelete(false); // FK_RollCall_ClassSession
            HasRequired(a => a.Presence).WithMany(b => b.Presence).HasForeignKey(c => c.PresenceId).WillCascadeOnDelete(false); // FK_RollCall_GeneralBaseType
            HasRequired(a => a.RecordStatus).WithMany(b => b.RollCalls_RecordStatusId).HasForeignKey(c => c.RecordStatusId).WillCascadeOnDelete(false); // FK_RollCall_GeneralBaseType1
        }
    }

}
// </auto-generated>

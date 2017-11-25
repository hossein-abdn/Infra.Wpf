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

    // RegisterQuestion
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class RegisterQuestionMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RegisterQuestion>
    {
        public RegisterQuestionMap()
            : this("dbo")
        {
        }

        public RegisterQuestionMap(string schema)
        {
            ToTable("RegisterQuestion", schema);
            Property(x => x.RegisterQuestionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.RegisterId).HasColumnName(@"RegisterId").HasColumnType("int");
            Property(x => x.TermQuestionId).HasColumnName(@"TermQuestionId").HasColumnType("int");
            Property(x => x.QuestionItemId).HasColumnName(@"QuestionItemId").HasColumnType("int");
            Property(x => x.AnswerItem).HasColumnName(@"AnswerItem").HasColumnType("bit").IsOptional();
            Property(x => x.AnswerText).HasColumnName(@"AnswerText").HasColumnType("nvarchar").IsOptional();
            Property(x => x.RecordStatusId).HasColumnName(@"RecordStatusId").HasColumnType("int");

            // Foreign keys
            HasRequired(a => a.GeneralBaseType).WithMany(b => b.RegisterQuestions).HasForeignKey(c => c.RecordStatusId).WillCascadeOnDelete(false); // FK_RegisterQuestion_GeneralBaseType
            HasRequired(a => a.QuestionItem).WithMany(b => b.RegisterQuestions).HasForeignKey(c => c.QuestionItemId).WillCascadeOnDelete(false); // FK_RegisterQuestion_QuestionItem
            HasRequired(a => a.Register).WithMany(b => b.RegisterQuestions).HasForeignKey(c => c.RegisterId).WillCascadeOnDelete(false); // FK_RegisterQuestion_Register
            HasRequired(a => a.TermQuestion).WithMany(b => b.RegisterQuestions).HasForeignKey(c => c.TermQuestionId).WillCascadeOnDelete(false); // FK_RegisterQuestion_TermQuestion
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>

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

    // RegisterQuestion
    ///<summary>
    /// جدول پاسخ سوالات ثبت نام
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class RegisterQuestion
    {
        [Column(@"RegisterQuestionId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        [Display(Name = "Register question ID")]
        public int RegisterQuestionId { get; set; } // RegisterQuestionId (Primary key)

        ///<summary>
        /// مرتبط با ثبت نام شونده
        ///</summary>
        [Required]
        [Display(Name = "Register ID")]
        public int RegisterId { get; set; } // RegisterId

        ///<summary>
        /// مرتبط با خود سوال
        ///</summary>
        [Required]
        [Display(Name = "Term question ID")]
        public int TermQuestionId { get; set; } // TermQuestionId

        ///<summary>
        /// مرتبط با گزینه پاسخ
        ///</summary>
        [Required]
        [Display(Name = "Question item ID")]
        public int QuestionItemId { get; set; } // QuestionItemId

        ///<summary>
        /// مربوط به پاسخ های گزینه ای
        ///</summary>
        [Display(Name = "Answer item")]
        public bool? AnswerItem { get; set; } // AnswerItem

        ///<summary>
        /// مربوط به پاسخ های متنی
        ///</summary>
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Name = "Answer text")]
        public string AnswerText { get; set; } // AnswerText (length: 200)

        [Required]
        [Display(Name = "Record status ID")]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Foreign keys

        /// <summary>
        /// Parent GeneralBaseType pointed by [RegisterQuestion].([RecordStatusId]) (FK_RegisterQuestion_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType GeneralBaseType { get; set; } // FK_RegisterQuestion_GeneralBaseType

        /// <summary>
        /// Parent QuestionItem pointed by [RegisterQuestion].([QuestionItemId]) (FK_RegisterQuestion_QuestionItem)
        /// </summary>
        public virtual QuestionItem QuestionItem { get; set; } // FK_RegisterQuestion_QuestionItem

        /// <summary>
        /// Parent Register pointed by [RegisterQuestion].([RegisterId]) (FK_RegisterQuestion_Register)
        /// </summary>
        public virtual Register Register { get; set; } // FK_RegisterQuestion_Register

        /// <summary>
        /// Parent TermQuestion pointed by [RegisterQuestion].([TermQuestionId]) (FK_RegisterQuestion_TermQuestion)
        /// </summary>
        public virtual TermQuestion TermQuestion { get; set; } // FK_RegisterQuestion_TermQuestion

        public RegisterQuestion()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
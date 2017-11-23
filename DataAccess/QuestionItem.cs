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

    // QuestionItem
    ///<summary>
    /// جدول گزینه های سوال
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class QuestionItem
    {
        [Column(@"QuestionItemId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        [Display(Name = "Question item ID")]
        public int QuestionItemId { get; set; } // QuestionItemId (Primary key)

        [Required]
        [Display(Name = "Question ID")]
        public int QuestionId { get; set; } // QuestionId

        ///<summary>
        /// نوبت قرارگیری گزینه پاسخ
        ///</summary>
        [Required]
        [Display(Name = "Turn")]
        public int Turn { get; set; } // Turn

        ///<summary>
        /// تستی، تشریحی، تستی همراه تشریحی
        ///</summary>
        [Required]
        [Display(Name = "Question type ID")]
        public int QuestionTypeId { get; set; } // QuestionTypeId

        ///<summary>
        /// متن گزینه
        ///</summary>
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Name = "Text")]
        public string Text { get; set; } // Text (length: 100)

        ///<summary>
        /// تعداد کاراکتر پاسخ
        ///</summary>
        [Display(Name = "Character number")]
        public int? CharacterNumber { get; set; } // CharacterNumber

        ///<summary>
        /// نوع پاسخ (عددی، متنی)
        ///</summary>
        [Display(Name = "Answer type ID")]
        public int? AnswerTypeId { get; set; } // AnswerTypeId

        [Display(Name = "Record status ID")]
        public int? RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child RegisterQuestions where [RegisterQuestion].[QuestionItemId] point to this entity (FK_RegisterQuestion_QuestionItem)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<RegisterQuestion> RegisterQuestions { get; set; } // RegisterQuestion.FK_RegisterQuestion_QuestionItem

        // Foreign keys

        /// <summary>
        /// Parent GeneralBaseType pointed by [QuestionItem].([AnswerTypeId]) (FK_QuestionItem_GeneralBaseType1)
        /// </summary>
        public virtual GeneralBaseType AnswerType { get; set; } // FK_QuestionItem_GeneralBaseType1

        /// <summary>
        /// Parent GeneralBaseType pointed by [QuestionItem].([QuestionTypeId]) (FK_QuestionItem_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType QuestionType { get; set; } // FK_QuestionItem_GeneralBaseType

        /// <summary>
        /// Parent GeneralBaseType pointed by [QuestionItem].([RecordStatusId]) (FK_QuestionItem_GeneralBaseType2)
        /// </summary>
        public virtual GeneralBaseType RecordStatus { get; set; } // FK_QuestionItem_GeneralBaseType2

        /// <summary>
        /// Parent Question pointed by [QuestionItem].([QuestionId]) (FK_QuestionItem_Question)
        /// </summary>
        public virtual Question Question { get; set; } // FK_QuestionItem_Question

        public QuestionItem()
        {
            RegisterQuestions = new System.Collections.Generic.List<RegisterQuestion>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
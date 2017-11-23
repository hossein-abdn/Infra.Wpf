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

    // ClassScore
    ///<summary>
    /// جدول مجموع امتیاز دانش آموزان در یک کلاس
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class ClassScore
    {
        [Column(@"ClassScoreId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        [Display(Name = "Class score ID")]
        public int ClassScoreId { get; set; } // ClassScoreId (Primary key)

        ///<summary>
        /// نوع امتیاز (جایزه، رتبه)
        ///</summary>
        [Display(Name = "Score type ID")]
        public int? ScoreTypeId { get; set; } // ScoreTypeId

        [Display(Name = "Class ID")]
        public int? ClassId { get; set; } // ClassId

        [Display(Name = "Student ID")]
        public int? StudentId { get; set; } // StudentId

        [Display(Name = "Score")]
        public int? Score { get; set; } // Score

        [Display(Name = "Record status ID")]
        public int? RecordStatusId { get; set; } // RecordStatusId

        // Foreign keys

        /// <summary>
        /// Parent Class pointed by [ClassScore].([ClassId]) (FK_ClassScore_Class)
        /// </summary>
        public virtual Class Class { get; set; } // FK_ClassScore_Class

        /// <summary>
        /// Parent GeneralBaseType pointed by [ClassScore].([RecordStatusId]) (FK_ClassScore_GeneralBaseType1)
        /// </summary>
        public virtual GeneralBaseType RecordStatus { get; set; } // FK_ClassScore_GeneralBaseType1

        /// <summary>
        /// Parent GeneralBaseType pointed by [ClassScore].([ScoreTypeId]) (FK_ClassScore_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType ScoreType { get; set; } // FK_ClassScore_GeneralBaseType

        /// <summary>
        /// Parent Student pointed by [ClassScore].([StudentId]) (FK_ClassScore_Student)
        /// </summary>
        public virtual Student Student { get; set; } // FK_ClassScore_Student

        public ClassScore()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
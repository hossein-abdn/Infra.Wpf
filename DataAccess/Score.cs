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

    // Score
    ///<summary>
    /// جدول امتیازات به تفکیک آیتم های امتیازی
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Score
    {
        [Column(@"ScoreId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int ScoreId { get; set; } // ScoreId (Primary key)

        [Required]
        public int ScoreItemId { get; set; } // ScoreItemId

        ///<summary>
        /// غیر اجباری به دلیل آیتم معدل
        ///</summary>
        public int? ClassId { get; set; } // ClassId

        [Required]
        public int StudentId { get; set; } // StudentId

        [Required]
        public int Score_ { get; set; } // Score

        [Required]
        public int TermId { get; set; } // TermId

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Foreign keys

        /// <summary>
        /// Parent Class pointed by [Score].([ClassId]) (FK_Score_Class)
        /// </summary>
        public virtual Class Class { get; set; } // FK_Score_Class

        /// <summary>
        /// Parent GeneralBaseType pointed by [Score].([RecordStatusId]) (FK_Score_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType GeneralBaseType { get; set; } // FK_Score_GeneralBaseType

        /// <summary>
        /// Parent ScoreItem pointed by [Score].([ScoreItemId]) (FK_Score_ScoreItem)
        /// </summary>
        public virtual ScoreItem ScoreItem { get; set; } // FK_Score_ScoreItem

        /// <summary>
        /// Parent Student pointed by [Score].([StudentId]) (FK_Score_Student)
        /// </summary>
        public virtual Student Student { get; set; } // FK_Score_Student

        /// <summary>
        /// Parent Term pointed by [Score].([TermId]) (FK_Score_Term)
        /// </summary>
        public virtual Term Term { get; set; } // FK_Score_Term

        public Score()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

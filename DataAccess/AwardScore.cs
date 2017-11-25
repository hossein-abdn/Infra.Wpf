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

    // AwardScore
    ///<summary>
    /// جدول امتیاز جوایز
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class AwardScore
    {
        [Column(@"AwardScoreId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int AwardScoreId { get; set; } // AwardScoreId (Primary key)

        [Required]
        public int AwardId { get; set; } // AwardId

        [Required]
        public int Score { get; set; } // Score

        [Required]
        public int TermId { get; set; } // TermId

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child StudentAwards where [StudentAward].[AwardScoreId] point to this entity (FK_StudentAward_AwardScore)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<StudentAward> StudentAwards { get; set; } // StudentAward.FK_StudentAward_AwardScore

        // Foreign keys

        /// <summary>
        /// Parent Award pointed by [AwardScore].([AwardId]) (FK_AwardScore_Award)
        /// </summary>
        public virtual Award Award { get; set; } // FK_AwardScore_Award

        /// <summary>
        /// Parent GeneralBaseType pointed by [AwardScore].([RecordStatusId]) (FK_AwardScore_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType GeneralBaseType { get; set; } // FK_AwardScore_GeneralBaseType

        /// <summary>
        /// Parent Term pointed by [AwardScore].([TermId]) (FK_AwardScore_Term)
        /// </summary>
        public virtual Term Term { get; set; } // FK_AwardScore_Term

        public AwardScore()
        {
            StudentAwards = new System.Collections.Generic.List<StudentAward>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

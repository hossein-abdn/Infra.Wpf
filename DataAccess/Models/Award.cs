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


namespace DataAccess.Models
{
    using DataAccess.Mappings;

    // Award
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.29.1.0")]
    public class Award
    {
        public int AwardId { get; set; } // AwardId (Primary key)
        public string Name { get; set; } // Name (length: 30)

        ///<summary>
        /// تعداد جایزه
        ///</summary>
        public int? Count { get; set; } // Count
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child AwardScores where [AwardScore].[AwardId] point to this entity (FK_AwardScore_Award)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<AwardScore> AwardScores { get; set; } // AwardScore.FK_AwardScore_Award
        /// <summary>
        /// Child StudentAwards where [StudentAward].[AwardId] point to this entity (FK_StudentAward_Award)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<StudentAward> StudentAwards { get; set; } // StudentAward.FK_StudentAward_Award

        // Foreign keys

        /// <summary>
        /// Parent GeneralBaseType pointed by [Award].([RecordStatusId]) (FK_Award_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType GeneralBaseType { get; set; } // FK_Award_GeneralBaseType

        public Award()
        {
            Count = 0;
            AwardScores = new System.Collections.Generic.List<AwardScore>();
            StudentAwards = new System.Collections.Generic.List<StudentAward>();
        }
    }

}
// </auto-generated>

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

    // Festival
    ///<summary>
    /// جدول جشنواره
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Festival
    {
        [Column(@"FestivalId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int FestivalId { get; set; } // FestivalId (Primary key)

        [Required]
        [MaxLength(30)]
        [StringLength(30)]
        public string Name { get; set; } // Name (length: 30)

        [MaxLength(100)]
        [StringLength(100)]
        public string Description { get; set; } // Description (length: 100)

        [Required]
        public int ClassSessionId { get; set; } // ClassSessionId

        ///<summary>
        /// رتبه اول
        ///</summary>
        public int? FirstPlaceId { get; set; } // FirstPlaceId

        ///<summary>
        /// رتبه دوم
        ///</summary>
        public int? SecondPlaceId { get; set; } // SecondPlaceId

        ///<summary>
        /// رتبه سوم
        ///</summary>
        public int? ThirdPlaceId { get; set; } // ThirdPlaceId

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Foreign keys

        /// <summary>
        /// Parent ClassSession pointed by [Festival].([ClassSessionId]) (FK_Festival_ClassSession)
        /// </summary>
        public virtual ClassSession ClassSession { get; set; } // FK_Festival_ClassSession

        /// <summary>
        /// Parent GeneralBaseType pointed by [Festival].([RecordStatusId]) (FK_Festival_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType GeneralBaseType { get; set; } // FK_Festival_GeneralBaseType

        /// <summary>
        /// Parent Student pointed by [Festival].([FirstPlaceId]) (FK_Festival_Student)
        /// </summary>
        public virtual Student FirstPlace { get; set; } // FK_Festival_Student

        /// <summary>
        /// Parent Student pointed by [Festival].([SecondPlaceId]) (FK_Festival_Student1)
        /// </summary>
        public virtual Student SecondPlace { get; set; } // FK_Festival_Student1

        /// <summary>
        /// Parent Student pointed by [Festival].([ThirdPlaceId]) (FK_Festival_Student2)
        /// </summary>
        public virtual Student ThirdPlace { get; set; } // FK_Festival_Student2

        public Festival()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

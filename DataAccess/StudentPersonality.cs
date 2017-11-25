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

    // StudentPersonality
    ///<summary>
    /// جدول مشخصات شخصیتی دانش آموز
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class StudentPersonality
    {
        [Column(@"StudentPersonalityId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int StudentPersonalityId { get; set; } // StudentPersonalityId (Primary key)

        [Required]
        public int StudentId { get; set; } // StudentId

        [Required]
        public int TermId { get; set; } // TermId

        [Required]
        public System.DateTime RegisterDate { get; set; } // RegisterDate

        [Required]
        public int UserId { get; set; } // UserId

        [Required]
        [MaxLength(200)]
        [StringLength(200)]
        public string Detail { get; set; } // Detail (length: 200)

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Foreign keys

        /// <summary>
        /// Parent GeneralBaseType pointed by [StudentPersonality].([RecordStatusId]) (FK_StudentPersonality_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType GeneralBaseType { get; set; } // FK_StudentPersonality_GeneralBaseType

        /// <summary>
        /// Parent Student pointed by [StudentPersonality].([StudentId]) (FK_StudentPersonality_Student)
        /// </summary>
        public virtual Student Student { get; set; } // FK_StudentPersonality_Student

        /// <summary>
        /// Parent Term pointed by [StudentPersonality].([TermId]) (FK_StudentPersonality_Term)
        /// </summary>
        public virtual Term Term { get; set; } // FK_StudentPersonality_Term

        /// <summary>
        /// Parent User pointed by [StudentPersonality].([UserId]) (FK_StudentPersonality_User)
        /// </summary>
        public virtual User User { get; set; } // FK_StudentPersonality_User

        public StudentPersonality()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

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

    // Place
    ///<summary>
    /// جدول مکان جلسات
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Place
    {
        [Column(@"PlaceId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int PlaceId { get; set; } // PlaceId (Primary key)

        [Required]
        [MaxLength(30)]
        [StringLength(30)]
        public string Name { get; set; } // Name (length: 30)

        ///<summary>
        /// منزل دانش آموز، غیره
        ///</summary>
        [Required]
        public int PlaceTypeId { get; set; } // PlaceTypeId

        ///<summary>
        /// برای منزل دانش آموز
        ///</summary>
        public int? TermId { get; set; } // TermId

        ///<summary>
        /// جداکننده هیئت، اردو
        ///</summary>
        [Required]
        public int PlaceGroupId { get; set; } // PlaceGroupId

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child Camps where [Camp].[PlaceId] point to this entity (FK_Camp_Place)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Camp> Camps { get; set; } // Camp.FK_Camp_Place
        /// <summary>
        /// Child ReligiousMissions where [ReligiousMission].[PlaceId] point to this entity (FK_ReligiousMission_Place)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<ReligiousMission> ReligiousMissions { get; set; } // ReligiousMission.FK_ReligiousMission_Place

        // Foreign keys

        /// <summary>
        /// Parent GeneralBaseType pointed by [Place].([PlaceGroupId]) (FK_Place_GeneralBaseType1)
        /// </summary>
        public virtual GeneralBaseType PlaceGroup { get; set; } // FK_Place_GeneralBaseType1

        /// <summary>
        /// Parent GeneralBaseType pointed by [Place].([PlaceTypeId]) (FK_Place_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType PlaceType { get; set; } // FK_Place_GeneralBaseType

        /// <summary>
        /// Parent GeneralBaseType pointed by [Place].([RecordStatusId]) (FK_Place_GeneralBaseType2)
        /// </summary>
        public virtual GeneralBaseType RecordStatus { get; set; } // FK_Place_GeneralBaseType2

        /// <summary>
        /// Parent Term pointed by [Place].([TermId]) (FK_Place_Term)
        /// </summary>
        public virtual Term Term { get; set; } // FK_Place_Term

        public Place()
        {
            Camps = new System.Collections.Generic.List<Camp>();
            ReligiousMissions = new System.Collections.Generic.List<ReligiousMission>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

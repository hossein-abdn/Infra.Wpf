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

    // Parent
    ///<summary>
    /// جدول والدین
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Parent
    {
        [Column(@"ParentId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int ParentId { get; set; } // ParentId (Primary key)

        [MaxLength(30)]
        [StringLength(30)]
        public string FirstName { get; set; } // FirstName (length: 30)

        [MaxLength(50)]
        [StringLength(50)]
        public string LastName { get; set; } // LastName (length: 50)

        [MaxLength(30)]
        [StringLength(30)]
        public string Job { get; set; } // Job (length: 30)

        ///<summary>
        /// مواردی که می توانند به پایگاه کمک کنند
        ///</summary>
        [MaxLength(50)]
        [StringLength(50)]
        public string AssistantContext { get; set; } // AssistantContext (length: 50)

        [Required]
        public int StudentId { get; set; } // StudentId

        [Required]
        public bool WithoutParent { get; set; } // WithoutParent

        [Required]
        public int RelationshipId { get; set; } // RelationshipId

        [MaxLength(50)]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } // Email (length: 50)

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child PhoneNumbers where [PhoneNumber].[ParentId] point to this entity (FK_PhoneNumber_Parent)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<PhoneNumber> PhoneNumbers { get; set; } // PhoneNumber.FK_PhoneNumber_Parent
        /// <summary>
        /// Child RollCalls where [RollCall].[ParentId] point to this entity (FK_RollCall_Parent)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<RollCall> RollCalls { get; set; } // RollCall.FK_RollCall_Parent

        // Foreign keys

        /// <summary>
        /// Parent GeneralBaseType pointed by [Parent].([RecordStatusId]) (FK_Parent_GeneralBaseType1)
        /// </summary>
        public virtual GeneralBaseType RecordStatus { get; set; } // FK_Parent_GeneralBaseType1

        /// <summary>
        /// Parent GeneralBaseType pointed by [Parent].([RelationshipId]) (FK_Parent_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType Relationship { get; set; } // FK_Parent_GeneralBaseType

        /// <summary>
        /// Parent Student pointed by [Parent].([StudentId]) (FK_Parent_Student)
        /// </summary>
        public virtual Student Student { get; set; } // FK_Parent_Student

        public Parent()
        {
            WithoutParent = false;
            PhoneNumbers = new System.Collections.Generic.List<PhoneNumber>();
            RollCalls = new System.Collections.Generic.List<RollCall>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

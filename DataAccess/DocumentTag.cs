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

    // DocumentTag
    ///<summary>
    /// جدول تگ های اسناد
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class DocumentTag
    {
        [Column(@"DocumentTagId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        [Display(Name = "Document tag ID")]
        public int DocumentTagId { get; set; } // DocumentTagId (Primary key)

        [Required]
        [Display(Name = "Tag ID")]
        public int TagId { get; set; } // TagId

        [Required]
        [Display(Name = "Document ID")]
        public int DocumentId { get; set; } // DocumentId

        [Required]
        [Display(Name = "Record status ID")]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Foreign keys

        /// <summary>
        /// Parent Document pointed by [DocumentTag].([DocumentId]) (FK_DocumentTag_Document)
        /// </summary>
        public virtual Document Document { get; set; } // FK_DocumentTag_Document

        /// <summary>
        /// Parent GeneralBaseType pointed by [DocumentTag].([RecordStatusId]) (FK_DocumentTag_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType GeneralBaseType { get; set; } // FK_DocumentTag_GeneralBaseType

        /// <summary>
        /// Parent Tag pointed by [DocumentTag].([TagId]) (FK_DocumentTag_Tag)
        /// </summary>
        public virtual Tag Tag { get; set; } // FK_DocumentTag_Tag

        public DocumentTag()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
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

namespace DataAccess.Models
{
    using Mapping;

    // Label
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Label
    {

        ///<summary>
        /// جدول برچسب
        ///</summary>
        [Column(@"LabelId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int LabelId { get; set; } // LabelId (Primary key)

        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "عنوان")]
        public string Title { get; set; } // Title (length: 50)

        [Required]
        public int UserId { get; set; } // UserId

        [Required]
        [Display(Name = "تاریخ ایجاد")]
        public System.DateTime CreateDate { get; set; } // CreateDate

        [Required]
        [Display(Name = "وضعیت")]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child DebtDemandLabels where [DebtDemandLabel].[LabelId] point to this entity (FK_DebtDemandLabel_Label)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<DebtDemandLabel> DebtDemandLabels { get; set; } // DebtDemandLabel.FK_DebtDemandLabel_Label
        /// <summary>
        /// Child LoanLabels where [LoanLabel].[LabelId] point to this entity (FK_LoanLabel_Label)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<LoanLabel> LoanLabels { get; set; } // LoanLabel.FK_LoanLabel_Label
        /// <summary>
        /// Child TransactionLabels where [TransactionLabel].[LabelId] point to this entity (FK_TransactionLabel_Label)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<TransactionLabel> TransactionLabels { get; set; } // TransactionLabel.FK_TransactionLabel_Label

        // Foreign keys

        /// <summary>
        /// Parent User pointed by [Label].([UserId]) (FK_Label_User)
        /// </summary>
        public virtual User User { get; set; } // FK_Label_User

        public Label()
        {
            DebtDemandLabels = new System.Collections.Generic.List<DebtDemandLabel>();
            LoanLabels = new System.Collections.Generic.List<LoanLabel>();
            TransactionLabels = new System.Collections.Generic.List<TransactionLabel>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

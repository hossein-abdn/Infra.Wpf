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

    // Loan
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Loan
    {
        [Column(@"LoanId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int LoanId { get; set; } // LoanId (Primary key)

        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        public string Title { get; set; } // Title (length: 50)

        [Required]
        public int Amount { get; set; } // Amount

        public int? TransactionId { get; set; } // TransactionId

        [Required]
        public System.DateTime CreateDate { get; set; } // CreateDate

        [Required]
        public System.DateTime StartDate { get; set; } // StartDate

        [Required]
        public int PaidInstallment { get; set; } // PaidInstallment

        [Required]
        public int RemainInstallment { get; set; } // RemainInstallment

        [Required]
        public int SettleStatusId { get; set; } // SettleStatusId

        [Required]
        public int PaidSettle { get; set; } // PaidSettle

        [Required]
        public int RemainSettle { get; set; } // RemainSettle

        [Required]
        public int UserId { get; set; } // UserId

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child Installments where [Installment].[LoanId] point to this entity (FK_Installment_Loan)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Installment> Installments { get; set; } // Installment.FK_Installment_Loan
        /// <summary>
        /// Child LoanLabels where [LoanLabel].[LoanId] point to this entity (FK_LoanLabel_Loan)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<LoanLabel> LoanLabels { get; set; } // LoanLabel.FK_LoanLabel_Loan

        // Foreign keys

        /// <summary>
        /// Parent Transaction pointed by [Loan].([TransactionId]) (FK_Loan_Transaction)
        /// </summary>
        public virtual Transaction Transaction { get; set; } // FK_Loan_Transaction

        /// <summary>
        /// Parent User pointed by [Loan].([UserId]) (FK_Loan_User)
        /// </summary>
        public virtual User User { get; set; } // FK_Loan_User

        public Loan()
        {
            PaidInstallment = 0;
            RemainInstallment = 0;
            PaidSettle = 0;
            Installments = new System.Collections.Generic.List<Installment>();
            LoanLabels = new System.Collections.Generic.List<LoanLabel>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
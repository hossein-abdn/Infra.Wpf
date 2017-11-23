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

    // AccountDetail
    ///<summary>
    /// جدول جزئیات حساب
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class AccountDetail
    {
        [Column(@"AccountDetailId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        [Display(Name = "Account detail ID")]
        public int AccountDetailId { get; set; } // AccountDetailId (Primary key)

        [Display(Name = "Account ID")]
        public int? AccountId { get; set; } // AccountId

        ///<summary>
        /// تاریخ صورت حساب
        ///</summary>
        [Display(Name = "Bill date")]
        public System.DateTime? BillDate { get; set; } // BillDate

        ///<summary>
        /// شرح
        ///</summary>
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Name = "Description")]
        public string Description { get; set; } // Description (length: 150)

        ///<summary>
        /// تعداد اقلام
        ///</summary>
        [Required]
        [Display(Name = "Count")]
        public int Count { get; set; } // Count

        ///<summary>
        /// هزینه پایه
        ///</summary>
        [Required]
        [Display(Name = "Base charge")]
        public int BaseCharge { get; set; } // BaseCharge

        ///<summary>
        /// مبلغ طلبکار
        ///</summary>
        [Required]
        [Display(Name = "Creditor")]
        public int Creditor { get; set; } // Creditor

        ///<summary>
        /// مبلغ بدهکار
        ///</summary>
        [Required]
        [Display(Name = "Debtor")]
        public int Debtor { get; set; } // Debtor

        public AccountDetail()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
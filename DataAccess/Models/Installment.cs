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

    // Installment
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Installment : Infra.Wpf.Repository.ModelBase<Installment>
    {

        ///<summary>
        /// جدول اقساط وام
        ///</summary>
        [Column(@"InstallmentId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int InstallmentId { get { return Get<int>(); } set { Set(value); } } // InstallmentId (Primary key)

        [Required]
        public int LoanId { get { return Get<int>(); } set { Set(value); } } // LoanId

        public int? TransactionId { get { return Get<int?>(); } set { Set(value); } } // TransactionId

        [Required]
        [Display(Name = "مبلغ")]
        public int Amount { get { return Get<int>(); } set { Set(value); } } // Amount

        [Required]
        [Display(Name = "وضعیت پرداخت")]
        public int SettleStatusId { get { return Get<int>(); } set { Set(value); } } // SettleStatusId

        public int? NotificationId { get { return Get<int?>(); } set { Set(value); } } // NotificationId

        [Required]
        [Display(Name = "تاریخ قسط")]
        public System.DateTime DueDate { get { return Get<System.DateTime>(); } set { Set(value); } } // DueDate

        [Required]
        [Display(Name = "تاریخ ایجاد")]
        public System.DateTime CreateDate { get { return Get<System.DateTime>(); } set { Set(value); } } // CreateDate

        [Required]
        [Display(Name = "وضعیت")]
        public int RecordStatusId { get { return Get<int>(); } set { Set(value); } } // RecordStatusId

        [Required]
        public int UserId { get { return Get<int>(); } set { Set(value); } } // UserId

        // Foreign keys

        /// <summary>
        /// Parent Loan pointed by [Installment].([LoanId]) (FK_Installment_Loan)
        /// </summary>
        public virtual Loan Loan { get; set; } // FK_Installment_Loan

        /// <summary>
        /// Parent Notification pointed by [Installment].([NotificationId]) (FK_Installment_Notification)
        /// </summary>
        public virtual Notification Notification { get; set; } // FK_Installment_Notification

        /// <summary>
        /// Parent Transaction pointed by [Installment].([TransactionId]) (FK_Installment_Transaction)
        /// </summary>
        public virtual Transaction Transaction { get; set; } // FK_Installment_Transaction

        /// <summary>
        /// Parent User pointed by [Installment].([UserId]) (FK_Installment_User)
        /// </summary>
        public virtual User User { get; set; } // FK_Installment_User

        public Installment()
        {
            Amount = 0;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

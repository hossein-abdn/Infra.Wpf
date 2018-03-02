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

    // User
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class User
    {

        ///<summary>
        /// جدول کاربران
        ///</summary>
        [Column(@"UserId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int UserId { get; set; } // UserId (Primary key)

        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string UserName { get; set; } // UserName (length: 50)

        [Required]
        [MaxLength(60)]
        [StringLength(60)]
        [DataType(DataType.Password)]
        public string Password { get; set; } // Password (length: 60)

        [Required]
        public System.DateTime CreateDate { get; set; } // CreateDate

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child Accounts where [Account].[UserId] point to this entity (FK_Account_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Account> Accounts { get; set; } // Account.FK_Account_User
        /// <summary>
        /// Child DebtDemands where [DebtDemand].[UserId] point to this entity (FK_DebtDemand_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<DebtDemand> DebtDemands { get; set; } // DebtDemand.FK_DebtDemand_User
        /// <summary>
        /// Child Installments where [Installment].[UserId] point to this entity (FK_Installment_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Installment> Installments { get; set; } // Installment.FK_Installment_User
        /// <summary>
        /// Child Labels where [Label].[UserId] point to this entity (FK_Label_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Label> Labels { get; set; } // Label.FK_Label_User
        /// <summary>
        /// Child Loans where [Loan].[UserId] point to this entity (FK_Loan_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Loan> Loans { get; set; } // Loan.FK_Loan_User
        /// <summary>
        /// Child Notes where [Note].[UserId] point to this entity (FK_Note_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Note> Notes { get; set; } // Note.FK_Note_User
        /// <summary>
        /// Child Notifications where [Notification].[UserId] point to this entity (FK_Notification_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Notification> Notifications { get; set; } // Notification.FK_Notification_User
        /// <summary>
        /// Child People where [Person].[UserId] point to this entity (FK_Person_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Person> People { get; set; } // Person.FK_Person_User
        /// <summary>
        /// Child Transactions where [Transaction].[UserId] point to this entity (FK_Transaction_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Transaction> Transactions { get; set; } // Transaction.FK_Transaction_User
        /// <summary>
        /// Child TransactionGroups where [TransactionGroup].[UserId] point to this entity (FK_TransactionGroup_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<TransactionGroup> TransactionGroups { get; set; } // TransactionGroup.FK_TransactionGroup_User
        /// <summary>
        /// Child UserRoles where [UserRole].[UserId] point to this entity (FK_UserRole_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<UserRole> UserRoles { get; set; } // UserRole.FK_UserRole_User

        public User()
        {
            Accounts = new System.Collections.Generic.List<Account>();
            DebtDemands = new System.Collections.Generic.List<DebtDemand>();
            Installments = new System.Collections.Generic.List<Installment>();
            Labels = new System.Collections.Generic.List<Label>();
            Loans = new System.Collections.Generic.List<Loan>();
            Notes = new System.Collections.Generic.List<Note>();
            Notifications = new System.Collections.Generic.List<Notification>();
            People = new System.Collections.Generic.List<Person>();
            Transactions = new System.Collections.Generic.List<Transaction>();
            TransactionGroups = new System.Collections.Generic.List<TransactionGroup>();
            UserRoles = new System.Collections.Generic.List<UserRole>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

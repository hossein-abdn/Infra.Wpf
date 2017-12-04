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

    // Account
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Account
    {
        [Column(@"AccountId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int AccountId { get; set; } // AccountId (Primary key)

        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        public string Title { get; set; } // Title (length: 50)

        [Required]
        public int InitialBalance { get; set; } // InitialBalance

        public int? Balance { get; set; } // Balance

        [Required]
        public int UserId { get; set; } // UserId

        [Required]
        public System.DateTime CreateDate { get; set; } // CreateDate

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child Transactions where [Transaction].[AccountId] point to this entity (FK_Transaction_Account)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Transaction> Transactions { get; set; } // Transaction.FK_Transaction_Account

        // Foreign keys

        /// <summary>
        /// Parent User pointed by [Account].([UserId]) (FK_Account_User)
        /// </summary>
        public virtual User User { get; set; } // FK_Account_User

        public Account()
        {
            InitialBalance = 0;
            Balance = 0;
            Transactions = new System.Collections.Generic.List<Transaction>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    // Person
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Person : Infra.Wpf.Repository.ModelBase<Person>
    {

        ///<summary>
        /// جدول اشخاص
        ///</summary>
        [Column(@"PersonId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int PersonId { get { return Get<int>(); } set { Set(value); } } // PersonId (Primary key)

        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "نام شخص")]
        public string Name { get { return Get<string>(); } set { Set(value); } } // Name (length: 50)

        [Display(Name = "ترتیب نمایش")]
        public int? OrderItem { get { return Get<int?>(); } set { Set(value); } } // OrderItem

        [Required]
        public int UserId { get { return Get<int>(); } set { Set(value); } } // UserId

        [Required]
        [Display(Name = "تاریخ ایجاد")]
        public System.DateTime CreateDate { get { return Get<System.DateTime>(); } set { Set(value); } } // CreateDate

        [Required]
        [Display(Name = "وضعیت")]
        public Enums.RecordStatus RecordStatusId { get { return Get<Enums.RecordStatus>(); } set { Set(value); } } // RecordStatusId

        [NotMapped]
        public bool Marage { get { return Get<bool>(); } set { Set(value); } }

        [NotMapped]
        [Required]
        public List<User> UserList
        {
            get { return Get<List<User>>(); }
            set { Set(value); }
        }



        // Reverse navigation

        /// <summary>
        /// Child DebtDemands where [DebtDemand].[PersonId] point to this entity (FK_DebtDemand_Person)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<DebtDemand> DebtDemands { get; set; } // DebtDemand.FK_DebtDemand_Person
        /// <summary>
        /// Child Transactions where [Transaction].[PersonId] point to this entity (FK_Transaction_Person)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Transaction> Transactions { get; set; } // Transaction.FK_Transaction_Person

        // Foreign keys

        /// <summary>
        /// Parent User pointed by [Person].([UserId]) (FK_Person_User)
        /// </summary>
        public virtual User User { get; set; } // FK_Person_User

        public Person()
        {
            UserList = new List<User>();
            DebtDemands = new System.Collections.Generic.List<DebtDemand>();
            Transactions = new System.Collections.Generic.List<Transaction>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

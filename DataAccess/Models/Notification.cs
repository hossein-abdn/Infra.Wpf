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

    // Notification
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Notification : Infra.Wpf.Repository.ModelBase<Notification>
    {

        ///<summary>
        /// جدول هشدارها
        ///</summary>
        [Column(@"NotificationId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int NotificationId { get { return Get<int>(); } set { Set(value); } } // NotificationId (Primary key)

        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        public string Title { get { return Get<string>(); } set { Set(value); } } // Title (length: 50)

        [Required]
        public System.DateTime NotificationDate { get { return Get<System.DateTime>(); } set { Set(value); } } // NotificationDate

        [Required]
        public int StatusId { get { return Get<int>(); } set { Set(value); } } // StatusId

        [Required]
        public int UserId { get { return Get<int>(); } set { Set(value); } } // UserId

        [Required]
        public System.DateTime CreateDate { get { return Get<System.DateTime>(); } set { Set(value); } } // CreateDate

        [Required]
        public int RecordStatusId { get { return Get<int>(); } set { Set(value); } } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child DebtDemands where [DebtDemand].[NotificationId] point to this entity (FK_DebtDemand_Notification)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<DebtDemand> DebtDemands { get; set; } // DebtDemand.FK_DebtDemand_Notification
        /// <summary>
        /// Child Installments where [Installment].[NotificationId] point to this entity (FK_Installment_Notification)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Installment> Installments { get; set; } // Installment.FK_Installment_Notification
        /// <summary>
        /// Child Notes where [Note].[NotificationId] point to this entity (FK_Note_Notification)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Note> Notes { get; set; } // Note.FK_Note_Notification

        // Foreign keys

        /// <summary>
        /// Parent User pointed by [Notification].([UserId]) (FK_Notification_User)
        /// </summary>
        public virtual User User { get; set; } // FK_Notification_User

        public Notification()
        {
            DebtDemands = new System.Collections.Generic.List<DebtDemand>();
            Installments = new System.Collections.Generic.List<Installment>();
            Notes = new System.Collections.Generic.List<Note>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

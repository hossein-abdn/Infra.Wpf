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

    // User
    ///<summary>
    /// جدول کاربران
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class User
    {
        [Column(@"UserId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int UserId { get; set; } // UserId (Primary key)

        [MaxLength(30)]
        [StringLength(30)]
        public string FirstName { get; set; } // FirstName (length: 30)

        [MaxLength(50)]
        [StringLength(50)]
        public string LastName { get; set; } // LastName (length: 50)

        [Required]
        [MaxLength(30)]
        [StringLength(30)]
        [DataType(DataType.Text)]
        public string UserName { get; set; } // UserName (length: 30)

        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; } // Password (length: 50)

        ///<summary>
        /// فعال بودن حساب کاربری
        ///</summary>
        [Required]
        public bool IsActive { get; set; } // IsActive

        ///<summary>
        /// نقش کاربر در سطح دسترسی
        ///</summary>
        [Required]
        public int RoleId { get; set; } // RoleId

        [Required]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child StudentPersonalities where [StudentPersonality].[UserId] point to this entity (FK_StudentPersonality_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<StudentPersonality> StudentPersonalities { get; set; } // StudentPersonality.FK_StudentPersonality_User
        /// <summary>
        /// Child UserPermissions where [UserPermission].[UserId] point to this entity (FK_UserPermission_User)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<UserPermission> UserPermissions { get; set; } // UserPermission.FK_UserPermission_User

        // Foreign keys

        /// <summary>
        /// Parent GeneralBaseType pointed by [User].([RecordStatusId]) (FK_User_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType GeneralBaseType { get; set; } // FK_User_GeneralBaseType

        /// <summary>
        /// Parent Role pointed by [User].([RoleId]) (FK_User_Role)
        /// </summary>
        public virtual Role Role { get; set; } // FK_User_Role

        public User()
        {
            IsActive = true;
            StudentPersonalities = new System.Collections.Generic.List<StudentPersonality>();
            UserPermissions = new System.Collections.Generic.List<UserPermission>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

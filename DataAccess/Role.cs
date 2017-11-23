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

    // Role
    ///<summary>
    /// جدول نقش ها
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Role
    {
        [Column(@"RoleId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        [Display(Name = "Role ID")]
        public int RoleId { get; set; } // RoleId (Primary key)

        [Required]
        [MaxLength(30)]
        [StringLength(30)]
        [Display(Name = "Name")]
        public string Name { get; set; } // Name (length: 30)

        [Required]
        [Display(Name = "Record status ID")]
        public int RecordStatusId { get; set; } // RecordStatusId

        // Reverse navigation

        /// <summary>
        /// Child RolePermissions where [RolePermission].[RoleId] point to this entity (FK_RolePermission_Role)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<RolePermission> RolePermissions { get; set; } // RolePermission.FK_RolePermission_Role
        /// <summary>
        /// Child Users where [User].[RoleId] point to this entity (FK_User_Role)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<User> Users { get; set; } // User.FK_User_Role

        // Foreign keys

        /// <summary>
        /// Parent GeneralBaseType pointed by [Role].([RecordStatusId]) (FK_Role_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType GeneralBaseType { get; set; } // FK_Role_GeneralBaseType

        public Role()
        {
            RolePermissions = new System.Collections.Generic.List<RolePermission>();
            Users = new System.Collections.Generic.List<User>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
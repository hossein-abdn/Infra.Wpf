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

    // RolePermission
    ///<summary>
    /// جدول سطح دسترسی هر نقش
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class RolePermission
    {
        [Column(@"RolePermissionId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int RolePermissionId { get; set; } // RolePermissionId (Primary key)

        [Required]
        public int RoleId { get; set; } // RoleId

        [Required]
        public int PermissionId { get; set; } // PermissionId

        // Foreign keys

        /// <summary>
        /// Parent Permission pointed by [RolePermission].([PermissionId]) (FK_RolePermission_Permission)
        /// </summary>
        public virtual Permission Permission { get; set; } // FK_RolePermission_Permission

        /// <summary>
        /// Parent Role pointed by [RolePermission].([RoleId]) (FK_RolePermission_Role)
        /// </summary>
        public virtual Role Role { get; set; } // FK_RolePermission_Role

        public RolePermission()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

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

    // Logs
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Log : Infra.Wpf.Repository.ModelBase<Log>
    {
        [Column(@"LogId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int LogId { get { return Get<int>(); } set { Set(value); } } // LogId (Primary key)

        public string CallSite { get { return Get<string>(); } set { Set(value); } } // CallSite

        [MaxLength(20)]
        [StringLength(20)]
        public string Level { get { return Get<string>(); } set { Set(value); } } // Level (length: 20)

        [MaxLength(20)]
        [StringLength(20)]
        public string Type { get { return Get<string>(); } set { Set(value); } } // Type (length: 20)

        public string Message { get { return Get<string>(); } set { Set(value); } } // Message

        public string Exception { get { return Get<string>(); } set { Set(value); } } // Exception

        [MaxLength(50)]
        [StringLength(50)]
        public string PersianDate { get { return Get<string>(); } set { Set(value); } } // PersianDate (length: 50)

        public int? UserId { get { return Get<int?>(); } set { Set(value); } } // UserId

        public System.DateTime? CreateDate { get { return Get<System.DateTime?>(); } set { Set(value); } } // CreateDate

        public Log()
        {
            CreateDate = System.DateTime.Now;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

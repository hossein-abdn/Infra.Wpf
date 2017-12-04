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

    // LoanLabel
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class LoanLabel
    {
        [Column(@"LoanLabelId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int LoanLabelId { get; set; } // LoanLabelId (Primary key)

        [Required]
        public int LoanId { get; set; } // LoanId

        [Required]
        public int LabelId { get; set; } // LabelId

        // Foreign keys

        /// <summary>
        /// Parent Label pointed by [LoanLabel].([LabelId]) (FK_LoanLabel_Label)
        /// </summary>
        public virtual Label Label { get; set; } // FK_LoanLabel_Label

        /// <summary>
        /// Parent Loan pointed by [LoanLabel].([LoanId]) (FK_LoanLabel_Loan)
        /// </summary>
        public virtual Loan Loan { get; set; } // FK_LoanLabel_Loan

        public LoanLabel()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
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

    // Class
    ///<summary>
    /// جدول کلاس
    ///</summary>
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.33.0.0")]
    public partial class Class
    {
        [Column(@"ClassId", Order = 1, TypeName = "int")]
        [Required]
        [Key]
        public int ClassId { get; set; } // ClassId (Primary key)

        [Required]
        public int TermId { get; set; } // TermId

        public int? GroupId { get; set; } // GroupId

        [Required]
        public int TeacherId { get; set; } // TeacherId

        [Required]
        [MaxLength(30)]
        [StringLength(30)]
        public string Name { get; set; } // Name (length: 30)

        ///<summary>
        /// امتیاز کلاس محاسبه شود یا خیر
        ///</summary>
        [Required]
        public bool CalcScore { get; set; } // CalcScore

        ///<summary>
        /// اجباری، اختیاری
        ///</summary>
        [Required]
        public int ClassTypeId { get; set; } // ClassTypeId

        ///<summary>
        /// عمومی، والدین، هیئت، اردو، جشنواره
        ///</summary>
        [Required]
        public int ClassModelId { get; set; } // ClassModelId

        [Required]
        public int RecoredStatusId { get; set; } // RecoredStatusId

        // Reverse navigation

        /// <summary>
        /// Child ClassPoints where [ClassPoint].[ClassId] point to this entity (FK_ClassPoint_Class)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<ClassPoint> ClassPoints { get; set; } // ClassPoint.FK_ClassPoint_Class
        /// <summary>
        /// Child ClassScadules where [ClassScadule].[ClassId] point to this entity (FK_ClassScadule_Class)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<ClassScadule> ClassScadules { get; set; } // ClassScadule.FK_ClassScadule_Class
        /// <summary>
        /// Child ClassScores where [ClassScore].[ClassId] point to this entity (FK_ClassScore_Class)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<ClassScore> ClassScores { get; set; } // ClassScore.FK_ClassScore_Class
        /// <summary>
        /// Child Exams where [Exam].[ClassId] point to this entity (FK_Exam_Class)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Exam> Exams { get; set; } // Exam.FK_Exam_Class
        /// <summary>
        /// Child Scores where [Score].[ClassId] point to this entity (FK_Score_Class)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<Score> Scores { get; set; } // Score.FK_Score_Class
        /// <summary>
        /// Child StudentClasses where [StudentClass].[ClassId] point to this entity (FK_StudentClass_Class)
        /// </summary>
        public virtual System.Collections.Generic.ICollection<StudentClass> StudentClasses { get; set; } // StudentClass.FK_StudentClass_Class

        // Foreign keys

        /// <summary>
        /// Parent GeneralBaseType pointed by [Class].([ClassModelId]) (FK_Class_GeneralBaseType1)
        /// </summary>
        public virtual GeneralBaseType ClassModel { get; set; } // FK_Class_GeneralBaseType1

        /// <summary>
        /// Parent GeneralBaseType pointed by [Class].([ClassTypeId]) (FK_Class_GeneralBaseType)
        /// </summary>
        public virtual GeneralBaseType ClassType { get; set; } // FK_Class_GeneralBaseType

        /// <summary>
        /// Parent GeneralBaseType pointed by [Class].([RecoredStatusId]) (FK_Class_GeneralBaseType2)
        /// </summary>
        public virtual GeneralBaseType RecoredStatus { get; set; } // FK_Class_GeneralBaseType2

        /// <summary>
        /// Parent Group pointed by [Class].([GroupId]) (FK_Class_Group)
        /// </summary>
        public virtual Group Group { get; set; } // FK_Class_Group

        /// <summary>
        /// Parent Teacher pointed by [Class].([TeacherId]) (FK_Class_Teacher)
        /// </summary>
        public virtual Teacher Teacher { get; set; } // FK_Class_Teacher

        /// <summary>
        /// Parent Term pointed by [Class].([TermId]) (FK_Class_Term)
        /// </summary>
        public virtual Term Term { get; set; } // FK_Class_Term

        public Class()
        {
            CalcScore = true;
            ClassPoints = new System.Collections.Generic.List<ClassPoint>();
            ClassScadules = new System.Collections.Generic.List<ClassScadule>();
            ClassScores = new System.Collections.Generic.List<ClassScore>();
            Exams = new System.Collections.Generic.List<Exam>();
            Scores = new System.Collections.Generic.List<Score>();
            StudentClasses = new System.Collections.Generic.List<StudentClass>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>

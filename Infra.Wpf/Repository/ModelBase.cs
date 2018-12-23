using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Infra.Wpf.Common.Helpers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infra.Wpf.Common.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infra.Wpf.Repository
{
    public class ModelBase<T> : INotifyPropertyChanged, INotifyDataErrorInfo, IValidator<T> where T : class
    {
        #region Properties

        private static IEnumerable<PropertyInfo> modelProperties { get; set; }

        private Dictionary<string, object> members = new Dictionary<string, object>();

        internal TrackingCollection<IValidationRule> NestedValidators { get; } = new TrackingCollection<IValidationRule>();

        private static TrackingCollection<IValidationRule> dataAnnotationValidatiors { get; set; }

        private ValidationResult ValidationResult { get; set; } = new ValidationResult();

        private static Func<CascadeMode> s_cascadeMode = () => ValidatorOptions.CascadeMode;
        private Func<CascadeMode> cascadeMode = s_cascadeMode;

        [NotMapped]
        public CascadeMode CascadeMode
        {
            get
            {
                return cascadeMode();
            }
            set
            {
                cascadeMode = () => value;
            }
        }

        public bool HasErrors
        {
            get
            {
                return !ValidationResult?.IsValid ?? false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private bool isValidate { get; set; }

        private bool isConfigDataAnnotation { get; set; }

        private List<string> excludeValidation { get; set; }

        #endregion

        #region Methods

        static ModelBase()
        {
            var type = typeof(T);
            modelProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => x.GetGetMethod().IsVirtual == false);
        }

        private void AddDataAnnotationValidation()
        {
            foreach (var item in modelProperties)
            {
                if (excludeValidation?.Any(x => x == item.Name) ?? false)
                    break;

                var isNullable = Nullable.GetUnderlyingType(item.PropertyType) != null;
                Type type = isNullable == true ? Nullable.GetUnderlyingType(item.PropertyType) : item.PropertyType;
                var isRequired = item.IsRequired();

                var isKey = item.GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.KeyAttribute)) != null;
                if ((isRequired == false && type != typeof(string)) || isKey == true)
                    continue;

                if (type == typeof(DateTime))
                {
                    if (isNullable == true)
                        AddRequiredValidation<DateTime?>(item.Name);
                    else
                        AddRequiredValidation<DateTime>(item.Name);
                }
                else if (type == typeof(TimeSpan))
                {
                    if (isNullable == true)
                        AddRequiredValidation<TimeSpan?>(item.Name);
                    else
                        AddRequiredValidation<TimeSpan>(item.Name);
                }
                else if (type == typeof(bool))
                {
                    if (isNullable == true)
                        AddRequiredValidation<bool?>(item.Name);
                    else
                        AddRequiredValidation<bool>(item.Name);
                }
                else if (type == typeof(char))
                {
                    if (isNullable == true)
                        AddRequiredValidation<char?>(item.Name);
                    else
                        AddRequiredValidation<char>(item.Name);
                }
                else if (type == typeof(long))
                {
                    if (isNullable == true)
                        AddRequiredValidation<long?>(item.Name);
                    else
                        AddRequiredValidation<long>(item.Name);
                }
                else if (type == typeof(ulong))
                {
                    if (isNullable == true)
                        AddRequiredValidation<ulong?>(item.Name);
                    else
                        AddRequiredValidation<ulong>(item.Name);
                }
                else if (type == typeof(int))
                {
                    if (isNullable == true)
                        AddRequiredValidation<int?>(item.Name);
                    else
                        AddRequiredValidation<int>(item.Name);
                }
                else if (type == typeof(uint))
                {
                    if (isNullable == true)
                        AddRequiredValidation<uint?>(item.Name);
                    else
                        AddRequiredValidation<uint>(item.Name);
                }
                else if (type == typeof(short))
                {
                    if (isNullable == true)
                        AddRequiredValidation<short?>(item.Name);
                    else
                        AddRequiredValidation<short>(item.Name);
                }
                else if (type == typeof(ushort))
                {
                    if (isNullable == true)
                        AddRequiredValidation<ushort?>(item.Name);
                    else
                        AddRequiredValidation<ushort>(item.Name);
                }
                else if (type == typeof(byte))
                {
                    if (isNullable == true)
                        AddRequiredValidation<byte?>(item.Name);
                    else
                        AddRequiredValidation<byte>(item.Name);
                }
                else if (type == typeof(sbyte))
                {
                    if (isNullable == true)
                        AddRequiredValidation<byte?>(item.Name);
                    else
                        AddRequiredValidation<byte>(item.Name);
                }
                else if (type == typeof(float))
                {
                    if (isNullable == true)
                        AddRequiredValidation<float?>(item.Name);
                    else
                        AddRequiredValidation<float>(item.Name);
                }
                else if (type == typeof(double))
                {
                    if (isNullable == true)
                        AddRequiredValidation<double?>(item.Name);
                    else
                        AddRequiredValidation<double>(item.Name);
                }
                else if (type == typeof(decimal))
                {
                    if (isNullable == true)
                        AddRequiredValidation<decimal?>(item.Name);
                    else
                        AddRequiredValidation<decimal>(item.Name);
                }
                else if (type == typeof(string))
                {
                    var maxLength = item.GetMaxLength();
                    if (isRequired == true || maxLength != null)
                        AddStringValidation(item.Name, isRequired, maxLength);
                }
                else if (type.IsEnum)
                {
                    if (isNullable == true)
                        AddRequiredValidation<int?>(item.Name);
                    else
                        AddRequiredValidation<int>(item.Name);
                }
            }
        }

        private void AddRequiredValidation<PropType>(string propName)
        {
            var expression = Common.Helpers.DynamicExpression.ParseLambda<T, PropType>(propName);

            var rule = PropertyRule.Create(expression, () => CascadeMode);
            dataAnnotationValidatiors.Add(rule);

            var ruleBuilder = new RuleBuilder<T, PropType>(rule, this);
            ruleBuilder.NotEmpty().WithMessage("وارد کردن «{PropertyName}» اجباری است.");
        }

        private void AddStringValidation(string propName, bool isRequired, int? maxLength)
        {
            var expression = Common.Helpers.DynamicExpression.ParseLambda<T, string>(propName);
            var rule = PropertyRule.Create(expression, () => CascadeMode);
            dataAnnotationValidatiors.Add(rule);

            var ruleBuilder = new RuleBuilder<T, string>(rule, this);
            if (isRequired)
                ruleBuilder.NotEmpty().WithMessage("وارد کردن «{PropertyName}» اجباری است.");
            if (maxLength != null)
                ruleBuilder.MaximumLength(maxLength.Value).WithMessage("طول «{PropertyName}» باید کمتر از {MaxLength} باشد.");
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            if (isValidate)
                ValidateAsync(prop);
        }

        public void OnErrorChanged(string prop)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(prop));
        }

        protected void Set<T>(T value, [CallerMemberName]string prop = null)
        {
            members[prop] = value;
            OnPropertyChanged(prop);
        }

        protected T Get<T>([CallerMemberName]string prop = null)
        {
            object val;
            if (members.TryGetValue(prop, out val))
                return (T)val;

            return default(T);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return ValidationResult?.Errors.Where(x => x.PropertyName == propertyName).ToList();
        }

        public ValidationResult Validate(T instance)
        {
            return DoValidation();
        }

        public ValidationResult Validate(object instance)
        {
            return DoValidation();
        }

        public ValidationResult Validate(ValidationContext context)
        {
            return DoValidation();
        }

        public ValidationResult Validate(string prop = null)
        {
            return DoValidation(prop);
        }

        public Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = default(CancellationToken))
        {
            return Task.Run(() => DoValidation(), cancellation);
        }

        public Task<ValidationResult> ValidateAsync(ValidationContext context, CancellationToken cancellation = default(CancellationToken))
        {
            return Task.Run(() => DoValidation(), cancellation);
        }

        public Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellation = default(CancellationToken))
        {
            return Task.Run(() => DoValidation(), cancellation);
        }

        public Task<ValidationResult> ValidateAsync(string prop = null, CancellationToken cancellation = default(CancellationToken))
        {
            return Task.Run(() => DoValidation(prop), cancellation);
        }

        public void ConfigDataAnnotation()
        {
            if (isConfigDataAnnotation == false)
            {
                isConfigDataAnnotation = true;
                if (dataAnnotationValidatiors == null)
                {
                    dataAnnotationValidatiors = new TrackingCollection<IValidationRule>();
                    AddDataAnnotationValidation();
                }

                foreach (var item in dataAnnotationValidatiors)
                    NestedValidators.Add(item);
            }
        }

        private ValidationResult DoValidation(string prop = null)
        {
            if (isValidate == false)
            {
                isValidate = true;
                ConfigDataAnnotation();
            }

            ValidationContext<T> context = null;
            IEnumerable<ValidationFailure> failures = null;

            if (string.IsNullOrEmpty(prop))
            {
                context = new ValidationContext<T>(this as T, new PropertyChain(), ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory());
                failures = NestedValidators.SelectMany(x => x.Validate(context));
                ValidationResult = new ValidationResult(failures);
                foreach (var item in modelProperties)
                    OnErrorChanged(item.Name);
                return ValidationResult;
            }
            else
            {
                context = new ValidationContext<T>(this as T, new PropertyChain(), ValidatorOptions.ValidatorSelectors.MemberNameValidatorSelectorFactory(new string[] { prop }));
                failures = NestedValidators.SelectMany(x => x.Validate(context));
                ValidationResult = new ValidationResult(failures);
                OnErrorChanged(prop);
                return ValidationResult;
            }
        }

        public IRuleBuilderInitial<T, TProperty> AddRule<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("Cannot pass null to AddRule");

            var rule = PropertyRule.Create(expression, () => CascadeMode);
            NestedValidators.Add(rule);

            var ruleBuilder = new RuleBuilder<T, TProperty>(rule, this);
            return ruleBuilder;
        }

        public void RemoveRule(string prop)
        {
            var propList = NestedValidators.Where(x => (x as PropertyRule).PropertyName == prop).ToList();
            if (propList != null)
            {
                for (int i = propList.Count() - 1; i >= 0; i--)
                    NestedValidators.Remove(propList[i]);
            }
        }

        public void RemoveRule<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var propList = NestedValidators.Where(x => (x as PropertyRule).Expression.GetMember() == expression.GetMember()).ToList();
            if (propList != null)
            {
                for (int i = propList.Count() - 1; i >= 0; i--)
                    NestedValidators.Remove(propList[i]);
            }
        }

        public IValidatorDescriptor CreateDescriptor()
        {
            return new ValidatorDescriptor<T>(NestedValidators);
        }

        public bool CanValidateInstancesOfType(Type type)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }

        public void RuleSet(string ruleSetName, Action action)
        {
            if (string.IsNullOrEmpty(ruleSetName))
                throw new ArgumentNullException("A name must be specified when calling RuleSet.");
            if (action == null)
                throw new ArgumentNullException("A ruleset definition must be specified when calling RuleSet.");

            using (NestedValidators.OnItemAdded(r => r.RuleSet = ruleSetName))
            {
                action();
            }
        }

        public void When(Func<T, bool> predicate, Action action)
        {
            var propertyRules = new List<IValidationRule>();

            Action<IValidationRule> onRuleAdded = propertyRules.Add;

            using (NestedValidators.OnItemAdded(onRuleAdded))
            {
                action();
            }

            propertyRules.ForEach(x => x.ApplyCondition(ctx => predicate((T)ctx.InstanceToValidate)));
        }

        public void Unless(Func<T, bool> predicate, Action action)
        {
            When(x => !predicate(x), action);
        }

        public void Include(IValidator<T> rulesToInclude)
        {
            if (rulesToInclude == null)
                throw new ArgumentNullException("Cannot pass null to Include");
            var rule = IncludeRule.Create<T>(rulesToInclude, () => CascadeMode);
            NestedValidators.Add(rule);
        }

        public void Exclude(string[] propList)
        {
            excludeValidation = new List<string>(propList);
        }

        #endregion
    }
}

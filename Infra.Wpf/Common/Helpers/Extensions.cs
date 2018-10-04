using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Infra.Wpf.Common.Helpers
{
    public static class Extensions
    {
        public static bool Between(this int i, int first, int second)
        {
            if (i >= first && i <= second)
                return true;
            else
                return false;
        }

        public static T Copy<T>(this T @this)
        {
            var sourceType = @this.GetType();
            var sourceProperties = sourceType.GetProperties();
            var copyInstance = Activator.CreateInstance(sourceType);

            foreach (var property in sourceProperties)
                property.SetValue(copyInstance, property.GetValue(@this));

            return (T) copyInstance;
        }

        public static T DeepCopy<T>(this T item)
        {
            Type itemType = typeof(T);
            var mapperExpression = new MapperConfigurationExpression();

            CreateNestedMappers(itemType, mapperExpression);

            var mapperConfiguration = new MapperConfiguration(mapperExpression);

            var mapper = mapperConfiguration.CreateMapper();

            return mapper.Map<T>(item);
        }

        public static void CreateNestedMappers(Type itemType, MapperConfigurationExpression mapperExpression)
        {
            if (mapperExpression.Mappers.Any(x => x.IsMatch(new TypePair(itemType, itemType))) == false)
                mapperExpression.CreateMap(itemType, itemType);

            PropertyInfo[] itemProperties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var itemProperty in itemProperties)
            {
                Type itemPropertyType = itemProperty.PropertyType;
                if (Filter(itemPropertyType) || mapperExpression.Mappers.Any(x => x.IsMatch(new TypePair(itemPropertyType, itemPropertyType))))
                    continue;

                if (itemPropertyType.IsGenericType)
                {
                    Type itemGenericType = itemPropertyType.GetGenericArguments()[0];
                    if (Filter(itemGenericType) || mapperExpression.Mappers.Any(x => x.IsMatch(new TypePair(itemGenericType, itemGenericType))))
                        continue;

                    CreateNestedMappers(itemGenericType, mapperExpression);
                }
                else
                    CreateNestedMappers(itemPropertyType, mapperExpression);
            }
        }

        static bool Filter(Type type)
        {
            return type.IsPrimitive || NoPrimitiveTypes.Contains(type.Name);
        }

        public static readonly HashSet<string> NoPrimitiveTypes = new HashSet<string>() { "String", "DateTime", "Decimal" };

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return (Expression<Func<T, bool>>) DynamicExpression.ParseLambda(typeof(T), typeof(bool), "@0(it) and @1(it)", first, second);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return (Expression<Func<T, bool>>) DynamicExpression.ParseLambda(typeof(T), typeof(bool), "@0(it) or @1(it)", first, second);
        }

        public static Binding Clone(this Binding original)
        {
            var duplicate = new Binding();
            duplicate.BindsDirectlyToSource = original.BindsDirectlyToSource;
            duplicate.Converter = original.Converter;
            duplicate.ConverterCulture = original.ConverterCulture;
            duplicate.ConverterParameter = original.ConverterParameter;
            duplicate.FallbackValue = original.FallbackValue;
            duplicate.IsAsync = original.IsAsync;
            duplicate.Mode = original.Mode;
            duplicate.Path = original.Path;
            duplicate.UpdateSourceTrigger = original.UpdateSourceTrigger;
            duplicate.ValidatesOnDataErrors = true;
            duplicate.ValidatesOnExceptions = true;

            foreach (var rule in original.ValidationRules)
                duplicate.ValidationRules.Add(rule);

            if (original.RelativeSource != null) duplicate.RelativeSource = original.RelativeSource;
            else if (original.ElementName != null) duplicate.ElementName = original.ElementName;
            else if (original.Source != null) duplicate.Source = original.Source;

            return duplicate;
        }

        public static int? GetMaxLength(this PropertyInfo obj)
        {
            if (obj.PropertyType != typeof(string))
                return null;

            var attrib = obj?.GetCustomAttributes(typeof(MaxLengthAttribute), false);
            if (attrib != null && attrib.Count() > 0)
                return ((MaxLengthAttribute) attrib[0]).Length;

            attrib = obj?.GetCustomAttributes(typeof(StringLengthAttribute), false);
            if (attrib != null && attrib.Count() > 0)
                return ((StringLengthAttribute) attrib[0]).MaximumLength;

            return null;
        }

        public static bool IsRequired(this PropertyInfo obj)
        {
            var attrib = obj?.GetCustomAttributes(typeof(RequiredAttribute), false);
            if (attrib != null && attrib.Count() > 0)
                return true;

            return false;
        }

        public static T FindObject<T>(this DependencyObject current) where T : DependencyObject
        {
            foreach (var item in LogicalTreeHelper.GetChildren(current))
            {
                if (item.GetType() == typeof(T))
                    return (T) item;

                if (item is DependencyObject)
                {
                    var result = FindObject<T>((DependencyObject) item);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }
    }
}

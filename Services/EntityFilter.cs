using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
 
using System.Data;
 
namespace MotorMemo.Services 
{
    public class EntityFrameworkFilter<T>
    {
        public IQueryable<T> Filter(IQueryable<T> queryable, List<values> keys, bool OrElse = false)
        {
            IQueryable<T> query = queryable;
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression propertyExpression = parameter;
            Expression? combinedExpression = null;

            foreach (var kvp in keys.Where(w => !string.IsNullOrEmpty(w.Value)))
            {
                if (kvp.Key.Contains("."))
                {
                    var relatedTypes = kvp.Key.Split('.');
                    var relatedPropertyName = FirstCharToUpperStringCreate(relatedTypes[0]);
                    var relatedProperty = typeof(T).GetProperty(relatedPropertyName);

                    if (relatedProperty == null)
                    {
                        throw new ArgumentException($"Property {relatedPropertyName} not found in type {typeof(T)}");
                    }

                    // Create an expression for the related entity
                    propertyExpression = Expression.Property(parameter, relatedPropertyName);

                    for (int i = 1; i < relatedTypes.Length - 1; i++)
                    {
                        var subRelatedPropertyName = relatedTypes[i];
                        var subRelatedProperty = relatedProperty.PropertyType.GetProperty(subRelatedPropertyName);

                        if (subRelatedProperty == null)
                        {
                            throw new ArgumentException($"Property {subRelatedPropertyName} not found in type {relatedProperty.PropertyType}");
                        }

                        // Create an expression for the related subentity
                        propertyExpression = Expression.Property(propertyExpression, subRelatedPropertyName);

                        relatedProperty = subRelatedProperty;
                        relatedPropertyName = subRelatedPropertyName;
                    }
                }

                var columnExpression = Expression.Property(propertyExpression, FirstCharToUpperStringCreate(kvp.Key.Split('.').Last()));
                var propertyType = columnExpression.Type;
                Expression? filterExpression = null;

                if (propertyType == typeof(decimal))
                {
                    decimal result;
                    var value = decimal.TryParse(kvp.Value, out result); // Convert the string value to a decimal value
                    filterExpression = Expression.Equal(columnExpression, Expression.Constant(result));
                }
                else if (propertyType == typeof(int) || propertyType == typeof(int?))
                {
                    int result;
                    if (int.TryParse(kvp.Value, out result)) // Convert the string value to a decimal value
                    {
                        filterExpression = Expression.Equal(columnExpression, Expression.Constant(result));
                    }
                    else
                    {
                        filterExpression = Expression.Constant(false);
                    }
                }
                else if (propertyType == typeof(DateTime))
                {
                    if (DateTime.TryParse(kvp.Value, out var dateTimeValue))
                    {
                        filterExpression = Expression.Equal(columnExpression, Expression.Constant(dateTimeValue, columnExpression.Type));
                    }
                    else
                    {
                        filterExpression = Expression.Constant(false);
                    }
                }


                else if (propertyType == typeof(long) || propertyType == typeof(long?))
                {
                    long result;
                    if (long.TryParse(kvp.Value, out result)) // Convert the string value to a decimal value
                    {
                        filterExpression = Expression.Equal(columnExpression, Expression.Constant(result));
                    }
                    else
                    {
                        filterExpression = Expression.Constant(false);
                    }
                }

                // ... (other data types)

                else if (propertyType == typeof(string))
                {
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var columnToLowerExpression = Expression.Call(columnExpression, toLowerMethod);
                    var filterToLowerExpression = Expression.Constant(kvp.Value.ToLower());
                    filterExpression = Expression.Call(columnToLowerExpression, "Contains", Type.EmptyTypes, filterToLowerExpression);
                }

                if (OrElse)
                {
                    combinedExpression = combinedExpression == null
                        ? filterExpression
                        : Expression.OrElse(combinedExpression, filterExpression);
                }
                else
                {
                    combinedExpression = combinedExpression == null
                        ? filterExpression
                        : Expression.AndAlso(combinedExpression, filterExpression);
                }
            }

            if (combinedExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                query = query.Where(lambda);
            }

            return query;
        }

        private string FirstCharToUpperStringCreate(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return string.Create(input.Length, input, static (Span<char> chars, string str) =>
            {
                chars[0] = char.ToUpperInvariant(str[0]);
                str.AsSpan(1).CopyTo(chars[1..]);
            });
        }
    }
  

    public class values
    {
        public string Key { get; set; } = null!;
        public string? Value { get; set; }

    }
    public class QueryStringParameters
    {
        const int maxPageSize = 25;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public List<values> keys { get; set; } = new List<values>();
    }

    public class PageDetail<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public PageDetail(int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        }
        public static PageDetail<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();

            return new PageDetail<T>(count, pageNumber, pageSize);
        }
    }



}
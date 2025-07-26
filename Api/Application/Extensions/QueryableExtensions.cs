using System.Security.Claims;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using Shared;

namespace Application.Extensions;

public static class QueryableExtensions {
    public static IQueryable<Receipt> ApplyEmployeeReceiptFilter(this IQueryable<Receipt> query, ClaimsPrincipal userInfo) {
        var uploaderUserId = int.Parse(userInfo.FindFirst(ClaimTypes.NameIdentifier).Value);
        var uploaderUserRoleName = userInfo.FindFirst(ClaimTypes.Role).Value;
        if (uploaderUserRoleName == Roles.Employee) {
            query = query.Where(e => e.UploadedByUserId == uploaderUserId);
        }

        return query;
    }

    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, KeyValuePair<string, string>[] filters) {
        if (filters.Length == 0) {
            return query;
        }

        Expression filterPredicate = Expression.Constant(true);
        var variable = Expression.Parameter(typeof(T), "x");

        foreach (var filter in filters) {
            var propertyInfo = typeof(T).GetProperty(filter.Key);
            if (propertyInfo == null) {
                continue;
            }
            var property = Expression.Property(variable, propertyInfo);

            if (propertyInfo.PropertyType == typeof(string)) {
                var value = Expression.Constant(filter.Value, typeof(string));
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var contains = Expression.Call(property, method, value);
                filterPredicate = Expression.AndAlso(filterPredicate, contains);
            }
            else if (propertyInfo.PropertyType == typeof(int)) {
                if (int.TryParse(filter.Value, out var intValue)) {
                    var value = Expression.Constant(intValue, typeof(int));
                    var equals = Expression.Equal(property, value);
                    filterPredicate = Expression.AndAlso(filterPredicate, equals);
                }
            }
            else if (propertyInfo.PropertyType.IsEnum) {
                if (Enum.TryParse(propertyInfo.PropertyType, filter.Value, true, out var enumValue)) {
                    var value = Expression.Constant(enumValue, propertyInfo.PropertyType);
                    var equals = Expression.Equal(property, value);
                    filterPredicate = Expression.AndAlso(filterPredicate, equals);
                }
            }
        }

        var filterLambda = Expression.Lambda<Func<T, bool>>(filterPredicate, variable);
        return query.Where(filterLambda);
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string sortBy, ESortDirection sortDirection) {
        if (string.IsNullOrEmpty(sortBy)) {
            return query;
        }

        var variable = Expression.Parameter(typeof(T), "x");
        var propertyInfo = typeof(T).GetProperty(sortBy);
        if (propertyInfo == null) {
            return query;
        }
        var property = Expression.Property(variable, propertyInfo);
        var conversion = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<T, object>>(conversion, variable);

        if (sortDirection == ESortDirection.Asc) {
            query = query.OrderBy(lambda);
        }
        else if (sortDirection == ESortDirection.Desc) {
            query = query.OrderByDescending(lambda);
        }

        return query;
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int itemsPerPage, int pageNumber) {
        int itemsToSkip = (pageNumber-1) * itemsPerPage;
        return query
            .Skip(itemsToSkip)
            .Take(itemsPerPage);
    }
}

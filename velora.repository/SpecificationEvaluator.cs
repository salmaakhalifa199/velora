﻿using Microsoft.EntityFrameworkCore;
using velora.core.Data;
using velora.core.Entities;
using velora.repository.Specifications;

namespace velora.repository
{
    public static class SpecificationEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery, ISpecifications<TEntity> Spec)
        {
            var Query = InputQuery;
            if (Spec.Criteria is not null)
            {
                Query = Query.Where(Spec.Criteria);
            }

            if (Spec.OrderBy is not null)
                Query = Query.OrderBy(Spec.OrderBy);

            if (Spec.OrderByDescending is not null)
                Query = Query.OrderByDescending(Spec.OrderByDescending);

            if (Spec.IsPaginated)
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);

            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            return Query;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace velora.repository.Specifications
{
    public interface ISpecifications<T> 
    {
        public Expression<Func<T , bool>> Criteria {  get; set; }
        public List<Expression<Func<T , object>>> Includes { get; set; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; }

    }
}

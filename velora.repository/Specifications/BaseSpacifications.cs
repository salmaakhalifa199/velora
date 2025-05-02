using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using velora.core.Data;

namespace velora.repository.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> 
    {
        public BaseSpecifications() { }
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression) 
        {
            Criteria = criteriaExpression;
        }
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public Expression<Func<T, object>> OrderBy { get; private set; }
     
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginated { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> inculdeExpression)
         => Includes.Add(inculdeExpression);

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
         => OrderBy = orderByExpression;
        protected void AddOrderByDes(Expression<Func<T, object>> orderByDesExpression)
         => OrderByDescending = orderByDesExpression;
        protected void ApplyPagination(int skip, int take)
        {
            Take = take;
            Skip = skip;
            IsPaginated = true;
        }


    }

}

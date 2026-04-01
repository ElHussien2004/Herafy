using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class QueryGenerator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity>(IQueryable<TEntity> InputQuery, ISpecifications<TEntity> specifications) where TEntity : class
        {
            var Query = InputQuery;

            if (specifications.Criteria != null)
            {
                Query = Query.Where(specifications.Criteria);
            }
            if (specifications.OrderBy is not null)
            {
                Query = Query.OrderBy(specifications.OrderBy);
            }
            if (specifications.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(specifications.OrderByDescending);
            }

            if (specifications.IncludeExpressions?.Any() == true)
            {
                foreach (var include in specifications.IncludeExpressions)
                {
                    Query = Query.Include(include);
                }
            }


            return Query;
        }
    }
}

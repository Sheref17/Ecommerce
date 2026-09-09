using ECommerce.Application.Abstractions.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery 
            , ISpecification<T> specification , bool applyPaging = true)
        {
            var query = inputQuery;

            if(specification.Criteria is not null) 
                query = query.Where(specification.Criteria);

            if(specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);

            else if(specification.OrderByDescending is not null)
                query = query.OrderByDescending(specification.OrderByDescending);

            if(applyPaging && specification.IsPagingEnabled)
                query = query.Skip(specification.Skip).Take(specification.Take);

            return query;


        }

    }
}

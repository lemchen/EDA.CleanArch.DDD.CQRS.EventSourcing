using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Abstractions.Messages.Queries.Paging;

namespace Application.Abstractions.EventSourcing.Projections;

public interface IProjectionsRepository
{
    Task<TProjection> FindAsync<TProjection>(Expression<Func<TProjection, bool>> predicate, CancellationToken cancellationToken)
        where TProjection : IProjection;

    Task<TProjection> GetAsync<TProjection, TId>(TId id, CancellationToken cancellationToken)
        where TProjection : IProjection;

    Task<IPagedResult<TProjection>> GetAllAsync<TProjection>(IPaging paging, Expression<Func<TProjection, bool>> predicate, CancellationToken cancellationToken)
        where TProjection : IProjection;

    Task<IPagedResult<TProjectionResult>> GetAllNestedAsync<TProjection, TProjectionResult>(IPaging paging, Expression<Func<TProjection, bool>> predicate, Expression<Func<TProjection, IEnumerable<TProjectionResult>>> selector, CancellationToken cancellationToken)
        where TProjection : IProjection
        where TProjectionResult : IProjection;

    Task UpsertAsync<TProjection>(TProjection replacement, CancellationToken cancellationToken)
        where TProjection : IProjection;

    Task DeleteAsync<TProjection>(Expression<Func<TProjection, bool>> filter, CancellationToken cancellationToken)
        where TProjection : IProjection;
}
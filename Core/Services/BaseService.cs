using RentMaster.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentMaster.Core.Services;

public abstract class BaseService<T> where T : class
{
    protected readonly BaseRepository<T> Repository;

    protected BaseService(BaseRepository<T> repository)
    {
        Repository = repository;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Repository.GetAllAsync();
    }

    public virtual async Task<T?> GetByUidAsync(Guid id)
    {
        return await Repository.FindByUidAsync(id);
    }

    public virtual async Task<T> CreateAsync(T model)
    {
        return await Repository.CreateAsync(model);
    }

    public virtual async Task UpdateAsync(T model)
    {
        await Repository.UpdateAsync(model);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await Repository.FindByUidAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"Entity with id {id} not found");
        
        await Repository.DeleteAsync(entity);
    }
}

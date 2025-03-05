﻿using ASC.DataAccess;
using ASC.Model.BaseTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    private DbContext dbContext;

    public Repository(DbContext _dbContext)
    {
        this.dbContext = _dbContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        var entityToInsert = entity as BaseEntity;
        entityToInsert.CreatedDate = DateTime.UtcNow;
        entityToInsert.UpdatedDate = DateTime.UtcNow;
        var result = dbContext.Set<T>().AddAsync(entity).Result;
        return result as T;
    }

    public void Update(T entity)
    {
        var entityToUpdate = entity as BaseEntity;
        entityToUpdate.UpdatedDate = DateTime.UtcNow;
        dbContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        var entityToDelete = entity as BaseEntity;
        entityToDelete.UpdatedDate = DateTime.UtcNow;
        entityToDelete.IsDeleted = true;
        dbContext.Set<T>().Remove(entity);
    }

    public async Task<T> FindAsync(string partitionkey, string rowkey)
    {
        var result = dbContext.Set<T>().FindAsync(partitionkey, rowkey).Result;
        return result as T;
    }

    public async Task<IEnumerable<T>> FindAllByPartitionKeyAsync(string partitionkey)
    {
        var result = dbContext.Set<T>().Where(t => t.PartitionKey == partitionkey).ToListAsync().Result;
        return result as IEnumerable<T>;
    }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        var result = dbContext.Set<T>().ToListAsync().Result;
        return result as IEnumerable<T>;
    }
}
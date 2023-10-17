using System;
namespace asp_net_with_angular_client_template.Repository.Implementations
{
    /// <summary>
    /// Base repository
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
	public interface IBaseRepository<T>
	{
        Task<T> AddAsync(T data);
        Task<T> UpdateAsync(T data);
        Task<bool> DeleteAsync(Guid id);
        Task<T> GetById(Guid id);
        Task<List<T>> GetAll();
        Task SaveAsync();
    }
}


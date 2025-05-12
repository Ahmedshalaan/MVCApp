using Demo.DAL.Entities;

namespace Demo.DAL.Persistence.Repositories._Generic
{
	public interface IGenericRepository<T> where T : ModelBase
	{
		Task<T?> GetAsync(int id);
		Task<IEnumerable<T>> GetAllAsync(bool withAsNoTracking = true);
		IQueryable<T> GetIQueryable();
		IEnumerable<T> GetIEnumerable();
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}

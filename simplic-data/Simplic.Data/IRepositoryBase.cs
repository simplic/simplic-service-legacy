using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    /// <summary>
    /// Simplic repository data base
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="I">Entity key type</typeparam>
    public interface IRepositoryBase<T, I>
    {
        /// <summary>
        /// Get data by id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Instance of <see cref="T"/> if exists</returns>
        T Get(I id);

        /// <summary>
        /// Get all objects
        /// </summary>
        /// <returns>Enumerable of <see cref="T"/></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Create or update data
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <returns>True if successful</returns>
        bool Save(T obj);

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="obj">Object to delete</param>
        /// <returns>True if successful</returns>
        bool Delete(T obj);
    }
}

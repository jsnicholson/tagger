using Microsoft.EntityFrameworkCore;

namespace Data.Repository {
    public class GenericRepository<T> where T : class {
        protected DbContext _context;

        IEnumerable<T> GetAll() {

        }
    }
}
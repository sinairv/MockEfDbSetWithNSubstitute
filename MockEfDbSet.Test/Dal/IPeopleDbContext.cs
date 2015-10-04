using System.Data.Entity;
using System.Threading.Tasks;

namespace MockEfDbSet.Test.Dal
{
    public interface IPeopleDbContext
    {
        DbSet<Person> People { get; }
        Task<int> SaveChangesAsync();
    }
}

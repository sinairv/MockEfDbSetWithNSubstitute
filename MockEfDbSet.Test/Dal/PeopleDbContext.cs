using System.Data.Entity;

namespace MockEfDbSet.Test.Dal
{
    public class PeopleDbContext : DbContext, IPeopleDbContext
    {
        public DbSet<Person> People { get; set; }
    }
}

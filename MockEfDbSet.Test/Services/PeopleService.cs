using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using MockEfDbSet.Test.Dal;

namespace MockEfDbSet.Test.Services
{
    public class PeopleService
    {
        private readonly IPeopleDbContext _peopleDbContext;

        public PeopleService(IPeopleDbContext peopleDbContext)
        {
            if (peopleDbContext == null) 
                throw new ArgumentNullException("peopleDbContext");
            _peopleDbContext = peopleDbContext;
        }

        public Person[] GetAllPeople()
        {
            return _peopleDbContext.People.ToArray();
        }

        public async Task<Person[]> GetAllPeopleAsync()
        {
            return await _peopleDbContext.People.ToArrayAsync();
        }

        public async void AddPersonAsync(Person person)
        {
            _peopleDbContext.People.Add(person);
            await _peopleDbContext.SaveChangesAsync();
        }

        public Person GetPerson(int id)
        {
            return _peopleDbContext.People.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            return await _peopleDbContext.People.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Person GetPersonNoTracking(int id)
        {
            return _peopleDbContext.People.AsNoTracking().FirstOrDefault(p => p.Id == id);
        }

        public async Task<Person> GetPersonNoTrackingAsync(int id)
        {
            return await _peopleDbContext.People.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async void RemovePersonAsync(Person person)
        {
            _peopleDbContext.People.Remove(person);
            await _peopleDbContext.SaveChangesAsync();
        }

        public async Task AddOrUpdatePerson(Person person)
        {
            _peopleDbContext.People.AddOrUpdate(person);
            await _peopleDbContext.SaveChangesAsync();
        }
    }
}

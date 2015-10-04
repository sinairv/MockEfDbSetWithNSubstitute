using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
    }
}

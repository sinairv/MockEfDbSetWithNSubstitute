using NUnit.Framework;
using NSubstitute;
using MockEfDbSet.Test.Dal;
using System.Data.Entity;
using MockEfDbSet.Test.Services;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using MockEfDbSet.Test.Test.Infrastructure;

namespace MockEfDbSet.Test.ServiceTests
{
    [TestFixture]
    public class PeopleServiceTests
    {
        [Test]
        public void AddPerson_AddsThePersonAndCallsSaveChanges()
        {
            // Mocking DbSet for "Add" operations is quite straight-forward

            // Arrange
            var mockSet = Substitute.For<DbSet<Person>>();
            var mockContext = Substitute.For<IPeopleDbContext>();
            mockContext.People.Returns(mockSet);
            var service = new PeopleService(mockContext);

            // Act
            service.AddPersonAsync(new Person { FirstName = "John", LastName = "Doe" });

            // Assert
            // verify that DbSet.Add has been called once
            mockSet.Received(1).Add(Arg.Any<Person>());
            // verify that DbContext.SaveChangesAsync has been called once
            mockContext.Received(1).SaveChangesAsync();
        }
        
        [Test]
        public void GetAllPeople_RetrievesAllPersonRecords()
        {
            // Mocking DbSet for synchronous read operations can be a little tricky

            // Arrange

            // first create the collection of data as an IQueryable
            var data = new List<Person> 
            { 
                new Person { Id = 1, FirstName = "BBB" }, 
                new Person { Id = 2, FirstName = "ZZZ" }, 
                new Person { Id = 3, FirstName = "AAA" }, 
            }.AsQueryable();

            // create a mock DbSet exposing both DbSet and IQueryable interfaces for setup
            var mockSet = Substitute.For<DbSet<Person>, IQueryable<Person>>();

            // setup all IQueryable methods using what you have from "data"
            ((IQueryable<Person>)mockSet).Provider.Returns(data.Provider);
            ((IQueryable<Person>)mockSet).Expression.Returns(data.Expression);
            ((IQueryable<Person>)mockSet).ElementType.Returns(data.ElementType);
            ((IQueryable<Person>)mockSet).GetEnumerator().Returns(data.GetEnumerator());

            // do the wiring between context and DbSet
            var mockContext = Substitute.For<IPeopleDbContext>();
            mockContext.People.Returns(mockSet);
            var service = new PeopleService(mockContext);

            // Act
            var people = service.GetAllPeople();

            // Assert
            Assert.That(people.Length, Is.EqualTo(3));
            Assert.That(people[0].FirstName, Is.EqualTo("BBB"));
            Assert.That(people[1].FirstName, Is.EqualTo("ZZZ"));
            Assert.That(people[2].FirstName, Is.EqualTo("AAA"));
        }

        [Test]
        public async void GetAllPeopleAsync_RetrievesAllPersonRecords()
        {
            // Mocking DbSet for asynchronous read operations is much more tricky
            // This is where we need the classes defined in the "Test.Infrastructure"
            // namespace.

            // Arrange

            // first create the collection of data as an IQueryable
            var data = new List<Person> 
            { 
                new Person { Id = 1, FirstName = "BBB" }, 
                new Person { Id = 2, FirstName = "ZZZ" }, 
                new Person { Id = 3, FirstName = "AAA" }, 
            }.AsQueryable();

            // create a mock DbSet exposing both DbSet, IQueryable, and IDbAsyncEnumerable interfaces for setup
            var mockSet = Substitute.For<DbSet<Person>, IQueryable<Person>, IDbAsyncEnumerable<Person>>();

            // setup all IQueryable and IDbAsyncEnumerable methods using what you have from "data"
            // the setup below is a bit different from the test above
            ((IDbAsyncEnumerable<Person>)mockSet).GetAsyncEnumerator()
                .Returns(new TestDbAsyncEnumerator<Person>(data.GetEnumerator()));
            ((IQueryable<Person>)mockSet).Provider.Returns(new TestDbAsyncQueryProvider<Person>(data.Provider));
            ((IQueryable<Person>)mockSet).Expression.Returns(data.Expression);
            ((IQueryable<Person>)mockSet).ElementType.Returns(data.ElementType);
            ((IQueryable<Person>)mockSet).GetEnumerator().Returns(data.GetEnumerator());

            // do the wiring between context and DbSet
            var mockContext = Substitute.For<IPeopleDbContext>();
            mockContext.People.Returns(mockSet);
            var service = new PeopleService(mockContext);

            // Act
            var people = await service.GetAllPeopleAsync();

            // Assert
            Assert.That(people.Length, Is.EqualTo(3));
            Assert.That(people[0].FirstName, Is.EqualTo("BBB"));
            Assert.That(people[1].FirstName, Is.EqualTo("ZZZ"));
            Assert.That(people[2].FirstName, Is.EqualTo("AAA"));
        }
    }
}

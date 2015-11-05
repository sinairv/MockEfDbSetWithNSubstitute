using System.Collections.Generic;
using System.Linq;
using MockEfDbSet.Test.Dal;
using MockEfDbSet.Test.TestUtils;
using NSubstitute;
using NUnit.Framework;

namespace MockEfDbSet.Test.MiscTests
{
    [TestFixture]
    public class EnumeratorTests
    {
        [Test]
        public void MockDbSetCanBeEnumeratedMoreThanOnce()
        {
            var data = new List<Person>
            {
                new Person { Id = 1, FirstName = "BBB" },
                new Person { Id = 2, FirstName = "ZZZ" },
                new Person { Id = 3, FirstName = "AAA" },
            };

            var mockSet = NSubstituteUtils.CreateMockDbSet(data);
            var mockContext = Substitute.For<IPeopleDbContext>();
            mockContext.People.Returns(mockSet);

            var people1 = mockContext.People.ToArray();
            var people2 = mockContext.People.ToArray();

            Assert.That(people1.Length, Is.EqualTo(3));
            Assert.That(people2.Length, Is.EqualTo(3));
        }

    }
}

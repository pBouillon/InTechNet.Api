using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace InTechNet.UnitTests.Extensions
{
    public static class DbSetExtensions
    {
        public static Mock<DbSet<T>> AsMockedDbSet<T>(this IEnumerable<T> sourceList)
            where T : class
        {
            var queryable = sourceList.AsQueryable();

            var mockedDbSet = new Mock<DbSet<T>>();

            mockedDbSet.As<IQueryable<T>>()
                .Setup(_ => _.Provider)
                .Returns(queryable.Provider);

            mockedDbSet.As<IQueryable<T>>()
                .Setup(_ => _.Expression)
                .Returns(queryable.Expression);

            mockedDbSet.As<IQueryable<T>>()
                .Setup(_ => _.ElementType)
                .Returns(queryable.ElementType);

            mockedDbSet.As<IQueryable<T>>()
                .Setup(_ => _.GetEnumerator())
                .Returns(queryable.GetEnumerator());

            return mockedDbSet;
        }
    }
}

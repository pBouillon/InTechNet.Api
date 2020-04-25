using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace InTechNet.UnitTests.Extensions
{
    /// <summary>
    /// Set of extensions for DbSets
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// Convert an IEnumerable to a mocked DbSet
        /// </summary>
        /// <typeparam name="T">Items of the collection to be mocked</typeparam>
        /// <param name="sourceList">Collection to be mocked</param>
        /// <returns>A mock of the associated DbSet for this collection</returns>
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

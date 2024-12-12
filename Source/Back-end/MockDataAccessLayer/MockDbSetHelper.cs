using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

public static class MockDbSetHelper
{
    public static Mock<DbSet<T>> CreateMockDbSet<T>(IEnumerable<T> sourceData) where T : class
    {
        var queryable = sourceData.AsQueryable();

        var mockDbSet = new Mock<DbSet<T>>();

        // Setup for IQueryable
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);

        // Setup for async enumeration
        mockDbSet.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<System.Threading.CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

        return mockDbSet;
    }
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());

    public T Current => _inner.Current;
}

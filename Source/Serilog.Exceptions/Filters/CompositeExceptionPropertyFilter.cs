namespace Serilog.Exceptions.Filters
{
    using System;

    /// <summary>
    /// Abstraction over collection of filters that filters property is
    /// any of given filters alone would filter it. This is equivalent to
    /// OR over a set of booleans. Executes filters in the order they were
    /// passed to a constructor.
    /// </summary>
    public class CompositeExceptionPropertyFilter : IExceptionPropertyFilter
    {
        private readonly IExceptionPropertyFilter[] filters;

        public CompositeExceptionPropertyFilter(params IExceptionPropertyFilter[] filters)
        {
            if (filters == null)
            {
                throw new ArgumentNullException(
                    nameof(filters),
                    "Cannot create composite exception properties filter, because null collection of filters was given");
            }

            if (filters.Length == 0)
            {
                throw new ArgumentException(
                    "Cannot create composite exception properties filter, because empty collection of filters was given",
                    nameof(filters));
            }

            for (int i = 0; i < filters.Length; i++)
            {
                if (filters[i] == null)
                {
                    throw new ArgumentException(
                        $"Cannot create composite exception properties filter, because filter at index {i} is null",
                        nameof(filters));
                }
            }

            this.filters = filters;
        }

        public bool ShouldPropertyBeFiltered(Exception exception, string propertyName, object value)
        {
            for (int i = 0; i < this.filters.Length; i++)
            {
                if (this.filters[i].ShouldPropertyBeFiltered(exception, propertyName, value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
using System;
using System.Collections.Generic;
public class FilterState<T>
{
    private readonly HashSet<T> _appliedFilters = new();

    public void Toggle(T filter)
    {
        if (!_appliedFilters.Add(filter))
            _appliedFilters.Remove(filter);
    }

    public bool HasAppliedFilters()
    {
        return _appliedFilters.Count > 0;
    }
    public bool IsApplied(T filter)
    => _appliedFilters.Contains(filter);

    public IReadOnlyCollection<T> AppliedFilters => _appliedFilters;
}

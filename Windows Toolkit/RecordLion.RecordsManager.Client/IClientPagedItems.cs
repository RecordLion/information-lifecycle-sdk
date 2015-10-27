using System;
using System.Collections.Generic;

namespace RecordLion.RecordsManager.Client
{
    public interface IClientPagedItems<T>
    {
        int ItemCount { get; }

        IEnumerable<T> Items { get; }

        int Page { get; }

        int PageCount { get; }
    }
}
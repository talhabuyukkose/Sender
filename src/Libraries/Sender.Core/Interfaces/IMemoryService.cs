using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Core.Interfaces
{
    public interface IMemoryService
    {
        ICacheEntry CreateEntry(object key);
        void Dispose();
        void Remove(object key);
        void Set(object key, object value, DateTime timeSpan);
        bool TryGetValue(object key, out object? value);
    }
}

// "NetworkWriterPooled" instead of "PooledNetworkWriter" to group files, for
// easier IDE workflow and more elegant code.
using System;

namespace Mirror
{
    /// <summary>Pooled NetworkWriter, automatically returned to pool when using 'using'</summary>
    public class NetworkWriterPooled : NetworkWriter, IDisposable
    {
        public void Dispose() => NetworkWriterPool.Return(this);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hexagon.Shared.DTOs.Base
{
    public class BaseDto: IDisposable, IAsyncDisposable 
    {
        IDisposable? _disposableResource = new MemoryStream();
        IAsyncDisposable? _asyncDisposableResource = new MemoryStream();


        public string ID { get; set; }
        public string ParentID { get; set; }
        public string Path{ get; set; }
        public bool IdTraslated { get; set; } = false;
        public virtual string Name { get; set; }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposableResource?.Dispose();
                (_asyncDisposableResource as IDisposable)?.Dispose();
                _disposableResource = null;
                _asyncDisposableResource = null;
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_asyncDisposableResource != null)
            {
                await _asyncDisposableResource.DisposeAsync().ConfigureAwait(false);
            }

            if (_disposableResource is IAsyncDisposable disposable)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                _disposableResource?.Dispose();
            }

            _asyncDisposableResource = null;
            _disposableResource = null;
        }
    }
}

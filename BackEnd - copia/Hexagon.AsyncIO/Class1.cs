using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO; 
using System.Text.Json.Serialization;

using System.Text.Json; 
namespace Hexagon.AsyncIO
{


    public class AsyncIO<TEntity> where TEntity : class
    {
        public delegate void EventReadArrayHandler(object sender, EventReadArray e);
        public class EventReadArray : EventArgs
        {
            public TEntity Item
            {
                set; get;
            }
            public bool Cancel { get; set; } = false;
        }
        string FullFileName = null;
        public AsyncIO(string FullFileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(FullFileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FullFileName));
            }
            this.FullFileName = FullFileName;
        }
        public event EventReadArrayHandler ReadArrayElement;

        protected virtual void OnReadArrayElement(EventReadArray e)
        {
            ReadArrayElement?.Invoke(this, e);
        }


        public async Task WriteJsonAsync(TEntity TEntity)
        {
            using var FileStream = new FileStream(FullFileName, FileMode.Create, FileAccess.Write);

            await Task.Run(() =>
            {
                using (var streamWriter = new StreamWriter(FileStream))
                {
                    System.Text.Json.JsonSerializer.SerializeAsync<TEntity>(streamWriter.BaseStream, TEntity);
                };
            });
        }

        public async Task<TEntity> ReadJsonAsync()
        {
            using var FileStream = new FileStream(FullFileName, FileMode.Open, FileAccess.Read );
            TEntity ret  = null ;
            await Task.Run(() =>
            {
                using (var StreamReader = new StreamReader(FileStream))
                {
                    ret = System.Text.Json.JsonSerializer.DeserializeAsync<TEntity>(StreamReader.BaseStream ).Result;
                };
            });
            return ret;
        }

        public async Task<string> ReadJsonPropAsync(string PropertyName, TEntity TEntity)
        {
            using var FileStream = new FileStream(FullFileName, FileMode.Open, FileAccess.Read);
            using var StreamReader = new StreamReader(FileStream);

            using var jsonDocument = await JsonDocument.ParseAsync(StreamReader.BaseStream);
            var jsonElement = jsonDocument.RootElement.GetProperty(PropertyName);
            return jsonElement.GetString();
        }
        public async Task<string> ReadJsonArrayAsync(string PropertyName)
        {
            using var FileStream = new FileStream(FullFileName, FileMode.Open, FileAccess.Read);
            using var StreamReader = new StreamReader(FileStream);
            using var jsonDocument = await JsonDocument.ParseAsync(StreamReader.BaseStream);

            try
            {
                var jsonElement = jsonDocument.RootElement.GetProperty(PropertyName);
                var E = new EventReadArray();
                foreach (var item in jsonElement.EnumerateArray())
                {
                    
                    E.Item = JsonSerializer.Deserialize<TEntity>(item.ToString());
                    OnReadArrayElement(E);
                    if (E.Cancel)
                        break;
                }


            }
            catch (Exception ex)
            {

                throw;
            }

            finally
            {
                jsonDocument.Dispose();
            }
            return "";
        }

    }



    }

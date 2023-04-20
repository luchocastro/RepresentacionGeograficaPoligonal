using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

using System.Text.Json ;
using Hexagon.Model.Models;

namespace Hexagon.Services.Helpers
{
    public delegate void ThresholdReachedEventHandler(object sender, EventArgs e);
    
    public class JsonHelper<TEntity> where TEntity : Base
    {
        public Line Line { get; set; } =new Line();
        public event EventHandler ReadLine;
        public class LineArgs : EventArgs
        { 
            public Line Line { get; set; }
        }
        protected virtual void OnReadLine(EventArgs e)
        {
            ReadLine?.Invoke(this, e);
        }
        public JsonHelper( )
        { 
        }
        public JsonHelper(TEntity TEntity)
        {
            Task.Run(() =>
            Read(TEntity));
            }
        JsonTextReader JSonReader = null;
        public void ProcesAsync(string  PropertyName, TEntity TEntity, Func<bool> func)
        {
            using var FileStream = new FileStream(TEntity.Path, FileMode.Open, FileAccess.Read);
            using var StreamReader = new StreamReader(FileStream);

            using var jsonDocument = JsonDocument.Parse(StreamReader.BaseStream);
            
            var jsonElement = jsonDocument.RootElement.GetProperty(PropertyName);
            if (jsonElement.ValueKind != JsonValueKind.Array) return;

             foreach (JsonElement element in jsonElement.EnumerateArray ())
            {

                
            }
            
        }
        public async Task<string> ReadArrayAsync(string PropertyName, TEntity TEntity )
        {
            using var FileStream = new FileStream(TEntity.Path, FileMode.Open, FileAccess.Read);
            using var StreamReader = new StreamReader(FileStream);

            using var jsonDocument = await JsonDocument.ParseAsync(StreamReader.BaseStream);
            var jsonElement = jsonDocument.RootElement.GetProperty(PropertyName);
            return jsonElement.GetString();
        }
        public async Task<string> ProcesAsync(string PropertyName, TEntity TEntity)
        {
            using var FileStream = new FileStream(TEntity.Path, FileMode.Open, FileAccess.Read);
            using var StreamReader = new StreamReader(FileStream);
            using var jsonDocument = await JsonDocument.ParseAsync(StreamReader.BaseStream);

            try
            {
                var jsonElement = jsonDocument.RootElement;
                EventArgs E = new EventArgs();

                foreach (var item in jsonElement.EnumerateArray())
                {
                    // HACER ALGO
                    this.Line = new Line();
                    OnReadLine(E);
                }
                E = new EventArgs();
                this.Line = new Line();
                OnReadLine(E);

            }
            catch (Exception)
            {

                throw;
            }
           
            finally
            {
                jsonDocument.Dispose();
            }
            return "";
        }
        public async Task Read(TEntity TEntity)
        {
            using var FileStream = new FileStream(TEntity.Path, FileMode.Open, FileAccess.Read);
            using var StreamReader = new StreamReader(FileStream);
            await Task.Run(() =>
          {
              using (var reader = new JsonTextReader(StreamReader))
              {
                  JSonReader = reader;
              };
          });

        }
        public async Task<List<object[]>> ProcesAsync(string [] PropertyNames)
        {
            var ret = new List<object[]>();
            // make sure we are looking at the correct json element
            

            await JSonReader.ReadAsync().ConfigureAwait(false);
            if (JSonReader.TokenType == JsonToken.PropertyName)
            {
                // process properties for data that we want
                while (await JSonReader.ReadAsync().ConfigureAwait(false) &&
                       JSonReader.TokenType != JsonToken.EndObject)
                {
                    var QProperties = 0;
                    foreach (var item in PropertyNames)
                    {


                        if (JSonReader.Value.ToString() == item)
                        {
                            var prop = new object[2];
                            // get the value
                            prop[0] = item;
                            await JSonReader.ReadAsync().ConfigureAwait(false);
                            prop[1] = JSonReader.Value;
                            ret.Add(prop);
                        }
                        QProperties++;
                        
                    }
                    if (PropertyNames.Length == QProperties)
                        break;
                }
            }
            return ret;

        }
        public async Task ProcesAsync(string PropertyName, Func<bool> Function)
        {
            // move to the start of the array
            await JSonReader.ReadAsync().ConfigureAwait(false);

            // process properties for data that we want
            while (await JSonReader.ReadAsync().ConfigureAwait(false) &&
                   JSonReader.TokenType != JsonToken.EndObject)
            {
                // step through each complete object in the Json Array
                if (JSonReader.TokenType == JsonToken.PropertyName)
                {
                    while (await JSonReader.ReadAsync().ConfigureAwait(false) &&
                       JSonReader.TokenType != JsonToken.EndArray)
                    {
                        
                        Function();
                    }
                    break;
                }
            }
        }


        }
        
    
    public static class JsonHelpers
    {
        /// <summary>
        /// Advances the reader to the first child property with the provided name,
        /// no matter how deep it is.
        /// After calling this method, the cursor is on the value node.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="property">The name of the property to search for.</param>
        /// <returns>True if a node of that type was found.</returns>
        public static bool AdvanceTo(this JsonTextReader reader, string property)
        {
            var depth = 0;
            while (reader.Read() && depth >= 0)
            {
                if (reader.TokenType == JsonToken.PropertyName
                    && (string)reader.Value == property)
                {
                    reader.Read();
                    return true;
                }
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                    case JsonToken.StartArray:
                    case JsonToken.StartConstructor:
                        depth++;
                        break;
                    case JsonToken.EndObject:
                    case JsonToken.EndArray:
                    case JsonToken.EndConstructor:
                        depth--;
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Advances the reader to the first child node with the provided type.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="tokenType">The type of token to advance to.</param>
        /// <returns>True if a node of that type was found.</returns>
        public static bool AdvanceTo(this JsonTextReader reader, JsonToken tokenType)
        {
            var depth = 0;
            while (reader.Read() && depth >= 0)
            {
                if (reader.TokenType == tokenType) return true;
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                    case JsonToken.StartArray:
                    case JsonToken.StartConstructor:
                        depth++;
                        break;
                    case JsonToken.EndObject:
                    case JsonToken.EndArray:
                    case JsonToken.EndConstructor:
                        depth--;
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Enumerates the direct children of the current node.
        /// The code using this enumeration must entirely consume the current node
        /// after each move to the next item.
        /// If it doesn't want to do anything with the node, for example, it should
        /// call Skip on the reader.
        /// If the current node is an object, the method enumerates property names.
        /// If the current node is an array, the method advances the reader each time
        /// MoveNext is called, but enumerates null values.
        /// If the current node is neither an object nor an array, the method skips
        /// the node.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>An enumeration of all child property names.</returns>
        public static IEnumerable<string> Children(this JsonTextReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                        {
                            yield break;
                        }
                        if (reader.TokenType == JsonToken.PropertyName)
                        {
                            var propertyName = reader.Value;
                            reader.Read(); // move to the value before yielding.
                            yield return propertyName as string;
                        }
                    }
                    break;
                case JsonToken.StartArray:
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndArray)
                        {
                            yield break;
                        }
                        yield return null;
                    }
                    break;
                default:
                    reader.Skip();
                    reader.Read();
                    yield break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace Extensions.Newtonsoft.Json
{
	public static class JsonSerializerExtensions
	{
		public static Task<T> DeserializeJson<T>(this string data, JsonSerializerSettings settings = null)
		{
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
			return stream.DeserializeJson<T>(settings);
		}

		public static Task<T> DeserializeJson<T>(this Stream stream, JsonSerializerSettings settings = null)
		{
			return Task.Run(() =>
			{
				using (stream)
				using (var streamReader = new StreamReader(stream))
				using (var jsonReader = new JsonTextReader(streamReader))
				{
					var serializer = settings == null ? JsonSerializer.CreateDefault() : JsonSerializer.Create(settings);
					return serializer.Deserialize<T>(jsonReader);
				}
			});
		}

		public static async Task<string> SerializeJson<T>(this T instance, JsonSerializerSettings settings = null)
		{
			using (var stream = new MemoryStream())
			{
				await instance.SerializeJson(stream, settings).ConfigureAwait(false);
				return Encoding.UTF8.GetString(stream.ToArray());
			}
		}

		public static Task SerializeJson<T>(this T instance, Stream toStream, JsonSerializerSettings settings = null)
		{
			return Task.Run(() =>
			{
				using (var streamWriter = new StreamWriter(toStream))
				{
					var serializer = settings == null ? JsonSerializer.CreateDefault() : JsonSerializer.Create(settings);
					serializer.Serialize(streamWriter, instance);
				}
			})                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   ;
		}
	}
}




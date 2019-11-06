using NeonTetra.Contracts;
using NeonTetra.Contracts.Serialization;
using NeonTetra.Core.Serialization.SerializationConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace NeonTetra.Core.Serialization
{
    public class JsonSerializer : ISerializer
    {
        private readonly ContractResolver _contractResolver;
        private readonly Newtonsoft.Json.JsonSerializer _serializer;
        private readonly JsonSerializerSettings _settings;

        public JsonSerializer(IDIContainer container)
        {
            _contractResolver = new ContractResolver(container);

            _settings = new JsonSerializerSettings();
            MutateSerializerSettings(_settings);

            _serializer = Newtonsoft.Json.JsonSerializer.Create(_settings);
        }

        public string ContentType => "application/json";
        public KnownSerializerFormats SerializerFormat => KnownSerializerFormats.JSON;

        public void MutateSerializerSettings(object settings)
        {
            var jsonSettings = settings as JsonSerializerSettings;
            if (jsonSettings == null) return;

            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            if (!jsonSettings.Converters.OfType<StringEnumConverter>().Any())
                jsonSettings.Converters.Add(new StringEnumConverter());
            if (!jsonSettings.Converters.OfType<IPEndPointConverter>().Any())
                jsonSettings.Converters.Add(new IPEndPointConverter());
            if (!jsonSettings.Converters.OfType<IPAddressConverter>().Any())
                jsonSettings.Converters.Add(new IPAddressConverter());

            jsonSettings.ContractResolver = _contractResolver;
        }

        public byte[] Serialize(object entity)
        {
            return System.Text.Encoding.UTF8.GetBytes(SerializeToString(entity));
        }

        public string SerializeToString(object entity)
        {
            return JsonConvert.SerializeObject(entity, _settings);
        }

        public T Deserialize<T>(Stream stream)
        {
            if (stream.CanSeek && stream.Position > 0) stream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);

            return Deserialize<T>(buffer);
        }

        public T Deserialize<T>(byte[] input)
        {
            return Deserialize<T>(System.Text.Encoding.UTF8.GetString(input, 0, input.Length).Replace("\uFEFF", ""));
        }

        public object Deserialize(byte[] input)
        {
            return Deserialize(System.Text.Encoding.UTF8.GetString(input, 0, input.Length));
        }

        public T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, _settings);
        }

        public object Deserialize(string input, Type serviceType)
        {
            return JsonConvert.DeserializeObject(input, serviceType, _settings);
        }

        public object Deserialize(string input)
        {
            return JsonConvert.DeserializeObject(input, _settings);
        }

        public T Deserialize<T>(object input)
        {
            if (input is JObject) return ((JObject)input).ToObject<T>(_serializer);

            if (input is string) return Deserialize<T>((string)input);

            if (input is byte[]) return Deserialize<T>((byte[])input);

            if (input is Stream) return Deserialize<T>((Stream)input);

            throw new InvalidOperationException("Invalid input type: " + input.GetType());
        }
    }
}
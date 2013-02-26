using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ClientTest.Serializers
{
    public class JsonDataContractSerializerTest<T> : SerializerTest<T>
    {
        public override SerializerType SerializerType
        {
            get { return SerializerType.JsonDataContract; }
        }

        public JsonDataContractSerializerTest(T instance, long runs)
            : base(instance, runs)
        {
        }

        public override void Execute()
        {
            // First pass is to get size of serialized data so
            // copy is not included in timed results, and to load serializer
            // into memory so that it's cached.
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, Instance);
                SerializedInstance = stream.ToArray();
            }

            var timer = new Stopwatch();
            timer.Start();
            for (var i = 0; i < Runs; i++)
            {
                using (var stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, Instance);
                }
            }

            timer.Stop();
            var serializationTime = timer.ElapsedMilliseconds;

            // Deserialized once untimed and cast result to DeserializedInstance.
            using (var stream = new MemoryStream(SerializedInstance))
            {
                DeserializedInstance = (T)serializer.ReadObject(stream);
            }

            timer.Reset();
            timer.Start();
            for (var i = 0; i < Runs; i++)
            {
                using (var stream = new MemoryStream(SerializedInstance))
                {
                    serializer.ReadObject(stream);
                }
            }
            timer.Stop();
            SetMetrics(serializationTime, timer.ElapsedMilliseconds);
        }

        protected override string GetSerializationStringData()
        {
            if (SerializedInstance == null)
                throw new InvalidOperationException();
            // Return the JSON string.
            var encoding = new UTF8Encoding(false);
            var strval = encoding.GetString(
                SerializedInstance,
                0,
                SerializedInstance.Length
                );
            return strval;
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ClientTest.Serializers
{
    public class BinaryFormatterSerializerTest<T> : SerializerTest<T>
    {
        public override SerializerType SerializerType
        {
            get { return SerializerType.BinaryFormatter; }
        }

        public BinaryFormatterSerializerTest(T instance, long runs)
            : base(instance, runs)
        {
        }

        public override void Execute()
        {
            // First pass is to get size of serialized data so
            // copy is not included in timed results, and to load serializer
            // into memory so that it's cached.
            var serializer = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, Instance);
                SerializedInstance = stream.ToArray();
            }

            var timer = new Stopwatch();
            timer.Start();
            for (var i = 0; i < Runs; i++)
            {
                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, Instance);
                }
            }

            timer.Stop();
            var serializationTime = timer.ElapsedMilliseconds;

            // Deserialized once untimed and cast result to DeserializedInstance.
            using (var stream = new MemoryStream(SerializedInstance))
            {
                DeserializedInstance = (T)serializer.Deserialize(stream);
            }

            timer.Reset();
            timer.Start();
            for (var i = 0; i < Runs; i++)
            {
                using (var stream = new MemoryStream(SerializedInstance))
                {
                    serializer.Deserialize(stream);
                }
            }
            timer.Stop();
            ReportExecuteTimes(serializationTime, timer.ElapsedMilliseconds);
        }

        protected override string GetSerializationStringData()
        {
            //return "<BINARY DATA>";
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

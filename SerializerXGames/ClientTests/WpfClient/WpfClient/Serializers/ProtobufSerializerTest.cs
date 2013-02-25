using System.Diagnostics;
using System.IO;

namespace ClientTest.Serializers
{
    public class ProtobufSerializerTest<T> : SerializerTest<T>
    {
        public override SerializerType SerializerType
        {
            get { return SerializerType.Protobuf; }
        }

        public ProtobufSerializerTest(T instance, long runs)
            : base(instance, runs)
        {
        }

        public override void Execute()
        {
            // First pass is to get size of serialized data so
            // copy is not included in timed results, and to load serializer
            // into memory so that it's cached.
            using (var stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, Instance);
                SerializedInstance = stream.ToArray();
            }

            var timer = new Stopwatch();
            timer.Start();
            for (var i = 0; i < Runs; i++)
            {
                using (var stream = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize(stream, Instance);
                }
            }

            timer.Stop();
            var serializationTime = timer.ElapsedMilliseconds;

            // Deserialized once untimed and cast result to DeserializedInstance.
            using (var stream = new MemoryStream(SerializedInstance))
            {
                DeserializedInstance = ProtoBuf.Serializer.Deserialize<T>(stream);
            }

            timer.Reset();
            timer.Start();
            for (var i = 0; i < Runs; i++)
            {
                using (var stream = new MemoryStream(SerializedInstance))
                {
                    ProtoBuf.Serializer.Deserialize<T>(stream);
                }
            }
            timer.Stop();
            ReportExecuteTimes(serializationTime, timer.ElapsedMilliseconds);
        }

        protected override string GetSerializationStringData()
        {
            return "<BINARY DATA>";
        }
    }
}

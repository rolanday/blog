using System;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace ClientTest.Serializers
{
    public class JsonNewtonsoftSerializerTest<T> : SerializerTest<T>
    {
        public override SerializerType SerializerType
        {
            get { return SerializerType.JsonNewtonsoft; }
        }
       
        public JsonNewtonsoftSerializerTest(T instance, long runs)
            : base(instance, runs)
        {
        }

        public override void Execute()
        {
            var json = JsonConvert.SerializeObject(Instance);
            SerializedInstance = (new UTF8Encoding(false)).GetBytes(json);
            var timer = new Stopwatch();
            timer.Start();
            for (var i = 0; i < Runs; i++)
            {
                JsonConvert.SerializeObject(Instance);
            }

            timer.Stop();
            var serializationTime = timer.ElapsedMilliseconds;

            DeserializedInstance = JsonConvert.DeserializeObject<T>(json);
            timer.Reset();
            timer.Start();
            for (var i = 0; i < Runs; i++)
            {
                JsonConvert.DeserializeObject<T>(json);
            }
            timer.Stop();
            ReportExecuteTimes(serializationTime, timer.ElapsedMilliseconds);
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

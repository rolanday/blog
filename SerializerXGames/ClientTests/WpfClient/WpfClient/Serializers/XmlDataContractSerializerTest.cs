﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ClientTest.Serializers
{
    public class XmlDataContractSerializerTest<T> : SerializerTest<T>
    {
        public override SerializerType SerializerType
        {
            get { return SerializerType.XmlDataContract;}
        }

        public XmlDataContractSerializerTest(T instance, long runs)
            : base(instance, runs)
        {
        }

        public override void Execute()
        {
            // First pass is to get size of serialized data so
            // copy is not included in timed results, and to load serializer
            // into memory so that it's cached.
            var serializer = new DataContractSerializer(typeof(T));
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

            ReportExecuteTimes(serializationTime, timer.ElapsedMilliseconds);
        }

        protected override string GetSerializationStringData()
        {
            string retval;
            if (SerializedInstance == null)
                throw new InvalidOperationException();
            // Return the XML string.
            using (var stream = new MemoryStream(SerializedInstance))
            {
                retval = XDocument.Load(stream).ToString();
            }

            return retval;
        }
    }
}

﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace ClientTest.Serializers
{
    public abstract class SerializerTest<T> : NotifyBase
    {
        public abstract SerializerType SerializerType { get; }

        private readonly T _instance;
        public T Instance { get { return _instance; } }

        private T _deserializedInstance;
        public T DeserializedInstance
        {
            get { return _deserializedInstance; }
            protected set { SetProperty(ref _deserializedInstance, value, "DeserializedInstance"); }
        }

        private readonly long _runs;
        public long Runs
        {
            get { return _runs; }
        }

        private long _serializationTime;
        public long SerializationTime
        {
            get { return _serializationTime; }
            set { SetProperty(ref _serializationTime, value, "SerializationTime"); }
        }

        private long _deserializationTime;
        public long DeserializationTime
        {
            get { return _deserializationTime; }
            set { SetProperty(ref _deserializationTime, value, "DeserializationTime"); }
        }

        private byte[] _serializedInstance;
        public byte[] SerializedInstance
        {
            get { return _serializedInstance; }
            protected set
            {
                SetProperty(ref _serializedInstance, value, "SerializedInstance");
                // Also notify that SerializedDataLength has changed which
                // is a shortcut to this props length val.
                OnPropertyChanged("SerializedDataLength");

                _serializedDataStringView = GetSerializationStringData();
                OnPropertyChanged("SerializedDataStringView");
            }            
        }

        private string _serializedDataStringView;
        public string SerializedDataStringView
        {
            get { return _serializedDataStringView; }
        }

        public long SerializedDataLength
        {
            get { return _serializedInstance == null ? 0 : _serializedInstance.Length; }
        }

        private long _compressedLength;
        public long CompressedLength
        {
            get { return _compressedLength; }
            set { SetProperty(ref _compressedLength, value, "CompressedLength"); }
        }

        /// <summary>
        /// Executes serialization performance tests.
        /// </summary>
        public abstract void Execute();

        protected abstract string GetSerializationStringData();


        protected void SetMetrics(long serializationTime, long deserializationTime)
        {
            SerializationTime = serializationTime;
            DeserializationTime = deserializationTime;

            Debug.Assert(SerializedInstance != null && SerializedInstance.Length > 0);
            // Compress data
            using (var stream = new MemoryStream())
            {
                using (var gzip = new GZipStream(stream, CompressionMode.Compress, true))
                {
                    gzip.Write(SerializedInstance, 0, SerializedInstance.Length);
                }
                CompressedLength = stream.Length;
            }
        }

        protected SerializerTest(T instance, long runs)
        {
            if (ReferenceEquals(instance, null))
                throw new ArgumentNullException("instance");
            if (runs < 1)
                throw new ArgumentException("Value must be greater than 0.", "runs");
            _instance = instance;
            _runs = runs;
        }
    }

    public enum SerializerType
    {
        BinaryFormatter,
        JsonDataContract,
        JsonNewtonsoft,
        Protobuf,
        Xml,
        XmlDataContract
    }
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace ClientTest.Serializers
{
    /// <summary>
    /// To get an properties in view w/o needing to specify type thereby
    /// minimizing edits to change class being tested.
    /// </summary>
    public interface IDataView
    {
        byte[] SerializedInstance { get; }
        SerializerType SerializerType { get; }
        string SerializedDataStringView { get; }
        object DeserializedObject { get; }
    };

    public abstract class SerializerTest<T> : NotifyBase, IDataView
    {
        public abstract SerializerType SerializerType { get; }

        private readonly T _instance;
        [Browsable(false)]
        public T Instance { get { return _instance; } }

        private T _deserializedInstance;
        [Browsable(false)]
        public T DeserializedInstance
        {
            get { return _deserializedInstance; }
            protected set { SetProperty(ref _deserializedInstance, value, "DeserializedInstance"); }
        }

        [Browsable(false)]
        public object DeserializedObject { get { return DeserializedInstance; } }

        private readonly long _runs;
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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

        private readonly string _className;
        [Browsable(false)]
        public string ClassName { get { return _className; } }

        protected SerializerTest(T instance, long runs)
        {
            if (ReferenceEquals(instance, null))
                throw new ArgumentNullException("instance");
            if (runs < 1)
                throw new ArgumentException("Value must be greater than 0.", "runs");
            _instance = instance;
            _runs = runs;
            _className = typeof(T).Name;
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

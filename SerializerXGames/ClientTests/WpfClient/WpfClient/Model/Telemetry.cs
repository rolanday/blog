using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ClientTest.Model
{
    [Serializable]
    [DataContract]
    [ProtoInclude(1, typeof(Speed))]
    [ProtoInclude(2, typeof(Temperature))]
    public class Telemetry
    {
        [DataMember(Order = 3)]
        public long TimeStamp { get; set; }
        protected Telemetry() {}
        protected Telemetry(long timestamp)
        {
            TimeStamp = timestamp;
        }
    }
}

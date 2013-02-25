using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ClientTest.Model
{
    [Serializable]
    [ProtoContract]
    [DataContract]
    [ProtoInclude(1, typeof(Employee))]    
    public class Citizen : Human
    {
        [DataMember(Order = 2)]
        public string Nationality { get; set; }
        [DataMember(Order = 3)]
        public string NationalId { get; set; }
        [DataMember(Order = 4)]
        public string Address { get; set; }
    }
}

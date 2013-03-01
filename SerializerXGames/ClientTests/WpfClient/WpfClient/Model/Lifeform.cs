using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ClientTest.Model
{
    [Serializable]
    [DataContract]
    [ProtoInclude(1, typeof(Human))]
    public abstract class Lifeform
    {
        public abstract Kingdom Kingdom { get; }
    }

    public enum Kingdom
    {
        Animal,
        Archaebacteria,
        Eubacteria,
        Fungi,
        Plant,
        Protists
    }
}

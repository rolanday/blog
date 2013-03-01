using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ClientTest.Model
{
    [Serializable]
    [DataContract]
    public class Relation : Human
    {
        public Relationship Relationship { get; set; }
    }

    public enum Relationship
    {
        Child,
        Cousin,
        Friend,
        Parent,
        Sibling,
        Spouse
    }
}

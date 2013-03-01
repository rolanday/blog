using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ClientTest.Model
{
    [Serializable]
    [DataContract]
    [ProtoInclude(1, typeof(Citizen))]
    [ProtoInclude(2, typeof(Relation))]    
    public class Human : Lifeform
    {
        // DataContract serializer requires collection to be setable
        // so makeing backing property read-write and implementing
        // setter. Xml, Json, and Proto serializers behave differently
        // and populate the collection as opposed to rehydrating and
        // setting.
        //private readonly Collection<Relation> _relationships = new Collection<Relation>(); 
        private Collection<Relation> _relationships = new Collection<Relation>(); 
        public override Kingdom Kingdom
        {
            get { return Kingdom.Animal; }
        }

        [DataMember(Order = 3)]
        public string Name { get; set; }
        [DataMember(Order = 4)]
        public Sex Sex { get; set; }
        [DataMember(Order = 5)]
        public ushort Height { get; set; }
        [DataMember(Order = 6)]
        public ushort Weight { get; set; }
        [DataMember(Order = 7)]
        public Color EyeColor { get; set; }
        [DataMember(Order = 8)]
        public Color HairColor { get; set; }
        [DataMember(Order = 9)]
        public long DateOfBirth { get; set; }
        [DataMember(Order = 10)]
        // See comment above.
        //public Collection<Relation> Relationships { get { return _relationships; } } 
        public Collection<Relation> Relationships
        {
            get { return _relationships; }
            set { _relationships = value; } // For data contract serializer.
        } 
    }

    public enum Sex
    {
        Male,
        Female
    }

    public enum Color
    {
        Black,
        Blue,
        Brown,
        Gray,
        Green,
        Hazel,
        White
    }
}

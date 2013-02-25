using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace ClientTest.Model
{
    [Serializable]
    [ProtoContract]
    [DataContract]
    public class Employee : Citizen
    {
        [DataMember(Order = 1)]
        public string Employer { get; set; }
        [DataMember(Order = 2)]
        public string EmployerAddress { get; set; }
        [DataMember(Order = 3)]
        public string EmployeeId { get; set; }
        [DataMember(Order = 4)]
        public string Occupation { get; set; }
        [DataMember(Order = 5)]
        public string Department { get; set; }
        [DataMember(Order = 6)]
        public long StartDate { get; set; }
        [DataMember(Order = 7)]
        public EmploymentStatus EmploymentStatus { get; set; }

        public static Employee JamesTKirk
        {
            get
            {
                return new Employee
                           {
                               Name = "James Tiberius Kirk",
                               Sex = Sex.Male,
                               EyeColor = Color.Hazel,
                               HairColor = Color.Brown,
                               Height = 178,
                               Weight = 78,
                               DateOfBirth = new DateTime(2228, 03, 22).Ticks,
                               Address = "361 E. 1st St., Riverside, IA",
                               Nationality = "United Federation of Planets",
                               Employer = "Starfleet Command",
                               StartDate = new DateTime(2250, 1, 1).Ticks,
                               EmployerAddress = "San Francisco, CA",
                               EmployeeId = "SC937-0176CEC",
                               Department = "Enterprise",
                               Occupation = "Starfleet officer",
                               EmploymentStatus = EmploymentStatus.Active,
                               Relationships =
                                   {
                                       new Relation
                                           {
                                               Name = "George Kirk",
                                               Relationship = Relationship.Parent,
                                               Sex = Sex.Male
                                           }
                                   }
                           };
            }
        }

    }

    public enum EmploymentStatus
    {
        Active,
        Furlough,
        LeaveOfAbsence,
        Terminiated
    }
}

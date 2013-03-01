using System;
using System.Runtime.Serialization;

namespace ClientTest.Model
{
    [Serializable]
    [DataContract]
    public class Speed : Telemetry
    {
        [DataMember(Order = 1)]
        public SpeedMeasurement Measurement { get; set; }
        [DataMember(Order = 2)]
        public long Value { get; set; }

        public Speed() {}

        public Speed(SpeedMeasurement measurement, long value, long timestamp)
            : base(timestamp)
        {
            Measurement = measurement;
            Value = value;
        }
    }

    public enum SpeedMeasurement
    {
        Engine,
        Wheel,
        Vehicle
    }

}

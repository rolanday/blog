using System;
using System.Runtime.Serialization;

namespace ClientTest.Model
{
    [Serializable]
    [DataContract]
    public class Temperature : Telemetry
    {
        [DataMember(Order = 1)]
        public TemperatureMeasurement Measurement { get; set; }
        [DataMember(Order = 2)]
        public long Value { get; set; }

        public Temperature() {}
        public Temperature(TemperatureMeasurement measurement, long value, long timestamp)
            : base(timestamp)
        {
            Measurement = measurement;
            Value = value;
        }
    }

    public enum TemperatureMeasurement
    {
        Fuel,
        Oil,
        Water,
        Exhaust
    }
}

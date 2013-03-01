using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace ClientTest.Model
{
    [Serializable]
    [DataContract]
    public class TelemetryLog
    {
        private Collection<Speed> _speeds = new Collection<Speed>();
        private Collection<Temperature> _temps = new Collection<Temperature>();

        [DataMember(Order = 1)]
        public DateTime EventTime { get; set; }

        [DataMember(Order = 2)]
        public Collection<Speed> Speeds
        {
            get { return _speeds; }
            set { _speeds = value; }
        }

        [DataMember(Order = 3)]
        public Collection<Temperature> Temps
        {
            get { return _temps; }
            set { _temps = value; }
        }

        public static TelemetryLog SampleData
        {
            get
            {
                var self = new TelemetryLog { EventTime = DateTime.UtcNow };
                const int samples = 10;
                var rg = new Random(0);
                for (var i = 0; i < samples; i++)
                {
                    // Just some random data that makes no sense in reality
                    var speed = new Speed(SpeedMeasurement.Engine, rg.Next(0, 10000), DateTime.UtcNow.Ticks);
                    self._speeds.Add(speed);
                    speed = new Speed(SpeedMeasurement.Vehicle, rg.Next(0, 200), DateTime.UtcNow.Ticks);
                    self._speeds.Add(speed);
                    speed = new Speed(SpeedMeasurement.Wheel, rg.Next(0, 3000), DateTime.UtcNow.Ticks);
                    self._speeds.Add(speed);
                    var temperature = new Temperature(TemperatureMeasurement.Exhaust, rg.Next(0, 1000), DateTime.UtcNow.Ticks);
                    self._temps.Add(temperature);
                    temperature = new Temperature(TemperatureMeasurement.Fuel, rg.Next(0, 300), DateTime.UtcNow.Ticks);
                    self._temps.Add(temperature);
                    temperature = new Temperature(TemperatureMeasurement.Oil, rg.Next(0, 300), DateTime.UtcNow.Ticks);
                    self._temps.Add(temperature);
                    temperature = new Temperature(TemperatureMeasurement.Water, rg.Next(0, 300), DateTime.UtcNow.Ticks);
                    self._temps.Add(temperature);
                }

                return self;
            }
        }
    }
}

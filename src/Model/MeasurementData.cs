using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Netatmo.Net.Model
{
    public class MeasurementData
    {
        [JsonProperty(PropertyName = "body")]
        public dynamic Data { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "time_exec")]
        public float TimeExec { get; set; }

        [JsonProperty(PropertyName = "time_server")]
        public long TimeServer { get; set; }

        [JsonIgnore]
        public List<Measurement> Measurements;

        public void CreateMeasurementData(bool optimized, MeasurementType[] measurementTypes)
        {
            Measurements = new List<Measurement>();

            if (optimized)
            {
                var array = Data as JArray;
                if (array == null)
                    throw new Exception("Unexpected JSON format");

                foreach (var a in array)
                {
                    var begintime = (long)JsonConvert.DeserializeObject(a["beg_time"].ToString(), typeof(long));
                    long stepTime = 0;

                    if (a["step_time"] != null) //contains does somehow not seem to work here
                    {
                        stepTime = (long)JsonConvert.DeserializeObject(a["step_time"].ToString(), typeof(long));
                    }

                    var valueSteps = a["value"] as JArray;
                    for (var i = 0; i < valueSteps.Count; i++)
                    {
                        var valueArray = (double?[])JsonConvert.DeserializeObject(valueSteps[i].ToString(), typeof(double?[]));
                        var measurementValues = new List<MeasurementValue>();

                        for (var j = 0; j < measurementTypes.Count(); j++)
                        {
                            var value = new MeasurementValue {Type = measurementTypes[j], Value = valueArray[j]};
                            measurementValues.Add(value);
                        }

                        var measurement = new Measurement(begintime + (stepTime * i), measurementValues);
                        Measurements.Add(measurement);
                    }
                }
            }
            else
            {
                var dict = (Dictionary<long, double?[]>)JsonConvert.DeserializeObject(Data.ToString(), typeof(Dictionary<long, double?[]>));
                foreach (var entry in dict)
                {
                    var measurementValues = new List<MeasurementValue>();

                    for (var j = 0; j < measurementTypes.Count(); j++)
                    {
                        var value = new MeasurementValue { Type = measurementTypes[j], Value = entry.Value[j] };
                        measurementValues.Add(value);
                    }

                    var measurement = new Measurement(entry.Key, measurementValues);
                    Measurements.Add(measurement);
                }
            }
        }
    }
}

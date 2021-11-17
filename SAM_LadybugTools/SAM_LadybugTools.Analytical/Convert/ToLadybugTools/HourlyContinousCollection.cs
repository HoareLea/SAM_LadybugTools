extern alias SAM_Newtonsoft;

using SAM_Newtonsoft::Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static string ToLadybugTools_HourlyContinousCollection(this Profile profile, Dictionary<string, object> metadata = null)
        {
            if (profile == null)
                return null;

            string name = profile.Name;

            double[] values = null;

            int endHour = 23;
            int endDay = -1;
            int endMonth = -1;

            int count = profile.Count;
            if (count <= 24)
            {
                values = profile.GetDailyValues();
                endDay = 1;
                endMonth = 1;
            }
            else
            {
                values = profile.GetYearlyValues();
                endDay = 31;
                endMonth = 12;
            }
                

            if (values == null)
                return null;

            return ToLadybugTools_HourlyContinousCollection(values, profile.ProfileGroup, name, 0, 1, 1, endHour, endDay, endMonth, 1, false, metadata);

        }

        public static string ToLadybugTools_HourlyContinousCollection(double[] values, ProfileGroup profileGroup, string name, int startHour, int startDay,int startMonth, int endHour, int endDay, int endMonth, int timestep, bool leapYear, Dictionary<string, object> metadata = null)
        {
            if (values == null)
                return null;

            JObject result = new JObject();
            result.Add("values", new JArray(values));

            JObject jObject_Header = new JObject();
            
            if(metadata != null)
            {
                JObject jObject_Metadata = new JObject();
                foreach (KeyValuePair<string, object> keyValuePair in metadata)
                    jObject_Metadata.Add(keyValuePair.Key, keyValuePair.Value as dynamic);

                jObject_Header.Add("metadata", jObject_Metadata);
            }

            JObject jObject_AnalysisPeriod = new JObject();
            jObject_AnalysisPeriod.Add("st_day", startDay);
            jObject_AnalysisPeriod.Add("end_month", endMonth);
            jObject_AnalysisPeriod.Add("st_hour", startHour);
            jObject_AnalysisPeriod.Add("end_hour", endHour);
            jObject_AnalysisPeriod.Add("timestep", timestep);
            jObject_AnalysisPeriod.Add("is_leap_year", leapYear);
            jObject_AnalysisPeriod.Add("st_month", startMonth);
            jObject_AnalysisPeriod.Add("end_day", endDay);
            jObject_AnalysisPeriod.Add("type", "AnalysisPeriod");

            jObject_Header.Add("analysis_period", jObject_AnalysisPeriod);

            string unit = null;
            string dataType = null;
            switch (profileGroup)
            {
                case ProfileGroup.Gain:
                    unit = "W";
                    dataType = "Power";
                    break;

                case ProfileGroup.Humidistat:
                    unit = "unknown";
                    dataType = "GenericType";
                    break;

                case ProfileGroup.Thermostat:
                    unit = "C";
                    dataType = "Temperature"; 
                    break;

                default:
                    return null;
            }

            jObject_Header.Add("unit", unit);
            jObject_Header.Add("type", "Header");

            JObject jObject_DataType = new JObject();
            jObject_DataType.Add("base_unit", unit);
            jObject_DataType.Add("name", name);
            jObject_DataType.Add("type", "DataTypeBase");
            jObject_DataType.Add("data_type", dataType);
            if(profileGroup == ProfileGroup.Humidistat)
            {
                jObject_DataType.Add("min", double.MinValue);
                jObject_DataType.Add("max", double.MaxValue);
            }
            jObject_DataType.Add("point_in_time", true);
            jObject_DataType.Add("cumulative", false);
            jObject_DataType.Add("abbreviation", "Unknown Data Type");
            jObject_DataType.Add("unit_descr", null);

            jObject_Header.Add("data_type", jObject_DataType);

            result.Add("header", jObject_Header);
            result.Add("type", "HourlyContinuousCollection");

            return result.ToString();
        }
    }
}
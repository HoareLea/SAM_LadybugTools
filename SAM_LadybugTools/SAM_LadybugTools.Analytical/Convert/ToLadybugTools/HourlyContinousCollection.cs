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

            string unit = null;
            string name = profile.Name;
            string dataType = "GenericType";
            switch(profile.ProfileGroup)
            {
                case ProfileGroup.Gain:
                    unit = "W";
                    break;
                
                case ProfileGroup.Humidistat:
                    unit = "-";
                    break;
                
                case ProfileGroup.Thermostat:
                    unit = "C";
                    break;

                default:
                    return null;
            }

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

            return ToLadybugTools_HourlyContinousCollection(values, unit, name, dataType, 0, 1, 1, endHour, endDay, endMonth, 1, false, metadata);

        }

        public static string ToLadybugTools_HourlyContinousCollection(double[] values, string unit, string name, string dataType, int startHour, int startDay,int startMonth, int endHour, int endDay, int endMonth, int timestep, bool leapYear, Dictionary<string, object> metadata = null)
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

            jObject_Header.Add("unit", unit);
            jObject_Header.Add("type", "Header");

            JObject jObject_DataType = new JObject();
            jObject_DataType.Add("base_unit", "C");
            jObject_DataType.Add("name", name);
            jObject_DataType.Add("type", "DataTypeBase");
            jObject_DataType.Add("data_type", dataType);

            jObject_Header.Add("data_type", jObject_DataType);

            result.Add("header", jObject_Header);
            result.Add("type", "HourlyContinuousCollection");

            return result.ToString();
        }
    }
}
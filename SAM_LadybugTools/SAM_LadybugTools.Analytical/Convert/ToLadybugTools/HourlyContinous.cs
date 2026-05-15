using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static string ToLadybugTools_HourlyContinous(this Profile profile, Dictionary<string, object> metadata = null)
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

            ProfileGroup profileGroup = profile.ProfileGroup;
            if(profileGroup == ProfileGroup.Undefined)
            {
                profileGroup = profile.ProfileType.ProfileGroup();
            }

            return ToLadybugTools_HourlyContinous(values, profileGroup, name, 0, 1, 1, endHour, endDay, endMonth, 1, false, metadata);

        }

        public static string ToLadybugTools_HourlyContinous(double[] values, ProfileGroup profileGroup, string name, int startHour, int startDay,int startMonth, int endHour, int endDay, int endMonth, int timestep, bool leapYear, Dictionary<string, object> metadata = null)
        {
            if (values == null)
                return null;

            JsonObject result = new JsonObject();

            JsonArray valuesArray = new JsonArray();
            foreach (double v in values)
                valuesArray.Add(v);
            result["values"] = valuesArray;

            JsonObject jsonObject_Header = new JsonObject();

            if (metadata != null)
            {
                JsonObject jsonObject_Metadata = new JsonObject();
                foreach (KeyValuePair<string, object> keyValuePair in metadata)
                    jsonObject_Metadata[keyValuePair.Key] = JsonValue.Create(keyValuePair.Value);

                jsonObject_Header["metadata"] = jsonObject_Metadata;
            }

            JsonObject jsonObject_AnalysisPeriod = new JsonObject
            {
                ["st_day"] = startDay,
                ["end_month"] = endMonth,
                ["st_hour"] = startHour,
                ["end_hour"] = endHour,
                ["timestep"] = timestep,
                ["is_leap_year"] = leapYear,
                ["st_month"] = startMonth,
                ["end_day"] = endDay,
                ["type"] = "AnalysisPeriod",
            };

            jsonObject_Header["analysis_period"] = jsonObject_AnalysisPeriod;

            string unit = null;
            string dataType = null;
            switch (profileGroup)
            {
                case ProfileGroup.Gain:
                    unit = "fraction";
                    dataType = "Fraction";
                    break;

                case ProfileGroup.Humidistat:
                    unit = "%";
                    dataType = "Fraction";
                    break;

                case ProfileGroup.Thermostat:
                    unit = "C";
                    dataType = "Temperature";
                    break;

                default:
                    unit = "unknown";
                    dataType = "GenericType";
                    break;
            }

            jsonObject_Header["unit"] = unit;
            jsonObject_Header["type"] = "Header";

            JsonObject jsonObject_DataType = new JsonObject
            {
                ["base_unit"] = unit,
                ["name"] = name,
                ["type"] = "DataTypeBase",
                ["data_type"] = dataType,
            };
            if (profileGroup == ProfileGroup.Humidistat || profileGroup == ProfileGroup.Undefined)
            {
                jsonObject_DataType["min"] = double.MinValue;
                jsonObject_DataType["max"] = double.MaxValue;
            }
            jsonObject_DataType["point_in_time"] = true;
            jsonObject_DataType["cumulative"] = false;
            jsonObject_DataType["abbreviation"] = "Unknown Data Type";
            jsonObject_DataType["unit_descr"] = null;

            jsonObject_Header["data_type"] = jsonObject_DataType;

            result["header"] = jsonObject_Header;
            result["type"] = "HourlyContinuous";

            return result.ToString();
        }
    }
}
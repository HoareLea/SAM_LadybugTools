using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static List<ScheduleUnitType> ScheduleUnitTypes(this ProfileType profileType)
        {
            switch (profileType)
            {
                case ProfileType.Cooling:
                case ProfileType.Heating:
                    return new List<ScheduleUnitType>() { ScheduleUnitType.Temperature };

                case ProfileType.Humidification:
                case ProfileType.Dehumidification:
                    return new List<ScheduleUnitType>() { ScheduleUnitType.Percent };

                case ProfileType.EquipmentLatent:
                case ProfileType.EquipmentSensible:
                case ProfileType.Infiltration:
                case ProfileType.Lighting:
                case ProfileType.Pollutant:
                    return new List<ScheduleUnitType>() { ScheduleUnitType.Dimensionless };

                case ProfileType.Occupancy:
                    return new List<ScheduleUnitType>() { ScheduleUnitType.Dimensionless, ScheduleUnitType.ActivityLevel };
            }

            return null;
        }
    }
}
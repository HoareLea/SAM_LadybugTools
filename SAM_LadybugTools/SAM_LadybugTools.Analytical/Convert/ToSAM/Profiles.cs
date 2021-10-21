using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<Profile> ToSAM_Profiles(this ProgramTypeAbridged programTypeAbridged)
        {
            if(programTypeAbridged == null)
            {
                return null;
            }

            List<Profile> result = new List<Profile>();

            PeopleAbridged peopleAbridged = programTypeAbridged.People;
            if (peopleAbridged != null)
            {
                Profile profile = new Profile(peopleAbridged.ActivitySchedule, ProfileType.Occupancy);
                result.Add(profile);
            }

            LightingAbridged lightingAbridged = programTypeAbridged.Lighting;
            if (lightingAbridged != null)
            {
                Profile profile = new Profile(lightingAbridged.Schedule, ProfileType.Lighting);
                result.Add(profile);
            }

            ElectricEquipmentAbridged electricEquipmentAbridged = programTypeAbridged.ElectricEquipment;
            if (electricEquipmentAbridged != null)
            {
                Profile profile = new Profile(electricEquipmentAbridged.Identifier, ProfileType.EquipmentSensible);
                result.Add(profile);
            }

            InfiltrationAbridged infiltrationAbridged = programTypeAbridged.Infiltration;
            if (infiltrationAbridged != null)
            {
                Profile profile = new Profile(infiltrationAbridged.Schedule, ProfileType.Infiltration);
                result.Add(profile);
            }

            SetpointAbridged setPointAbridged = programTypeAbridged.Setpoint;
            if (setPointAbridged != null)
            {
                Profile profile_Cooling = new Profile(setPointAbridged.CoolingSchedule, ProfileType.Cooling);
                result.Add(profile_Cooling);

                Profile profile_Heating = new Profile(setPointAbridged.HeatingSchedule, ProfileType.Heating);
                result.Add(profile_Heating);
            }

            return result;
        }

        public static List<Profile> ToSAM_Profiles(this ProgramType programType)
        {
            if (programType == null)
            {
                return null;
            }

            List<Profile> result = new List<Profile>();

            People people = programType.People;
            if (people != null)
            {
                AnyOf<ScheduleRuleset, ScheduleFixedInterval> occupancySchedule = people.OccupancySchedule;
                AnyOf<ScheduleRuleset, ScheduleFixedInterval> activitySchedule = people.ActivitySchedule;
            }

            Lighting lighting = programType.Lighting;
            if (lighting != null)
            {
                AnyOf<ScheduleRuleset, ScheduleFixedInterval> schedule = lighting.Schedule;
            }

            ElectricEquipment electricEquipment = programType.ElectricEquipment;
            if (electricEquipment != null)
            {
                AnyOf<ScheduleRuleset, ScheduleFixedInterval> schedule = electricEquipment.Schedule;
            }

            Infiltration infiltration = programType.Infiltration;
            if (infiltration != null)
            {
                AnyOf<ScheduleRuleset, ScheduleFixedInterval> schedule = infiltration.Schedule;
            }

            Setpoint setPoint = programType.Setpoint;
            if (setPoint != null)
            {
                AnyOf<ScheduleRuleset, ScheduleFixedInterval> coolingSchedule = setPoint.CoolingSchedule;
                AnyOf<ScheduleRuleset, ScheduleFixedInterval> heatingSchedule = setPoint.HeatingSchedule;
            }

            return result;
        }
    }
}
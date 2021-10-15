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

            return null;
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
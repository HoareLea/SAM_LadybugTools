using HoneybeeSchema;
using HoneybeeSchema.Energy;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<Profile> ToSAM_Profiles(this ProgramTypeAbridged programTypeAbridged, IEnumerable<ISchedule> schedules = null)
        {
            if(programTypeAbridged == null)
            {
                return null;
            }

            List<ScheduleRulesetAbridged> scheduleRulesetAbridgeds = HoneybeeSchema.Helper.EnergyLibrary.DefaultScheduleRuleset?.ToList();

            List<Profile> result = new List<Profile>();

            PeopleAbridged peopleAbridged = programTypeAbridged.People;
            if (peopleAbridged != null)
            {
                ISchedule schedule = null;

                if(schedules != null)
                {
                    schedule = schedules?.First(x => x.Identifier == peopleAbridged.OccupancySchedule);
                }

                if(schedule == null && scheduleRulesetAbridgeds != null)
                {
                    schedule = scheduleRulesetAbridgeds.Find(x => x.Identifier == peopleAbridged.OccupancySchedule);
                }
                
                if(schedule != null)
                {
                    Profile profile = schedule.ToSAM(ProfileType.Occupancy);
                    result.Add(profile);
                }
            }

            LightingAbridged lightingAbridged = programTypeAbridged.Lighting;
            if (lightingAbridged != null)
            {
                ISchedule schedule = null;

                if (schedules != null)
                {
                    schedule = schedules?.First(x => x.Identifier == lightingAbridged.Schedule);
                }

                if (schedule == null && scheduleRulesetAbridgeds != null)
                {
                    schedule = scheduleRulesetAbridgeds.Find(x => x.Identifier == lightingAbridged.Schedule);
                }

                if (schedule != null)
                {
                    Profile profile = schedule.ToSAM(ProfileType.Lighting);
                    result.Add(profile);
                }

            }

            ElectricEquipmentAbridged electricEquipmentAbridged = programTypeAbridged.ElectricEquipment;
            if (electricEquipmentAbridged != null)
            {
                ISchedule schedule = null;

                if (schedules != null)
                {
                    schedule = schedules?.First(x => x.Identifier == electricEquipmentAbridged.Schedule);
                }

                if (schedule == null && scheduleRulesetAbridgeds != null)
                {
                    schedule = scheduleRulesetAbridgeds.Find(x => x.Identifier == electricEquipmentAbridged.Schedule);
                }

                if (schedule != null)
                {
                    Profile profile = schedule.ToSAM(ProfileType.EquipmentSensible);
                    result.Add(profile);
                }

            }

            InfiltrationAbridged infiltrationAbridged = programTypeAbridged.Infiltration;
            if (infiltrationAbridged != null)
            {
                ISchedule schedule = null;

                if (schedules != null)
                {
                    schedule = schedules?.First(x => x.Identifier == infiltrationAbridged.Schedule);
                }

                if (schedule == null && scheduleRulesetAbridgeds != null)
                {
                    schedule = scheduleRulesetAbridgeds.Find(x => x.Identifier == infiltrationAbridged.Schedule);
                }

                if (schedule != null)
                {
                    Profile profile = schedule.ToSAM(ProfileType.Infiltration);//= new Profile(infiltrationAbridged.Schedule, ProfileType.EquipmentSensible);
                    result.Add(profile);
                }
            }

            SetpointAbridged setPointAbridged = programTypeAbridged.Setpoint;
            if (setPointAbridged != null)
            {
                ISchedule schedule = null;

                if (schedules != null)
                {
                    schedule = schedules?.First(x => x.Identifier == setPointAbridged.CoolingSchedule);
                }

                if (schedule == null && scheduleRulesetAbridgeds != null)
                {
                    schedule = scheduleRulesetAbridgeds.Find(x => x.Identifier == setPointAbridged.CoolingSchedule);
                }

                if (schedule != null)
                {
                    Profile profile_Cooling = schedule.ToSAM(ProfileType.Cooling);//= new Profile(setPointAbridged.CoolingSchedule, ProfileType.Cooling);
                    result.Add(profile_Cooling);
                }

                schedule = null;

                if (schedules != null)
                {
                    schedule = schedules?.First(x => x.Identifier == setPointAbridged.HeatingSchedule);
                }

                if (schedule == null && scheduleRulesetAbridgeds != null)
                {
                    schedule = scheduleRulesetAbridgeds.Find(x => x.Identifier == setPointAbridged.HeatingSchedule);
                }

                if (schedule != null)
                {
                    Profile profile_Heating = schedule.ToSAM(ProfileType.Heating);//= new Profile(setPointAbridged.HeatingSchedule, ProfileType.Heating);
                    result.Add(profile_Heating);
                }
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
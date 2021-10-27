using HoneybeeSchema;
using System.Collections.Generic;
using System.Linq;

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

            List<ScheduleRulesetAbridged> scheduleRulesetAbridgeds = HoneybeeSchema.Helper.EnergyLibrary.DefaultScheduleRuleset?.ToList();

            List<Profile> result = new List<Profile>();

            PeopleAbridged peopleAbridged = programTypeAbridged.People;
            if (peopleAbridged != null)
            {
                ScheduleRulesetAbridged scheduleRulesetAbridged = scheduleRulesetAbridgeds.Find(x => x.Identifier == peopleAbridged.ActivitySchedule);
                if(scheduleRulesetAbridged != null)
                {
                    Profile profile = scheduleRulesetAbridged.ToSAM(ProfileType.Occupancy);
                    result.Add(profile);
                }
            }

            LightingAbridged lightingAbridged = programTypeAbridged.Lighting;
            if (lightingAbridged != null)
            {
                ScheduleRulesetAbridged scheduleRulesetAbridged = scheduleRulesetAbridgeds.Find(x => x.Identifier == lightingAbridged.Schedule);
                if (scheduleRulesetAbridged != null)
                {
                    Profile profile = scheduleRulesetAbridged.ToSAM(ProfileType.Lighting);
                    result.Add(profile);
                }

            }

            ElectricEquipmentAbridged electricEquipmentAbridged = programTypeAbridged.ElectricEquipment;
            if (electricEquipmentAbridged != null)
            {
                ScheduleRulesetAbridged scheduleRulesetAbridged = scheduleRulesetAbridgeds.Find(x => x.Identifier == electricEquipmentAbridged.Schedule);
                if (scheduleRulesetAbridged != null)
                {
                    Profile profile = scheduleRulesetAbridged.ToSAM(ProfileType.EquipmentSensible);
                    result.Add(profile);
                }

            }

            InfiltrationAbridged infiltrationAbridged = programTypeAbridged.Infiltration;
            if (infiltrationAbridged != null)
            {
                ScheduleRulesetAbridged scheduleRulesetAbridged = scheduleRulesetAbridgeds.Find(x => x.Identifier == infiltrationAbridged.Schedule);
                if (scheduleRulesetAbridged != null)
                {
                    Profile profile = scheduleRulesetAbridged.ToSAM(ProfileType.Infiltration);//= new Profile(infiltrationAbridged.Schedule, ProfileType.EquipmentSensible);
                    result.Add(profile);
                }
            }

            SetpointAbridged setPointAbridged = programTypeAbridged.Setpoint;
            if (setPointAbridged != null)
            {
                ScheduleRulesetAbridged scheduleRulesetAbridged;

                scheduleRulesetAbridged = scheduleRulesetAbridgeds.Find(x => x.Identifier == setPointAbridged.CoolingSchedule);
                if (scheduleRulesetAbridged != null)
                {
                    Profile profile_Cooling = scheduleRulesetAbridged.ToSAM(ProfileType.Cooling);//= new Profile(setPointAbridged.CoolingSchedule, ProfileType.Cooling);
                    result.Add(profile_Cooling);
                }

                scheduleRulesetAbridged = scheduleRulesetAbridgeds.Find(x => x.Identifier == setPointAbridged.HeatingSchedule);
                if (scheduleRulesetAbridged != null)
                {
                    Profile profile_Heating = scheduleRulesetAbridged.ToSAM(ProfileType.Heating);//= new Profile(setPointAbridged.HeatingSchedule, ProfileType.Heating);
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
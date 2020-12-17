using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ProgramType ToLadybugTools(this InternalCondition internalCondition, ProfileLibrary profileLibrary)
        {
            if (internalCondition == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(internalCondition);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            People people = null;
            Lighting lighting = null;
            ElectricEquipment electricEquipment = null;

            if (profileLibrary != null)
            {
                Dictionary<ProfileType, Profile> dictionary = internalCondition.GetProfileDictionary(profileLibrary);

                double peoplePerArea = 0;
                if (!internalCondition.TryGetValue(InternalConditionParameter.AreaPerPerson, out peoplePerArea))
                    peoplePerArea = 0;

                if (peoplePerArea != 0)
                    peoplePerArea = 1 / peoplePerArea;

                double sensibleGain = 0;
                if (!internalCondition.TryGetValue(InternalConditionParameter.OccupancySensibleGainPerPerson, out sensibleGain))
                    sensibleGain = 0;

                double latentGain = 0;
                if (!internalCondition.TryGetValue(InternalConditionParameter.OccupancyLatentGainPerPerson, out latentGain))
                    latentGain = 0;

                if (dictionary.ContainsKey(ProfileType.Occupancy))
                {
                    Profile profile = dictionary[ProfileType.Occupancy];
                    if (profile != null)
                    {
                        ScheduleFixedInterval scheduleFixedInterval = profile.ToLadybugTools(ProfileType.Occupancy);
                        if(scheduleFixedInterval != null)
                        {
                            ScheduleFixedInterval scheduleFixedInterval_ActivityLevel = profile.ToLadybugTools_ActivityLevel(sensibleGain + latentGain);
                            if(scheduleFixedInterval_ActivityLevel != null)
                            {
                                people = new People(string.Format("{0}_People", uniqueName), peoplePerArea, scheduleFixedInterval, scheduleFixedInterval_ActivityLevel, profile.Name);
                            }
                        }
                    }
                }

                if (dictionary.ContainsKey(ProfileType.Lighting))
                {
                    double lightingGainPerArea = 0;
                    if (!internalCondition.TryGetValue(InternalConditionParameter.LightingGainPerArea, out lightingGainPerArea))
                        lightingGainPerArea = 0;

                    Profile profile = dictionary[ProfileType.Lighting];
                    if (profile != null)
                    {
                        ScheduleFixedInterval scheduleFixedInterval = profile.ToLadybugTools(ProfileType.Lighting);
                        if (scheduleFixedInterval != null)
                        {
                            lighting = new Lighting(string.Format("{0}_Lighting", uniqueName), lightingGainPerArea, scheduleFixedInterval, profile.Name);
                        }
                    }
                }

                if (dictionary.ContainsKey(ProfileType.EquipmentSensible))
                {
                    double equipmentSensibleGainPerArea = 0;
                    if (!internalCondition.TryGetValue(InternalConditionParameter.EquipmentSensibleGainPerArea, out equipmentSensibleGainPerArea))
                        equipmentSensibleGainPerArea = 0;

                    Profile profile = dictionary[ProfileType.EquipmentSensible];
                    if (profile != null)
                    {
                        ScheduleFixedInterval scheduleFixedInterval = profile.ToLadybugTools(ProfileType.EquipmentSensible);
                        if (scheduleFixedInterval != null)
                        {
                            electricEquipment = new ElectricEquipment(string.Format("{0}_ElectricEquipment", uniqueName), equipmentSensibleGainPerArea, scheduleFixedInterval, profile.Name);
                        }
                    }
                }

            }

            ProgramType result = new ProgramType(uniqueName, internalCondition.Name, people, lighting, electricEquipment);
            return result;
        }
    }
}
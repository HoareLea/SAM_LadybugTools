﻿using HoneybeeSchema;
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
            if (profileLibrary != null)
            {
                Dictionary<ProfileType, Profile> dictionary = internalCondition.GetProfileDictionary(profileLibrary);

                double peoplePerArea = 0;
                if (!internalCondition.TryGetValue(InternalConditionParameter.OccupancyPerArea, out peoplePerArea))
                    peoplePerArea = 0;

                double sensibleGain = 0;
                if (internalCondition.TryGetValue(InternalConditionParameter.OccupancySensibleGainPerPerson, out sensibleGain))
                    sensibleGain = 0;

                double latentGain = 0;
                if (internalCondition.TryGetValue(InternalConditionParameter.OccupancyLatentGainPerPerson, out latentGain))
                    latentGain = 0;

                if (dictionary.ContainsKey(ProfileType.Occupancy))
                {
                    Profile profile = dictionary[ProfileType.Occupancy];
                    if (profile != null)
                    {
                        string uniqueName_Profile = Core.LadybugTools.Query.UniqueName(typeof(PeopleAbridged), uniqueName);

                        people = new People(string.Format("{0}_People", uniqueName), peoplePerArea, profile.ToLadybugTools(), profile.ToLadybugTools_ActivityLevel(sensibleGain + latentGain));
                    }
                }
            }

            ProgramType result = new ProgramType(uniqueName, internalCondition.Name, people);
            return result;
        }
    }
}
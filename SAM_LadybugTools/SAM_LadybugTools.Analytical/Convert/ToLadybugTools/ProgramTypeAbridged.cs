using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ProgramTypeAbridged ToLadybugTools(this InternalCondition internalCondition, ProfileLibrary profileLibrary)
        {
            if (internalCondition == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(internalCondition);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            PeopleAbridged peopleAbridged = null;
            if (profileLibrary != null)
            {
                Dictionary<ProfileType, Profile> dictionary = internalCondition.GetProfileDictionary(profileLibrary);

                double peoplePerArea = 0;
                if (!internalCondition.TryGetValue(InternalConditionParameter.OccupancyPerArea, out peopleAbridged))
                    peoplePerArea = 0;

                if (dictionary.ContainsKey(ProfileType.Occupancy))
                {
                    Profile profile = dictionary[ProfileType.Occupancy];
                    if (profile != null)
                    {
                        string uniqueName_Profile = Core.LadybugTools.Query.UniqueName(profile);
                        peopleAbridged = new PeopleAbridged(string.Format("{0}_People", uniqueName), peoplePerArea, uniqueName_Profile, uniqueName_Profile);
                    }
                }
            }

            ProgramTypeAbridged result = new ProgramTypeAbridged(uniqueName, internalCondition.Name, peopleAbridged);
            return result;
        }
    }
}
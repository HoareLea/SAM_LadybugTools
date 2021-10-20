using HoneybeeSchema;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ScheduleRuleset ToLadybugTools(this Profile profile, ProfileType profileType = ProfileType.Undefined)
        {
            if (profile == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(profile);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            //TODO: Add more sophisticated Method to create ScheduleRulesetAbridged
            List<Profile> profiles = profile.GetProfiles()?.ToList();
            if (profiles == null)
                profiles = new List<Profile>();

            if (profiles.Count == 0)
                profiles.Add(profile);

            profiles.RemoveAll(x => x == null);

            if (profiles.Count == 0)
                return null;

            Dictionary<System.Guid, ScheduleDay> dictionary_ScheduleDay = new Dictionary<System.Guid, ScheduleDay>();
            foreach (Profile profile_Temp in profiles)
            {
                System.Guid guid = profile_Temp.Guid;

                if (dictionary_ScheduleDay.ContainsKey(guid))
                    continue;

                ScheduleDay scheduleDay = profile_Temp.ToLadybugTools_ScheduleDay();
                if (scheduleDay == null)
                    continue;

                dictionary_ScheduleDay[guid] = scheduleDay;
            }

            if (dictionary_ScheduleDay == null || dictionary_ScheduleDay.Count == 0)
                return null;

            int index = 0;
            while (profiles.Count < 7)
            {
                profiles.Add(profiles[index]);
                index++;
            }

            Dictionary<System.Guid, ScheduleRuleAbridged> dictionary_ScheduleRuleAbridged = new Dictionary<System.Guid, ScheduleRuleAbridged>();

            for (int i = 0; i < 7; i++)
            {
                System.Guid guid = profiles[i].Guid;
                if (!dictionary_ScheduleDay.ContainsKey(guid))
                    continue;

                ScheduleDay scheduleDay = dictionary_ScheduleDay[guid];

                ScheduleRuleAbridged scheduleRuleAbridged;
                if (!dictionary_ScheduleRuleAbridged.TryGetValue(guid, out scheduleRuleAbridged))
                {
                    scheduleRuleAbridged = new ScheduleRuleAbridged(scheduleDay.Identifier);
                    dictionary_ScheduleRuleAbridged[guid] = scheduleRuleAbridged;
                }

                switch (i)
                {
                    case 0:
                        scheduleRuleAbridged.ApplyMonday = true;
                        break;
                    case 1:
                        scheduleRuleAbridged.ApplyTuesday = true;
                        break;
                    case 2:
                        scheduleRuleAbridged.ApplyWednesday = true;
                        break;
                    case 3:
                        scheduleRuleAbridged.ApplyThursday = true;
                        break;
                    case 4:
                        scheduleRuleAbridged.ApplyFriday = true;
                        break;
                    case 5:
                        scheduleRuleAbridged.ApplySaturday = true;
                        break;
                    case 6:
                        scheduleRuleAbridged.ApplySunday = true;
                        break;
                }
            }

            List<ScheduleDay> scheduleDays = dictionary_ScheduleDay.Values.ToList();
            List<ScheduleRuleAbridged> scheduleRuleAbridgedes = dictionary_ScheduleRuleAbridged.Values.ToList();
            ScheduleRuleset result = new ScheduleRuleset(uniqueName, scheduleDays, scheduleDays.First().Identifier, profile.Name, null, scheduleRuleAbridgedes);

            return result;
        }

        public static ScheduleRuleset ToLadybugTools_ActivityLevel(this Profile profile, double value)
        {
            if (profile == null || double.IsNaN(value))
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(profile);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            uniqueName = Core.LadybugTools.Query.UniqueName(typeof(ActivityLevel), uniqueName);

            ScheduleDay scheduleDay = new ScheduleDay(uniqueName, new List<double>() { value }, profile.Name, new List<List<int>>() { new List<int>() { 0, 0 } });

            ScheduleRuleAbridged scheduleRuleAbridged = new ScheduleRuleAbridged(scheduleDay.Identifier, true, true, true, true, true, true, true);

            ScheduleRuleset scheduleRulset = new ScheduleRuleset(uniqueName, new List<ScheduleDay>() { scheduleDay }, scheduleDay.Identifier, profile.Name, new List<ScheduleRuleAbridged>() { scheduleRuleAbridged});

            return scheduleRulset;
        }
    }
}
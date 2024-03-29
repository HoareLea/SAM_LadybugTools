﻿using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Profile ToSAM(this HoneybeeSchema.Energy.ISchedule schedule, ProfileType? profileType)
        {
            if(schedule is ScheduleRulesetAbridged)
            {
                return ToSAM((ScheduleRulesetAbridged)schedule, profileType);
            }

            return null;
        }
        
        public static Profile ToSAM(this ScheduleRulesetAbridged scheduleRulesetAbridged, ProfileType? profileType)
        {
            if (scheduleRulesetAbridged == null)
                return null;

            Dictionary<string, Profile> profiles = new Dictionary<string, Profile>();

            List<ScheduleDay> scheduleDays = scheduleRulesetAbridged.DaySchedules;
            if(scheduleDays != null)
            {
                foreach(ScheduleDay scheduleDay in scheduleDays)
                {
                    Profile profile = scheduleDay.ToSAM();
                    if (profile == null)
                        continue;

                    profiles[scheduleDay.Identifier] = profile;
                }
            }

            Profile result = null;
            if(profileType != null && profileType.HasValue)
            {
                result = new Profile(scheduleRulesetAbridged.Identifier, profileType.Value);
            }
            else
            {
                result = new Profile(scheduleRulesetAbridged.Identifier, scheduleRulesetAbridged.Type);
            }

            List<ScheduleRuleAbridged> scheduleRuleAbridges = scheduleRulesetAbridged.ScheduleRules;
            if(scheduleRuleAbridges == null || scheduleRuleAbridges.Count == 0)
            {
                string name = scheduleRulesetAbridged.DefaultDaySchedule;
                if(!string.IsNullOrWhiteSpace(name))
                {
                    if(profiles.TryGetValue(name, out Profile profile))
                    {
                        result.Add(profile);
                    }
                }
            }
            else
            {
                ScheduleRuleAbridged scheduleRuleAbridged = scheduleRuleAbridges[0]; //TODO: Not fully implemented assumtion: there is just one scheduleRuleAbrided
                string name = null;
                Profile profile = null;

                //Sunday
                name = null;
                scheduleRuleAbridged = scheduleRuleAbridges.Find(x => x.ApplySunday);
                if (scheduleRuleAbridged != null)
                    name = scheduleRuleAbridged.ScheduleDay;

                if (string.IsNullOrWhiteSpace(name))
                    name = scheduleRulesetAbridged.DefaultDaySchedule;

                if (!string.IsNullOrWhiteSpace(name) && profiles.TryGetValue(name, out profile) && profile != null)
                    result.Add(profile);

                //Monday
                name = null;
                scheduleRuleAbridged = scheduleRuleAbridges.Find(x => x.ApplyMonday);
                if (scheduleRuleAbridged != null)
                    name = scheduleRuleAbridged.ScheduleDay;

                if (string.IsNullOrWhiteSpace(name))
                    name = scheduleRulesetAbridged.DefaultDaySchedule;

                if (!string.IsNullOrWhiteSpace(name) && profiles.TryGetValue(name, out profile) && profile != null)
                    result.Add(profile);

                //Tuesday
                name = null;
                scheduleRuleAbridged = scheduleRuleAbridges.Find(x => x.ApplyTuesday);
                if (scheduleRuleAbridged != null)
                    name = scheduleRuleAbridged.ScheduleDay;

                if (string.IsNullOrWhiteSpace(name))
                    name = scheduleRulesetAbridged.DefaultDaySchedule;

                if (!string.IsNullOrWhiteSpace(name) && profiles.TryGetValue(name, out profile) && profile != null)
                    result.Add(profile);

                //Wednesday
                name = null;
                scheduleRuleAbridged = scheduleRuleAbridges.Find(x => x.ApplyWednesday);
                if (scheduleRuleAbridged != null)
                    name = scheduleRuleAbridged.ScheduleDay;

                if (string.IsNullOrWhiteSpace(name))
                    name = scheduleRulesetAbridged.DefaultDaySchedule;

                if (!string.IsNullOrWhiteSpace(name) && profiles.TryGetValue(name, out profile) && profile != null)
                    result.Add(profile);

                //Thursday
                name = null;
                scheduleRuleAbridged = scheduleRuleAbridges.Find(x => x.ApplyThursday);
                if (scheduleRuleAbridged != null)
                    name = scheduleRuleAbridged.ScheduleDay;

                if (string.IsNullOrWhiteSpace(name))
                    name = scheduleRulesetAbridged.DefaultDaySchedule;

                if (!string.IsNullOrWhiteSpace(name) && profiles.TryGetValue(name, out profile) && profile != null)
                    result.Add(profile);

                //Friday
                name = null;
                scheduleRuleAbridged = scheduleRuleAbridges.Find(x => x.ApplyFriday);
                if (scheduleRuleAbridged != null)
                    name = scheduleRuleAbridged.ScheduleDay;

                if (string.IsNullOrWhiteSpace(name))
                    name = scheduleRulesetAbridged.DefaultDaySchedule;

                if (!string.IsNullOrWhiteSpace(name) && profiles.TryGetValue(name, out profile) && profile != null)
                    result.Add(profile);

                //Saturday
                name = null;
                scheduleRuleAbridged = scheduleRuleAbridges.Find(x => x.ApplySaturday);
                if (scheduleRuleAbridged != null)
                    name = scheduleRuleAbridged.ScheduleDay;

                if (string.IsNullOrWhiteSpace(name))
                    name = scheduleRulesetAbridged.DefaultDaySchedule;

                if (!string.IsNullOrWhiteSpace(name) && profiles.TryGetValue(name, out profile) && profile != null)
                    result.Add(profile);
            }

            return result;
        }

        public static Profile ToSAM(this ScheduleDay scheduleDay)
        {
            if (scheduleDay == null)
                return null;

            List<double> values = scheduleDay.Values;
            if (values == null)
                return null;

            Profile result = new Profile(scheduleDay.Identifier, scheduleDay.Type);

            if (values.Count == 1)
            {
                result.Add(new Core.Range<int>(0, 23), values[0]);
            }
            else
            {
                List<List<int>> times = scheduleDay.Times;
                if (times.Count == values.Count)
                {
                    for (int i = 1; i < times.Count; i++)
                    {
                        int min = times[i - 1][0];
                        int max = times[i][0];

                        result.Add(new Core.Range<int>(min, max - 1), values[i - 1]);
                    }

                    int index = times.Count - 1;
                    result.Add(new Core.Range<int>(times[index][0], 23), values[index]);
                }
            }

            return result;
        }
    }
}
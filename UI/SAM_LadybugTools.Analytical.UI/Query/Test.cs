using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools.UI
{
    public static partial class Query
    {
        public static Honeybee.UI.Dialog_ScheduleRulesetManager Test()
        {
            List<ScheduleRulesetAbridged> scheduleRulesetAbridgeds = new List<ScheduleRulesetAbridged>();
            List<ScheduleTypeLimit> scheduleTypeLimits = new List<ScheduleTypeLimit>();

            //Honeybee.UI.Dialog_ScheduleRulesetManager dialog_ScheduleRulesetManager = new Honeybee.UI.Dialog_ScheduleRulesetManager(scheduleRulesetAbridgeds, scheduleTypeLimits);

            //return dialog_ScheduleRulesetManager;

            throw new System.NotImplementedException();
        }
    }
}
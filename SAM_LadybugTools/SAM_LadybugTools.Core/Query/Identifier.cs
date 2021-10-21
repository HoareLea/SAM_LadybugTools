using HoneybeeSchema;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        public static string Identifier(this HoneybeeSchema.AnyOf<ScheduleRuleset, ScheduleFixedInterval> anyOf)
        {
            if(anyOf == null || anyOf.Obj == null)
            {
                return null;
            }

            if(anyOf.Obj is ScheduleRuleset)
            {
                return ((ScheduleRuleset)anyOf.Value).Identifier;
            }

            if (anyOf.Obj is ScheduleFixedInterval)
            {
                return ((ScheduleFixedInterval)anyOf.Value).Identifier;
            }

            return null;
        }
    }
}
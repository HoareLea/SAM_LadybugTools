using HoneybeeSchema;
using HoneybeeSchema.Energy;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ProfileLibrary ToSAM_ProfileLibrary(this ModelEnergyProperties modelEnergyProperties)
        {
            if(modelEnergyProperties == null)
            {
                return null;
            }

            IEnumerable<IProgramtype> programTypes = modelEnergyProperties.ProgramTypeList;
            if(programTypes == null)
            {
                return null;
            }

            IEnumerable<ISchedule> schedules = modelEnergyProperties.ScheduleList;

            ProfileLibrary result = new ProfileLibrary(string.Empty);
            foreach (IProgramtype programType in programTypes)
            {
                List<Profile> profiles = null;

                if(programType is ProgramTypeAbridged)
                {
                    profiles = ((ProgramTypeAbridged)programType).ToSAM_Profiles(schedules);
                } 
                else if(programType is ProgramType)
                {
                    profiles = ((ProgramType)programType).ToSAM_Profiles();
                }

                if(profiles != null)
                {
                    foreach(Profile profile in profiles)
                    {
                        result.Add(profile);
                    }
                }
            }

            return result;
        }
    }
}
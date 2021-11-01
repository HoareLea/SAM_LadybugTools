using HoneybeeSchema;
using HoneybeeSchema.Energy;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<InternalCondition> ToSAM_InternalConditions(this ModelEnergyProperties modelEnergyProperties)
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

            List<InternalCondition> result = new List<InternalCondition>();
            foreach (IProgramtype programType in programTypes)
            {
                InternalCondition internalCondition = null;

                if(programType is ProgramTypeAbridged)
                {
                    internalCondition = ((ProgramTypeAbridged)programType).ToSAM_InternalCondition(modelEnergyProperties);
                } 
                else if(programType is ProgramType)
                {
                    internalCondition = ((ProgramType)programType).ToSAM_InternalCondtion();
                }

                if(internalCondition != null)
                {
                    result.Add(internalCondition);
                }
            }

            return result;
        }
    }
}
using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Space ToSAM(this HoneybeeSchema.Room room, IEnumerable<InternalCondition> internalConditions = null)
        {
            if(room == null)
            {
                return null;
            }

            Space result = new Space(room.DisplayName);

            string programType = room.Properties?.Energy?.ProgramType;
            if(!string.IsNullOrWhiteSpace(programType))
            {
                InternalCondition internalCondition = null;
                if (internalConditions != null)
                {
                    foreach(InternalCondition internalCondition_Temp in internalConditions)
                    {
                        if(internalCondition_Temp.Name == programType)
                        {
                            internalCondition = internalCondition_Temp;
                            break;
                        }
                    }
                }

                if(internalCondition == null)
                {
                    internalCondition = new InternalCondition(programType);
                }

                result.InternalCondition = internalCondition;
            }

            return result;
        }
    }
}
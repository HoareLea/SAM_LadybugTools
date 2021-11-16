using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Space ToSAM(this Room room, IEnumerable<InternalCondition> internalConditions = null)
        {
            if(room == null)
            {
                return null;
            }

            Geometry.Spatial.Point3D location = null;
            double area = double.NaN;
            double volume = double.NaN;

            Geometry.Spatial.Shell shell = Geometry.LadybugTools.Query.Shell(room);
            if(shell != null)
            {
                location = shell.InternalPoint3D();
                area = Geometry.Spatial.Query.Area(shell, 0.1);
                volume = Geometry.Spatial.Query.Volume(shell);
            }

            Space result = new Space(room.DisplayName, location);
            if (!double.IsNaN(area))
            {
                result.SetValue(SpaceParameter.Area, area);
            }

            if(!double.IsNaN(volume))
            {
                result.SetValue(SpaceParameter.Volume, volume);
            }

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

                if(internalCondition != null && !double.IsNaN(volume))
                {
                    if(internalCondition.TryGetValue(InternalConditionParameter.FlowPerExteriorArea, out double flowPerExteriorArea))
                    {
                        internalCondition = new InternalCondition(System.Guid.NewGuid(), internalCondition);
                        double exteriorArea = Geometry.LadybugTools.Query.ExteriorArea(room);

                        double flow = flowPerExteriorArea * exteriorArea;

                        internalCondition.SetValue(Analytical.InternalConditionParameter.InfiltrationAirChangesPerHour, flow / volume * 3600);
                    }
                }

                if(!double.IsNaN(area) && internalCondition.TryGetValue(InternalConditionParameter.TotalMetabolicRate, out double totalMetabolicRate) && internalCondition.TryGetValue(Analytical.InternalConditionParameter.AreaPerPerson, out double areaPerPerson))
                {
                    double people = area / areaPerPerson;

                    double occupancyLatentGainPerPerson = 0;
                    double occupancySensibleGainPerPerson = 0;

                    if (internalCondition.TryGetValue(InternalConditionParameter.LatentFraction, out double latentFraction))
                    {
                        occupancyLatentGainPerPerson = totalMetabolicRate * latentFraction / people;
                        occupancySensibleGainPerPerson = totalMetabolicRate * (1 - latentFraction) / people;
                    }
                    else
                    {
                        //2021-XI-16 if latent is missin or autocalcutlate zero will be used in Tas! custom specific settings
                        occupancySensibleGainPerPerson = totalMetabolicRate * (1 - latentFraction) / people;
                    }

                    internalCondition.SetValue(Analytical.InternalConditionParameter.OccupancyLatentGainPerPerson, occupancyLatentGainPerPerson);
                    internalCondition.SetValue(Analytical.InternalConditionParameter.OccupancySensibleGainPerPerson, occupancySensibleGainPerPerson);
                }

                result.InternalCondition = internalCondition;
            }

            return result;
        }
    }
}
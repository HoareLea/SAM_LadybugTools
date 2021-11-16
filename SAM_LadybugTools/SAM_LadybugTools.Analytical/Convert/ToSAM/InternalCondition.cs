using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static InternalCondition ToSAM_InternalCondition(this ProgramTypeAbridged programTypeAbridged, ModelEnergyProperties modelEnergyProperties)
        {
            if(programTypeAbridged == null)
            {
                return null;
            }

            InternalCondition result = new InternalCondition(programTypeAbridged.Identifier);

            PeopleAbridged peopleAbridged = programTypeAbridged.People;
            if(peopleAbridged != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.OccupancyProfileName, peopleAbridged.OccupancySchedule);
                result.SetValue(Analytical.InternalConditionParameter.AreaPerPerson, 1 / peopleAbridged.PeoplePerArea);

                if(modelEnergyProperties != null)
                {
                    IEnumerable<HoneybeeSchema.Energy.ISchedule> schedules = modelEnergyProperties.ScheduleList;
                    if(schedules != null)
                    {
                        foreach(HoneybeeSchema.Energy.ISchedule schedule in schedules)
                        {
                            if(schedule.Identifier == peopleAbridged.ActivitySchedule)
                            {
                                
                                Profile profile = schedule.ToSAM(ProfileType.Other);
                                if(profile != null)
                                {
                                    result.SetValue(InternalConditionParameter.TotalMetabolicRatePerPerson, profile.MaxValue);
                                }
                            }
                        }
                    }
                }


                if (peopleAbridged.LatentFraction != null && peopleAbridged.LatentFraction.Obj is double)
                {
                    result.SetValue(InternalConditionParameter.LatentFraction, (double)peopleAbridged.LatentFraction.Obj); //TODO: Recalculate value per space
                }
                
            }

            LightingAbridged lightingAbridged = programTypeAbridged.Lighting;
            if(lightingAbridged != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.LightingProfileName, lightingAbridged.Schedule);
                result.SetValue(Analytical.InternalConditionParameter.LightingGainPerArea, lightingAbridged.WattsPerArea);
            }

            ElectricEquipmentAbridged electricEquipmentAbridged = programTypeAbridged.ElectricEquipment;
            if (electricEquipmentAbridged != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.EquipmentSensibleProfileName, electricEquipmentAbridged.Schedule);
                result.SetValue(Analytical.InternalConditionParameter.EquipmentSensibleGainPerArea, electricEquipmentAbridged.WattsPerArea);
            }

            InfiltrationAbridged infiltrationAbridged= programTypeAbridged.Infiltration;
            if (infiltrationAbridged != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.InfiltrationProfileName, infiltrationAbridged.Schedule);
                result.SetValue(InternalConditionParameter.FlowPerExteriorArea, infiltrationAbridged.FlowPerExteriorArea); //TODO: Recalculate value per space
            }

            SetpointAbridged setPoint = programTypeAbridged.Setpoint;
            if(setPoint != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.CoolingProfileName, setPoint.CoolingSchedule);

                result.SetValue(Analytical.InternalConditionParameter.HeatingProfileName, setPoint.HeatingSchedule);
            }

            VentilationAbridged ventilationAbridged = programTypeAbridged.Ventilation;
            if (ventilationAbridged != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.SupplyAirFlow, ventilationAbridged.FlowPerZone);
                result.SetValue(Analytical.InternalConditionParameter.SupplyAirFlowPerArea, ventilationAbridged.FlowPerArea);
                result.SetValue(Analytical.InternalConditionParameter.SupplyAirFlowPerPerson, ventilationAbridged.FlowPerPerson);

                result.SetValue(Analytical.InternalConditionParameter.ExhaustAirFlow, ventilationAbridged.FlowPerZone);
                result.SetValue(Analytical.InternalConditionParameter.ExhaustAirFlowPerArea, ventilationAbridged.FlowPerArea);
                result.SetValue(Analytical.InternalConditionParameter.ExhaustAirFlowPerPerson, ventilationAbridged.FlowPerPerson);
            }

            return result;
        }

        public static InternalCondition ToSAM_InternalCondtion(this ProgramType programType)
        {
            if (programType == null)
            {
                return null;
            }

            InternalCondition result = new InternalCondition(programType.DisplayName);

            People people = programType.People;
            if (people != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.OccupancyProfileName, Core.LadybugTools.Query.Identifier(people.OccupancySchedule));
            }

            Lighting lighting = programType.Lighting;
            if (lighting != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.LightingProfileName, Core.LadybugTools.Query.Identifier(lighting.Schedule));
            }

            ElectricEquipment electricEquipment = programType.ElectricEquipment;
            if (electricEquipment != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.EquipmentSensibleProfileName, Core.LadybugTools.Query.Identifier(electricEquipment.Schedule));
            }

            Infiltration infiltration = programType.Infiltration;
            if (infiltration != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.InfiltrationProfileName, Core.LadybugTools.Query.Identifier(infiltration.Schedule));
            }

            Setpoint setPoint = programType.Setpoint;
            if (setPoint != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.CoolingProfileName, Core.LadybugTools.Query.Identifier(setPoint.CoolingSchedule));

                result.SetValue(Analytical.InternalConditionParameter.HeatingProfileName, Core.LadybugTools.Query.Identifier(setPoint.HeatingSchedule));
            }

            return result;
        }
    }
}
using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static InternalCondition ToSAM_InternalCondition(this ProgramTypeAbridged programTypeAbridged)
        {
            if(programTypeAbridged == null)
            {
                return null;
            }

            InternalCondition result = new InternalCondition(programTypeAbridged.Identifier);

            PeopleAbridged peopleAbridged = programTypeAbridged.People;
            if(peopleAbridged != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.OccupancyProfileName, peopleAbridged.ActivitySchedule);
                result.SetValue(Analytical.InternalConditionParameter.AreaPerPerson, peopleAbridged.PeoplePerArea);
                
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
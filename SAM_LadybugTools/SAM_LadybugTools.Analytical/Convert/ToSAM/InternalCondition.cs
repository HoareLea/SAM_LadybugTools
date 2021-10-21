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
                result.SetValue(Analytical.InternalConditionParameter.OccupancyProfileName, peopleAbridged.Identifier);
                
            }

            LightingAbridged lightingAbridged = programTypeAbridged.Lighting;
            if(lightingAbridged != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.LightingProfileName, lightingAbridged.Identifier);
                result.SetValue(Analytical.InternalConditionParameter.LightingGainPerArea, lightingAbridged.WattsPerArea);
            }

            ElectricEquipmentAbridged electricEquipmentAbridged = programTypeAbridged.ElectricEquipment;
            if (electricEquipmentAbridged != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.EquipmentSensibleProfileName, electricEquipmentAbridged.Identifier);
                result.SetValue(Analytical.InternalConditionParameter.EquipmentSensibleGainPerArea, electricEquipmentAbridged.WattsPerArea);
            }

            InfiltrationAbridged infiltrationAbridged= programTypeAbridged.Infiltration;
            if (infiltrationAbridged != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.InfiltrationProfileName, infiltrationAbridged.Identifier);
                result.SetValue(InternalConditionParameter.FlowPerExteriorArea, infiltrationAbridged.FlowPerExteriorArea); //TODO: Recalculate value per space
            }

            SetpointAbridged setPoint = programTypeAbridged.Setpoint;
            if(setPoint != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.CoolingProfileName, setPoint.Identifier);

                result.SetValue(Analytical.InternalConditionParameter.HeatingProfileName, setPoint.Identifier);
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
                result.SetValue(Analytical.InternalConditionParameter.OccupancyProfileName, people.DisplayName);
            }

            Lighting lighting = programType.Lighting;
            if (lighting != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.LightingProfileName, lighting.DisplayName);
            }

            ElectricEquipment electricEquipment = programType.ElectricEquipment;
            if (electricEquipment != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.EquipmentSensibleProfileName, electricEquipment.DisplayName);
            }

            Infiltration infiltration = programType.Infiltration;
            if (infiltration != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.InfiltrationProfileName, infiltration.DisplayName);
            }

            Setpoint setPoint = programType.Setpoint;
            if (setPoint != null)
            {
                result.SetValue(Analytical.InternalConditionParameter.CoolingProfileName, setPoint.DisplayName);

                result.SetValue(Analytical.InternalConditionParameter.HeatingProfileName, setPoint.DisplayName);
            }

            return result;
        }
    }
}
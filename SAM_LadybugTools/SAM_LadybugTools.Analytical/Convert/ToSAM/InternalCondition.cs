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

            InternalCondition result = new InternalCondition(programTypeAbridged.DisplayName);

            PeopleAbridged peopleAbridged = programTypeAbridged.People;
            if(peopleAbridged != null)
            {
                result.SetValue(InternalConditionParameter.OccupancyProfileName, peopleAbridged.DisplayName);
            }

            LightingAbridged lightingAbridged = programTypeAbridged.Lighting;
            if(lightingAbridged != null)
            {
                result.SetValue(InternalConditionParameter.LightingProfileName, lightingAbridged.DisplayName);
            }

            ElectricEquipmentAbridged electricEquipmentAbridged = programTypeAbridged.ElectricEquipment;
            if (electricEquipmentAbridged != null)
            {
                result.SetValue(InternalConditionParameter.EquipmentSensibleProfileName, electricEquipmentAbridged.DisplayName);
            }

            InfiltrationAbridged infiltrationAbridged= programTypeAbridged.Infiltration;
            if (infiltrationAbridged != null)
            {
                result.SetValue(InternalConditionParameter.InfiltrationProfileName, infiltrationAbridged.DisplayName);
            }

            SetpointAbridged setPoint = programTypeAbridged.Setpoint;
            if(setPoint != null)
            {
                result.SetValue(InternalConditionParameter.CoolingProfileName, setPoint.DisplayName);

                result.SetValue(InternalConditionParameter.HeatingProfileName, setPoint.DisplayName);
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
                result.SetValue(InternalConditionParameter.OccupancyProfileName, people.DisplayName);
            }

            Lighting lighting = programType.Lighting;
            if (lighting != null)
            {
                result.SetValue(InternalConditionParameter.LightingProfileName, lighting.DisplayName);
            }

            ElectricEquipment electricEquipment = programType.ElectricEquipment;
            if (electricEquipment != null)
            {
                result.SetValue(InternalConditionParameter.EquipmentSensibleProfileName, electricEquipment.DisplayName);
            }

            Infiltration infiltration = programType.Infiltration;
            if (infiltration != null)
            {
                result.SetValue(InternalConditionParameter.InfiltrationProfileName, infiltration.DisplayName);
            }

            Setpoint setPoint = programType.Setpoint;
            if (setPoint != null)
            {
                result.SetValue(InternalConditionParameter.CoolingProfileName, setPoint.DisplayName);

                result.SetValue(InternalConditionParameter.HeatingProfileName, setPoint.DisplayName);
            }

            return result;
        }
    }
}
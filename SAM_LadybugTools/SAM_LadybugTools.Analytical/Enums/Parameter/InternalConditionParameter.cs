using System.ComponentModel;
using SAM.Core.Attributes;

namespace SAM.Analytical.LadybugTools
{
    [AssociatedTypes(typeof(InternalCondition)), Description("LadybugToos Internal Condition Parameter")]
    public enum InternalConditionParameter
    {
        [ParameterProperties("Flow Per Exterior Area", "Flow Per Exterior Area"), ParameterValue(Core.ParameterType.Double)] FlowPerExteriorArea,
    }
}
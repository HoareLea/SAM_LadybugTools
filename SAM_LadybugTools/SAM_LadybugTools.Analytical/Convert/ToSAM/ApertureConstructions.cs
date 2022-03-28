using HoneybeeSchema;
using HoneybeeSchema.Energy;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<ApertureConstruction> ToSAM_ApertureConstructions(this ModelEnergyProperties modelEnergyProperties, MaterialLibrary materialLibrary = null)
        {
            if(modelEnergyProperties == null)
            {
                return null;
            }

            List<ApertureConstruction> result = new List<ApertureConstruction>();

            IEnumerable<IConstruction> constructions_Honeybee = modelEnergyProperties.ConstructionList;
            if(constructions_Honeybee != null)
            {
                foreach (IConstruction construction_Honeybee in constructions_Honeybee)
                {
                    ApertureConstruction apertureConstruction = construction_Honeybee?.ToSAM_ApertureConstruction(materialLibrary);
                    if (apertureConstruction != null)
                    {
                        result.Add(apertureConstruction);
                    }
                }
            }

            GlobalConstructionSet globalConstructionSet = modelEnergyProperties.GlobalConstructionSet;
            if (globalConstructionSet != null)
            {
                List<AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged>> constructionAbridges_Honeybee = modelEnergyProperties.GlobalConstructionSet.Constructions;
                if (constructionAbridges_Honeybee != null)
                {
                    materialLibrary.AddMaterials(globalConstructionSet.Materials?.ConvertAll(x => x.Obj as HoneybeeSchema.Energy.IMaterial));

                    foreach (AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> @object in constructionAbridges_Honeybee)
                    {
                        WindowConstructionAbridged construction_Honeybee = @object.Obj as WindowConstructionAbridged;
                        if (construction_Honeybee == null)
                        {
                            continue;
                        }

                        ApertureConstruction apertureConstruction = construction_Honeybee?.ToSAM_ApertureConstruction(materialLibrary);
                        if (apertureConstruction != null)
                        {
                            PanelType? panelType = Query.PanelType(globalConstructionSet, apertureConstruction.Name);
                            if (panelType != null && panelType.HasValue && panelType.Value != PanelType.Undefined)
                            {
                                apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, panelType);
                            }

                            result.Add(apertureConstruction);
                        }
                    }
                }
            }

            return result;
        }
    }
}
using HoneybeeSchema;
using HoneybeeSchema.Energy;
using SAM.Core;
using System.Collections.Generic;
using System.Linq;

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

            List<IConstruction> constructions_Honeybee = modelEnergyProperties.ConstructionList?.ToList();

            List<HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet>> constructionSets = modelEnergyProperties.ConstructionSets;
            if (constructionSets != null)
            {
                foreach (HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet> anyOf in constructionSets)
                {
                    List<ApertureConstruction> apertureConstructions_Temp = anyOf.Obj is ConstructionSetAbridged ? Query.ApertureConstructions((ConstructionSetAbridged)anyOf.Obj, constructions_Honeybee, materialLibrary) : Query.ApertureConstructions(anyOf.Obj as ConstructionSet, materialLibrary);
                    if (apertureConstructions_Temp != null && apertureConstructions_Temp.Count != 0)
                    {
                        result.AddRange(apertureConstructions_Temp);
                    }
                }
            }

            if (constructions_Honeybee != null)
            {
                foreach (IConstruction construction_Honeybee in constructions_Honeybee)
                {
                    ApertureConstruction apertureConstruction = construction_Honeybee?.ToSAM_ApertureConstruction(materialLibrary);
                    if (apertureConstruction != null && result.Find(x => x.Name == apertureConstruction.Name) == null)
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
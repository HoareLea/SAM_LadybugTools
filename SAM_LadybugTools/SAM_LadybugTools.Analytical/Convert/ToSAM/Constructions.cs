using HoneybeeSchema;
using HoneybeeSchema.Energy;
using SAM.Core;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<Construction> ToSAM_Constructions(this ModelEnergyProperties modelEnergyProperties, MaterialLibrary materialLibrary = null)
        {
            if(modelEnergyProperties == null)
            {
                return null;
            }

            List<Construction> result = new List<Construction>();

            List<IConstruction> constructions_Honeybee = modelEnergyProperties.ConstructionList?.ToList();

            List<HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet>> constructionSets = modelEnergyProperties.ConstructionSets;
            if(constructionSets != null)
            {
                foreach(HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet> anyOf in constructionSets)
                {
                    List<Construction> constructions_Temp = anyOf.Obj is ConstructionSetAbridged ? Query.Constructions((ConstructionSetAbridged)anyOf.Obj, constructions_Honeybee, materialLibrary) : Query.Constructions(anyOf.Obj as ConstructionSet, materialLibrary);
                    if(constructions_Temp != null && constructions_Temp.Count != 0)
                    {
                        result.AddRange(constructions_Temp);
                    }
                }
            }


            if(constructions_Honeybee != null)
            {
                foreach (IConstruction construction_Honeybee in constructions_Honeybee)
                {
                    Construction construction = construction_Honeybee?.ToSAM_Construction(materialLibrary);
                    if (construction != null && result.Find(x => x.Name == construction.Name) == null)
                    {
                        result.Add(construction);
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
                        IConstruction construction_Honeybee = @object.Obj as IConstruction;
                        if (construction_Honeybee == null)
                        {
                            continue;
                        }

                        if (construction_Honeybee is WindowConstructionAbridged)
                        {
                            continue;
                        }

                        Construction construction = construction_Honeybee?.ToSAM_Construction(materialLibrary);
                        if (construction != null && result.Find(x => x.Name == construction.Name) == null)
                        {
                            PanelType? panelType = Query.PanelType(globalConstructionSet, construction.Name);
                            if(panelType != null && panelType.HasValue && panelType.Value != PanelType.Undefined)
                            {
                                if(result.Find(x => x.PanelType() != PanelType.Undefined && x.PanelType() == panelType.Value) != null)
                                {
                                    continue;
                                }

                                construction.SetValue(ConstructionParameter.DefaultPanelType, panelType);
                            }

                            result.Add(construction);
                        }
                    }
                }
            }

            return result;
        }
    }
}
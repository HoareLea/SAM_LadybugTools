using HoneybeeSchema;
using HoneybeeSchema.Energy;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static List<Construction> Constructions(this ConstructionSet constructionSet, MaterialLibrary materialLibrary)
        {
            if (constructionSet == null )
            {
                return null;
            }
            List<Construction> result = new List<Construction>();

            if(constructionSet.AirBoundaryConstruction != null)
            {
                Construction construction = constructionSet.AirBoundaryConstruction.ToSAM_Construction(materialLibrary);
                if(construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.Air.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.FloorSet.ExteriorConstruction != null)
            {
                Construction construction = constructionSet.FloorSet.ExteriorConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.FloorExposed.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.FloorSet.InteriorConstruction != null)
            {
                Construction construction = constructionSet.FloorSet.InteriorConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.FloorInternal.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.FloorSet.GroundConstruction != null)
            {
                Construction construction = constructionSet.FloorSet.GroundConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.SlabOnGrade.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.RoofCeilingSet.ExteriorConstruction != null)
            {
                Construction construction = constructionSet.RoofCeilingSet.ExteriorConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.Roof.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.RoofCeilingSet.GroundConstruction != null)
            {
                Construction construction = constructionSet.RoofCeilingSet.GroundConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.UndergroundCeiling.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.RoofCeilingSet.InteriorConstruction != null)
            {
                Construction construction = constructionSet.RoofCeilingSet.InteriorConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.Ceiling.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.ShadeConstruction != null)
            {
                Construction construction = constructionSet.ShadeConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.Shade.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.WallSet.ExteriorConstruction != null)
            {
                Construction construction = constructionSet.WallSet.ExteriorConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.WallSet.GroundConstruction != null)
            {
                Construction construction = constructionSet.WallSet.GroundConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.UndergroundWall.ToString());
                    result.Add(construction);
                }
            }

            if (constructionSet.WallSet.InteriorConstruction != null)
            {
                Construction construction = constructionSet.WallSet.InteriorConstruction.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallInternal.ToString());
                    result.Add(construction);
                }
            }

            return result;
        }

        public static List<Construction> Constructions(this ConstructionSetAbridged constructionSetAbridged, IEnumerable<IConstruction> constructions, MaterialLibrary materialLibrary)
        {
            if(constructionSetAbridged == null || constructions == null)
            {
                return null;
            }

            List<IConstruction> constructions_Temp = (new List<IConstruction>(constructions)).FindAll(x => x != null);

            List<Construction> result = new List<Construction>();

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.AirBoundaryConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.AirBoundaryConstruction)?.ToSAM_Construction(materialLibrary);
                if(construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.Air.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.FloorSet.ExteriorConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.FloorSet.ExteriorConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.FloorExposed.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.FloorSet.InteriorConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.FloorSet.InteriorConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.FloorInternal.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.FloorSet.GroundConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.FloorSet.GroundConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.SlabOnGrade.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.RoofCeilingSet.ExteriorConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.RoofCeilingSet.ExteriorConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.Roof.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.RoofCeilingSet.GroundConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.RoofCeilingSet.GroundConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.UndergroundCeiling.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.RoofCeilingSet.InteriorConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.RoofCeilingSet.InteriorConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.Ceiling.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.ShadeConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.ShadeConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.Shade.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.WallSet.GroundConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.WallSet.GroundConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.UndergroundWall.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.WallSet.InteriorConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.WallSet.InteriorConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallInternal.ToString());
                    result.Add(construction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.WallSet.ExteriorConstruction))
            {
                Construction construction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.WallSet.ExteriorConstruction)?.ToSAM_Construction(materialLibrary);
                if (construction != null)
                {
                    construction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(construction);
                }
            }

            return result;
        }
    }
}
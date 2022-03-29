using HoneybeeSchema;
using HoneybeeSchema.Energy;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static List<ApertureConstruction> ApertureConstructions(this ConstructionSet constructionSet, MaterialLibrary materialLibrary)
        {
            if (constructionSet == null )
            {
                return null;
            }
            List<ApertureConstruction> result = new List<ApertureConstruction>();

            if(constructionSet.ApertureSet.InteriorConstruction != null)
            {
                ApertureConstruction apertureConstruction = (constructionSet.ApertureSet.InteriorConstruction as IConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if(apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, Analytical.PanelType.WallInternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (constructionSet.ApertureSet.OperableConstruction != null)
            {
                ApertureConstruction apertureConstruction = (constructionSet.ApertureSet.OperableConstruction as IConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (constructionSet.ApertureSet.SkylightConstruction != null)
            {
                ApertureConstruction apertureConstruction = (constructionSet.ApertureSet.SkylightConstruction as IConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, Analytical.PanelType.Roof.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (constructionSet.ApertureSet.WindowConstruction != null)
            {
                ApertureConstruction apertureConstruction = (constructionSet.ApertureSet.WindowConstruction as IConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (constructionSet.DoorSet.ExteriorConstruction != null)
            {
                ApertureConstruction apertureConstruction = (constructionSet.DoorSet.ExteriorConstruction as IConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (constructionSet.DoorSet.ExteriorGlassConstruction != null)
            {
                ApertureConstruction apertureConstruction = (constructionSet.DoorSet.ExteriorGlassConstruction as IConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, Analytical.PanelType.CurtainWall.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (constructionSet.DoorSet.InteriorConstruction != null)
            {
                ApertureConstruction apertureConstruction = (constructionSet.DoorSet.InteriorConstruction as IConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, Analytical.PanelType.WallInternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (constructionSet.DoorSet.InteriorGlassConstruction != null)
            {
                ApertureConstruction apertureConstruction = (constructionSet.DoorSet.InteriorGlassConstruction as IConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, Analytical.PanelType.CurtainWall.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (constructionSet.DoorSet.OverheadConstruction != null)
            {
                ApertureConstruction apertureConstruction = (constructionSet.DoorSet.OverheadConstruction as IConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            return result;
        }

        public static List<ApertureConstruction> ApertureConstructions(this ConstructionSetAbridged constructionSetAbridged, IEnumerable<IConstruction> constructions, MaterialLibrary materialLibrary)
        {
            if(constructionSetAbridged == null || constructions == null)
            {
                return null;
            }

            List<IConstruction> constructions_Temp = (new List<IConstruction>(constructions)).FindAll(x => x != null);

            List<ApertureConstruction> result = new List<ApertureConstruction>();

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.ApertureSet.InteriorConstruction))
            {
                ApertureConstruction apertureConstruction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.ApertureSet.InteriorConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if(apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallInternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.ApertureSet.OperableConstruction))
            {
                ApertureConstruction apertureConstruction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.ApertureSet.OperableConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.ApertureSet.SkylightConstruction))
            {
                ApertureConstruction apertureConstruction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.ApertureSet.SkylightConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.Roof.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.ApertureSet.WindowConstruction))
            {
                ApertureConstruction apertureConstruction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.ApertureSet.WindowConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.DoorSet.ExteriorConstruction))
            {
                ApertureConstruction apertureConstruction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.DoorSet.ExteriorConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.DoorSet.ExteriorGlassConstruction))
            {
                ApertureConstruction apertureConstruction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.DoorSet.ExteriorGlassConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.CurtainWall.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.DoorSet.InteriorConstruction))
            {
                ApertureConstruction apertureConstruction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.DoorSet.InteriorConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallInternal.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.DoorSet.InteriorGlassConstruction))
            {
                ApertureConstruction apertureConstruction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.DoorSet.InteriorGlassConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.CurtainWall.ToString());
                    result.Add(apertureConstruction);
                }
            }

            if (!string.IsNullOrWhiteSpace(constructionSetAbridged.DoorSet.OverheadConstruction))
            {
                ApertureConstruction apertureConstruction = constructions_Temp.Find(x => x.Identifier == constructionSetAbridged.DoorSet.OverheadConstruction)?.ToSAM_ApertureConstruction(materialLibrary);
                if (apertureConstruction != null)
                {
                    apertureConstruction.SetValue(ConstructionParameter.DefaultPanelType, Analytical.PanelType.WallExternal.ToString());
                    result.Add(apertureConstruction);
                }
            }
            return result;
        }
    }
}
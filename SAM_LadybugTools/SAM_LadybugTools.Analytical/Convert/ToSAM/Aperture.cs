﻿using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Aperture ToSAM(this HoneybeeSchema.Aperture aperture, PanelType panelType, IEnumerable<ApertureConstruction> apertureConstructions = null)
        {
            if(aperture == null)
            {
                return null;
            }

            Geometry.Spatial.Face3D face3D = Geometry.LadybugTools.Convert.ToSAM(aperture.Geometry);
            if(face3D == null)
            {
                return null;
            }

            ApertureConstruction apertureConstruction = null;
            if (apertureConstructions != null && aperture.Properties?.Energy?.Construction != null)
            {
                foreach (ApertureConstruction apertureConstruction_Temp in apertureConstructions)
                {
                    if (apertureConstruction_Temp == null)
                    {
                        continue;
                    }

                    if (apertureConstruction_Temp.ApertureType != ApertureType.Window)
                    {
                        continue;
                    }

                    if (aperture.Properties.Energy.Construction == apertureConstruction_Temp.Name)
                    {
                        apertureConstruction = apertureConstruction_Temp;
                        break;
                    }
                }
            }

            if (apertureConstruction == null)
            {
                if (apertureConstructions != null)
                {
                    foreach (ApertureConstruction apertureConstruction_Temp in apertureConstructions)
                    {
                        if (apertureConstruction_Temp == null)
                        {
                            continue;
                        }

                        if (apertureConstruction_Temp.ApertureType != ApertureType.Window)
                        {
                            continue;
                        }

                        if (apertureConstruction_Temp.TryGetValue(ApertureConstructionParameter.DefaultPanelType, out string panelTypeString) && !string.IsNullOrWhiteSpace(panelTypeString))
                        {
                            PanelType panelType_Temp = Core.Query.Enum<PanelType>(panelTypeString);
                            if (panelType_Temp == panelType)
                            {
                                apertureConstruction = apertureConstruction_Temp;
                                break;
                            }
                        }
                    }
                }
            }

            if (apertureConstruction == null)
            {
                AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> construction_Honeybee = Query.DefaultApertureConstruction(ApertureType.Window, panelType);
                if (construction_Honeybee != null)
                {
                    apertureConstruction = construction_Honeybee.ToSAM_ApertureConstruction();
                }
            }

            if (apertureConstruction == null)
            {
                apertureConstruction = new ApertureConstruction(aperture.DisplayName, ApertureType.Window);
            }

            Aperture result = new Aperture(apertureConstruction, face3D, Analytical.Query.OpeningLocation(face3D));

            return result;
        }

        public static Aperture ToSAM(this HoneybeeSchema.Door door, PanelType panelType, IEnumerable<ApertureConstruction> apertureConstructions = null)
        {
            if (door == null)
            {
                return null;
            }

            Geometry.Spatial.Face3D face3D = Geometry.LadybugTools.Convert.ToSAM(door.Geometry);
            if (face3D == null)
            {
                return null;
            }

            ApertureType apertureType = door.IsGlass ? ApertureType.Window : ApertureType.Door;
            PanelType panelType_Temp = panelType;
            if(apertureType == ApertureType.Window && panelType_Temp == PanelType.WallExternal)
            {
                panelType_Temp = PanelType.CurtainWall;
            }

            ApertureConstruction apertureConstruction = null;
            if (apertureConstructions != null && door.Properties?.Energy?.Construction != null)
            {
                foreach(ApertureConstruction apertureConstruction_Temp in apertureConstructions)
                {
                    if(apertureConstruction_Temp == null)
                    {
                        continue;
                    }

                    if (apertureConstruction_Temp.ApertureType != apertureType)
                    {
                        continue;
                    }

                    if(door.Properties.Energy.Construction == apertureConstruction_Temp.Name)
                    {
                        apertureConstruction = apertureConstruction_Temp;
                        break;
                    }
                }
            }

            if (apertureConstructions != null)
            {
                foreach (ApertureConstruction apertureConstruction_Temp in apertureConstructions)
                {
                    if (apertureConstruction_Temp == null)
                    {
                        continue;
                    }

                    if (apertureConstruction_Temp.ApertureType != apertureType)
                    {
                        continue;
                    }

                    if (apertureConstruction_Temp.TryGetValue(ApertureConstructionParameter.DefaultPanelType, out string panelTypeString) && !string.IsNullOrWhiteSpace(panelTypeString))
                    {
                        PanelType panelType_ApertureConstructin = Core.Query.Enum<PanelType>(panelTypeString);
                        if (panelType_ApertureConstructin == panelType_Temp)
                        {
                            apertureConstruction = apertureConstruction_Temp;
                            break;
                        }
                    }
                }
            }

            if (apertureConstruction == null)
            {
                AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> construction_Honeybee = Query.DefaultApertureConstruction(apertureType, panelType);
                if (construction_Honeybee != null)
                {
                    apertureConstruction = construction_Honeybee.ToSAM_ApertureConstruction();
                }
            }

            if (apertureConstruction == null)
            {
                apertureConstruction = new ApertureConstruction(door.DisplayName, apertureType);
            }
            
            Aperture result = new Aperture(apertureConstruction, face3D, Analytical.Query.OpeningLocation(face3D));

            return result;
        }
    }
}
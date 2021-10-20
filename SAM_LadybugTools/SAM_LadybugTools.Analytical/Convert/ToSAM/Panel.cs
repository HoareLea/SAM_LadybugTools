using HoneybeeSchema;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Panel ToSAM(this Face face, IEnumerable<Construction> constructions = null, IEnumerable<ApertureConstruction> apertureConstructions = null) 
        {
            if(face == null)
            {
                return null;
            }

            Geometry.Spatial.Face3D face3D = Geometry.LadybugTools.Convert.ToSAM(face.Geometry);
            if(face3D == null)
            {
                return null;
            }

            PanelType panelType = PanelType.Undefined;

            PanelGroup panelGroup = face.FaceType.ToSAM();

            Construction construction = null;
            if (constructions != null && face.Properties?.Energy?.Construction != null)
            {
                foreach(Construction construction_Temp in constructions )
                {
                    if(construction_Temp.Name == face.Properties.Energy.Construction)
                    {
                        construction = construction_Temp;
                        break;
                    }
                }
            }

            AnyOf<Ground, Outdoors, Adiabatic, Surface> boundaryCondition = face.BoundaryCondition;
            if(boundaryCondition.Obj is Ground)
            {
                switch(panelGroup)
                {
                    case PanelGroup.Floor:
                        panelType = PanelType.Floor;
                        break;

                    case PanelGroup.Other:
                        panelType = PanelType.Air;
                        break;

                    case PanelGroup.Roof:
                        panelType = PanelType.UndergroundSlab;
                        break;

                    case PanelGroup.Wall:
                        panelType = PanelType.UndergroundWall;
                        break;
                }
            }
            else if (boundaryCondition.Obj is Adiabatic)
            {
                panelType = PanelType.Undefined;
            }
            else if (boundaryCondition.Obj is Outdoors)
            {
                switch (panelGroup)
                {
                    case PanelGroup.Floor:
                        panelType = PanelType.FloorExposed;
                        break;

                    case PanelGroup.Other:
                        panelType = PanelType.Shade;
                        break;

                    case PanelGroup.Roof:
                        panelType = PanelType.Roof;
                        break;

                    case PanelGroup.Wall:
                        panelType = PanelType.WallExternal;
                        break;
                }
            }
            else if(boundaryCondition.Obj is Surface)
            {
                Surface surface = ((Surface)boundaryCondition.Obj);

                construction = new Construction(surface.BoundaryConditionObjects?.FirstOrDefault());
            }

            if(construction == null)
            {

                AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> construction_Honeybee = Query.DefaultConstruction(panelType);
                if(construction_Honeybee != null)
                {
                    construction = construction_Honeybee.ToSAM_Construction();
                }
            }

            if (construction == null)
            {
                construction = new Construction(face.Identifier);
            }

            Panel panel = Create.Panel(construction, panelType, face3D);
            
            if(face.Apertures != null)
            {
                foreach(HoneybeeSchema.Aperture aperture_HoneybeeSchema in face.Apertures)
                {
                    Aperture aperture = aperture_HoneybeeSchema?.ToSAM(panelType.Internal(), apertureConstructions);
                    if(aperture != null)
                    {
                        panel.AddAperture(aperture);
                    }
                }
            }

            if (face.Doors != null)
            {
                foreach (HoneybeeSchema.Door door in face.Doors)
                {
                    Aperture aperture = door?.ToSAM(panelType.Internal(), apertureConstructions);
                    if (aperture != null)
                    {
                        panel.AddAperture(aperture);
                    }
                }
            }

            return panel;
        }

        public static Panel ToSAM(this Shade shade, IEnumerable<Construction> constructions = null)
        {
            if(shade == null)
            {
                return null;
            }

            Geometry.Spatial.Face3D face3D = Geometry.LadybugTools.Convert.ToSAM(shade.Geometry);
            if (face3D == null)
            {
                return null;
            }

            Construction construction = null;
            if (constructions != null && shade.Properties?.Energy?.Construction != null)
            {
                foreach (Construction construction_Temp in constructions)
                {
                    if (construction_Temp.Name == shade.Properties.Energy.Construction)
                    {
                        construction = construction_Temp;
                        break;
                    }
                }
            }

            if (construction == null)
            {

                AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> construction_Honeybee = Query.DefaultConstruction(PanelType.Shade);
                if (construction_Honeybee != null)
                {
                    construction = construction_Honeybee.ToSAM_Construction();
                }
            }

            if (construction == null)
            {
                construction = new Construction(shade.Identifier);
            }

            return Create.Panel(construction, PanelType.Shade, face3D);
        }
    }
}
using HoneybeeSchema;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Panel ToSAM(this Face face)
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

            Panel panel = Create.Panel(construction, panelType, face3D);
            
            if(face.Apertures != null)
            {
                foreach(HoneybeeSchema.Aperture aperture_HoneybeeSchema in face.Apertures)
                {
                    Aperture aperture = aperture_HoneybeeSchema?.ToSAM();
                    if(aperture != null)
                    {
                        panel.AddAperture(aperture);
                    }
                }
            }

            if (face.Doors != null)
            {
                foreach (Door door in face.Doors)
                {
                    Aperture aperture = door?.ToSAM();
                    if (aperture != null)
                    {
                        panel.AddAperture(aperture);
                    }
                }
            }

            return panel;
        }
    }
}
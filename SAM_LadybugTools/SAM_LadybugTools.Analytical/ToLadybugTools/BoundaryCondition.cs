using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static AnyOf<Ground, Outdoors, Adiabatic, Surface> ToLadybugTools_BoundaryCondition(this Panel panel)
        {
            if (panel == null)
                return null;
            switch (panel.PanelType)
            {
                case PanelType.UndergroundWall:
                case PanelType.SlabOnGrade:
                case PanelType.UndergroundSlab:
                case PanelType.Floor:
                    return new Ground();
                
                case PanelType.Wall:
                case PanelType.CurtainWall:
                case PanelType.WallExternal:
                case PanelType.Roof:
                case PanelType.SolarPanel:
                case PanelType.Shade:
                case PanelType.FloorExposed:
                case PanelType.FloorRaised:
                    return new Outdoors();

                case PanelType.Undefined:
                    return new Adiabatic();

                case PanelType.FloorInternal:
                case PanelType.WallInternal:
                case PanelType.Ceiling:
                case PanelType.UndergroundCeiling:
                    //return new Surface()
                    return null;

            }

            return null;
        }
    }
}

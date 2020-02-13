using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoneybeeDotNet;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static AnyOf<Ground, Outdoors, Adiabatic, Surface> ToLadybugTools_BoundaryCondition(this Panel panel)
        {
            if (panel == null)
                return null;
            //TOD: Finish all types//
            switch (panel.PanelType)
            {
                case PanelType.SlabOnGrade:
                    return new Ground();
                case PanelType.WallExternal:
                case PanelType.Roof:
                    return new Outdoors();
                case PanelType.Undefined:
                    return new Adiabatic();
                case PanelType.FloorInternal:
                case PanelType.WallInternal:
                    //return new Surface()
                    return null;
            }

            return null;
        }
    }
}

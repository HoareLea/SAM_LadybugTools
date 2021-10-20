using HoneybeeSchema;
using SAM.Core;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> DefaultApertureConstruction(this ApertureType apertureType, bool @internal)
        {
            string name = DefaultConstructionName(apertureType, @internal);
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            GlobalConstructionSet globalConstructionSet = GlobalConstructionSet.Default;
            if (globalConstructionSet == null)
            {
                return null;
            }


            List<AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged>> constructions = globalConstructionSet.Constructions;
            foreach(AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> construction in constructions)
            {
                string name_Temp = null;

                if (construction.Obj is OpaqueConstructionAbridged)
                {
                    name_Temp = ((OpaqueConstructionAbridged)construction.Obj).Identifier;
                }
                else if (construction.Obj is WindowConstructionAbridged)
                {
                    name_Temp = ((WindowConstructionAbridged)construction.Obj).Identifier;
                }
                else if (construction.Obj is ShadeConstruction)
                {
                    name_Temp = ((ShadeConstruction)construction.Obj).Identifier;
                }
                else if (construction.Obj is AirBoundaryConstructionAbridged)
                {
                    name_Temp = ((AirBoundaryConstructionAbridged)construction.Obj).Identifier;
                }

                if(string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                if(name.Equals(name_Temp))
                {
                    return construction;
                }
            }


            return null;
        }
    }
}
using HoneybeeSchema;
using HoneybeeSchema.Energy;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ApertureConstruction ToSAM(this WindowConstructionAbridged windowConstructionAbridged, Core.MaterialLibrary materialLibrary = null)
        {
            if(windowConstructionAbridged == null)
            {
                return null;
            }

            List<ConstructionLayer> constructionLayers = Query.ConstructionLayers(materialLibrary, windowConstructionAbridged.Materials);

            ApertureConstruction result = new ApertureConstruction(System.Guid.NewGuid(), windowConstructionAbridged.Identifier, ApertureType.Window, constructionLayers);
            return result;
        }

        public static ApertureConstruction ToSAM_ApertureConstruction(this IConstruction construction, Core.MaterialLibrary materialLibrary = null)
        {
            if (construction == null)
            {
                return null;
            }

            if(construction is WindowConstructionAbridged)
            {
                return ((WindowConstructionAbridged)construction).ToSAM(materialLibrary);
            }

            return null;
        }

        public static ApertureConstruction ToSAM_ApertureConstruction(this AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> construction, Core.MaterialLibrary materialLibrary = null)
        {
            if (construction == null)
            {
                return null;
            }

            string name = null;
            List<string> materialNames = null;
            ApertureType apertureType = ApertureType.Undefined;


            if (construction.Obj is OpaqueConstructionAbridged)
            {
                name = ((OpaqueConstructionAbridged)construction.Obj).Identifier;
                materialNames = ((OpaqueConstructionAbridged)construction.Obj).Materials;
                apertureType = ApertureType.Door;
            }
            else if (construction.Obj is WindowConstructionAbridged)
            {
                name = ((WindowConstructionAbridged)construction.Obj).Identifier;
                materialNames = ((WindowConstructionAbridged)construction.Obj).Materials;
                apertureType = ApertureType.Window;
            }
            else if (construction.Obj is ShadeConstruction)
            {
                name = ((ShadeConstruction)construction.Obj).Identifier;
                apertureType = ApertureType.Door;
            }
            else if (construction.Obj is AirBoundaryConstructionAbridged)
            {
                name = ((AirBoundaryConstructionAbridged)construction.Obj).Identifier;
                apertureType = ApertureType.Window;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            List<ConstructionLayer> constructionLayers = null;
            if (materialNames != null)
            {
                constructionLayers = new List<ConstructionLayer>();
                foreach (string materialName in materialNames)
                {
                    double thickness = 0;

                    AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas> material_Honeybee = SAM.Core.LadybugTools.Query.DefaultMaterial(materialName);
                    if (material_Honeybee != null)
                    {
                        if (material_Honeybee.Obj is EnergyMaterial)
                        {
                            thickness = ((EnergyMaterial)material_Honeybee.Obj).Thickness;
                        }
                        else if (material_Honeybee.Obj is EnergyMaterialNoMass)
                        {

                        }
                        else if (material_Honeybee.Obj is EnergyWindowMaterialGlazing)
                        {
                            thickness = ((EnergyWindowMaterialGlazing)material_Honeybee.Obj).Thickness;
                        }
                        else if (material_Honeybee.Obj is EnergyWindowMaterialGas)
                        {
                            thickness = ((EnergyWindowMaterialGas)material_Honeybee.Obj).Thickness;
                        }
                    }

                    constructionLayers.Add(new ConstructionLayer(materialName, thickness));
                }
            }

            return new ApertureConstruction(System.Guid.NewGuid(), name, apertureType, constructionLayers);
        }
    }
}
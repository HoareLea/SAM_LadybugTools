using HoneybeeSchema;
using System.Collections.Generic;
using System.Reflection;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        public static AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas> DefaultMaterial(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            GlobalConstructionSet globalConstructionSet = GlobalConstructionSet.Default;
            if (globalConstructionSet == null)
            {
                return null;
            }

            List<AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas>> materials = globalConstructionSet.Materials;
            if (materials == null)
            {
                return null;
            }

            foreach (AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas> material in materials)
            {
                if (material.Obj is EnergyMaterial)
                {
                    if (name.Equals(((EnergyMaterial)material.Obj).Identifier))
                    {
                        return material;
                    }
                }
                else if (material.Obj is EnergyMaterialNoMass)
                {
                    if (name.Equals(((EnergyMaterialNoMass)material.Obj).Identifier))
                    {
                        return material;
                    }
                }
                else if (material.Obj is EnergyWindowMaterialGlazing)
                {
                    if (name.Equals(((EnergyWindowMaterialGlazing)material.Obj).Identifier))
                    {
                        return material;
                    }
                }
                else if (material.Obj is EnergyWindowMaterialGas)
                {
                    if (name.Equals(((EnergyWindowMaterialGas)material.Obj).Identifier))
                    {
                        return material;
                    }
                }
            }

            return null;
        }

    }
}
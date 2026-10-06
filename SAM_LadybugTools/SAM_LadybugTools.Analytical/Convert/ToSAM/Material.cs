// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Core.IMaterial ToSAM(this HoneybeeSchema.Energy.IMaterial material)
        {
            if (material == null)
            {
                return null;
            }

            if (material is EnergyWindowFrame)
            {
                return ((EnergyWindowFrame)material).ToSAM();
            }

            if(material is EnergyWindowMaterialGlazing)
            {
                return ((EnergyWindowMaterialGlazing)material).ToSAM();
            }

            if (material is EnergyWindowMaterialGas)
            {
                return ((EnergyWindowMaterialGas)material).ToSAM();
            }

            if (material is EnergyMaterial)
            {
                EnergyMaterial energyMaterial = material as EnergyMaterial;

                // SAM round-trip metadata always wins over the legacy density heuristic
                if (Query.TryGetSAMMaterialType(energyMaterial, out Core.MaterialType materialType) && materialType != Core.MaterialType.Undefined)
                {
                    if (materialType == Core.MaterialType.Gas)
                    {
                        return energyMaterial.ToSAM_GasMaterial();
                    }

                    return energyMaterial.ToSAM();
                }

                if (energyMaterial.Density < 5)
                {
                    return ((EnergyMaterial)material).ToSAM_GasMaterial();
                }
                else
                {
                    return ((EnergyMaterial)material).ToSAM();
                }
            }

            if(material is EnergyMaterialNoMass)
            {
                EnergyMaterialNoMass energyMaterialNoMass = (EnergyMaterialNoMass)material;

                // SAM air gaps travel as EnergyMaterialNoMass; restore them as gas when marked
                if (Query.TryGetSAMMaterialType(energyMaterialNoMass, out Core.MaterialType materialType) && materialType == Core.MaterialType.Gas)
                {
                    return energyMaterialNoMass.ToSAM_GasMaterial();
                }

                return energyMaterialNoMass.ToSAM();
            }

            return null;
        }

        public static Core.IMaterial ToSAM(AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas> material)
        {
            if(material?.Obj is HoneybeeSchema.Energy.IMaterial)
            {
                return ToSAM((HoneybeeSchema.Energy.IMaterial)material.Obj);
            }

            return null;
        }
    }
}
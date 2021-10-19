using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Core.IMaterial ToSAM(this HoneybeeSchema.Energy.IMaterial material)
        {
            if(material == null)
            {
                return null;
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
                return ((EnergyMaterial)material).ToSAM();
            }

            return null;
        }

        public static Core.IMaterial ToSAM(AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas> material)
        {
            if(material.Obj is HoneybeeSchema.Energy.IMaterial)
            {
                return ToSAM((HoneybeeSchema.Energy.IMaterial)material.Obj);
            }

            return null;
        }
    }
}
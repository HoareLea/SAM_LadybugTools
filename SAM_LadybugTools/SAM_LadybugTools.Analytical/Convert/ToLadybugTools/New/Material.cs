
namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static HoneybeeSchema.Energy.IMaterial ToLadybugTools(this Core.IMaterial material, double thickness, bool windowMaterial = false)
        {
            if (material == null)
            {
                return null;
            }

            if (windowMaterial && material is Core.GasMaterial)
            {
                if(material is Core.GasMaterial)
                {
                    return ToLadybugTools_EnergyWindowMaterialGas((Core.GasMaterial)material, thickness);
                }

                if (material is Core.TransparentMaterial)
                {
                    return ToLadybugTools((Core.TransparentMaterial)material, thickness);
                }
            }

            if (material is Core.GasMaterial)
            {
                return ToLadybugTools((Core.GasMaterial)material);
            }

            if (material is Core.OpaqueMaterial)
            {
                return ToLadybugTools((Core.OpaqueMaterial)material, thickness);
            }

            return null;
        }
    }
}
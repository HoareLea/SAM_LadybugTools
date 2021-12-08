using HoneybeeSchema;
using SAM.Geometry.LadybugTools;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Shade ToLadybugTools_Shade(this IPartition partition)
        {
            if (partition == null)
                return null;

            Face3D face3D = partition.Face3D?.ToLadybugTools();
            if (face3D == null)
                return null;

            ShadePropertiesAbridged shadePropertiesAbridged = new ShadePropertiesAbridged();

            Shade shade = new Shade(Query.UniqueName(partition), face3D, shadePropertiesAbridged, partition.Name);

            return shade;
        }
    }
}
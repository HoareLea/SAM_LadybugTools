using SAM.Geometry.Spatial;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Convert
    {
        public static Plane ToSAM(this HoneybeeSchema.Plane plane)
        {
            if (plane == null)
            {
                return null;
            }

            Point3D origin = plane.O?.ToSAM();
            if (origin == null || !origin.IsValid())
            {
                return null;
            }

            Vector3D normal = plane.N?.ToSAM_Vector3D();
            if (normal == null || !normal.IsValid())
            {
                return null;
            }

            Vector3D axisX = plane.X?.ToSAM_Vector3D();
            if (axisX == null || !normal.IsValid())
            {
                return new Plane(origin, normal);
            }

            Vector3D axisY = normal.AxisY(axisX);

            return new Plane(origin, axisX, axisY);
        }
    }
}
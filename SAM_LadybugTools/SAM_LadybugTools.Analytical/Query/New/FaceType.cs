namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static HoneybeeSchema.FaceType? FaceType(this IPartition partition)
        {
            if(partition == null)
            {
                return null;
            }

            if(partition is AirPartition)
            {
                return HoneybeeSchema.FaceType.AirBoundary;
            }

            if(partition is Roof)
            {
                return HoneybeeSchema.FaceType.RoofCeiling;
            }

            if(partition is Floor)
            {
                return HoneybeeSchema.FaceType.Floor;
            }

            if(partition is Wall)
            {
                return HoneybeeSchema.FaceType.Wall;
            }

            return null;
        }
    }
}
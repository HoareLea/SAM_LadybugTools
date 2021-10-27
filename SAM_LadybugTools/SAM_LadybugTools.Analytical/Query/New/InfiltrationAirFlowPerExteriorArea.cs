using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static double InfiltrationAirFlowPerExteriorArea(this ArchitecturalModel architecturalModel, Space space)
        {
            if (architecturalModel == null || space == null)
                return double.NaN;

            double volume = double.NaN;
            space.TryGetValue(SpaceParameter.Volume, out volume);
            if (double.IsNaN(volume))
                return double.NaN;

            InternalCondition internalCondintion = space.InternalCondition;
            if (internalCondintion == null)
                return double.NaN;

            double airFlow = SAM.Analytical.Query.CalculatedInfiltrationAirFlow(space);
            if (double.IsNaN(airFlow))
                return double.NaN;

            if (airFlow == 0)
                return 0;

            List<IPartition> partitions = architecturalModel.GetPartitions(space);
            if (partitions == null || partitions.Count == 0)
                return double.NaN;

            partitions.RemoveAll(x => !architecturalModel.ExposedToSun(x));

            double area = 0;
            foreach(Panel panel in partitions)
            {
                double area_Temp = panel.GetArea();
                if (!double.IsNaN(area_Temp))
                    area += area_Temp;
            }

            if (area == 0)
                return 0;

            return airFlow / area;
        }
    }
}
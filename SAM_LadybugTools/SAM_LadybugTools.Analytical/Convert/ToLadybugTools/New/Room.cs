using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Room ToLadybugTools(this Space space, ArchitecturalModel architecturalModel, double silverSpacing = Core.Tolerance.MacroDistance, double tolerance = Core.Tolerance.Distance)
        {
            if (space == null || architecturalModel == null)
                return null;

            List<Face> faces = null;

            List<IPartition> partitions = architecturalModel.OrientedPartitions(space, false, silverSpacing, tolerance);
            if(partitions != null)
            {
                faces = new List<Face>();
                foreach (IPartition partition in partitions)
                {
                    Face face = partition?.ToLadybugTools_Face(architecturalModel, space);
                    if(face == null)
                    {
                        continue;
                    }

                    faces.Add(face);
                }
            }

            string uniqueName = Core.LadybugTools.Query.UniqueName(space);

            RoomPropertiesAbridged roomPropertiesAbridged = new RoomPropertiesAbridged();

            Room result = new Room(uniqueName, faces, roomPropertiesAbridged, space.Name);

            InternalCondition internalCondition = space.InternalCondition;
            if(internalCondition != null)
            {
                string uniqueName_InternalCondition = Core.LadybugTools.Query.UniqueName(internalCondition);
                if (!string.IsNullOrWhiteSpace(uniqueName_InternalCondition))
                {
                    roomPropertiesAbridged = result.Properties;
                    RoomEnergyPropertiesAbridged roomEnergyPropertiesAbridged = roomPropertiesAbridged.Energy;
                    if (roomEnergyPropertiesAbridged == null)
                        roomEnergyPropertiesAbridged = new RoomEnergyPropertiesAbridged(programType: uniqueName_InternalCondition);

                    result.Properties.Energy = roomEnergyPropertiesAbridged;
                }
            }
            
            return result;
        }
    }
}
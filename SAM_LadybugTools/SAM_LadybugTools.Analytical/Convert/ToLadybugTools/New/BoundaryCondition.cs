using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static AnyOf<Ground, Outdoors, Adiabatic, Surface> ToLadybugTools_BoundaryCondition(this IPartition partition, BuildingModel buildingModel, Space space)
        {
            if (partition == null || buildingModel == null)
            {
                return null;
            }

            if (partition is IHostPartition)
            {
                if (((IHostPartition)partition).Adiabatic())
                {
                    return new Adiabatic();
                }
            }

            if (buildingModel.External(partition))
            {
                return new Outdoors();
            }

            if (buildingModel.Underground(partition))
            {
                if (!buildingModel.Internal(partition))
                {
                    return new Ground();
                }
            }

            int index_Adjacent = -1;
            List<Space> spaces = null;
            if (partition != null)
            {
                spaces = buildingModel.GetSpaces(partition);
                if (spaces != null && spaces.Count != 0)
                {
                    index_Adjacent = spaces.FindIndex(x => x.Guid != space.Guid);
                    index_Adjacent = buildingModel.UniqueIndex(spaces[index_Adjacent]);
                }
            }

            if (index_Adjacent == -1)
            {
                return null;
            }

            //boundaryConditionObjects have to be provided
            //https://www.ladybug.tools/honeybee-schema/model.html#tag/surface_model

            List<string> uniqueNames = new List<string>();
            uniqueNames.Add(Core.LadybugTools.Query.UniqueName(partition as Core.SAMObject, index_Adjacent));
            uniqueNames.Add(Core.LadybugTools.Query.UniqueName(spaces[index_Adjacent]));
            return new Surface(uniqueNames);
        }
    }
}
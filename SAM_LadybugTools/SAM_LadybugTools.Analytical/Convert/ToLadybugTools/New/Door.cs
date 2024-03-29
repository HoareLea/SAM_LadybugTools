﻿using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static HoneybeeSchema.Door ToLadybugTools(this IOpening opening, BuildingModel buildingModel, Space space)
        {
            if (opening == null || buildingModel == null)
                return null;

            OpeningType openingType = opening.Type();
            if (openingType == null)
            {
                return null;
            }

            //Opaque Windows to be replaced by Doors
            if (opening is Window && buildingModel.GetMaterialType(openingType.PaneMaterialLayers) != MaterialType.Opaque)
            {
                return null;
            }

            IHostPartition hostPartition = buildingModel.GetHostPartition(opening);
            
            int index = -1;
            int index_Adjacent = -1;
            List<Space> spaces = null;
            if (hostPartition != null)
            {
                spaces = buildingModel.GetSpaces(hostPartition);
                if (spaces != null && spaces.Count != 0)
                {
                    index = spaces.FindIndex(x => x.Guid == space.Guid);
                    index = buildingModel.UniqueIndex(spaces[index]);
                    
                    index_Adjacent = spaces.FindIndex(x => x.Guid != space.Guid);
                    index_Adjacent = buildingModel.UniqueIndex(spaces[index_Adjacent]);
                }
            }

            HoneybeeSchema.AnyOf<Outdoors, Surface> anyOf = null;
            if(index == -1 || index_Adjacent == -1)
            {
                anyOf = new Outdoors();
            }
            else
            {
                bool reversed = index_Adjacent < index;
                List<string> uniqueNames = new List<string>();
                uniqueNames.Add(Core.LadybugTools.Query.UniqueName(opening as SAMObject, index_Adjacent));
                uniqueNames.Add(Query.UniqueName(hostPartition, index_Adjacent));
                uniqueNames.Add(Query.UniqueName(spaces[index_Adjacent]));
                anyOf = new Surface(uniqueNames);
            }

            Face3D face3D = Geometry.LadybugTools.Convert.ToLadybugTools(opening);

            DoorEnergyPropertiesAbridged doorEnergyPropertiesAbridged = new DoorEnergyPropertiesAbridged(construction: Query.UniqueName(opening.Type(), !(index_Adjacent != -1 && index <= index_Adjacent)));

            return new HoneybeeSchema.Door(
                identifier: Core.LadybugTools.Query.UniqueName(opening as SAMObject, index),
                geometry: face3D,
                boundaryCondition: anyOf,
                properties: new DoorPropertiesAbridged(doorEnergyPropertiesAbridged),
                displayName: opening.Name);
        }
    }
}
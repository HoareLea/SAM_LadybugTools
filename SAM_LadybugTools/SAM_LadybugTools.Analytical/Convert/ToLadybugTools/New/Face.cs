using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Face ToLadybugTools_Face(this IPartition partition, BuildingModel buildingModel, Space space)
        {
            Face3D face3D = Geometry.LadybugTools.Convert.ToLadybugTools(partition?.Face3D);
            if (face3D == null)
            {
                return null;
            }

            FaceType? faceType = partition.FaceType();
            if(faceType == null || !faceType.HasValue)
            {
                return null;
            }

            int index = -1;
            int index_Adjacent = -1;
            bool reverse = true;
            List<Space> spaces = buildingModel.GetSpaces(partition);
            if (spaces != null && spaces.Count != 0)
            {
                index = spaces.FindIndex(x => x.Guid == space.Guid);
                index = buildingModel.UniqueIndex(spaces[index]);

                index_Adjacent = spaces.FindIndex(x => x.Guid != space.Guid);
                index_Adjacent = buildingModel.UniqueIndex(spaces[index_Adjacent]);

                reverse = buildingModel.UniqueIndex(spaces[0]) != index;
            }

            AnyOf<Ground, Outdoors, Adiabatic, Surface> boundaryCondition = partition.ToLadybugTools_BoundaryCondition(buildingModel, space);

            FaceEnergyPropertiesAbridged faceEnergyPropertiesAbridged = new FaceEnergyPropertiesAbridged();
            if (partition is IHostPartition)
            {
                faceEnergyPropertiesAbridged.Construction = Query.UniqueName(((IHostPartition)partition).Type(), reverse);
            }
            
            Face face = new Face(Query.UniqueName(partition, index), face3D, faceType.Value, boundaryCondition, new FacePropertiesAbridged(faceEnergyPropertiesAbridged), partition.Name);
            if (partition is IHostPartition)
            {
                List<IOpening> openings = ((IHostPartition)partition).GetOpenings();
                if (openings != null && openings.Count != 0)
                {
                    List<HoneybeeSchema.Aperture> apertures = new List<HoneybeeSchema.Aperture>();
                    List<HoneybeeSchema.Door> doors = new List<HoneybeeSchema.Door>();

                    foreach (IOpening opening in openings)
                    {
                        MaterialType materialType = MaterialType.Opaque;

                        OpeningType openingType = opening.Type();
                        if(openingType != null)
                        {
                            materialType = buildingModel.GetMaterialType(openingType.PaneMaterialLayers);
                        }

                        
                        if (opening is Window && materialType != MaterialType.Opaque)
                        {
                            HoneybeeSchema.Aperture aperture = ((Window)opening).ToLadybugTools(buildingModel, space);
                            if(aperture != null)
                            {
                                apertures.Add(aperture);
                            }
                        }
                        else
                        {
                            HoneybeeSchema.Door door = opening.ToLadybugTools(buildingModel, space);
                            if(door != null)
                            {
                                doors.Add(door);
                            }
                        }
                    }

                    if (apertures != null && apertures.Count != 0)
                    {
                        face.Apertures = apertures;
                    }

                    if (doors != null && doors.Count != 0)
                    {
                        face.Doors = doors;
                    }
                }
            }

            return face;
        }
    }
}
using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Face ToLadybugTools_Face(this IHostPartition hostPartition, ArchitecturalModel architecturalModel, Space space)
        {
            Face3D face3D = Geometry.LadybugTools.Convert.ToLadybugTools(hostPartition?.Face3D);
            if (face3D == null)
            {
                return null;
            }

            FaceType? faceType = hostPartition.FaceType();
            if(faceType == null || !faceType.HasValue)
            {
                return null;
            }

            int index = -1;
            int index_Adjacent = -1;
            bool reverse = true;
            List<Space> spaces = architecturalModel.GetSpaces(hostPartition);
            if (spaces != null && spaces.Count != 0)
            {
                index = spaces.FindIndex(x => x.Guid == space.Guid);
                index = architecturalModel.UniqueIndex(spaces[index]);

                index_Adjacent = spaces.FindIndex(x => x.Guid != space.Guid);
                index_Adjacent = architecturalModel.UniqueIndex(spaces[index_Adjacent]);

                reverse = architecturalModel.UniqueIndex(spaces[0]) != index;
            }

            AnyOf<Ground, Outdoors, Adiabatic, Surface> boundaryCondition = hostPartition.ToLadybugTools_BoundaryCondition(architecturalModel, space);

            FaceEnergyPropertiesAbridged faceEnergyPropertiesAbridged = new FaceEnergyPropertiesAbridged();
            if (faceType != FaceType.AirBoundary)
            {
                faceEnergyPropertiesAbridged.Construction = Query.UniqueName(hostPartition.Type(), reverse);
            }
                

            Face face = new Face(Core.LadybugTools.Query.UniqueName(hostPartition as SAMObject, index), face3D, faceType.Value, boundaryCondition, new FacePropertiesAbridged(faceEnergyPropertiesAbridged), hostPartition.Name);

            List<IOpening> openings = hostPartition.GetOpenings();
            if(openings != null && openings.Count != 0)
            {
                List<HoneybeeSchema.Aperture> apertures = new List<HoneybeeSchema.Aperture>();
                List<HoneybeeSchema.Door> doors = new List<HoneybeeSchema.Door>();

                foreach (IOpening opening in openings)
                {
                    if(opening is Door)
                    {

                    }
                    else
                    {

                    }
                }

                if(apertures!= null && apertures.Count != 0)
                {
                    face.Apertures = apertures;
                }

                if(doors != null && doors.Count != 0)
                {
                    face.Doors = doors;
                }
            }

            return face;
        }
    }
}
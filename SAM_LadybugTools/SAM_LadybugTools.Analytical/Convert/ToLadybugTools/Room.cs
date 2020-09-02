using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Room ToLadybugTools(this Space space, IEnumerable<Panel> panels)
        {
            if (space == null)
                return null;

            List<Face> faces = null;
            if(panels != null)
            {
                faces = new List<Face>();
                foreach(Panel panel in panels)
                {
                    Face face = panel.ToLadybugTools_Face(space);
                    if (face == null)
                        continue;

                    faces.Add(face);
                }
            }

            string uniqueName = Core.LadybugTools.Query.UniqueName(space);

            Room result = new Room(uniqueName, faces, new RoomPropertiesAbridged(), space.Name);

            return result;
        }
    }
}
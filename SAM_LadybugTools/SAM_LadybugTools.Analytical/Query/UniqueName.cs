namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static string UniqueName(this Construction construction, bool reverse = true)
        {
            if(construction == null)
            {
                return null;
            }
            
            return UniqueName((Core.SAMType)construction, reverse);
        }

        public static string UniqueName(this ApertureConstruction apertureConstruction, bool reverse = true, int index = -1)
        {
            if (apertureConstruction == null)
            {
                return null;
            }

            return UniqueName((Core.SAMType)apertureConstruction, reverse, index);
        }

        public static string UniqueName(this Panel panel, int index = -1)
        {
            if(panel == null)
            {
                return null;
            }

            string result = panel.Guid.ToString("N");
            if(index != -1)
            {
                result = string.Format("{0}__{1}", index, result);
            }

            return result;
        }

        public static string UniqueName(this Space space)
        {
            if(space == null)
            {
                return null;
            }

            return space.Guid.ToString("N");
        }

        private static string UniqueName(this Core.SAMType sAMType, bool reverse = true, int index = -1)
        {
            if (sAMType == null)
                return null;

            string name = sAMType.Name;
            if (reverse)
            {
                string sufix = "REVERSED";
                name = string.IsNullOrEmpty(name) ? sufix : name + " " + sufix;
            }

            return Core.LadybugTools.Query.UniqueName(sAMType.Guid, name, index);
        }
    }
}
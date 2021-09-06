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
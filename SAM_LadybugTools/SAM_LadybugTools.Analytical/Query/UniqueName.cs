namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static string UniqueName(this Construction construction, bool reverse = true)
        {
            if (construction == null)
                return null;

            string name = construction.Name;
            if(reverse)
            {
                string sufix = "REVERSED";
                name = string.IsNullOrEmpty(name) ? sufix : name + " " + sufix;
            }

            return Core.LadybugTools.Query.UniqueName(construction.Guid, name);
        }
    }
}
namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        public static string ClassName(this object @object)
        {
            if(@object == null)
            {
                return null;
            }

            if(!Core.Query.TryGetFieldValue(@object, ".class", out object @class))
            {
                return null;
            }

            if (!Core.Query.TryGetFieldValue(@class, "Name", out string result))
            {
                return null;
            }

            return result;
        }
    }
}
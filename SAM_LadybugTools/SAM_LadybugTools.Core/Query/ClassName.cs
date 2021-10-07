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

            if(!Core.Query.TryGetFieldValue(@object, ".class", out object result))
            {
                return null;
            }

            return null;

            //return result;
        }
    }
}
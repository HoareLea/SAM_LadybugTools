using HoneybeeSchema;
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static SAMObject ToSAM(this IDdBaseModel ddBaseModel)
        {
            if (ddBaseModel == null)
            {
                return null;
            }

            if(ddBaseModel is Model)
            {
                return ((Model)ddBaseModel).ToSAM();
            }

            return null;
        }

        public static SAMObject ToSAM(string pathOrJson)
        {
            if(string.IsNullOrWhiteSpace(pathOrJson))
            {
                return null;
            }

            string json = pathOrJson;
            if (System.IO.File.Exists(pathOrJson))
            {
                json = System.IO.File.ReadAllText(pathOrJson);
            }

            SAMObject result = null;
            try
            {
                IDdBaseModel ddBaseModel = Core.LadybugTools.Convert.ToHoneybee(json);

                if (ddBaseModel != null)
                {
                    result = ToSAM(ddBaseModel);
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }
    }
}
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
    }
}
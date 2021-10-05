using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Space ToSAM(this Room room)
        {
            if(room == null)
            {
                return null;
            }

            Space result = new Space(room.DisplayName);

            return result;
        }
    }
}
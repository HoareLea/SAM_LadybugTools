namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static string UniqueName(this HostPartitionType hostPartitionType, bool reverse = true)
        {
            if(hostPartitionType == null)
            {
                return null;
            }
            
            return UniqueName((Core.SAMType)hostPartitionType, reverse);
        }

        public static string UniqueName(this OpeningType openingType, bool reverse = true)
        {
            if (openingType == null)
            {
                return null;
            }

            return UniqueName((Core.SAMType)openingType, reverse);
        }
    }
}
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

        public static string UniqueName(this IPartition partition, int index = -1)
        {
            if (partition == null)
            {
                return null;
            }

            string result = partition.Guid.ToString("N");
            if (index != -1)
            {
                result = string.Format("{0}__{1}", index, result);
            }

            return result;
        }
    }
}
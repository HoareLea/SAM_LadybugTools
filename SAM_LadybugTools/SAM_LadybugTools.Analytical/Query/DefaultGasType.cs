namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static DefaultGasType DefaultGasType(this HoneybeeSchema.GasType? gasType)
        {
            if(gasType == null)
            {
                return Analytical.DefaultGasType.Undefined;
            }

            return DefaultGasType(gasType.Value);
        }

        public static DefaultGasType DefaultGasType(this HoneybeeSchema.GasType gasType)
        {
            switch(gasType)
            {
                case HoneybeeSchema.GasType.Air:
                    return Analytical.DefaultGasType.Air;

                case HoneybeeSchema.GasType.Argon:
                    return Analytical.DefaultGasType.Argon;

                case HoneybeeSchema.GasType.Krypton:
                    return Analytical.DefaultGasType.Krypton;

                case HoneybeeSchema.GasType.Xenon:
                    return Analytical.DefaultGasType.Xenon;
            }

            return Analytical.DefaultGasType.Undefined;
        }
    }
}
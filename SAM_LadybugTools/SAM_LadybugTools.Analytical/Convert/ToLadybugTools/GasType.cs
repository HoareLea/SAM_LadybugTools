using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static GasType? ToLadybugTools(this DefaultGasType defaultGasType)
        {
            if (defaultGasType == DefaultGasType.Undefined)
                return null;

            switch(defaultGasType)
            {
                case DefaultGasType.Air:
                    return GasType.Air;
                case DefaultGasType.Argon:
                    return GasType.Argon;
                case DefaultGasType.Krypton:
                    return GasType.Krypton;
                case DefaultGasType.Xenon:
                    return GasType.Xenon;
            }

            return null;
        }
    }
}
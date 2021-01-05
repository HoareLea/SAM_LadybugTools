using HoneybeeSchema;

namespace SAM.Core.LadybugTools
{
    public static partial class Create
    {
        public static Log Log(this Model model)
        {
            if (model == null)
                return null;

            Log result = new Log();
            Modify.AddRange(result, ((IDdBaseModel)model).Log());

            model.OrphanedApertures?.ForEach(x => Modify.AddRange(result, Log(x as Aperture)));
            model.OrphanedDoors?.ForEach(x => Modify.AddRange(result, Log(x as Door)));
            model.OrphanedFaces?.ForEach(x => Modify.AddRange(result, Log(x as Face)));
            model.OrphanedShades?.ForEach(x => Modify.AddRange(result, Log(x as Shade)));
            model.Rooms?.ForEach(x => Modify.AddRange(result, Log(x as Room)));

            ModelEnergyProperties modelEnergyProperties = model.Properties.Energy;
            if(modelEnergyProperties != null)
            {
                modelEnergyProperties.Constructions?.ForEach(x => Modify.AddRange(result, Log(x as AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, WindowConstructionShadeAbridged, AirBoundaryConstructionAbridged, OpaqueConstruction, WindowConstruction, WindowConstructionShade, AirBoundaryConstruction, ShadeConstruction>)));
                modelEnergyProperties.ConstructionSets?.ForEach(x => Modify.AddRange(result, Log(x as AnyOf<ConstructionSetAbridged, ConstructionSet>)));
                modelEnergyProperties.Materials?.ForEach(x => Modify.AddRange(result, Log(x as AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGas, EnergyWindowMaterialGasCustom, EnergyWindowMaterialGasMixture, EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialBlind, EnergyWindowMaterialGlazing, EnergyWindowMaterialShade>)));
                modelEnergyProperties.Hvacs?.ForEach(x => Modify.AddRange(result, Log(x as AnyOf<IdealAirSystemAbridged, VAV, PVAV, PSZ, PTAC, ForcedAirFurnace, FCUwithDOAS, WSHPwithDOAS, VRFwithDOAS, FCU, WSHP, VRF, Baseboard, EvaporativeCooler, Residential, WindowAC, GasUnitHeater>)));
                modelEnergyProperties.ProgramTypes?.ForEach(x => Modify.AddRange(result, Log(x as AnyOf<ProgramTypeAbridged, ProgramType>)));
                modelEnergyProperties.Schedules?.ForEach(x => Modify.AddRange(result, Log(x as AnyOf<ScheduleRulesetAbridged, ScheduleFixedIntervalAbridged, ScheduleRuleset, ScheduleFixedInterval>)));
                modelEnergyProperties.ScheduleTypeLimits?.ForEach(x => Modify.AddRange(result, Log(x as ScheduleTypeLimit)));
            }

            return result;
        }
        
        public static Log Log(this IIDdBase iIDdBase)
        {
            if (iIDdBase == null)
                return null;

            Log result = new Log();
            string identifier = iIDdBase.Identifier;
            if(string.IsNullOrWhiteSpace(identifier))
            {
                result.Add("Identifier is missing for {0}", LogRecordType.Error, iIDdBase.GetType().FullName);
                return result;
            }

            if (identifier.Length > 100)
                result.Add("Identifier is to long for {0}: {1}", LogRecordType.Error, iIDdBase.GetType().FullName, identifier);

            if(identifier.Contains(",") || identifier.Contains(";") || identifier.Contains("!") || identifier.Contains("\n") || identifier.Contains("\t"))
                result.Add("Identifier for {0} contains invalid characters: {1}", LogRecordType.Error, iIDdBase.GetType().FullName, identifier);

            return result;
        }

        public static Log Log(this Aperture aperture)
        {
            if (aperture == null)
                return null;

            return Log((IIDdBase)aperture);
        }

        public static Log Log(this Door door)
        {
            if (door == null)
                return null;

            return Log((IIDdBase)door);
        }

        public static Log Log(this Face face)
        {
            if (face == null)
                return null;

            return Log((IIDdBase)face);
        }

        public static Log Log(this Shade shade)
        {
            if (shade == null)
                return null;

            return Log((IIDdBase)shade);
        }

        public static Log Log(this Room room)
        {
            if (room == null)
                return null;

            Log result = Log((IIDdBase)room);
            
            return result;
        }

        public static Log Log(this AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, WindowConstructionShadeAbridged, AirBoundaryConstructionAbridged, OpaqueConstruction, WindowConstruction, WindowConstructionShade, AirBoundaryConstruction, ShadeConstruction> construction)
        {
            if (construction == null)
                return null;

            if(construction.Obj is IIDdBase)
                return Log((IIDdBase)construction.Obj);

            return null;
        }

        public static Log Log(this AnyOf<ConstructionSetAbridged, ConstructionSet> constructionSet)
        {
            if (constructionSet == null)
                return null;

            if (constructionSet.Obj is IIDdBase)
                return Log((IIDdBase)constructionSet.Obj);

            if(constructionSet.Obj is ConstructionSet)
            {
                ConstructionSet constructionSet_Temp = (ConstructionSet)constructionSet.Obj;
                //TODO: Implement identifier Log for construction in constructionSet
            }

            return null;
        }

        public static Log Log(this AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGas, EnergyWindowMaterialGasCustom, EnergyWindowMaterialGasMixture, EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialBlind, EnergyWindowMaterialGlazing, EnergyWindowMaterialShade> material)
        {
            if (material == null)
                return null;

            if (material.Obj is IIDdBase)
                return Log((IIDdBase)material.Obj);

            return null;
        }

        public static Log Log(this AnyOf<IdealAirSystemAbridged, VAV, PVAV, PSZ, PTAC, ForcedAirFurnace, FCUwithDOAS, WSHPwithDOAS, VRFwithDOAS, FCU, WSHP, VRF, Baseboard, EvaporativeCooler, Residential, WindowAC, GasUnitHeater> hvac)
        {
            if (hvac == null)
                return null;

            if (hvac.Obj is IIDdBase)
                return Log((IIDdBase)hvac.Obj);

            return null;
        }

        public static Log Log(this AnyOf<ProgramTypeAbridged, ProgramType> programType)
        {
            if (programType == null)
                return null;

            if (programType.Obj is IIDdBase)
                return Log((IIDdBase)programType.Obj);

            return null;
        }

        public static Log Log(this AnyOf<ScheduleRulesetAbridged, ScheduleFixedIntervalAbridged, ScheduleRuleset, ScheduleFixedInterval> schedule)
        {
            if (schedule == null)
                return null;

            if (schedule.Obj is IIDdBase)
                return Log((IIDdBase)schedule.Obj);

            return null;
        }

        public static Log Log(this ScheduleTypeLimit scheduleTypeLimit)
        {
            if (scheduleTypeLimit == null)
                return null;

            Log result = Log((IIDdBase)scheduleTypeLimit);

            return result;
        }
    }
}

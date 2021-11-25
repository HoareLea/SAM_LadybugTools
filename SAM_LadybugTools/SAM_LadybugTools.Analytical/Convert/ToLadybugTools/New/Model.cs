using HoneybeeSchema;
using SAM.Architectural;
using SAM.Core;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Model ToLadybugTools(this BuildingModel buildingModel, double silverSpacing = Tolerance.MacroDistance, double tolerance = Tolerance.Distance)
        {
            if (buildingModel == null)
                return null;

            BuildingModel architecturalModel_Temp = new BuildingModel(buildingModel);
            
            architecturalModel_Temp.OffsetAperturesOnEdge(0.1, tolerance);
            architecturalModel_Temp.ReplaceTransparentPartitions(0.1);
            architecturalModel_Temp.UpdateMaterialsByMaterialLayerThickness(tolerance);
            architecturalModel_Temp.UpdateMaterialsByHeatTransferCoefficients(true, true);

            string uniqueName = Core.LadybugTools.Query.UniqueName(architecturalModel_Temp);

            List<Room> rooms = null;
            List<AnyOf<IdealAirSystemAbridged, VAV, PVAV, PSZ, PTAC, ForcedAirFurnace, FCUwithDOASAbridged, WSHPwithDOASAbridged, VRFwithDOASAbridged, FCU, WSHP, VRF, Baseboard, EvaporativeCooler, Residential, WindowAC, GasUnitHeater>> hvacs = null;

            List<Space> spaces = architecturalModel_Temp.GetSpaces();
            if (spaces != null)
            {
                hvacs = new List<AnyOf<IdealAirSystemAbridged, VAV, PVAV, PSZ, PTAC, ForcedAirFurnace, FCUwithDOASAbridged, WSHPwithDOASAbridged, VRFwithDOASAbridged, FCU, WSHP, VRF, Baseboard, EvaporativeCooler, Residential, WindowAC, GasUnitHeater>>();
                rooms = new List<Room>();

                Dictionary<double, List<IPartition>> dictionary_elevations = Geometry.Spatial.Query.ElevationDictionary(buildingModel.GetPartitions());
                List<Level> levels = dictionary_elevations?.Keys.ToList().ConvertAll(x => Architectural.Create.Level(x));

                for (int i = 0; i < spaces.Count; i++)
                {
                    Space space = spaces[i];
                    if (space == null)
                        continue;

                    Room room = space.ToLadybugTools(architecturalModel_Temp, silverSpacing, tolerance);
                    if (room == null)
                        continue;

                    if (levels != null && levels.Count > 0)
                    {
                        double elevation_Min = Analytical.Query.MinElevation(buildingModel, space);
                        if (!double.IsNaN(elevation_Min))
                        {
                            double difference_Min = double.MaxValue;
                            Level level_Min = null;
                            foreach(Level level in levels)
                            {
                                double difference = System.Math.Abs(elevation_Min - level.Elevation);
                                if(difference < difference_Min)
                                {
                                    difference_Min = difference;
                                    level_Min = level;
                                }
                            }

                            room.Story = level_Min.Name;
                        }
                    }

                    InternalCondition internalCondition = space.InternalCondition;
                    if(internalCondition != null)
                    {
                        //Michal Idea of filtering Uncondition Spaces
                        string name_InternalCondition = internalCondition.Name;

                        if(name_InternalCondition == null || (name_InternalCondition != null && !name_InternalCondition.ToLower().Contains("unconditioned") && !name_InternalCondition.ToLower().Contains("external")))
                        {

                            IdealAirSystemAbridged idealAirSystemAbridged = new IdealAirSystemAbridged(string.Format("{0}__{1}", i.ToString(), "IdealAir"), string.Format("Ideal Air System Abridged {0}", space.Name));
                            hvacs.Add(idealAirSystemAbridged);

                            if (room.Properties == null)
                                room.Properties = new RoomPropertiesAbridged();

                            if (room.Properties.Energy == null)
                                room.Properties.Energy = new RoomEnergyPropertiesAbridged();

                            room.Properties.Energy.Hvac = idealAirSystemAbridged.Identifier;
                        }

                    }

                    rooms.Add(room);
                }    
               
            }

            List<Shade> shades = null;
            List<Face> faces_Orphaned = null;

            List<IPartition> partitions_Shade = buildingModel.GetShadePartitions();
            if(partitions_Shade != null)
            {
                foreach (IPartition partition_Shade in partitions_Shade)
                {
                    if (partitions_Shade == null)
                        continue;

                    Shade shade = partition_Shade.ToLadybugTools_Shade();
                    if (shade == null)
                        continue;

                    if (shades == null)
                        shades = new List<Shade>();

                    shades.Add(shade);
                }
            }

            List<HostPartitionType> hostPartitionTypes = buildingModel.GetHostPartitionTypes();
            List<OpeningType> openingTypes = buildingModel.GetOpeningTypes();

            ConstructionSetAbridged constructionSetAbridged = Core.LadybugTools.Query.DefaultConstructionSetAbridged();
            List<HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet>> constructionSets = new List<HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet>>();// { constructionSetAbridged  };

            List<AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, WindowConstructionShadeAbridged, AirBoundaryConstructionAbridged, OpaqueConstruction, WindowConstruction, WindowConstructionShade, WindowConstructionDynamicAbridged, WindowConstructionDynamic, AirBoundaryConstruction, ShadeConstruction>> constructions = new List<AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, WindowConstructionShadeAbridged, AirBoundaryConstructionAbridged, OpaqueConstruction, WindowConstruction, WindowConstructionShade, WindowConstructionDynamicAbridged, WindowConstructionDynamic, AirBoundaryConstruction, ShadeConstruction>>();

            Dictionary<string, HoneybeeSchema.Energy.IMaterial> dictionary_Materials = new Dictionary<string, HoneybeeSchema.Energy.IMaterial>();
            if(hostPartitionTypes != null)
            {
                foreach(HostPartitionType hostPartitionType in hostPartitionTypes)
                {
                    List<MaterialLayer> materialLayers = hostPartitionType.MaterialLayers;
                    if (materialLayers == null)
                    {
                        continue;
                    }

                    constructions.Add(hostPartitionType.ToLadybugTools());
                    constructions.Add(hostPartitionType.ToLadybugTools(false));

                    foreach (MaterialLayer materialLayer in materialLayers)
                    {
                        IMaterial material = buildingModel.GetMaterial(materialLayer);
                        if (material == null)
                        {
                            continue;
                        }

                        if (dictionary_Materials.ContainsKey(material.Name))
                        {
                            continue;
                        }

                        HoneybeeSchema.Energy.IMaterial material_HoneybeeSchema = material.ToLadybugTools(materialLayer.Thickness, false);
                        if(material_HoneybeeSchema == null)
                        {
                            continue;
                        }

                        dictionary_Materials[material.Name] = material_HoneybeeSchema;
                    }
                }
            }

            if (openingTypes != null)
            {
                foreach (OpeningType openingType in openingTypes)
                {
                    List<MaterialLayer> materialLayers = openingType.PaneMaterialLayers;
                    if (materialLayers == null)
                    {
                        continue;
                    }

                    MaterialType materialType = buildingModel.GetMaterialType(materialLayers);

                    if(openingType is WindowType && materialType != MaterialType.Opaque)
                    {
                        WindowType windowType = (WindowType)openingType;

                        constructions.Add(windowType.ToLadybugTools_WindowConstructionAbridged());
                        constructions.Add(windowType.ToLadybugTools_WindowConstructionAbridged(false));
                    }
                    else
                    {
                        constructions.Add(openingType.ToLadybugTools());
                        constructions.Add(openingType.ToLadybugTools(false));
                    }

                    foreach (MaterialLayer materialLayer in materialLayers)
                    {
                        IMaterial material = buildingModel.GetMaterial(materialLayer);
                        if (material == null)
                        {
                            continue;
                        }

                        if (dictionary_Materials.ContainsKey(material.Name))
                        {
                            continue;
                        }

                        HoneybeeSchema.Energy.IMaterial material_HoneybeeSchema = material.ToLadybugTools(materialLayer.Thickness, false);
                        if (material_HoneybeeSchema == null)
                        {
                            continue;
                        }

                        dictionary_Materials[material.Name] = material_HoneybeeSchema;
                    }
                }
            }
            
            Dictionary<System.Guid, ProgramType> dictionary_ProgramTypes = new Dictionary<System.Guid, ProgramType>();
            if (spaces != null)
            {
                foreach (Space space in spaces)
                {
                    InternalCondition internalCondition = space?.InternalCondition;
                    if (internalCondition == null)
                        continue;

                    if (dictionary_ProgramTypes.ContainsKey(internalCondition.Guid))
                        continue;

                    ProgramType programType = space.ToLadybugTools(buildingModel);
                    if (programType != null)
                        dictionary_ProgramTypes[internalCondition.Guid] = programType;
                }
            }

            List<AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGas, EnergyWindowMaterialGasCustom, EnergyWindowMaterialGasMixture, EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialBlind, EnergyWindowMaterialGlazing, EnergyWindowMaterialShade>> materials = new List<AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGas, EnergyWindowMaterialGasCustom, EnergyWindowMaterialGasMixture, EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialBlind, EnergyWindowMaterialGlazing, EnergyWindowMaterialShade>>();
            //HoneybeeSchema.Helper.EnergyLibrary.DefaultMaterials?.ToList().ForEach(x => materials.Add(x as dynamic));
            dictionary_Materials.Values.ToList().ForEach(x => materials.Add(x as dynamic));

            List<AnyOf<ScheduleRulesetAbridged, ScheduleFixedIntervalAbridged, ScheduleRuleset, ScheduleFixedInterval>> schedules = new List<AnyOf<ScheduleRulesetAbridged, ScheduleFixedIntervalAbridged, ScheduleRuleset, ScheduleFixedInterval>>();
            //HoneybeeSchema.Helper.EnergyLibrary.DefaultScheduleRuleset?.ToList().ForEach(x => schedules.Add(x));

            List<HoneybeeSchema.AnyOf<ProgramTypeAbridged, ProgramType>> programTypes = new List<HoneybeeSchema.AnyOf<ProgramTypeAbridged, ProgramType>>();
            //HoneybeeSchema.Helper.EnergyLibrary.DefaultProgramTypes?.ToList().ForEach(x => programTypes.Add(x));
            dictionary_ProgramTypes.Values.ToList().ForEach(x => programTypes.Add(x));

            List<ScheduleTypeLimit> scheduleTypeLimits = new List<ScheduleTypeLimit>();
            //HoneybeeSchema.Helper.EnergyLibrary.DefaultScheduleTypeLimit?.ToList().ForEach(x => scheduleTypeLimits.Add(x));

            constructionSets.RemoveAll(x => x == null);
            constructions.RemoveAll(x => x == null);
            materials.RemoveAll(x => x == null);

            ModelEnergyProperties modelEnergyProperties = new ModelEnergyProperties(constructionSets, constructions, materials, hvacs, null, programTypes, schedules, scheduleTypeLimits);
            
            ModelProperties modelProperties = new ModelProperties(modelEnergyProperties);

            Model model = new Model(uniqueName, modelProperties, buildingModel.Name, null, rooms, faces_Orphaned, shades);
            model.AngleTolerance = Units.Convert.ToDegrees(Tolerance.Angle);// 2;
            model.Tolerance = Tolerance.MacroDistance;

            return model;
        }
    }
}
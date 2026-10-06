// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Analytical;
using SAM.Core;
using SAM.Geometry.Spatial;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SAM.Core.LadybugTools.Tests
{
    /// <summary>
    /// Semantic round-trip regressions for SAM AnalyticalModel -> Honeybee JSON -> SAM AnalyticalModel.
    /// Uses a reduced deterministic shoebox fixture covering opaque, gas, glazing and frame materials.
    /// </summary>
    public static class SemanticRoundTripTests
    {
        private const double Tolerance = 1e-9;

        private const string PaintName = "T00_Test Paint_0.1kg/m3_0.2W/mK";
        private const string ConcreteName = "T00_Test Concrete_2300kg/m3_2.3W/mK";
        private const string AirGapName = "T00_Air Gap__50mm_1.25W/m2K";
        private const string ArgonName = "T00_Argon__12mm_1.403W/m2K";
        private const string GlassName = "T00_Test Glass_6mm_g0.78_Lt0.89";
        private const string FrameName = "T00_Test Frame_7800kg/m3_0.176W/mK";
        private const string WallConstructionName = "T00_Wall Construction";
        private const string GlazingConstructionName = "T00_Glazing Construction";

        private static AnalyticalModel CreateShoebox()
        {
            OpaqueMaterial paint = Analytical.Create.OpaqueMaterial(PaintName, "Test", PaintName, "Low density paint", 0.2, 100, 0.1, 0.0007, 5.565, 0.64, 0.32, 0, 0, 0.92, 0.5, false);
            OpaqueMaterial concrete = Analytical.Create.OpaqueMaterial(ConcreteName, "Test", ConcreteName, null, 2.3, 1000, 2300, 0.1, 9999, 0.3, 0.3, 0, 0, 0.9, 0.9, true);
            GasMaterial airGap = Analytical.Create.GasMaterial(AirGapName, "Test", AirGapName, null, 0.024, 1006.1, 1.293, 0.0000178, 0.05, 1, 1.25, DefaultGasType.Air);
            GasMaterial argon = Analytical.Create.GasMaterial(ArgonName, "Test", ArgonName, null, 0.01622, 521.9, 1.782, 0.0000221, 0.012, 1, 1.403, DefaultGasType.Argon);
            TransparentMaterial glass = Analytical.Create.TransparentMaterial(GlassName, "Test", GlassName, null, 1, 0.006, 9999, 0.78, 0.89, 0.07, 0.07, 0.08, 0.08, 0.84, 0.84, false);
            OpaqueMaterial frame = Analytical.Create.OpaqueMaterial(FrameName, "Test", FrameName, null, 0.176, 2800, 7800, 0.05, 9999, 0.3, 0.3, 0, 0, 0.9, 0.9, false);

            MaterialLibrary materialLibrary = new MaterialLibrary("Test Material Library");
            materialLibrary.Add(paint);
            materialLibrary.Add(concrete);
            materialLibrary.Add(airGap);
            materialLibrary.Add(argon);
            materialLibrary.Add(glass);
            materialLibrary.Add(frame);

            Construction wallConstruction = new Construction(WallConstructionName, new List<ConstructionLayer>
            {
                new ConstructionLayer(ConcreteName, 0.1),
                new ConstructionLayer(AirGapName, 0.05),
                new ConstructionLayer(PaintName, 0.0002),
            });
            wallConstruction.SetValue(ConstructionParameter.DefaultPanelType, PanelType.WallExternal.ToString());

            ApertureConstruction glazingConstruction = new ApertureConstruction(
                System.Guid.NewGuid(),
                GlazingConstructionName,
                ApertureType.Window,
                new List<ConstructionLayer>
                {
                    new ConstructionLayer(GlassName, 0.006),
                    new ConstructionLayer(ArgonName, 0.012),
                    new ConstructionLayer(GlassName, 0.006),
                },
                new List<ConstructionLayer>
                {
                    new ConstructionLayer(FrameName, 0.05),
                });
            glazingConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, PanelType.WallExternal.ToString());

            // 4 m x 3 m x 2.5 m shoebox
            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(4, 0, 0);
            Point3D p2 = new Point3D(4, 3, 0);
            Point3D p3 = new Point3D(0, 3, 0);
            Point3D p4 = new Point3D(0, 0, 2.5);
            Point3D p5 = new Point3D(4, 0, 2.5);
            Point3D p6 = new Point3D(4, 3, 2.5);
            Point3D p7 = new Point3D(0, 3, 2.5);

            AdjacencyCluster adjacencyCluster = new AdjacencyCluster();
            Space space = new Space("Test Space", new Point3D(2, 1.5, 1.25));
            adjacencyCluster.AddObject(space);

            AddPanel(adjacencyCluster, space, wallConstruction, PanelType.SlabOnGrade, new Face3D(new Polygon3D(new List<Point3D> { p0, p3, p2, p1 })));
            AddPanel(adjacencyCluster, space, wallConstruction, PanelType.Roof, new Face3D(new Polygon3D(new List<Point3D> { p4, p5, p6, p7 })));
            AddPanel(adjacencyCluster, space, wallConstruction, PanelType.WallExternal, new Face3D(new Polygon3D(new List<Point3D> { p1, p2, p6, p5 })));
            AddPanel(adjacencyCluster, space, wallConstruction, PanelType.WallExternal, new Face3D(new Polygon3D(new List<Point3D> { p2, p3, p7, p6 })));
            AddPanel(adjacencyCluster, space, wallConstruction, PanelType.WallExternal, new Face3D(new Polygon3D(new List<Point3D> { p3, p0, p4, p7 })));

            // South wall (y = 0) hosts a 2 m x 1.5 m window, 0.5 m from every edge
            Panel wall = AddPanel(adjacencyCluster, space, wallConstruction, PanelType.WallExternal, new Face3D(new Polygon3D(new List<Point3D> { p0, p1, p5, p4 })));
            Aperture aperture = new Aperture(glazingConstruction, new Face3D(new Polygon3D(new List<Point3D>
            {
                new Point3D(1, 0, 0.5),
                new Point3D(3, 0, 0.5),
                new Point3D(3, 0, 2.0),
                new Point3D(1, 0, 2.0),
            })));
            wall.AddAperture(aperture);

            Location location = new Location("United Kingdom, London", -0.45, 51.48, 25);

            return new AnalyticalModel("T00_Shoebox", "Test shoebox description", location, null, adjacencyCluster, materialLibrary, new ProfileLibrary("Test Profile Library"));
        }

        private static Panel AddPanel(AdjacencyCluster adjacencyCluster, Space space, Construction construction, PanelType panelType, Face3D face3D)
        {
            Panel panel = Analytical.Create.Panel(construction, panelType, face3D);
            adjacencyCluster.AddObject(panel);
            adjacencyCluster.AddRelation(space, panel);
            return panel;
        }

        private static AnalyticalModel RoundTrip(AnalyticalModel analyticalModel)
        {
            HoneybeeSchema.Model model = SAM.Analytical.LadybugTools.Convert.ToLadybugTools(analyticalModel);
            Assert.NotNull(model);

            string json = model.ToJson();
            Assert.False(string.IsNullOrWhiteSpace(json));

            HoneybeeSchema.IDdBaseModel ddBaseModel = Convert.ToHoneybee(json, out Log _);
            HoneybeeSchema.Model model_Deserialised = Assert.IsType<HoneybeeSchema.Model>(ddBaseModel);

            SAMObject result = SAM.Analytical.LadybugTools.Convert.ToSAM((HoneybeeSchema.IDdBaseModel)model_Deserialised);
            return Assert.IsType<AnalyticalModel>(result);
        }

        private static IMaterial Material(AnalyticalModel analyticalModel, string name)
        {
            return analyticalModel.MaterialLibrary?.GetMaterial(name);
        }

        [Fact]
        public static void RoundTrip_PreservesModelMetadata()
        {
            AnalyticalModel original = CreateShoebox();
            AnalyticalModel roundTrip = RoundTrip(original);

            Assert.Equal(original.Name, roundTrip.Name);
            Assert.Equal(original.Description, roundTrip.Description);
            Assert.Equal(original.Guid, roundTrip.Guid);

            Assert.NotNull(roundTrip.Location);
            Assert.Equal(original.Location.Name, roundTrip.Location.Name);
            Assert.Equal(original.Location.Latitude, roundTrip.Location.Latitude, 9);
            Assert.Equal(original.Location.Longitude, roundTrip.Location.Longitude, 9);
            Assert.Equal(original.Location.Elevation, roundTrip.Location.Elevation, 9);

            Assert.Equal(original.ProfileLibrary?.Name, roundTrip.ProfileLibrary?.Name);
        }

        [Fact]
        public static void RoundTrip_PreservesVapourDiffusionFactor()
        {
            AnalyticalModel roundTrip = RoundTrip(CreateShoebox());

            Assert.Equal(5.565, Material(roundTrip, PaintName).GetValue<double>(SAM.Analytical.MaterialParameter.VapourDiffusionFactor), Tolerance);
            Assert.Equal(9999, Material(roundTrip, ConcreteName).GetValue<double>(SAM.Analytical.MaterialParameter.VapourDiffusionFactor), Tolerance);
            Assert.Equal(1, Material(roundTrip, AirGapName).GetValue<double>(SAM.Analytical.MaterialParameter.VapourDiffusionFactor), Tolerance);
            Assert.Equal(1, Material(roundTrip, ArgonName).GetValue<double>(SAM.Analytical.MaterialParameter.VapourDiffusionFactor), Tolerance);
            Assert.Equal(9999, Material(roundTrip, GlassName).GetValue<double>(SAM.Analytical.MaterialParameter.VapourDiffusionFactor), Tolerance);
            Assert.Equal(9999, Material(roundTrip, FrameName).GetValue<double>(SAM.Analytical.MaterialParameter.VapourDiffusionFactor), Tolerance);
        }

        [Fact]
        public static void RoundTrip_OpaquePaintStaysOpaque()
        {
            AnalyticalModel roundTrip = RoundTrip(CreateShoebox());

            // Density 0.1 kg/m3 must not reclassify this opaque material as gas
            OpaqueMaterial paint = Assert.IsType<OpaqueMaterial>(Material(roundTrip, PaintName));
            Assert.Equal(0.1, paint.Density, Tolerance);
            Assert.Equal(0.2, paint.ThermalConductivity, Tolerance);
            Assert.Equal(100, paint.SpecificHeatCapacity, Tolerance);
            Assert.Equal(0.0007, paint.GetValue<double>(Core.MaterialParameter.DefaultThickness), Tolerance);
            Assert.Equal(0.92, paint.GetValue<double>(OpaqueMaterialParameter.ExternalEmissivity), Tolerance);
            Assert.Equal(0.5, paint.GetValue<double>(OpaqueMaterialParameter.InternalEmissivity), Tolerance);
            Assert.False(paint.GetValue<bool>(OpaqueMaterialParameter.IgnoreThermalTransmittanceCalculations));
        }

        [Fact]
        public static void RoundTrip_IgnoreThermalTransmittanceCalculationsPreserved()
        {
            AnalyticalModel roundTrip = RoundTrip(CreateShoebox());

            OpaqueMaterial concrete = Assert.IsType<OpaqueMaterial>(Material(roundTrip, ConcreteName));
            Assert.True(concrete.GetValue<bool>(OpaqueMaterialParameter.IgnoreThermalTransmittanceCalculations));
        }

        [Fact]
        public static void RoundTrip_AirGapStaysGasWithOriginalThickness()
        {
            AnalyticalModel roundTrip = RoundTrip(CreateShoebox());

            GasMaterial airGap = Assert.IsType<GasMaterial>(Material(roundTrip, AirGapName));
            Assert.Equal(0.05, airGap.GetValue<double>(Core.MaterialParameter.DefaultThickness), Tolerance);
            Assert.Equal(1.25, airGap.GetValue<double>(GasMaterialParameter.HeatTransferCoefficient), Tolerance);
            Assert.Equal(1.293, airGap.Density, Tolerance);
            Assert.Equal(0.024, airGap.ThermalConductivity, Tolerance);
            Assert.Equal(1006.1, airGap.SpecificHeatCapacity, Tolerance);
        }

        [Fact]
        public static void RoundTrip_WindowGasPreservedWithOriginalHeatTransferCoefficient()
        {
            AnalyticalModel roundTrip = RoundTrip(CreateShoebox());

            GasMaterial argon = Assert.IsType<GasMaterial>(Material(roundTrip, ArgonName));
            Assert.Equal(0.012, argon.GetValue<double>(Core.MaterialParameter.DefaultThickness), Tolerance);
            Assert.Equal(1.403, argon.GetValue<double>(GasMaterialParameter.HeatTransferCoefficient), Tolerance);
            Assert.Equal(1.782, argon.Density, Tolerance);
            Assert.Equal(0.01622, argon.ThermalConductivity, Tolerance);
            Assert.Equal(521.9, argon.SpecificHeatCapacity, Tolerance);
        }

        [Fact]
        public static void RoundTrip_GlazingStaysTransparent()
        {
            AnalyticalModel roundTrip = RoundTrip(CreateShoebox());

            TransparentMaterial glass = Assert.IsType<TransparentMaterial>(Material(roundTrip, GlassName));
            Assert.Equal(0.006, glass.GetValue<double>(Core.MaterialParameter.DefaultThickness), Tolerance);
            Assert.Equal(0.78, glass.GetValue<double>(TransparentMaterialParameter.SolarTransmittance), Tolerance);
            Assert.False(glass.GetValue<bool>(TransparentMaterialParameter.IsBlind));
        }

        [Fact]
        public static void RoundTrip_ConstructionPreserved()
        {
            AnalyticalModel original = CreateShoebox();
            AnalyticalModel roundTrip = RoundTrip(original);

            Construction construction = roundTrip.AdjacencyCluster?.GetConstructions()?.Find(x => x.Name == WallConstructionName);
            Assert.NotNull(construction);

            Construction original_Construction = original.AdjacencyCluster.GetConstructions().Find(x => x.Name == WallConstructionName);
            Assert.Equal(original_Construction.Guid, construction.Guid);

            List<ConstructionLayer> layers = construction.ConstructionLayers;
            Assert.Equal(3, layers.Count);
            Assert.Equal(ConcreteName, layers[0].Name);
            Assert.Equal(0.1, layers[0].Thickness, Tolerance);
            Assert.Equal(AirGapName, layers[1].Name);
            Assert.Equal(0.05, layers[1].Thickness, Tolerance);
            Assert.Equal(PaintName, layers[2].Name);
            Assert.Equal(0.0002, layers[2].Thickness, Tolerance);

            Assert.Equal(PanelType.WallExternal.ToString(), construction.GetValue<string>(ConstructionParameter.DefaultPanelType));
        }

        [Fact]
        public static void RoundTrip_FrameConstructionAndLayersPreserved()
        {
            AnalyticalModel original = CreateShoebox();
            AnalyticalModel roundTrip = RoundTrip(original);

            ApertureConstruction apertureConstruction = roundTrip.AdjacencyCluster?.GetApertureConstructions()?.Find(x => x.Name == GlazingConstructionName);
            Assert.NotNull(apertureConstruction);

            ApertureConstruction original_ApertureConstruction = original.AdjacencyCluster.GetApertureConstructions().Find(x => x.Name == GlazingConstructionName);
            Assert.Equal(original_ApertureConstruction.Guid, apertureConstruction.Guid);

            List<ConstructionLayer> paneLayers = apertureConstruction.PaneConstructionLayers;
            Assert.Equal(3, paneLayers.Count);
            Assert.Equal(GlassName, paneLayers[0].Name);
            Assert.Equal(0.006, paneLayers[0].Thickness, Tolerance);
            Assert.Equal(ArgonName, paneLayers[1].Name);
            Assert.Equal(0.012, paneLayers[1].Thickness, Tolerance);
            Assert.Equal(GlassName, paneLayers[2].Name);
            Assert.Equal(0.006, paneLayers[2].Thickness, Tolerance);

            List<ConstructionLayer> frameLayers = apertureConstruction.FrameConstructionLayers;
            Assert.NotNull(frameLayers);
            Assert.Single(frameLayers);
            Assert.Equal(FrameName, frameLayers[0].Name);
            Assert.Equal(0.05, frameLayers[0].Thickness, Tolerance);

            Assert.NotNull(Material(roundTrip, FrameName));
            Assert.Equal(PanelType.WallExternal.ToString(), apertureConstruction.GetValue<string>(ApertureConstructionParameter.DefaultPanelType));
        }

        [Fact]
        public static void RoundTrip_CountsGeometryAndAdjacencyPreserved()
        {
            AnalyticalModel original = CreateShoebox();
            AnalyticalModel roundTrip = RoundTrip(original);

            AdjacencyCluster adjacencyCluster_Original = original.AdjacencyCluster;
            AdjacencyCluster adjacencyCluster_RoundTrip = roundTrip.AdjacencyCluster;

            Assert.Equal(adjacencyCluster_Original.GetSpaces().Count, adjacencyCluster_RoundTrip.GetSpaces().Count);
            Assert.Equal(adjacencyCluster_Original.GetPanels().Count, adjacencyCluster_RoundTrip.GetPanels().Count);

            foreach (Panel panel_Original in adjacencyCluster_Original.GetPanels())
            {
                Panel panel_RoundTrip = adjacencyCluster_RoundTrip.GetPanels().Find(x => x.Guid == panel_Original.Guid);
                Assert.NotNull(panel_RoundTrip);
                Assert.Equal(panel_Original.PanelType, panel_RoundTrip.PanelType);
                Assert.Equal(panel_Original.Construction?.Name, panel_RoundTrip.Construction?.Name);

                Face3D face3D_Original = panel_Original.GetFace3D();
                Face3D face3D_RoundTrip = panel_RoundTrip.GetFace3D();
                Assert.Equal(face3D_Original.GetArea(), face3D_RoundTrip.GetArea(), 4);

                Point3D centroid_Original = face3D_Original.GetCentroid();
                Point3D centroid_RoundTrip = face3D_RoundTrip.GetCentroid();
                Assert.True(centroid_Original.Distance(centroid_RoundTrip) < 0.001);

                List<Space> spaces_RoundTrip = adjacencyCluster_RoundTrip.GetRelatedObjects<Space>(panel_RoundTrip);
                Assert.Equal(adjacencyCluster_Original.GetRelatedObjects<Space>(panel_Original).Count, spaces_RoundTrip?.Count ?? 0);

                Assert.Equal(panel_Original.Apertures?.Count ?? 0, panel_RoundTrip.Apertures?.Count ?? 0);
            }
        }

        [Fact]
        public static void RoundTrip_GuidsPreserved()
        {
            AnalyticalModel original = CreateShoebox();
            AnalyticalModel roundTrip = RoundTrip(original);

            Space space_Original = original.AdjacencyCluster.GetSpaces().First();
            Space space_RoundTrip = roundTrip.AdjacencyCluster.GetSpaces().First();
            Assert.Equal(space_Original.Guid, space_RoundTrip.Guid);
            Assert.Equal(space_Original.Name, space_RoundTrip.Name);

            Panel panel_Original = original.AdjacencyCluster.GetPanels().First(x => x.Apertures != null && x.Apertures.Count != 0);
            Panel panel_RoundTrip = roundTrip.AdjacencyCluster.GetPanels().Find(x => x.Guid == panel_Original.Guid);
            Assert.NotNull(panel_RoundTrip);

            Aperture aperture_Original = panel_Original.Apertures.First();
            Aperture aperture_RoundTrip = panel_RoundTrip.Apertures.First();
            Assert.Equal(aperture_Original.Guid, aperture_RoundTrip.Guid);
            Assert.Equal(aperture_Original.ApertureConstruction?.Name, aperture_RoundTrip.ApertureConstruction?.Name);

            foreach (IMaterial material_Original in original.MaterialLibrary.GetMaterials())
            {
                IMaterial material_RoundTrip = Material(roundTrip, material_Original.Name);
                Assert.NotNull(material_RoundTrip);
                Assert.Equal(((SAMObject)material_Original).Guid, ((SAMObject)material_RoundTrip).Guid);
                Assert.Equal(material_Original.GetType(), material_RoundTrip.GetType());
            }
        }

        [Fact]
        public static void RoundTrip_NoUnreferencedHoneybeeDefaultsImported()
        {
            AnalyticalModel roundTrip = RoundTrip(CreateShoebox());

            List<IMaterial> materials = roundTrip.MaterialLibrary?.GetMaterials();
            Assert.NotNull(materials);
            Assert.Equal(6, materials.Count);
            Assert.DoesNotContain(materials, x => x.Name != null && x.Name.StartsWith("Generic"));

            List<Profile> profiles = roundTrip.ProfileLibrary?.GetProfiles();
            Assert.True(profiles == null || profiles.Count == 0);
        }

        [Fact]
        public static void RoundTrip_HoneybeeModelCarriesSAMUserData()
        {
            AnalyticalModel original = CreateShoebox();
            HoneybeeSchema.Model model = SAM.Analytical.LadybugTools.Convert.ToLadybugTools(original);

            string json = model.ToJson();
            Assert.Contains(UserDataKeys.VapourDiffusionFactor, json);
            Assert.Contains(UserDataKeys.MaterialType, json);
            Assert.Contains(UserDataKeys.FrameConstructionLayers, json);
            Assert.Contains(UserDataKeys.Guid, json);

            // Existing unrelated Honeybee user data must survive the merge
            HoneybeeSchema.Model model2 = HoneybeeSchema.Model.FromJson(json);
            Assert.True(Query.TryGetUserData(model2, UserDataKeys.Name, out string name));
            Assert.Equal(original.Name, name);
        }

        [Fact]
        public static void HoneybeeModelWithoutSamMetadata_StillConverts()
        {
            string json = SampleJson.RichModel();
            HoneybeeSchema.Model model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);
            Assert.NotNull(model);

            SAMObject result = SAM.Analytical.LadybugTools.Convert.ToSAM((HoneybeeSchema.IDdBaseModel)model);
            AnalyticalModel analyticalModel = Assert.IsType<AnalyticalModel>(result);

            Assert.NotNull(analyticalModel.AdjacencyCluster);
            Assert.NotEmpty(analyticalModel.AdjacencyCluster.GetSpaces());
            Assert.NotEmpty(analyticalModel.AdjacencyCluster.GetPanels());
        }

        [Fact]
        public static void HoneybeeMaterialWithoutSamMetadata_DensityHeuristicUnchanged()
        {
            // Non-SAM EnergyMaterial with low density keeps the legacy gas heuristic
            HoneybeeSchema.EnergyMaterial energyMaterial = new HoneybeeSchema.EnergyMaterial("HB Air Layer", 0.05, 0.1, 1.2, 1006);
            Core.IMaterial material = SAM.Analytical.LadybugTools.Convert.ToSAM((HoneybeeSchema.Energy.IMaterial)energyMaterial);
            Assert.IsType<GasMaterial>(material);

            HoneybeeSchema.EnergyMaterial energyMaterial_Solid = new HoneybeeSchema.EnergyMaterial("HB Concrete", 0.1, 2.3, 2300, 1000);
            Core.IMaterial material_Solid = SAM.Analytical.LadybugTools.Convert.ToSAM((HoneybeeSchema.Energy.IMaterial)energyMaterial_Solid);
            Assert.IsType<OpaqueMaterial>(material_Solid);

            // SAM metadata overrides the heuristic
            Core.LadybugTools.Modify.SetUserData(energyMaterial, UserDataKeys.MaterialType, MaterialType.Opaque.ToString());
            Core.IMaterial material_Metadata = SAM.Analytical.LadybugTools.Convert.ToSAM((HoneybeeSchema.Energy.IMaterial)energyMaterial);
            Assert.IsType<OpaqueMaterial>(material_Metadata);
        }

        [Fact]
        public static void UserData_RoundTripsThroughHoneybeeJson()
        {
            HoneybeeSchema.EnergyMaterial energyMaterial = new HoneybeeSchema.EnergyMaterial("Test", 0.1, 2.3, 2300, 1000);
            Core.LadybugTools.Modify.SetUserData(energyMaterial, UserDataKeys.VapourDiffusionFactor, 5.565);
            Core.LadybugTools.Modify.SetUserData(energyMaterial, UserDataKeys.Guid, "736c0eed-7e9b-450b-aed9-44c50695920b");

            string json = energyMaterial.ToJson();
            HoneybeeSchema.EnergyMaterial deserialised = HoneybeeSchema.EnergyMaterial.FromJson(json);

            Assert.True(Query.TryGetUserData(deserialised, UserDataKeys.VapourDiffusionFactor, out double vapourDiffusionFactor));
            Assert.Equal(5.565, vapourDiffusionFactor, Tolerance);

            Assert.True(Query.TryGetUserData(deserialised, UserDataKeys.Guid, out string guid));
            Assert.Equal("736c0eed-7e9b-450b-aed9-44c50695920b", guid);
        }

        [Fact]
        public static void UserData_DoesNotOverwriteUnrelatedKeys()
        {
            HoneybeeSchema.EnergyMaterial energyMaterial = new HoneybeeSchema.EnergyMaterial("Test", 0.1, 2.3, 2300, 1000);
            energyMaterial.UserData = new Dictionary<string, object> { ["custom.key"] = "custom-value" };

            Core.LadybugTools.Modify.SetUserData(energyMaterial, UserDataKeys.VapourDiffusionFactor, 5.565);

            string json = energyMaterial.ToJson();
            Assert.Contains("custom.key", json);

            HoneybeeSchema.EnergyMaterial deserialised = HoneybeeSchema.EnergyMaterial.FromJson(json);
            Assert.True(Query.TryGetUserData(deserialised, "custom.key", out string value));
            Assert.Equal("custom-value", value);
            Assert.True(Query.TryGetUserData(deserialised, UserDataKeys.VapourDiffusionFactor, out double vapourDiffusionFactor));
            Assert.Equal(5.565, vapourDiffusionFactor, Tolerance);
        }
    }
}

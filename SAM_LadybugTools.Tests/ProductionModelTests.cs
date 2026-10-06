// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Analytical;
using SAM.Core;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json.Nodes;
using Xunit;

namespace SAM.Core.LadybugTools.Tests
{
    /// <summary>
    /// Regression tests against a real production model (Fixtures/original_v1.sam).
    /// The Grasshopper Honeybee.SAMAnalytical component previously returned null for this
    /// payload with all exceptions swallowed; these tests pin the full
    /// AnalyticalModel -> Honeybee Model -> SAM AnalyticalModel pipeline to a non-null result.
    /// </summary>
    public static class ProductionModelTests
    {
        private static string ExtractSamJson(string path)
        {
            using (ZipArchive zipArchive = ZipFile.OpenRead(path))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    if (entry.FullName == "_ZipArchiveInfo")
                    {
                        continue;
                    }

                    using (StreamReader streamReader = new StreamReader(entry.Open()))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }

            return null;
        }

        private static AnalyticalModel LoadProductionModel()
        {
            string path = Path.Combine(System.AppContext.BaseDirectory, "Fixtures", "original_v1.sam");
            Assert.True(File.Exists(path), $"Fixture not found: {path}");

            string json = ExtractSamJson(path);
            Assert.False(string.IsNullOrWhiteSpace(json));

            return new AnalyticalModel((JsonObject)JsonNode.Parse(json));
        }

        [Fact]
        public static void ProductionModel_HoneybeeToSAM_ReturnsNonNullAnalyticalModel()
        {
            AnalyticalModel original = LoadProductionModel();
            Assert.NotNull(original);

            HoneybeeSchema.Model model = SAM.Analytical.LadybugTools.Convert.ToLadybugTools(original);
            Assert.NotNull(model);

            string json = model.ToJson();
            Assert.False(string.IsNullOrWhiteSpace(json));

            HoneybeeSchema.IDdBaseModel ddBaseModel = Convert.ToHoneybee(json, out Log log);
            Assert.NotNull(ddBaseModel);

            // Must return a non-null AnalyticalModel before any semantic property is asserted
            SAMObject result = SAM.Analytical.LadybugTools.Convert.ToSAM((HoneybeeSchema.Model)ddBaseModel);
            AnalyticalModel analyticalModel = Assert.IsType<AnalyticalModel>(result);

            Assert.NotNull(analyticalModel.AdjacencyCluster);
            Assert.NotEmpty(analyticalModel.AdjacencyCluster.GetPanels());
            Assert.NotEmpty(analyticalModel.AdjacencyCluster.GetSpaces());
        }

        [Fact]
        public static void ProductionModel_RoundTrip_PreservesModelGuidAndContent()
        {
            AnalyticalModel original = LoadProductionModel();

            HoneybeeSchema.Model model = SAM.Analytical.LadybugTools.Convert.ToLadybugTools(original);
            HoneybeeSchema.IDdBaseModel ddBaseModel = Convert.ToHoneybee(model.ToJson(), out Log _);

            AnalyticalModel result = Assert.IsType<AnalyticalModel>(SAM.Analytical.LadybugTools.Convert.ToSAM((HoneybeeSchema.Model)ddBaseModel));

            Assert.Equal(original.Guid, result.Guid);
            Assert.Equal(original.AdjacencyCluster.GetPanels().Count, result.AdjacencyCluster.GetPanels().Count);
            Assert.Equal(original.AdjacencyCluster.GetSpaces().Count, result.AdjacencyCluster.GetSpaces().Count);
        }

        [Fact]
        public static void EnergyWindowFrame_ToSAM_ConvertsToOpaqueMaterial()
        {
            // Regression: the EnergyWindowFrame branch in Convert.ToSAM(Material) was nested
            // inside the null guard and could never execute for a real frame object.
            HoneybeeSchema.EnergyWindowFrame energyWindowFrame = new HoneybeeSchema.EnergyWindowFrame("Test Frame", 0.05, 3.0);

            Core.IMaterial material = SAM.Analytical.LadybugTools.Convert.ToSAM((HoneybeeSchema.Energy.IMaterial)energyWindowFrame);

            OpaqueMaterial opaqueMaterial = Assert.IsType<OpaqueMaterial>(material);
            Assert.Equal("Test Frame", opaqueMaterial.Name);
            Assert.Equal(0.15, opaqueMaterial.ThermalConductivity, 9);
            Assert.Equal(0.05, opaqueMaterial.GetValue<double>(MaterialParameter.DefaultThickness), 9);
        }

        [Fact]
        public static void NullAnyOfMaterial_ToSAM_ReturnsNullWithoutThrowing()
        {
            // Regression: Query.DefaultMaterial returns a null AnyOf for unknown material names
            // and Convert.ToSAM(AnyOf) dereferenced it, throwing NullReferenceException deep
            // inside Modify.AddDefaultMaterials during model conversion.
            HoneybeeSchema.AnyOf<HoneybeeSchema.EnergyMaterial, HoneybeeSchema.EnergyMaterialNoMass, HoneybeeSchema.EnergyWindowMaterialGlazing, HoneybeeSchema.EnergyWindowMaterialGas> material = null;

            Assert.Null(SAM.Analytical.LadybugTools.Convert.ToSAM(material));
        }

        [Fact]
        public static void AddDefaultMaterials_UnknownLayerName_DoesNotThrow()
        {
            // A layer name missing from both the library and the Honeybee default set must be
            // skipped silently, matching the runtime production-model failure.
            MaterialLibrary materialLibrary = new MaterialLibrary("Test");
            List<ConstructionLayer> constructionLayers = new List<ConstructionLayer>
            {
                new ConstructionLayer("T00_Unknown Material Not In Defaults", 0.1),
                null,
            };

            List<IMaterial> materials = SAM.Analytical.LadybugTools.Modify.AddDefaultMaterials(materialLibrary, constructionLayers);

            Assert.NotNull(materials);
            Assert.Empty(materials);
        }

        [Fact]
        public static void ProductionModel_FrameMaterialRecoveredWhenHoneybeePrunesUnreferencedMaterials()
        {
            // Honeybee drops materials not referenced by any construction when a model passes
            // through the Ladybug Python round trip. The base SAM frame material is unreferenced
            // (frame layers live in user_data), so only the derived EnergyWindowFrame survives.
            const string frameName = "C00_Frame Notional building_7800kg/m3_0.176W/mK";

            AnalyticalModel original = LoadProductionModel();
            HoneybeeSchema.Model model = SAM.Analytical.LadybugTools.Convert.ToLadybugTools(original);

            // Simulate the Honeybee-side pruning of unreferenced materials at JSON level
            JsonObject jsonObject = (JsonObject)JsonNode.Parse(model.ToJson());
            JsonArray materials = (JsonArray)jsonObject["properties"]?["energy"]?["materials"];
            Assert.NotNull(materials);

            int count_Before = materials.Count;
            for (int i = materials.Count - 1; i >= 0; i--)
            {
                if (materials[i]?["identifier"]?.GetValue<string>() == frameName)
                {
                    materials.RemoveAt(i);
                }
            }
            Assert.True(materials.Count < count_Before);

            HoneybeeSchema.IDdBaseModel ddBaseModel = Convert.ToHoneybee(jsonObject.ToJsonString(), out Log _);
            Assert.NotNull(ddBaseModel);
            Assert.DoesNotContain(((HoneybeeSchema.Model)ddBaseModel).Properties.Energy.MaterialList, x => x?.Identifier == frameName);

            AnalyticalModel result = Assert.IsType<AnalyticalModel>(SAM.Analytical.LadybugTools.Convert.ToSAM((HoneybeeSchema.Model)ddBaseModel));

            ApertureConstruction apertureConstruction = result.AdjacencyCluster?.GetApertureConstructions()?.Find(x => x.Name == "SIM_EXT_GLZ");
            Assert.NotNull(apertureConstruction);
            Assert.Contains(apertureConstruction.FrameConstructionLayers, x => x.Name == frameName);

            OpaqueMaterial frameMaterial = Assert.IsType<OpaqueMaterial>(result.MaterialLibrary?.GetMaterial(frameName));

            OpaqueMaterial frameMaterial_Original = Assert.IsType<OpaqueMaterial>(original.MaterialLibrary?.GetMaterial(frameName));
            Assert.Equal(frameMaterial_Original.ThermalConductivity, frameMaterial.ThermalConductivity, 9);
            Assert.Equal(frameMaterial_Original.Density, frameMaterial.Density, 9);
            Assert.Equal(frameMaterial_Original.SpecificHeatCapacity, frameMaterial.SpecificHeatCapacity, 9);
        }
    }
}

// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SAM.Core.LadybugTools.Tests
{
    /// <summary>
    /// Round-trip tests verifying that identifiers, counts, geometry vertices,
    /// and openings survive a full Honeybee JSON → Model → JSON cycle without
    /// silent data loss.
    /// </summary>
    public static class RoundTripTests
    {
        private const double Tolerance = 1e-9;

        [Fact]
        public static void RichModel_RoundTrip_PreservesModelIdentifier()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);
            Assert.NotNull(model);

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            Assert.Equal("RichTestModel", reModel.Identifier);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesRoomCountAndIdentifier()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);
            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            Assert.NotNull(reModel.Rooms);
            Assert.Single(reModel.Rooms);
            Assert.Equal("Rich_Room_01", reModel.Rooms[0].Identifier);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesFaceCount()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);
            Assert.NotNull(model.Rooms[0].Faces);

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            Assert.Equal(6, reModel.Rooms[0].Faces.Count);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesFaceIdentifiers()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);
            var expectedIds = model.Rooms[0].Faces.Select(f => f.Identifier).ToList();

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);
            var resultIds = reModel.Rooms[0].Faces.Select(f => f.Identifier).ToList();

            Assert.Equal(expectedIds.Count, resultIds.Count);
            for (int i = 0; i < expectedIds.Count; i++)
                Assert.Equal(expectedIds[i], resultIds[i]);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesApertureCount()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);

            int originalApertureCount = model.Rooms[0].Faces.Sum(f => f.Apertures?.Count ?? 0);

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            int roundTripApertureCount = reModel.Rooms[0].Faces.Sum(f => f.Apertures?.Count ?? 0);
            Assert.Equal(1, originalApertureCount);
            Assert.Equal(originalApertureCount, roundTripApertureCount);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesApertureIdentifier()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);

            var aperture = model.Rooms[0].Faces
                .SelectMany(f => f.Apertures ?? Enumerable.Empty<HoneybeeSchema.Aperture>())
                .First();
            Assert.Equal("Rich_Room_01_Aperture_NorthWindow", aperture.Identifier);

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            var reAperture = reModel.Rooms[0].Faces
                .SelectMany(f => f.Apertures ?? Enumerable.Empty<HoneybeeSchema.Aperture>())
                .First();
            Assert.Equal("Rich_Room_01_Aperture_NorthWindow", reAperture.Identifier);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesDoorCount()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);

            int originalDoorCount = model.Rooms[0].Faces.Sum(f => f.Doors?.Count ?? 0);

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            int roundTripDoorCount = reModel.Rooms[0].Faces.Sum(f => f.Doors?.Count ?? 0);
            Assert.Equal(1, originalDoorCount);
            Assert.Equal(originalDoorCount, roundTripDoorCount);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesDoorIdentifier()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);

            var door = model.Rooms[0].Faces
                .SelectMany(f => f.Doors ?? Enumerable.Empty<HoneybeeSchema.Door>())
                .First();
            Assert.Equal("Rich_Room_01_Door_SouthEntrance", door.Identifier);

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            var reDoor = reModel.Rooms[0].Faces
                .SelectMany(f => f.Doors ?? Enumerable.Empty<HoneybeeSchema.Door>())
                .First();
            Assert.Equal("Rich_Room_01_Door_SouthEntrance", reDoor.Identifier);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesOrphanedShadeCountAndIdentifier()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);
            Assert.NotNull(model.OrphanedShades);
            Assert.Single(model.OrphanedShades);

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            Assert.NotNull(reModel.OrphanedShades);
            Assert.Single(reModel.OrphanedShades);
            Assert.Equal("Rich_OrphanedShade_Overhang", reModel.OrphanedShades[0].Identifier);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesFaceGeometryVertices()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);

            // Capture expected vertex data for every face before round-trip
            var expectedVertices = new Dictionary<string, List<List<double>>>();
            foreach (var face in model.Rooms[0].Faces)
                expectedVertices[face.Identifier] = face.Geometry.Boundary;

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            foreach (var face in reModel.Rooms[0].Faces)
            {
                Assert.True(expectedVertices.ContainsKey(face.Identifier),
                    $"Face '{face.Identifier}' was not in the original model");
                var expected = expectedVertices[face.Identifier];
                var actual = face.Geometry.Boundary;

                Assert.Equal(expected.Count, actual.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(3, expected[i].Count);
                    Assert.Equal(3, actual[i].Count);
                    for (int j = 0; j < 3; j++)
                        Assert.Equal(expected[i][j], actual[i][j], Tolerance);
                }
            }
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesApertureGeometryVertices()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);

            var aperture = model.Rooms[0].Faces
                .SelectMany(f => f.Apertures ?? Enumerable.Empty<HoneybeeSchema.Aperture>())
                .First();
            var expectedBoundary = aperture.Geometry.Boundary;

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            var reAperture = reModel.Rooms[0].Faces
                .SelectMany(f => f.Apertures ?? Enumerable.Empty<HoneybeeSchema.Aperture>())
                .First();
            var actualBoundary = reAperture.Geometry.Boundary;

            Assert.Equal(expectedBoundary.Count, actualBoundary.Count);
            for (int i = 0; i < expectedBoundary.Count; i++)
                for (int j = 0; j < 3; j++)
                    Assert.Equal(expectedBoundary[i][j], actualBoundary[i][j], Tolerance);
        }

        [Fact]
        public static void RichModel_RoundTrip_PreservesOrphanedShadeGeometryVertices()
        {
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);

            var expectedBoundary = model.OrphanedShades[0].Geometry.Boundary;

            string reSerialized = model.ToJson();
            var reModel = (HoneybeeSchema.Model)Convert.ToHoneybee(reSerialized, out Log _);

            var actualBoundary = reModel.OrphanedShades[0].Geometry.Boundary;

            Assert.Equal(expectedBoundary.Count, actualBoundary.Count);
            for (int i = 0; i < expectedBoundary.Count; i++)
                for (int j = 0; j < 3; j++)
                    Assert.Equal(expectedBoundary[i][j], actualBoundary[i][j], Tolerance);
        }

        [Fact]
        public static void RichModel_RoundTrip_ModelJson_DoubleSerialize_PreservesRoomCount()
        {
            // Verify the JSON itself is stable through the HoneybeeSchema serialisation
            string json = SampleJson.RichModel();

            var model = (HoneybeeSchema.Model)Convert.ToHoneybee(json, out Log _);
            string firstRound = model.ToJson();

            var model2 = (HoneybeeSchema.Model)Convert.ToHoneybee(firstRound, out Log _);
            string secondRound = model2.ToJson();

            var model3 = (HoneybeeSchema.Model)Convert.ToHoneybee(secondRound, out Log _);
            Assert.NotNull(model3.Rooms);
            Assert.Single(model3.Rooms);
            Assert.Equal(6, model3.Rooms[0].Faces.Count);
        }
    }
}

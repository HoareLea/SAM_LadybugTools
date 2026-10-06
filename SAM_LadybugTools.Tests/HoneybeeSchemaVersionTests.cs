// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Core;
using System;
using System.Text.Json;
using Xunit;

namespace SAM.Core.LadybugTools.Tests
{
    public static class HoneybeeSchemaVersionTests
    {
        [Fact]
        public static void HoneybeeSchemaVersion_ShouldReturnValidVersion()
        {
            Version version = Query.HoneybeeSchemaVersion();

            Assert.NotNull(version);
            Assert.Equal(2, version.Major);
        }

        [Fact]
        public static void HoneybeeSchemaVersionLog_CompatibleVersion_ShouldReturnEmptyLog()
        {
            string json = SampleJson.ModelWithOneRoom();

            using JsonDocument doc = JsonDocument.Parse(json);
            Log log = doc.HoneybeeSchemaVersionLog();

            Assert.NotNull(log);
            Assert.Empty(log);
        }

        [Fact]
        public static void HoneybeeSchemaVersionLog_MissingVersion_ShouldReturnEmptyLog()
        {
            string json = SampleJson.RoomWithoutVersion();

            using JsonDocument doc = JsonDocument.Parse(json);
            Log log = doc.HoneybeeSchemaVersionLog();

            Assert.NotNull(log);
            Assert.Empty(log);
        }

        [Fact]
        public static void HoneybeeSchemaVersionLog_InvalidVersionString_ShouldReturnEmptyLog()
        {
            string json = @"{""type"":""Model"",""version"":""not-a-version"",""identifier"":""test""}";

            using JsonDocument doc = JsonDocument.Parse(json);
            Log log = doc.HoneybeeSchemaVersionLog();

            Assert.NotNull(log);
            Assert.Empty(log);
        }

        [Fact]
        public static void HoneybeeSchemaVersionLog_IncompatibleMajorVersion_ShouldReturnWarningLog()
        {
            string json = @"{
  ""type"": ""Model"",
  ""version"": ""1.5901.5"",
  ""identifier"": ""OldModel"",
  ""rooms"": [],
  ""properties"": {
    ""type"": ""ModelProperties"",
    ""energy"": {
      ""type"": ""ModelEnergyProperties"",
      ""constructions"": [],
      ""construction_sets"": [],
      ""materials"": [],
      ""hvacs"": [],
      ""program_types"": [],
      ""schedules"": [],
      ""schedule_type_limits"": []
    }
  }
}";

            using JsonDocument doc = JsonDocument.Parse(json);
            Log log = doc.HoneybeeSchemaVersionLog();

            Assert.NotNull(log);
            Assert.NotEmpty(log);
            Assert.Contains(log, x => x.LogRecordType == LogRecordType.Warning);
        }

        [Fact]
        public static void HoneybeeSchemaVersionLog_CompatibleMajorVersion_DifferentMinor_ShouldReturnEmptyLog()
        {
            string json = @"{
  ""type"": ""Model"",
  ""version"": ""2.7.0"",
  ""identifier"": ""FutureModel"",
  ""rooms"": [],
  ""properties"": {
    ""type"": ""ModelProperties"",
    ""energy"": {
      ""type"": ""ModelEnergyProperties"",
      ""constructions"": [],
      ""construction_sets"": [],
      ""materials"": [],
      ""hvacs"": [],
      ""program_types"": [],
      ""schedules"": [],
      ""schedule_type_limits"": []
    }
  }
}";

            using JsonDocument doc = JsonDocument.Parse(json);
            Log log = doc.HoneybeeSchemaVersionLog();

            Assert.NotNull(log);
            Assert.Empty(log);
        }

        [Fact]
        public static void HoneybeeSchemaVersionLog_NullDocument_ShouldReturnEmptyLog()
        {
            Log log = Core.LadybugTools.Query.HoneybeeSchemaVersionLog(null);

            Assert.NotNull(log);
            Assert.Empty(log);
        }
    }
}

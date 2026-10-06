// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using System;
using System.Text.Json;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        /// <summary>
        /// The HoneybeeSchema version this assembly is compiled/linked against,
        /// read from the loaded HoneybeeSchema assembly (e.g. 2.6.0.0).
        /// </summary>
        public static Version HoneybeeSchemaVersion()
        {
            return typeof(HoneybeeSchema.IDdBaseModel).Assembly.GetName().Version;
        }

        /// <summary>
        /// Compares the schema "version" carried by a Honeybee JSON document against the
        /// supported HoneybeeSchema version and returns a <see cref="Log"/> describing any
        /// mismatch. Returns an empty (non-null) Log when there is nothing to flag - e.g. the
        /// JSON has no "version" property (as is the case for a bare Room) or the versions
        /// share the same major. A differing major version is reported as a warning so a
        /// mismatched export fails loudly instead of silently producing a partial/null object.
        /// </summary>
        public static Log HoneybeeSchemaVersionLog(this JsonDocument jsonDocument)
        {
            Log log = new Log();
            if (jsonDocument == null)
            {
                return log;
            }

            if (!jsonDocument.RootElement.TryGetProperty("version", out JsonElement jsonElement) || jsonElement.ValueKind != JsonValueKind.String)
            {
                return log;
            }

            string versionString = jsonElement.GetString();
            if (!Version.TryParse(versionString, out Version jsonVersion))
            {
                return log;
            }

            Version schemaVersion = HoneybeeSchemaVersion();
            if (schemaVersion != null && jsonVersion.Major != schemaVersion.Major)
            {
                log.Add("Honeybee JSON schema version {0} differs from the supported HoneybeeSchema major version {1}.x; conversion may be incomplete or fail.", LogRecordType.Warning, versionString, schemaVersion.Major);
            }

            return log;
        }
    }
}

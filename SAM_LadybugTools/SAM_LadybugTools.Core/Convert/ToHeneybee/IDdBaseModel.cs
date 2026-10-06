// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;

namespace SAM.Core.LadybugTools
{
    public static partial class Convert
    {
        public static IDdBaseModel ToHoneybee(this object @object)
        {
            return ToHoneybee(@object, out Log _);
        }

        public static IDdBaseModel ToHoneybee(this object @object, out Log log)
        {
            log = new Log();

            if (@object == null)
            {
                return null;
            }

            string json = ToString(@object);
            if (string.IsNullOrWhiteSpace(json))
            {
                log.Add("Could not serialise object of type {0} to Honeybee JSON.", LogRecordType.Error, @object.GetType().FullName);
                return null;
            }

            return ToHoneybee(json, out log);
        }

        public static IDdBaseModel ToHoneybee(string json)
        {
            return ToHoneybee(json, out Log _);
        }

        public static IDdBaseModel ToHoneybee(string json, out Log log)
        {
            log = new Log();

            System.Text.Json.JsonDocument jsonDocument;
            try
            {
                jsonDocument = System.Text.Json.JsonDocument.Parse(json);
            }
            catch (System.Exception exception)
            {
                log.Add("Could not parse Honeybee JSON: {0}", LogRecordType.Error, exception.Message);
                return null;
            }

            return ToHoneybee(jsonDocument, out log);
        }

        public static IDdBaseModel ToHoneybee(this System.Text.Json.JsonDocument jsonDocument)
        {
            return ToHoneybee(jsonDocument, out Log _);
        }

        public static IDdBaseModel ToHoneybee(this System.Text.Json.JsonDocument jsonDocument, out Log log)
        {
            log = new Log();

            if (jsonDocument == null)
            {
                log.Add("Honeybee JSON could not be parsed (null document).", LogRecordType.Error);
                return null;
            }

            if (!jsonDocument.RootElement.TryGetProperty("type", out System.Text.Json.JsonElement jsonElement) || jsonElement.ValueKind != System.Text.Json.JsonValueKind.String)
            {
                log.Add("Honeybee JSON is missing a string 'type' property; cannot determine the Honeybee object kind.", LogRecordType.Error);
                return null;
            }

            string type = jsonElement.GetString();
            if (string.IsNullOrWhiteSpace(type))
            {
                log.Add("Honeybee JSON 'type' property is empty.", LogRecordType.Error);
                return null;
            }

            // Warn (rather than silently mis-deserialise) when the JSON was written against a
            // different HoneybeeSchema major version than the one this assembly is built against.
            Core.Modify.AddRange(log, jsonDocument.HoneybeeSchemaVersionLog());

            string json = ToString(jsonDocument, false);
            if (string.IsNullOrWhiteSpace(json))
            {
                log.Add("Honeybee JSON could not be re-serialised for type '{0}'.", LogRecordType.Error, type);
                return null;
            }

            try
            {
                switch (type)
                {
                    case "Room":
                        return Room.FromJson(json);

                    case "Model":
                        return Model.FromJson(json);

                    case "Face":
                        return Face.FromJson(json);

                    case "Aperture":
                        return Aperture.FromJson(json);

                    case "Door":
                        return Door.FromJson(json);

                    case "Shade":
                        return Shade.FromJson(json);
                }
            }
            catch (System.Exception exception)
            {
                log.Add("Could not deserialise Honeybee '{0}': {1}", LogRecordType.Error, type, exception.Message);
                return null;
            }

            // Previously this threw NotImplementedException, which surfaced as an opaque crash.
            // Return null with a clear, diagnosable message instead.
            log.Add("Honeybee object type '{0}' is not supported for conversion (only 'Room', 'Model', 'Face', 'Aperture', 'Door', and 'Shade' are handled).", LogRecordType.Warning, type);
            return null;
        }
    }
}

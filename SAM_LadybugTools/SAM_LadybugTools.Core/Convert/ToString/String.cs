using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SAM.Core.LadybugTools
{
    public static partial class Convert
    {
        public static string ToString(this object @object)
        {
            if (@object == null)
                return null;

            //TODO:  Finish Implementation

            JsonWriterOptions jsonWriterOptions = new JsonWriterOptions
            {
                Indented = true
            };

            string result = null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(memoryStream, jsonWriterOptions))
                {
                    Modify.Write(utf8JsonWriter, @object);

                    utf8JsonWriter.Flush();
                }

                result = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            return result;
        }

        public static string ToString(this JsonDocument jsonDocument, bool indented = true)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Indented = indented });
                jsonDocument.WriteTo(utf8JsonWriter);
                utf8JsonWriter.Flush();
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}
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

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(memoryStream, jsonWriterOptions))
                    {
                        Modify.Write(utf8JsonWriter, @object);

                        utf8JsonWriter.Flush();
                    }

                    result = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            catch (Exception exception)
            {

            }

            return result;
        }
    }
}
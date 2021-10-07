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
                //Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //ToJson(@dynamic, dictionary);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(memoryStream, jsonWriterOptions))
                    {
                        ToString(@object, utf8JsonWriter);

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

        private static void ToString(dynamic @dynamic, Utf8JsonWriter utf8JsonWriter)
        {
            if (dynamic == null || utf8JsonWriter == null)
            {
                return;
            }

            Type type = @dynamic?.GetType();
            if (type == null)
            {
                return;
            }

            object @object = null;

            switch (type.FullName)
            {
                case "IronPython.Runtime.PythonDictionary":
                    Core.Query.TryInvokeDeclaredMethod(@dynamic, "items", out @object, new object[] { });
                    break;

                case "IronPython.Runtime.List":
                    Core.Query.TryInvokeDeclaredMethod(@dynamic, "GetObjectArray", out @object, new object[] { });
                    break;

                case "IronPython.Runtime.PythonTuple":
                    Core.Query.TryInvokeRuntimeMethod(@dynamic, "ToArray", out @object, new object[] { });
                    break;

                default:
                    try
                    {
                        @object = @dynamic.to_dict();
                    }
                    catch
                    {
                        return;
                    }
                    break;
            }

            if (@object == null)
            {
                return;
            }

            object[] array = @object as object[];
            if (array != null)
            {
                utf8JsonWriter.WriteStartObject();

                foreach (dynamic item in array)
                {
                    string name = item[0];
                    object value = item[1];
                    if (value == null)
                    {
                        utf8JsonWriter.WriteNull(name);
                    }

                    if (Core.Query.IsNumeric(value))
                    {
                        utf8JsonWriter.WriteNumber(name, value as dynamic);
                    }
                    else if (value is string)
                    {
                        utf8JsonWriter.WriteString(name, (string)value);
                    }
                    else if (value is Guid)
                    {
                        utf8JsonWriter.WriteString(name, (Guid)value);
                    }
                    else if (value is DateTime)
                    {
                        utf8JsonWriter.WriteString(name, (DateTime)value);
                    }
                    else if (value is bool)
                    {
                        utf8JsonWriter.WriteBoolean(name, (bool)value);
                    }
                    else
                    {
                        utf8JsonWriter.WritePropertyName(name);
                        try
                        {
                            ToString(value, utf8JsonWriter);
                        }
                        catch
                        {

                        }
                    }
                }

                utf8JsonWriter.WriteEndObject();
            }
            else
            {

            }
        }

        //private static void ToJson(dynamic @dynamic, Dictionary<string, object> dictionary)
        //{
        //    if (dynamic == null || dictionary == null)
        //    {
        //        return;
        //    }

        //    foreach (dynamic item in @dynamic.items())
        //    {
        //        string name = item[0];
        //        dynamic value = item[1];
        //        if (value == null)
        //        {
        //            dictionary[name] = null;
        //        }
        //        if (Core.Query.IsNumeric(value) || value is string)
        //        {
        //            dictionary[name] = value;
        //        }
        //        else
        //        {
        //            Dictionary<string, object> dictionary_Temp = new Dictionary<string, object>();

        //            Type type = value.GetType();

        //            switch(type.FullName)
        //            {
        //                case "IronPython.Runtime.PythonDictionary":
        //                    if (Modify.TryInvokeDeclaredMethod(value, "items", out object ccc, new object[] { }))
        //                    {
        //                        if (Modify.TryInvokeDeclaredMethod(ccc, "GetObjectArray", out object ddd, new object[] { }))
        //                        {
        //                            if (ddd is object[])
        //                            {

        //                            }
        //                        }

        //                        if (ccc is object[])
        //                        {

        //                        }
        //                    }
        //                    ToJson(value, dictionary_Temp);
        //                    break;

        //                case "IronPython.Runtime.List":
        //                    if (Modify.TryInvokeDeclaredMethod(value, "GetObjectArray", out object bbb, new object[] { }))
        //                    {
        //                        if (bbb is object[])
        //                        {

        //                        }
        //                    }
        //                    ToJson(value[0], dictionary_Temp);
        //                    break;

        //                case "IronPython.Runtime.PythonTuple":
        //                    if (Modify.TryInvokeRuntimeMethod(value, "ToArray", out object array, new object[] { }))
        //                    {
        //                        if (array is object[])
        //                        {

        //                        }
        //                    }
        //                    break;
        //            }

        //            //if(type.Name.EndsWith("List"))
        //            //{
        //            //    ToJson(value[0], dictionary_Temp);
        //            //}
        //            //else if (type.Name.EndsWith("Dictionary"))
        //            //{
        //            //    ToJson(value, dictionary_Temp);
        //            //}
        //            //else if (type.Name.EndsWith("Tuple"))
        //            //{

        //            //}


        //            dictionary[name] = dictionary_Temp;
        //        }
        //    }
        //}
    }
}
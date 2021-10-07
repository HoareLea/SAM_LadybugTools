using System;
using System.Text.Json;

namespace SAM.Core.LadybugTools
{
    public static partial class Modify
    {
        public static void Write(this Utf8JsonWriter utf8JsonWriter, dynamic @dynamic)
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
                    if(Core.Query.TryInvokeDeclaredMethod(@dynamic, "items", out @object, new object[] { }))
                    {
                        utf8JsonWriter.Write(@object);
                    }
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

                    if(@object == null)
                    {
                        return;
                    }

                    utf8JsonWriter.Write(@object);

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
                            utf8JsonWriter.Write(value);
                        }
                        catch(Exception exception)
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
    }
}

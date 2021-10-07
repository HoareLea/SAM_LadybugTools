using System;
using System.Collections;
using System.Collections.Generic;
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

            List<Tuple<string, object>> tuples = new List<Tuple<string, object>>();
            List<object> objects = new List<object>();

            if (@dynamic is IEnumerable)
            {
                objects.AddRange(@dynamic);
            }
            else
            {
                switch (type.FullName)
                {
                    case "IronPython.Runtime.PythonDictionary":
                        if (Core.Query.TryInvokeDeclaredMethod(@dynamic, "items", out object pythonDictionary, new object[] { }))
                        {
                            if (Core.Query.TryInvokeDeclaredMethod(pythonDictionary, "GetObjectArray", out object pythonDictionaryArray, new object[] { }))
                            {
                                object[] objectArray = pythonDictionaryArray as object[];
                                if (objectArray != null)
                                {
                                    foreach (dynamic item in objectArray)
                                    {
                                        tuples.Add(new Tuple<string, object>(item[0], item[1]));
                                    }
                                }
                            }
                        }
                        break;

                    case "IronPython.Runtime.List":
                        if (Core.Query.TryInvokeDeclaredMethod(@dynamic, "GetObjectArray", out object list, new object[] { }))
                        {
                            object[] objectArray = list as object[];
                            if (objectArray != null)
                            {
                                objects.AddRange(objectArray);
                            }
                        }
                        break;

                    case "IronPython.Runtime.PythonTuple":
                        if (Core.Query.TryInvokeRuntimeMethod(@dynamic, "ToArray", out object tuple, new object[] { }))
                        {
                            object[] objectArray = tuple as object[];
                            if (objectArray != null)
                            {
                                objects.AddRange(objectArray);
                            }
                        }
                        break;

                    default:
                        object @object = null;
                        try
                        {
                            @object = @dynamic.to_dict();
                        }
                        catch
                        {
                            return;
                        }

                        if (@object == null)
                        {
                            return;
                        }

                        utf8JsonWriter.Write(@object);

                        break;
                }
            }

            if(tuples != null && tuples.Count != 0)
            {
                utf8JsonWriter.WriteStartObject();

                foreach (Tuple<string, object> tuple in tuples)
                {
                    string name = tuple.Item1;
                    object value = tuple.Item2;
                    if (value == null)
                    {
                        utf8JsonWriter.WriteNull(name);
                    }
                    else if (Core.Query.IsNumeric(value))
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
                        utf8JsonWriter.Write(value);
                    }
                }

                utf8JsonWriter.WriteEndObject();
            }

            if(objects != null && objects.Count == 0)
            {
                utf8JsonWriter.WriteStartArray();

                foreach (object @object in objects)
                {
                    if (@object == null)
                    {
                        utf8JsonWriter.WriteNullValue();
                    }
                    else if (Core.Query.IsNumeric(@object))
                    {
                        utf8JsonWriter.WriteNumberValue(@object as dynamic);
                    }
                    else if (@object is string)
                    {
                        utf8JsonWriter.WriteStringValue((string)@object);
                    }
                    else if (@object is Guid)
                    {
                        utf8JsonWriter.WriteStringValue((Guid)@object);
                    }
                    else if (@object is DateTime)
                    {
                        utf8JsonWriter.WriteStringValue((DateTime)@object);
                    }
                    else if (@object is bool)
                    {
                        utf8JsonWriter.WriteBooleanValue((bool)@object);
                    }
                    else
                    {
                        utf8JsonWriter.Write(@object);
                    }
                }

                utf8JsonWriter.WriteEndArray();
            }
        }
    }
}

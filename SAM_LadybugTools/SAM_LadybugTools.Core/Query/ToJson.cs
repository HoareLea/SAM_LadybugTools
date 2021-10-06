using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        public static string ToJson(this object @object)
        {
            if (@object == null)
                return null;

            dynamic @dynamic = null;
            try
            {
                @dynamic = (@object as dynamic).to_dict();
            }
            catch(Exception exception)
            {
                @dynamic = null;
            }

            if(@dynamic != null)
            {
                //TODO:  Finish Implementation

                JsonWriterOptions jsonWriterOptions = new JsonWriterOptions
                {
                    Indented = true
                };

                try
                {
                    string json = null;

                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    ToJson(@dynamic, dictionary);

                    //using (MemoryStream memoryStream = new MemoryStream())
                    //{
                    //    using (Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(memoryStream, jsonWriterOptions))
                    //    {
                    //        ToJson(@dynamic, utf8JsonWriter);

                    //        utf8JsonWriter.Flush();
                    //    }

                    //    json = Encoding.UTF8.GetString(memoryStream.ToArray());
                    //}
                }
                catch(Exception exception)
                {

                }
                

            }

            MethodInfo methodInfo = @object.GetType().GetMethod("ToJson", new Type[] { });

            List<string> names = new List<string>();
            foreach (MethodInfo methodInfo_Temp in @object.GetType(). GetRuntimeMethods())
            {
                names.Add(methodInfo_Temp.Name);

                if (!methodInfo_Temp.Name.Equals("ToJson"))
                    continue;

                ParameterInfo[] parameterInfos = methodInfo_Temp.GetParameters();
                if (parameterInfos != null && parameterInfos.Length > 0)
                {
                    foreach (ParameterInfo parameterInfo in parameterInfos)
                    {
                    }
                }
            }

            if (methodInfo != null)
            {
                return methodInfo.Invoke(@object, new object[] { }) as string;
            }
            
            return null;
        }

        private static void ToJson(dynamic @dynamic, Utf8JsonWriter utf8JsonWriter)
        {
            if(dynamic == null || utf8JsonWriter == null)
            {
                return;
            }

            utf8JsonWriter.WriteStartObject();

            foreach (dynamic item in @dynamic.items())
            {
                string name = item[0];
                object value = item[1];
                if (value == null)
                {
                    utf8JsonWriter.WriteNull(name);
                }

                if(Core.Query.IsNumeric(value))
                {
                    utf8JsonWriter.WriteNumber(name, value as dynamic);
                }
                else if(value is string)
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
                        ToJson(value, utf8JsonWriter);
                    }
                    catch
                    {

                    }
                }
            }

            utf8JsonWriter.WriteEndObject();
        }

        private static void ToJson(dynamic @dynamic, Dictionary<string, object> dictionary)
        {
            if (dynamic == null || dictionary == null)
            {
                return;
            }

            foreach (dynamic item in @dynamic.items())
            {
                string name = item[0];
                dynamic value = item[1];
                if (value == null)
                {
                    dictionary[name] = null;
                }
                if (Core.Query.IsNumeric(value) || value is string)
                {
                    dictionary[name] = value;
                }
                else
                {
                    Dictionary<string, object> dictionary_Temp = new Dictionary<string, object>();

                    Type type = value.GetType();
                    if(type.Name.EndsWith("List"))
                    {
                        ToJson(value[0], dictionary_Temp);
                    }
                    else if (type.Name.EndsWith("Dictionary"))
                    {
                        ToJson(value, dictionary_Temp);
                    }
                    else if (type.Name.EndsWith("Tuple"))
                    {
                        if(Modify.TryInvokeMethod(value, "ToArray", out object array, new object[] { }))
                        {
                            if(array is object[])
                            {

                            }
                        }
                    }

                    
                    dictionary[name] = dictionary_Temp;
                }
            }
        }


    }
}
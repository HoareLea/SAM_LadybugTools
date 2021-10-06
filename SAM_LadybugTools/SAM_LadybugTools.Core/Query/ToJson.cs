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

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(memoryStream, jsonWriterOptions))
                        {
                            ToJson(@dynamic, utf8JsonWriter);

                            utf8JsonWriter.Flush();
                        }

                        json = Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
                catch(Exception exception)
                {

                }
                

            }

            MethodInfo methodInfo = @object.GetType().GetMethod("ToJson", new Type[] { });

            List<string> names = new List<string>();
            foreach (MethodInfo methodInfo_Temp in @object.GetType().GetRuntimeMethods())
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
                    utf8JsonWriter.WriteString(name, value as dynamic);
                }
                else
                {
                    try
                    {
                        //ToJson(item, utf8JsonWriter);
                    }
                    catch
                    {

                    }
                }
            }

            utf8JsonWriter.WriteEndObject();
        }


    }
}
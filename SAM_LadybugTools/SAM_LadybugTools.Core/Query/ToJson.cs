using System;
using System.Collections.Generic;
using System.Reflection;

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

                string result = System.Text.Json.JsonSerializer.Serialize(@dynamic);

                return result;
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
    }
}
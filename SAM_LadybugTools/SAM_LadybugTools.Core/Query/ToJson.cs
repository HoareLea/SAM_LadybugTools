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

            if (methodInfo == null)
                return null;

            return methodInfo.Invoke(@object, new object[] { }) as string;
        }
    }
}
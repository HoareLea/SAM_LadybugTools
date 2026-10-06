// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        /// <summary>
        /// Reads a namespaced value from Honeybee user_data. Handles both the in-memory
        /// representation (IDictionary) and the deserialised representation (JObject).
        /// </summary>
        public static bool TryGetUserData(this IIDdBaseModel dDBaseModel, string key, out object value)
        {
            value = null;

            if (dDBaseModel == null || string.IsNullOrEmpty(key))
            {
                return false;
            }

            object userData = dDBaseModel.UserData;
            if (userData == null)
            {
                return false;
            }

            if (userData is IDictionary<string, object> dictionary)
            {
                if (!dictionary.TryGetValue(key, out value))
                {
                    return false;
                }

                value = NormaliseUserDataValue(value);
                return true;
            }

            if (userData is LBT.Newtonsoft.Json.Linq.JObject jObject)
            {
                LBT.Newtonsoft.Json.Linq.JToken jToken = jObject[key];
                if (jToken == null || jToken.Type == LBT.Newtonsoft.Json.Linq.JTokenType.Null)
                {
                    return false;
                }

                value = NormaliseUserDataValue(jToken);
                return true;
            }

            return false;
        }

        public static bool TryGetUserData<T>(this IIDdBaseModel dDBaseModel, string key, out T value)
        {
            value = default;

            if (!TryGetUserData(dDBaseModel, key, out object @object) || @object == null)
            {
                return false;
            }

            if (@object is T t)
            {
                value = t;
                return true;
            }

            try
            {
                value = (T)System.Convert.ChangeType(@object, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Double-typed read following the SAM convention for unset numeric values: when the
        /// key is missing or not convertible the out value is double.NaN rather than
        /// default(double), because 0 is a legitimate physical quantity while NaN marks
        /// "undefined". Callers rely on this (via double.IsNaN fallbacks) when importing
        /// Honeybee models not authored by SAM, which carry no namespaced user_data.
        /// </summary>
        public static bool TryGetUserData(this IIDdBaseModel dDBaseModel, string key, out double value)
        {
            if (TryGetUserData<double>(dDBaseModel, key, out double value_Temp))
            {
                value = value_Temp;
                return true;
            }

            value = double.NaN;
            return false;
        }

        /// <summary>
        /// Converts LBT.Newtonsoft.Json LINQ tokens into plain .NET objects so callers do
        /// not depend on the JSON representation of user_data.
        /// </summary>
        private static object NormaliseUserDataValue(object value)
        {
            if (value is LBT.Newtonsoft.Json.Linq.JValue jValue)
            {
                return jValue.Value;
            }

            if (value is LBT.Newtonsoft.Json.Linq.JObject jObject)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (KeyValuePair<string, LBT.Newtonsoft.Json.Linq.JToken> pair in jObject)
                {
                    dictionary[pair.Key] = NormaliseUserDataValue(pair.Value);
                }

                return dictionary;
            }

            if (value is LBT.Newtonsoft.Json.Linq.JArray jArray)
            {
                List<object> list = new List<object>();
                foreach (LBT.Newtonsoft.Json.Linq.JToken jToken in jArray)
                {
                    list.Add(NormaliseUserDataValue(jToken));
                }

                return list;
            }

            return value;
        }
    }
}

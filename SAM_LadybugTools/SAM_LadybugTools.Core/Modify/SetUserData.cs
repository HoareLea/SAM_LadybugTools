// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Core.LadybugTools
{
    public static partial class Modify
    {
        /// <summary>
        /// Sets a namespaced value in Honeybee user_data without overwriting unrelated
        /// user data. Only plain JSON-serialisable values should be stored.
        /// </summary>
        public static void SetUserData(this IIDdBaseModel dDBaseModel, string key, object value)
        {
            if (dDBaseModel == null || string.IsNullOrEmpty(key) || value == null)
            {
                return;
            }

            object userData = dDBaseModel.UserData;

            if (userData is LBT.Newtonsoft.Json.Linq.JObject jObject)
            {
                jObject[key] = LBT.Newtonsoft.Json.Linq.JToken.FromObject(value);
                return;
            }

            if (userData is IDictionary<string, object> dictionary)
            {
                dictionary[key] = value;
                return;
            }

            if (userData == null)
            {
                dDBaseModel.UserData = new Dictionary<string, object> { [key] = value };
                return;
            }

            // Unknown user_data representation: do not overwrite existing data.
        }
    }
}

// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        /// <summary>
        /// Reads the original SAM object Guid stored in namespaced Honeybee user_data.
        /// </summary>
        public static bool TryGetSAMGuid(this HoneybeeSchema.IIDdBaseModel dDBaseModel, out System.Guid guid)
        {
            guid = System.Guid.Empty;

            if (!Core.LadybugTools.Query.TryGetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.Guid, out string text) || string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            return System.Guid.TryParse(text, out guid);
        }

        /// <summary>
        /// Reads the SAM material classification stored in namespaced Honeybee user_data.
        /// </summary>
        public static bool TryGetSAMMaterialType(this HoneybeeSchema.IIDdBaseModel dDBaseModel, out MaterialType materialType)
        {
            materialType = MaterialType.Undefined;

            if (!Core.LadybugTools.Query.TryGetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.MaterialType, out string text) || string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            return System.Enum.TryParse(text, true, out materialType);
        }

        /// <summary>
        /// Serialises SAM construction layers into a user_data friendly list of dictionaries.
        /// </summary>
        public static List<Dictionary<string, object>> UserDataConstructionLayers(this IEnumerable<ConstructionLayer> constructionLayers)
        {
            if (constructionLayers == null)
            {
                return null;
            }

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (ConstructionLayer constructionLayer in constructionLayers)
            {
                if (constructionLayer == null)
                {
                    continue;
                }

                result.Add(new Dictionary<string, object>
                {
                    [Core.LadybugTools.UserDataKeys.ConstructionLayerName] = constructionLayer.Name,
                    [Core.LadybugTools.UserDataKeys.ConstructionLayerThickness] = constructionLayer.Thickness,
                });
            }

            return result;
        }

        /// <summary>
        /// Reads SAM construction layers stored in namespaced Honeybee user_data.
        /// </summary>
        public static bool TryGetConstructionLayers(this HoneybeeSchema.IIDdBaseModel dDBaseModel, string key, out List<ConstructionLayer> constructionLayers)
        {
            constructionLayers = null;

            if (!Core.LadybugTools.Query.TryGetUserData(dDBaseModel, key, out object value) || value == null)
            {
                return false;
            }

            List<object> list = value as List<object>;
            if (list == null)
            {
                return false;
            }

            constructionLayers = new List<ConstructionLayer>();
            foreach (object item in list)
            {
                IDictionary<string, object> dictionary = item as IDictionary<string, object>;
                if (dictionary == null)
                {
                    return false;
                }

                if (!dictionary.TryGetValue(Core.LadybugTools.UserDataKeys.ConstructionLayerName, out object nameObject) || nameObject == null)
                {
                    return false;
                }

                double thickness = double.NaN;
                if (dictionary.TryGetValue(Core.LadybugTools.UserDataKeys.ConstructionLayerThickness, out object thicknessObject) && thicknessObject != null)
                {
                    try
                    {
                        thickness = System.Convert.ToDouble(thicknessObject, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        thickness = double.NaN;
                    }
                }

                constructionLayers.Add(new ConstructionLayer(nameObject.ToString(), thickness));
            }

            return constructionLayers.Count > 0;
        }
    }
}

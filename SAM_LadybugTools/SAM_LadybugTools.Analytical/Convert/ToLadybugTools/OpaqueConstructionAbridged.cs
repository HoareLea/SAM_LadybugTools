// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static OpaqueConstructionAbridged ToLadybugTools(this Construction construction, bool reverse = true)
        {
            if (construction == null)
                return null;

            List<ConstructionLayer> constructionLayers = construction.ConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;

            List<Dictionary<string, object>> constructionLayers_UserData = Query.UserDataConstructionLayers(constructionLayers);

            if (reverse)
            {
                constructionLayers.Reverse();
            }

            OpaqueConstructionAbridged result = new OpaqueConstructionAbridged(Query.UniqueName(construction, reverse), constructionLayers.ConvertAll(x => x.Name), construction.Name);

            Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.Guid, construction.Guid.ToString());
            Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.Name, construction.Name);
            Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.ConstructionLayers, constructionLayers_UserData);

            if (construction.TryGetValue(ConstructionParameter.DefaultPanelType, out string panelType) && !string.IsNullOrWhiteSpace(panelType))
            {
                Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.DefaultPanelType, panelType);
            }

            return result;
        }

        public static OpaqueConstructionAbridged ToLadybugTools(this ApertureConstruction apertureConstruction, bool reverse = true)
        {
            if (apertureConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = apertureConstruction.PaneConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;

            List<Dictionary<string, object>> constructionLayers_UserData = Query.UserDataConstructionLayers(constructionLayers);

            if (reverse)
            {
                constructionLayers.Reverse();
            }

            OpaqueConstructionAbridged result = new OpaqueConstructionAbridged(Query.UniqueName(apertureConstruction, reverse), constructionLayers.ConvertAll(x => x.Name), apertureConstruction.Name);

            Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.Guid, apertureConstruction.Guid.ToString());
            Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.Name, apertureConstruction.Name);
            Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.PaneConstructionLayers, constructionLayers_UserData);

            List<ConstructionLayer> frameConstructionLayers = apertureConstruction.FrameConstructionLayers;
            if (frameConstructionLayers != null && frameConstructionLayers.Count != 0)
            {
                Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.FrameConstructionLayers, Query.UserDataConstructionLayers(frameConstructionLayers));
            }

            if (apertureConstruction.TryGetValue(ApertureConstructionParameter.DefaultPanelType, out string panelType) && !string.IsNullOrWhiteSpace(panelType))
            {
                Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.DefaultPanelType, panelType);
            }

            return result;
        }
    }
}

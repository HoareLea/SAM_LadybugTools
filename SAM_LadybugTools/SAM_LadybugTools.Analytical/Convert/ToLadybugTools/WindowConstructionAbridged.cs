// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static WindowConstructionAbridged ToLadybugTools_WindowConstructionAbridged(this ApertureConstruction apertureConstruction, bool reverse = true)
        {
            if (apertureConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = apertureConstruction.PaneConstructionLayers;
            if (constructionLayers == null)
                return null;

            List<Dictionary<string, object>> constructionLayers_UserData = Query.UserDataConstructionLayers(constructionLayers);

            if(reverse)
            {
                constructionLayers.Reverse();
            }

            WindowConstructionAbridged result = new WindowConstructionAbridged(
                identifier: Query.UniqueName(apertureConstruction, reverse),
                materials: constructionLayers.ConvertAll(x => x.Name),
                displayName: apertureConstruction.Name);

            Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.Guid, apertureConstruction.Guid.ToString());
            Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.Name, apertureConstruction.Name);
            Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.PaneConstructionLayers, constructionLayers_UserData);

            List<ConstructionLayer> frameConstructionLayers = apertureConstruction.FrameConstructionLayers;
            if (frameConstructionLayers != null && frameConstructionLayers.Count != 0)
            {
                Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.FrameConstructionLayers, Query.UserDataConstructionLayers(frameConstructionLayers));
                result.Frame = Query.FrameMaterialIdentifier(apertureConstruction);
            }

            if (apertureConstruction.TryGetValue(ApertureConstructionParameter.DefaultPanelType, out string panelType) && !string.IsNullOrWhiteSpace(panelType))
            {
                Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.DefaultPanelType, panelType);
            }

            return result;
        }
    }
}

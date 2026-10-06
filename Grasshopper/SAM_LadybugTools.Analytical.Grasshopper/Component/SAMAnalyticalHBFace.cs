// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors

using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.LadybugTools.Properties;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.Grasshopper.LadybugTools
{
    public class SAMAnalyticalHBFace : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("920a78fd-5cc5-4e68-bfa4-c8f57ac7569b");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.2";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Honeybee;

        /// <summary>
        /// Initializes a new instance of the SAMGeometryByGHGeometry class.
        /// </summary>
        public SAMAnalyticalHBFace()
          : base("SAMAnalytical.HBFace", "SAMAnalytical.HBFace",
              "SAM Analytical Panel to Ladybug Tools HB Face",
              "SAM", "LadybugTools")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooPanelParam() { Name = "_panel", NickName = "_panel", Description = "SAM Analytical Panel", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_Boolean param_Boolean = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_offsetAperturesOnEdge_", NickName = "_offsetAperturesOnEdge_", Description = "Offset Apertures On Edge", Access = GH_ParamAccess.item };
                param_Boolean.SetPersistentData(true);
                result.Add(new GH_SAMParam(param_Boolean, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_GenericObject() { Name = "HBFace", NickName = "HBFace", Description = "Ladybug Tools HB Face", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_GenericObject() { Name = "HBShades", NickName = "HBShades", Description = "Ladybug Tools HB Shades", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            Panel panel = null;

            int index = Params.IndexOfInputParam("_panel");
            if (index == -1 || !dataAccess.GetData(index, ref panel) || panel == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            bool offsetAperturesOnEdge = true;
            index = Params.IndexOfInputParam("_offsetAperturesOnEdge_");
            if (index != -1)
            {
                dataAccess.GetData(index, ref offsetAperturesOnEdge);
            }

            if(offsetAperturesOnEdge)
            {
                panel = Create.Panel(panel);
                panel.OffsetAperturesOnEdge(0.1);
            }

            HoneybeeSchema.Face face = Analytical.LadybugTools.Convert.ToLadybugTools_Face(panel);

            List<HoneybeeSchema.Shade> shades = Analytical.LadybugTools.Convert.ToLadybugTools_Shades(panel);

            index = Params.IndexOfOutputParam("HBFace");
            if (index != -1)
            {
                dataAccess.SetData(index, face?.ToJson());
            }

            index = Params.IndexOfOutputParam("HBShades");
            if (index != -1)
            {
                dataAccess.SetDataList(index, shades?.ConvertAll(x => x.ToJson()));
            }
        }
    }
}

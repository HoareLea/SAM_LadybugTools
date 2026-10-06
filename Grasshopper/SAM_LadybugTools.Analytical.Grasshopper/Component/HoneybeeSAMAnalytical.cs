// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using HoneybeeSchema;
using SAM.Analytical.Grasshopper.LadybugTools.Properties;
using SAM.Core;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.Grasshopper.LadybugTools
{
    public class HoneybeeSAMAnalytical : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("9b2a4ec7-c7ed-43b5-9c77-fa8387bd601e");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.1";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Honeybee;

        /// <summary>
        /// Initializes a new instance of the SAMGeometryByGHGeometry class.
        /// </summary>
        public HoneybeeSAMAnalytical()
          : base("Honeybee.SAMAnalytical", "Honeybee.SAMAnalytical",
              "Converts Honeybee Object to SAM Analytical",
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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_GenericObject() { Name = "_honeybee", NickName = "_honeybee", Description = "SAM Honeybee Object", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_GenericObject() { Name = "analytical", NickName = "analytical", Description = "SAM Analytical", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "json", NickName = "json", Description = "Honeybee Json", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            GH_ObjectWrapper objectWrapper = null;

            int index = Params.IndexOfInputParam("_honeybee");
            if (index == -1 || !dataAccess.GetData(index, ref objectWrapper) || objectWrapper == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            object value = objectWrapper.Value;
            if(value is IGH_Goo)
            {
                value = (value as dynamic).Value;
            }

            if(value == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            SAMObject result = null;

            string json = null;

            try
            {
                json  = Core.LadybugTools.Convert.ToString(value);
            }
            catch (Exception exception)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("Failed to serialise Honeybee object to JSON ({0}): {1}", value.GetType().FullName, exception.Message));
            }

            if(string.IsNullOrWhiteSpace(json))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("Could not serialise the input object to Honeybee JSON (input type: {0}).", value.GetType().FullName));
            }
            else
            {
                try
                {
                    IDdBaseModel ddBaseModel = Core.LadybugTools.Convert.ToHoneybee(value, out Log log);

                    if (log != null)
                    {
                        foreach (LogRecord logRecord in log)
                        {
                            if (logRecord == null || string.IsNullOrWhiteSpace(logRecord.Text))
                            {
                                continue;
                            }

                            AddRuntimeMessage(logRecord.LogRecordType == LogRecordType.Error ? GH_RuntimeMessageLevel.Error : GH_RuntimeMessageLevel.Warning, logRecord.Text);
                        }
                    }

                    if (ddBaseModel == null)
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Honeybee JSON could not be deserialised to a Honeybee object.");
                    }
                    else
                    {
                        try
                        {
                            result = Analytical.LadybugTools.Convert.ToSAM(ddBaseModel);
                        }
                        catch (Exception exception)
                        {
                            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("ToSAM failed ({0}): {1}", ddBaseModel.GetType().Name, exception));
                        }

                        if (result == null)
                        {
                            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("SAM conversion of Honeybee '{0}' returned null.", ddBaseModel.GetType().Name));
                        }
                    }
                }
                catch (Exception exception)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, string.Format("Honeybee deserialisation failed: {0}", exception));
                }
            }

            if (result == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Conversion produced no SAM AnalyticalModel.");
            }

            index = Params.IndexOfOutputParam("analytical");
            if (index != -1)
            {
                dataAccess.SetData(index, result);
            }

            index = Params.IndexOfOutputParam("json");
            if (index != -1)
            {
                dataAccess.SetData(index, json);
            }
        }
    }
}

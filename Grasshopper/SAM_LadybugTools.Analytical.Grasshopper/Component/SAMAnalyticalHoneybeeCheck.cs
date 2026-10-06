// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors

using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.LadybugTools.Properties;
using SAM.Core;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public class SAMAnalyticalHoneybeeCheck : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("35ab1b3f-10c2-4a23-8755-17edcfe93608");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.1";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Honeybee;

        /// <summary>
        /// Initializes a new instance of the SAM_point3D class.
        /// </summary>
        public SAMAnalyticalHoneybeeCheck()
          : base("SAMAnalytical.HBModelCheck", "SAMAnalytical.HBModelCheck",
              "Check Honeybee object agains Honeybee schema",
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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "_json", NickName = "_json", Description = "Honeybee object in Json", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_Boolean param_Boolean = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_run", NickName = "_run", Description = "Run", Access = GH_ParamAccess.item };
                param_Boolean.SetPersistentData(false);
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
                result.Add(new GH_SAMParam(new GooLogParam() { Name = "Log", NickName = "Log", Description = "SAM Log", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooLogParam() { Name = "Messages", NickName = "Messages", Description = "SAM Log with Messages", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            bool run = false;
            int index = Params.IndexOfInputParam("_run");
            if (index == -1 || !dataAccess.GetData(index, ref run) || !run)
                return;

            string json = null;
            index = Params.IndexOfInputParam("_json");
            if (index == -1 || !dataAccess.GetData(index, ref json))
                return;

            Log log = new Log();

            if (string.IsNullOrWhiteSpace(json))
            {
                log.Add("Input JSON is null or empty.", LogRecordType.Error);

                index = Params.IndexOfOutputParam("Log");
                if (index != -1)
                {
                    dataAccess.SetData(index, log.Filter(new LogRecordType[] { LogRecordType.Error, LogRecordType.Warning, LogRecordType.Undefined }));
                }

                index = Params.IndexOfOutputParam("Messages");
                if (index != -1)
                {
                    dataAccess.SetData(index, log.Filter(new LogRecordType[] { LogRecordType.Message }));
                }

                return;
            }

            // Use the same validated conversion/deserialisation path as the rest of the
            // library rather than maintaining inconsistent parsing behaviour.
            HoneybeeSchema.IDdBaseModel iDdBaseModel = Core.LadybugTools.Convert.ToHoneybee(json, out Log parseLog);
            if (parseLog != null)
                Core.Modify.AddRange(log, parseLog);

            if (iDdBaseModel != null)
            {
                // Log assembly identity details for diagnostics
                log.Add("HoneybeeSchema assembly version: {0}", LogRecordType.Message, Core.LadybugTools.Query.HoneybeeSchemaVersion());

                try
                {
                    string assemblyPath = typeof(HoneybeeSchema.IDdBaseModel).Assembly.Location;
                    if (!string.IsNullOrWhiteSpace(assemblyPath))
                        log.Add("Loaded from: {0}", LogRecordType.Message, assemblyPath);
                }
                catch { }

                // Log type information from the deserialised object
                log.Add("Honeybee object type: {0}", LogRecordType.Message, iDdBaseModel.GetType().Name);

                // Run the existing validation logic (identifier checks, etc.)
                Log validationLog = null;
                if (iDdBaseModel is HoneybeeSchema.Model model)
                    validationLog = SAM.Core.LadybugTools.Create.Log(model);
                else if (iDdBaseModel is HoneybeeSchema.Room room)
                    validationLog = SAM.Core.LadybugTools.Create.Log(room);
                else if (iDdBaseModel is HoneybeeSchema.Face face)
                    validationLog = SAM.Core.LadybugTools.Create.Log(face);
                else if (iDdBaseModel is HoneybeeSchema.Aperture aperture)
                    validationLog = SAM.Core.LadybugTools.Create.Log(aperture);
                else if (iDdBaseModel is HoneybeeSchema.Door door)
                    validationLog = SAM.Core.LadybugTools.Create.Log(door);
                else if (iDdBaseModel is HoneybeeSchema.Shade shade)
                    validationLog = SAM.Core.LadybugTools.Create.Log(shade);
                else
                    validationLog = SAM.Core.LadybugTools.Create.Log((HoneybeeSchema.IIDdBase)iDdBaseModel);

                if (validationLog != null)
                    Core.Modify.AddRange(log, validationLog);
            }

            // The diagnostic Messages above mean the log is never empty on a successful run,
            // so gate the success message on the absence of problem records, not on Count().
            Log log_Problems = log.Filter(new LogRecordType[] { LogRecordType.Error, LogRecordType.Warning, LogRecordType.Undefined });
            if (log_Problems.Count() == 0)
                log.Add("All good! You can switch off your computer and go home now.", LogRecordType.Message);

            index = Params.IndexOfOutputParam("Log");
            if (index != -1)
            {
                dataAccess.SetData(index, log_Problems);
            }

            index = Params.IndexOfOutputParam("Messages");
            if (index != -1)
            {
                dataAccess.SetData(index, log.Filter(new LogRecordType[] { LogRecordType.Message }));
            }
        }
    }
}

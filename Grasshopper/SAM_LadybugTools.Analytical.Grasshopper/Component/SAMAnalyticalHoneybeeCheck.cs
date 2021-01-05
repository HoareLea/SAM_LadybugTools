using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.LadybugTools.Properties;
using SAM.Core;
using SAM.Core.Grasshopper;
using System;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public class SAMAnalyticalHoneybeeCheck : GH_SAMComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("35ab1b3f-10c2-4a23-8755-17edcfe93608");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

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
        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
        {
            inputParamManager.AddTextParameter("_json", "_json", "Honeybee object in Json", GH_ParamAccess.item);
            inputParamManager.AddBooleanParameter("_run", "_run", "Run", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager)
        {
            outputParamManager.AddParameter(new GooLogParam(), "Log", "Log", "SAM Log", GH_ParamAccess.item);
            outputParamManager.AddParameter(new GooLogParam(), "Messages", "Messages", "SAM Log with Messages", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            bool run = false;
            if (!dataAccess.GetData(1, ref run) || !run)
                return;

            string json = null;
            if (!dataAccess.GetData(0, ref json))
                return;

            Log log = null;
            try
            {
                HoneybeeSchema.IDdBaseModel iDdBaseModel = HoneybeeSchema.IDdBaseModel.FromJson(json);
                log = Create.Log(iDdBaseModel as dynamic);
            }
            catch
            {
                dataAccess.SetData(0, null);
                dataAccess.SetData(1, null);
                return;
            }

            if (log == null)
                log = new Log();

            if (log.Count() == 0)
                log.Add("All good! You can switch off your computer and go home now.");

            dataAccess.SetData(0, log.Filter(new LogRecordType[] { LogRecordType.Error, LogRecordType.Warning, LogRecordType.Undefined }));
            dataAccess.SetData(1, log.Filter(new LogRecordType[] { LogRecordType.Message }));
        }
    }
}
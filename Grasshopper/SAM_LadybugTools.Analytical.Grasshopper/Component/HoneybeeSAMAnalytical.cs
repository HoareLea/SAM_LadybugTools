using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using HoneybeeSchema;
using SAM.Analytical.Grasshopper.LadybugTools.Properties;
using SAM.Core;
using SAM.Core.Grasshopper;
using System;

namespace SAM.Analytical.Grasshopper.LadybugTools
{
    public class HoneybeeSAMAnalytical : GH_SAMComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("9b2a4ec7-c7ed-43b5-9c77-fa8387bd601e");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

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
        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
        {
            inputParamManager.AddGenericParameter("_honeybee", "_honeybee", "SAM Honeybee Object", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager)
        {
            outputParamManager.AddGenericParameter("analytical", "analytical", "SAM Analytical", GH_ParamAccess.item);
            outputParamManager.AddTextParameter("json", "json", "Honeybee Json", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            GH_ObjectWrapper objectWrapper = null;

            if (!dataAccess.GetData(0, ref objectWrapper) || objectWrapper == null)
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

            IDdBaseModel ddBaseModel = Core.LadybugTools.Convert.ToHoneybee(value);
            
            if (ddBaseModel != null)
            {
                result = Analytical.LadybugTools.Convert.ToSAM(ddBaseModel);
            }

            dataAccess.SetData(0, result);

            dataAccess.SetData(1, ddBaseModel?.ToJson());
        }
    }
}
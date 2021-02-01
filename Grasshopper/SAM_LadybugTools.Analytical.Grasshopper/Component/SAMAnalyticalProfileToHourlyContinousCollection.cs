using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.LadybugTools.Properties;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.Grasshopper.LadybugTools
{
    public class SAMAnalyticalProfileToHourlyContinousCollection : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("b87cf215-90e3-4345-b037-ccedecd959bc");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Honeybee;

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Initializes a new instance of the SAMGeometryByGHGeometry class.
        /// </summary>
        public SAMAnalyticalProfileToHourlyContinousCollection()
          : base("SAMAnalytical.ProfileToHourlyContinousCollection", "SAMAnalytical.ProfileToHourlyContinousCollection",
              "SAM Analytical Profile to HourlyContinousCollection",
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
                result.Add(new GH_SAMParam(new GooProfileParam() { Name = "_profile", NickName = "_profile", Description = "SAM Analytical Profile", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "HourlyContinousCollection", NickName = "HourlyContinousCollection", Description = "HourlyContinousCollection", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index = -1;

            Profile profile = null;

            index = Params.IndexOfInputParam("_profile");
            if(index == -1 || !dataAccess.GetData(index, ref profile) || profile == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfOutputParam("HourlyContinousCollection");
            if(index != -1)
            {
                string hourlyContinousCollection = Analytical.LadybugTools.Convert.ToLadybugTools_HourlyContinousCollection(profile);
                dataAccess.SetData(index, hourlyContinousCollection);
            }
        }
    }
}
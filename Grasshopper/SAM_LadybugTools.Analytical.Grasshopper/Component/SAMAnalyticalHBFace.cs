using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.LadybugTools.Properties;
using System;

namespace SAM.Analytical.Grasshopper.LadybugTools
{
    public class SAMAnalyticalHBFace : GH_Component
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("920a78fd-5cc5-4e68-bfa4-c8f57ac7569b");

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Honeybee;

        /// <summary>
        /// Initializes a new instance of the SAMGeometryByGHGeometry class.
        /// </summary>
        public SAMAnalyticalHBFace()
          : base("SAMAnalytical.HBFace", "SAMAnalytical.HBFace",
              "SAM Analytica Panel to Ladybug Tools HB Face",
              "SAM", "LadybugTools")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
        {
            inputParamManager.AddParameter(new GooPanelParam(), "_panel", "_panel", "SAM Analytica Panel", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager)
        {
            outputParamManager.AddGenericParameter("HBFace", "HBFace", "Ladybug Tools HB Face", GH_ParamAccess.item);
            outputParamManager.AddGenericParameter("HBShade", "HBShade", "Ladybug Tools HB Shade", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            Panel panel = null;

            if (!dataAccess.GetData(0, ref panel) || panel == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            HoneybeeSchema.Face face = Analytical.LadybugTools.Convert.ToLadybugTools_Face(panel);

            HoneybeeSchema.Shade shade = Analytical.LadybugTools.Convert.ToLadybugTools_Shade(panel);

            dataAccess.SetData(0, face?.ToJson());
            dataAccess.SetData(1, shade?.ToJson());
        }
    }
}
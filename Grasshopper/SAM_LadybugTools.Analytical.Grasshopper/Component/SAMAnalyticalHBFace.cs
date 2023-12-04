using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.LadybugTools.Properties;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.Grasshopper.LadybugTools
{
    public class SAMAnalyticalHBFace : GH_SAMComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("920a78fd-5cc5-4e68-bfa4-c8f57ac7569b");

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
        public SAMAnalyticalHBFace()
          : base("SAMAnalytical.HBFace", "SAMAnalytical.HBFace",
              "SAM Analytical Panel to Ladybug Tools HB Face",
              "SAM", "LadybugTools")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
        {
            inputParamManager.AddParameter(new GooPanelParam(), "_panel", "_panel", "SAM Analytical Panel", GH_ParamAccess.item);
            inputParamManager.AddBooleanParameter("_offsetAperturesOnEdge_", "_offsetAperturesOnEdge_", "Offset Apertures On Edge", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager)
        {
            outputParamManager.AddGenericParameter("HBFace", "HBFace", "Ladybug Tools HB Face", GH_ParamAccess.item);
            outputParamManager.AddGenericParameter("HBShades", "HBShades", "Ladybug Tools HB Shades", GH_ParamAccess.list);
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

            bool offsetAperturesOnEdge = true;
            dataAccess.GetData(1, ref offsetAperturesOnEdge);
            
            if(offsetAperturesOnEdge)
            {
                panel = Create.Panel(panel);
                panel.OffsetAperturesOnEdge(0.1);
            }

            HoneybeeSchema.Face face = Analytical.LadybugTools.Convert.ToLadybugTools_Face(panel);

            List<HoneybeeSchema.Shade> shades = Analytical.LadybugTools.Convert.ToLadybugTools_Shades(panel);

            dataAccess.SetData(0, face?.ToJson());
            dataAccess.SetDataList(1, shades?.ConvertAll(x => x.ToJson()));
        }
    }
}
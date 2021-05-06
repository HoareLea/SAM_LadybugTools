using Grasshopper.Kernel;
using HoneybeeSchema;
using SAM.Core.Grasshopper;
using SAM.Geometry.Grasshopper.LadybugTools.Properties;
using System;

namespace SAM.Geometry.Grasshopper.LadybugTools
{
    public class SAMGeometryLBGeometry : GH_SAMComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c306f8ae-e25b-4bc9-93d2-5155a86b55ef");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Initializes a new instance of the SAM_point3D class.
        /// </summary>
        public SAMGeometryLBGeometry()
          : base("SAMGeometry.LBGeometry", "SAMGeometry.LBGeometry",
              "Convert SAM Geometry to LadybugTools Geometry",
              "SAM", "LadybugTools")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
        {
            inputParamManager.AddParameter(new GooSAMGeometryParam(), "_SAMGeometry", "_SAMGeometry", "SAM Geometry", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager)
        {
            outputParamManager.AddTextParameter("LBGeometry", "LBgeo", "LB Geometry", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">
        /// The DA object is used to retrieve from inputs and store in outputs.
        /// </param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            ISAMGeometry sAMGeometry = null;

            if (!dataAccess.GetData(0, ref sAMGeometry) || sAMGeometry == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            HoneybeeObject honeybeeObject =  Geometry.LadybugTools.Convert.ToLadybugTools(sAMGeometry as dynamic) as HoneybeeObject;
            dataAccess.SetData(0, honeybeeObject?.ToJson());

            //object obj = objectWrapper.Value;

            //dynamic lBObject = obj as dynamic;
            //string aName = lBObject._name;
            //switch (aName)
            //{
            //    case ("Python Types: Point"):
            //        //dataAccess.SetData(0, point3D.ToGrasshopper());
            //        return;
            //}

            //Point3D point3D = obj as Point3D;
            //if (point3D != null)
            //{
            //    dataAccess.SetData(0, point3D.ToGrasshopper());
            //    return;
            //}

            //Segment3D segment3D = obj as Segment3D;
            //if (segment3D != null)
            //{
            //    //dataAccess.SetData(0, segment3D.ToGrasshopper());
            //    dataAccess.SetData(0, null);
            //    return;
            //}

            //Polygon3D polygon3D = obj as Polygon3D;
            //if (polygon3D != null)
            //{
            //    dataAccess.SetData(0, polygon3D.ToGrasshopper());
            //    return;
            //}

            //AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Cannot convert geometry");
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resources.SAM_Honeybee;
            }
        }
        
        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}
// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020–2026 Michal Dengusiak & Jakub Ziolkowski and contributors

using Grasshopper.Kernel;
using HoneybeeSchema;
using SAM.Core.Grasshopper;
using SAM.Geometry.Grasshopper.LadybugTools.Properties;
using System;
using System.Collections.Generic;

namespace SAM.Geometry.Grasshopper.LadybugTools
{
    public class SAMGeometryLBGeometry : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c306f8ae-e25b-4bc9-93d2-5155a86b55ef");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.1";

        /// <summary>
        /// Initializes a new instance of the SAM_point3D class.
        /// </summary>
        public SAMGeometryLBGeometry()
          : base("SAMGeometry.LBGeometry", "SAMGeometry.LBGeometry",
              "Convert SAM Geometry to LadybugTools Geometry",
              "SAM", "LadybugTools")
        {
            // GH_SAMVariableOutputParameterComponent.RegisterOutputParams clones each declared
            // Param via IGH_Param.Clone(), which resets NickName to Name for stock Grasshopper
            // param types when the two differ. Restore the legacy NickName ("LBgeo") here, once,
            // after base construction/registration has completed.
            int index = Params.IndexOfOutputParam("LBGeometry");
            if (index != -1)
            {
                Params.Output[index].NickName = "LBgeo";
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooSAMGeometryParam() { Name = "_SAMGeometry", NickName = "_SAMGeometry", Description = "SAM Geometry", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "LBGeometry", NickName = "LBgeo", Description = "LB Geometry", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
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

            int index = Params.IndexOfInputParam("_SAMGeometry");
            if (index == -1 || !dataAccess.GetData(index, ref sAMGeometry) || sAMGeometry == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            IHoneybeeObject honeybeeObject =  Geometry.LadybugTools.Convert.ToLadybugTools(sAMGeometry as dynamic) as IHoneybeeObject;

            index = Params.IndexOfOutputParam("LBGeometry");
            if (index != -1)
            {
                dataAccess.SetData(index, honeybeeObject?.ToJson());
            }

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

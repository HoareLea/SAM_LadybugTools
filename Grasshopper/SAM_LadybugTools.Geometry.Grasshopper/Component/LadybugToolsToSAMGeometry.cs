using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Newtonsoft.Json.Linq;

using SAM.Geometry.Grasshopper.LadybugTools.Properties;

namespace SAM.Geometry.Grasshopper.LadybugTools
{
    public class LadybugToolsToSAMGeometry : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SAMGeometryByGHGeometry class.
        /// </summary>
        public LadybugToolsToSAMGeometry()
          : base("LadybugToolsToSAMGeometry", "SAMgeo",
              "Description SAMGeometryByLBGeometry",
              "SAM", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
        {
            inputParamManager.AddGenericParameter("LBGeometry", "LBgeo", "Ladybug Geometry", GH_ParamAccess.item);
        }


        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager)
        {
            outputParamManager.AddGenericParameter("SAMGeometry", "SAMgeo", "SAM Geometry", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            GH_ObjectWrapper objectWrapper = null;

            if (!dataAccess.GetData(0, ref objectWrapper) || objectWrapper.Value == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            object obj = objectWrapper.Value;

            Type aType = obj.GetType();



            //ObjectHandle a;
            //a.
            //ObjectOperations a = new ObjectOperations()

            //a.
            //IronPython.Runtime.Types.PythonType pythonType = new IronPython.Runtime.Types.

            //string aClass = (obj as dynamic).__class__;
            //string aTypeName = (obj as dynamic)._type;
            //switch (aTypeName)
            //{
            //    case ("PythonType: \"Point2D\""):
            //        dataAccess.SetData(0, Convert.ToSAM_Point3D(obj));
            //        return;
            //    case ("PythonType: \"Face\""):
            //        dataAccess.SetData(0, Convert.ToSAM_Polygon3D(obj));
            //        return;
            //}

            //var aValue = (obj as GH_ObjectWrapper).Value;

            //GH_Point point = obj as GH_Point;
            //if (point != null)
            //{ 
            //    dataAccess.SetData(0, point.ToSAM());
            //    return;
            //}

            //GH_Line line = obj as GH_Line;
            //if (line != null)
            //{
            //    dataAccess.SetData(0, line.ToSAM());
            //    return;
            //}

            //GH_Curve curve = obj as GH_Curve;
            //if (curve != null)
            //{
            //    IGeometry geometry = null;

            //    if (curve.Value is Rhino.Geometry.LineCurve)
            //        geometry = ((Rhino.Geometry.LineCurve)curve.Value).Line.ToSAM();
            //    else
            //        geometry = curve.ToSAM();

            //    if(geometry != null)
            //    {
            //        dataAccess.SetData(0, geometry);
            //        return;
            //    }
            //}

            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Cannot convert geometry");
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
                return Resources.SAM_Small;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("920a78fd-5cc5-4e68-bfa4-c8f57ac7569a"); }
        }

        public static IEnumerable<string> GetPropertyNames(dynamic dynamicObject)
        {
            JObject jObject = (JObject)JToken.FromObject(dynamicObject);
            if (jObject == null)
                return null;

            Dictionary<string, object> dictionary = jObject.ToObject<Dictionary<string, object>>();
            if (dictionary == null)
                return null;

            return dictionary.Keys;

            //JObject attributesAsJObject = dynamicToGetPropertiesFor;
            //Dictionary<string, object> values = attributesAsJObject.ToObject<Dictionary<string, object>>();
            //List<string> toReturn = new List<string>();
            //foreach (string key in values.Keys)
            //    toReturn.Add(key);
            //return toReturn;
        }
    }
}
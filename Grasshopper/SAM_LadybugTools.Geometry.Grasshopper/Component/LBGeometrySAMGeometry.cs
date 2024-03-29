﻿extern alias SAM_Newtonsoft;

using SAM_Newtonsoft::Newtonsoft.Json.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper;
using SAM.Geometry.Grasshopper.LadybugTools.Properties;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAM.Geometry.Grasshopper.LadybugTools
{
    public class LBGeometrySAMGeometry : GH_SAMComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("920a78fd-5cc5-4e68-bfa4-c8f57ac7569a");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Initializes a new instance of the SAMGeometryByGHGeometry class.
        /// </summary>
        public LBGeometrySAMGeometry()
          : base("LBGeometry.SAMGeometry", "LBGeometry.SAMGeometry",
              "Ladybug Tools Geometry to SAM Geometry",
              "SAM", "LadybugTools")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager inputParamManager)
        {
            inputParamManager.AddGenericParameter("_LBGeometry", "_LBGeometry", "Ladybug Geometry", GH_ParamAccess.item);
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
        /// <param name="dataAccess">
        /// The DA object is used to retrieve from inputs and store in outputs.
        /// </param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            GH_ObjectWrapper objectWrapper = null;

            if (!dataAccess.GetData(0, ref objectWrapper) || objectWrapper.Value == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            dynamic obj = objectWrapper.Value;
            Type type = obj.GetType();

            MethodInfo methodInfo = null;
            foreach (MethodInfo methodInfo_Temp in type.GetRuntimeMethods())
            {
                if (methodInfo_Temp.Name == "get_Dict")
                {
                    methodInfo = methodInfo_Temp;
                    break;
                }
            }

            if (methodInfo == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            ParameterInfo[] a = methodInfo.GetParameters();
            if (a.Length == 0)
            {
                object magicValue = methodInfo.Invoke(obj, null);
            }
            else
            {
                object magicValue = methodInfo.Invoke(obj, new object[] { });
            }

            //ScriptEngine scriptEngine = Python.CreateEngine();
            //ScriptScope scriptScope = scriptEngine.CreateScope();

            //ScriptSource scriptSource = scriptEngine.CreateScriptSourceFromFile(@"C:\Users\ziolkowskij\Documents\GitHub\External\ladybug-geometry\ladybug_geometry\geometry2d\line.py");

            //GH_Component x = new GH_Component();

            //GH_DocumentObject k = new GH_DocumentObject("", null, null, null, null);

            //GH_UserObject b = new GH_UserObject() ;
            //b.

            //object result = scriptSource.Execute(scriptScope);

            //string parameter = scriptScope.GetVariable<string>("parameter");
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

            // if (curve.Value is Rhino.Geometry.LineCurve) geometry =
            // ((Rhino.Geometry.LineCurve)curve.Value).Line.ToSAM(); else geometry = curve.ToSAM();

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
                return Resources.SAM_Honeybee;
            }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

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
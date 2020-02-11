using System;
using System.Collections.Generic;
using System.Reflection;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json.Linq;

using SAM.Analytical.Grasshopper.LadybugTools.Properties;

namespace SAM.Analytical.Grasshopper.LadybugTools
{
    public class LBGeometrySAMGeometry : GH_Component
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("920a78fd-5cc5-4e68-bfa4-c8f57ac7569a");

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Honeybee;

        /// <summary>
        /// Initializes a new instance of the SAMGeometryByGHGeometry class.
        /// </summary>
        public LBGeometrySAMGeometry()
          : base("LBAnalytical.SAMAnalytical", "LBAnalytical.SAMAnalytical",
              "Ladybug Tools Analytical to SAM Analytical",
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
        /// <param name="dataAccess">The DA object is used to retrieve from inputs and store in outputs.</param>
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

            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Cannot convert geometry");
        }
    }
}
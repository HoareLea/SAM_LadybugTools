//using System;

//using Grasshopper.Kernel;
//using Grasshopper.Kernel.Types;
//using SAM.Core.Grasshopper.LadybugTools.Properties;

//namespace SAM.Analytical.Grasshopper.LadybugTools
//{
//    public class HBObjectJson : GH_Component
//    {
//        /// <summary>
//        /// Gets the unique ID for this component. Do not change this ID after release.
//        /// </summary>
//        public override Guid ComponentGuid => new Guid("98202117-a09e-4ed7-98a9-4b5a64011b23");

// ///
// <summary>
// /// Provides an Icon for the component. ///
// </summary>
// protected override System.Drawing.Bitmap Icon =&gt; Resources.SAM_Honeybee;

// ///
// <summary>
// /// Initializes a new instance of the SAMGeometryByGHGeometry class. ///
// </summary>
// public HBObjectJson() : base("HBObject.Json", "HBObject.Json", "Ladybug Tools HB Object to JSON",
// "SAM", "LadybugTools") { }

// ///
// <summary>
// /// Registers all the input parameters for this component. ///
// </summary>
// protected override void RegisterInputParams(GH_InputParamManager inputParamManager) {
// inputParamManager.AddGenericParameter("_HB Object", "_HB Object", "Ladybug Tools HB Object",
// GH_ParamAccess.item); inputParamManager.AddTextParameter("_path", "_path", "JSON file path
// including extension .json", GH_ParamAccess.item); inputParamManager.AddBooleanParameter("_run_",
// "_run_", "Run, set to True to export JSON to given path", GH_ParamAccess.item, false); }

// ///
// <summary>
// /// Registers all the output parameters for this component. ///
// </summary>
// protected override void RegisterOutputParams(GH_OutputParamManager outputParamManager) {
// outputParamManager.AddTextParameter("Json", "Json", "Json", GH_ParamAccess.item);
// outputParamManager.AddBooleanParameter("Successful", "Successful", "Correctly imported?",
// GH_ParamAccess.item); }

// /// <summary> /// This is the method that actually does the work. /// </summary> /// <param
// name="dataAccess">The DA object is used to retrieve from inputs and store in outputs.</param>
// protected override void SolveInstance(IGH_DataAccess dataAccess) { bool run = false; if
// (!dataAccess.GetData<bool>(2, ref run)) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error,
// "Invalid data"); dataAccess.SetData(1, false); return; } if (!run) return;

// string path = null; if (!dataAccess.GetData<string>(1, ref path)) {
// AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data"); dataAccess.SetData(1, false);
// return; }

// GH_ObjectWrapper objectWrapper = null; if (!dataAccess.GetData(0, ref objectWrapper) ||
// objectWrapper == null) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
// dataAccess.SetData(1, false); return; }

// object @object = objectWrapper.Value;

// if (@object is IGH_Goo) { try { @object = (@object as dynamic).Value; } catch (Exception
// exception) { @object = null; } }

// if (@object == null) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
// dataAccess.SetData(1, false); return; }

// string json = Core.LadybugTools.Query.ToJson(@object); if(json == null) { dataAccess.SetData(1,
// false); return; }

//            dataAccess.SetData(0, json);
//        }
//    }
//}
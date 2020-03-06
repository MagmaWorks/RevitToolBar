using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using Grasshopper.GUI.Canvas;
using System.Drawing;
using CarbonCalculator;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace CarbonCalculatorGH
{
    public class CCElement : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public CCElement()
          : base("CC Element", "CCE",
              "",
              "Magma Works", "Carbon Counting")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "", "Name of element", GH_ParamAccess.item);
            pManager.AddNumberParameter("Volume", "", "Volume of element in cubic metres", GH_ParamAccess.item);
            pManager.AddTextParameter("Unique ID", "", "Unique ID of element. Must be different for all elements.", GH_ParamAccess.item);
            pManager.AddTextParameter("Filter values", "", "List of filter values. Each value is paired with its corresponding key in the Carbon Calculator component.", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Carbon Element object", "", "A CCE object.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            if (!DA.GetData(0,ref name)) return;
            double vol = 0;
            if (!DA.GetData(1, ref vol)) return;
            string id = "";
            if (!DA.GetData(2, ref id)) return;
            List<string> filterVals = new List<string>();
            if (!DA.GetDataList(3, filterVals)) return;

            Element elem = new Element(name, vol, id, filterVals.ToArray());

            DA.SetData(0, elem);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.MagmaWorks_logo ;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("527c8fab-c3be-4631-9b96-6fb09884a214"); }
        }
    }
}


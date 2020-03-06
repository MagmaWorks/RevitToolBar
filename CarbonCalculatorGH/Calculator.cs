using System;
using System.Windows;
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
    public class Calculator : GH_Component
    {
        Window myWindow;
        bool closed = false;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Calculator()
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
            pManager.AddGenericParameter("Elements", "", "Elements", GH_ParamAccess.list);
            pManager.AddGenericParameter("Filters", "", "Filter names", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //pManager.AddGenericParameter("Carbon Element object", "", "A CCE object.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Element> elements = new List<Element>();
            if (!DA.GetDataList(0, elements)) return;
            List<string> filters = new List<string>();
            if (!DA.GetDataList(1, filters)) return;
            var set = new ElementSet(filters.ToArray());

            foreach (var elem in elements)
            {
                set.AddElement(elem);
            }

            var vm = new AppVM(set);

            if (myWindow == null || closed)
            {
                var newVM = vm;
                myWindow = new Window()
                {
                    Title = "Carbon Calculator",
                    Content = new UserControl1(),
                    DataContext = newVM
                };
                myWindow.Closed += MyWindow_Closed;
                myWindow.Show();
                closed = false;
            }
            else
            {
                (myWindow.DataContext as AppVM).Model.changeElementSet(set);
            }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.MagmaWorks_logo;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c7d2d860-34ee-4283-ac57-8efec93e3376"); }
        }

        private void MyWindow_Closed(object sender, EventArgs e)
        {
            closed = true;
        }
    }
}


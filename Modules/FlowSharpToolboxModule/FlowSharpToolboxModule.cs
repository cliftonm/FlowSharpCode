using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Clifton.Core.ModuleManagement;
using Clifton.Core.Semantics;
using Clifton.Core.ServiceManagement;

using FlowSharpLib;
using FlowSharpServices;

namespace FlowSharpToolboxModule
{
    public class FlowSharpToolboxModule : IModule
    {
        public void InitializeServices(IServiceManager serviceManager)
        {
            serviceManager.RegisterSingleton<IFlowSharpToolboxService, FlowSharpToolboxService>();
        }
    }

    public class FlowSharpToolboxService : ServiceBase, IFlowSharpToolboxService
    {
        protected ToolboxController toolboxController;
        protected Canvas toolboxCanvas;
        protected PluginManager pluginManager;
        protected Control pnlToolbox;

        public override void FinishedInitialization()
        {
            base.FinishedInitialization();
            ServiceManager.Get<ISemanticProcessor>().Register<FlowSharpMembrane, FlowSharpReceptor>();
        }

        public void CreateToolbox(Control parent, BaseController canvasController)
        {
            pnlToolbox = parent;
            pluginManager = new PluginManager();
            pluginManager.InitializePlugins();

            toolboxCanvas = new ToolboxCanvas();
            toolboxCanvas.Initialize(parent);
            toolboxController = new ToolboxController(toolboxCanvas, canvasController);

            InitializeToolbox();
            InitializePluginsInToolbox();
            UpdateToolboxPaths();
        }

        protected void InitializeToolbox()
        {
            int x = pnlToolbox.Width / 2 - 12;
            toolboxController.AddElement(new Box(toolboxCanvas) { DisplayRectangle = new Rectangle(x - 50, 15, 25, 25) });
            toolboxController.AddElement(new Ellipse(toolboxCanvas) { DisplayRectangle = new Rectangle(x, 15, 25, 25) });
            toolboxController.AddElement(new Diamond(toolboxCanvas) { DisplayRectangle = new Rectangle(x + 50, 15, 25, 25) });

            toolboxController.AddElement(new LeftTriangle(toolboxCanvas) { DisplayRectangle = new Rectangle(x - 60, 60, 25, 25) });
            toolboxController.AddElement(new RightTriangle(toolboxCanvas) { DisplayRectangle = new Rectangle(x - 20, 60, 25, 25) });
            toolboxController.AddElement(new UpTriangle(toolboxCanvas) { DisplayRectangle = new Rectangle(x + 20, 60, 25, 25) });
            toolboxController.AddElement(new DownTriangle(toolboxCanvas) { DisplayRectangle = new Rectangle(x + 60, 60, 25, 25) });

            toolboxController.AddElement(new HorizontalLine(toolboxCanvas) { DisplayRectangle = new Rectangle(x - 50, 130, 30, 20) });
            toolboxController.AddElement(new VerticalLine(toolboxCanvas) { DisplayRectangle = new Rectangle(x, 125, 20, 30) });
            toolboxController.AddElement(new DiagonalConnector(toolboxCanvas, new Point(x + 50, 125), new Point(x + 50 + 25, 125 + 25)));

            // toolboxElements.Add(new ToolboxDynamicConnectorLR(toolboxCanvas) { DisplayRectangle = new Rectangle(x - 50, 185, 25, 25)});
            toolboxController.AddElement(new DynamicConnectorLR(toolboxCanvas, new Point(x - 50, 175), new Point(x - 50 + 25, 175 + 25)));
            toolboxController.AddElement(new DynamicConnectorLD(toolboxCanvas, new Point(x, 175), new Point(x + 25, 175 + 25)));
            toolboxController.AddElement(new DynamicConnectorUD(toolboxCanvas, new Point(x + 50, 175), new Point(x + 50 + 25, 175 + 25)));

            toolboxController.AddElement(new ToolboxText(toolboxCanvas) { DisplayRectangle = new Rectangle(x, 230, 25, 25) });
            // toolboxElements.Add(new DiagonalLine(toolboxCanvas) { DisplayRectangle = new Rectangle(x + 25, 230, 25, 25) });
        }

        protected void InitializePluginsInToolbox()
        {
            int x = pnlToolbox.Width / 2 - 12;
            List<Type> pluginShapes = pluginManager.GetShapeTypes();

            // Plugin shapes
            int n = x - 60;
            int y = 260;

            foreach (Type t in pluginShapes)
            {
                GraphicElement pluginShape = Activator.CreateInstance(t, new object[] { toolboxCanvas }) as GraphicElement;
                pluginShape.DisplayRectangle = new Rectangle(n, y, 25, 25);
                toolboxController.AddElement(pluginShape);

                // Next toolbox shape position:
                n += 40;

                if (n > x + 60)
                {
                    n = x - 60;
                    y += 40;
                }
            }
        }

        protected void UpdateToolboxPaths()
        {
            toolboxController.Elements.ForEach(el => el.UpdatePath());
        }
    }

    public class FlowSharpReceptor : IReceptor
    {

    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Clifton.Core.ModuleManagement;
using Clifton.Core.Semantics;
using Clifton.Core.ServiceManagement;

using FlowSharpLib;
using FlowSharpServices;

namespace FlowSharpModule
{
    public class FlowSharpCanvasModule : IModule
    {
        public void InitializeServices(IServiceManager serviceManager)
        {
            serviceManager.RegisterSingleton<IFlowSharpCanvasService, FlowSharpService>();
        }
    }

    public class FlowSharpService : ServiceBase, IFlowSharpCanvasService
    {
        protected MouseController mouseController;
        protected CanvasController canvasController;
        protected Canvas canvas;
        protected Canvas toolboxCanvas;
        protected List<GraphicElement> elements;
        protected List<GraphicElement> toolboxElements;

        public override void FinishedInitialization()
        {
            base.FinishedInitialization();
            ServiceManager.Get<ISemanticProcessor>().Register<FlowSharpMembrane, FlowSharpReceptor>();
        }

        public void CreateCanvas(Control parent)
        {
            elements = new List<GraphicElement>();
            canvas = new Canvas();
            canvas.Initialize(parent);
            canvasController = new CanvasController(canvas, elements);
            mouseController = new MouseController(canvasController);
            mouseController.HookMouseEvents();
            mouseController.InitializeBehavior();
        }
    }

    public class FlowSharpReceptor : IReceptor
    {

    }
}

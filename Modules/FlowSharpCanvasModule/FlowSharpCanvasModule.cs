using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Clifton.Core.ModuleManagement;
using Clifton.Core.Semantics;
using Clifton.Core.ServiceManagement;

using FlowSharpLib;
using FlowSharpServices;

namespace FlowSharpCanvasModule
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
        protected List<GraphicElement> elements;

        public override void FinishedInitialization()
        {
            base.FinishedInitialization();
            ServiceManager.Get<ISemanticProcessor>().Register<FlowSharpMembrane, FlowSharpReceptor>();
        }

        public BaseController CreateCanvas(Control parent)
        {
            elements = new List<GraphicElement>();
            canvas = new Canvas();
            canvas.Initialize(parent);
            canvasController = new CanvasController(canvas, elements);
            mouseController = new MouseController(canvasController);
            mouseController.HookMouseEvents();
            mouseController.InitializeBehavior();
            canvasController.MouseController = mouseController;

            return canvasController;
        }
    }

    public class FlowSharpReceptor : IReceptor
    {

    }
}

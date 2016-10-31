using System;
using System.Windows.Forms;

using Clifton.Core.ServiceManagement;

using FlowSharpLib;

namespace FlowSharpServices
{
    public class TextChangedEventArgs : EventArgs
    {
        public string Text { get; set; }
    }

    public interface IFlowSharpCanvasService : IService
    {
        BaseController CreateCanvas(Control parent);
    }

    public interface IFlowSharpToolboxService : IService
    {
        void CreateToolbox(Control parent, BaseController canvasController);
    }

    public interface ICodeEditorService : IService
    {
        event EventHandler<TextChangedEventArgs> TextChanged;

        void CreateEditor(Control parent);
        void AddAssembly(string filename);
        void AddAssembly(Type t);

        void SetText(string text);
    }
}

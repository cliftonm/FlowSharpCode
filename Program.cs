using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Clifton.WinForm.ServiceInterfaces;

using FlowSharpLib;
using FlowSharpServices;

namespace FlowSharpCode
{
    static partial class Program
    {
        public static ICodeEditorService codeEditorService;

        private static Form form;
        private static Control docCanvas;
        private static Control docEditor;
        private static Control docToolbar;
        private static Control propGridToolbar;
        private static Panel pnlCodeEditor;
        private static PropertyGrid propGrid;
        private static BaseController canvasController;

        private static ElementProperties elementProperties;

        private static Dictionary<Keys, Action> keyActions = new Dictionary<Keys, Action>();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Bootstrap();

            IDockingFormService dockingService = ServiceManager.Get<IDockingFormService>();
            form = dockingService.CreateMainForm();
            form.Text = "FlowSharpCode";
            form.Size = new Size(1200, 800);
            form.Shown += OnShown;
            ((IBaseForm)form).ProcessCmdKeyEvent += OnProcessCmdKeyEvent;
            form.SuspendLayout();

            docCanvas = dockingService.CreateDocument(DockState.Document, "Canvas");
            Panel pnlFlowSharp = new Panel() { Dock = DockStyle.Fill };
            docCanvas.Controls.Add(pnlFlowSharp);
            canvasController = ServiceManager.Get<IFlowSharpCanvasService>().CreateCanvas(pnlFlowSharp);

            docEditor = dockingService.CreateDocument(docCanvas, DockAlignment.Right, "Editor", 0.50);
            pnlCodeEditor = new Panel() { Dock = DockStyle.Fill };
            docEditor.Controls.Add(pnlCodeEditor);

            docToolbar = dockingService.CreateDocument(DockState.DockLeft, "Toolbar");
            Panel pnlToolbox = new Panel() { Dock = DockStyle.Fill };
            docToolbar.Controls.Add(pnlToolbox);
            ServiceManager.Get<IFlowSharpToolboxService>().CreateToolbox(pnlToolbox, canvasController);

            propGridToolbar = dockingService.CreateDocument(docToolbar, DockAlignment.Bottom, "Properties", 0.50);
            propGrid = new PropertyGrid() { Dock = DockStyle.Fill };
            propGridToolbar.Controls.Add(propGrid);

            canvasController.ElementSelected += ElementSelected;
            canvasController.UpdateSelectedElement += UpdateSelectedElement;
            propGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(OnPropertyValueChanged);


            Menu menu = new Menu();
            menu.Initialize();
            form.Controls.Add(menu.menuStrip1);

            MenuController menuController = new MenuController(form, menu, canvasController);

            keyActions[Keys.Control | Keys.C] = menuController.Copy;
            keyActions[Keys.Control | Keys.V] = menuController.Paste;
            keyActions[Keys.Delete] = menuController.Delete;
            keyActions[Keys.Up] = () => canvasController.DragSelectedElements(new Point(0, -1));
            keyActions[Keys.Down] = () => canvasController.DragSelectedElements(new Point(0, 1));
            keyActions[Keys.Left] = () => canvasController.DragSelectedElements(new Point(-1, 0));
            keyActions[Keys.Right] = () => canvasController.DragSelectedElements(new Point(1, 0));

            // codeEditor.AddAssembly(typeof(XDocument));

            form.ResumeLayout();

            Application.Run(form);
        }

        private static void OnShown(object sender, EventArgs e)
        {
            codeEditorService = ServiceManager.Get<ICodeEditorService>();
            codeEditorService.CreateEditor(pnlCodeEditor);
            codeEditorService.AddAssembly("Clifton.Core.dll");
            codeEditorService.TextChanged += CodeEditorServiceTextChanged;
        }

        private static void CodeEditorServiceTextChanged(object sender, TextChangedEventArgs e)
        {
            if (canvasController.SelectedElements.Count == 1)
            {
                GraphicElement el = canvasController.SelectedElements[0];
                el.Json["Code"] = e.Text;
            }
        }

        private static void OnProcessCmdKeyEvent(object sender, ProcessCmdKeyEventArgs args)
        {
            Action act;

            if (canvasController.Canvas.Focused && keyActions.TryGetValue(args.KeyData, out act))
            {
                act();
                args.Handled = true;
            }
        }

        private static void ElementSelected(object controller, ElementEventArgs args)
        {
            elementProperties = null;

            if (args.Element != null)
            {
                GraphicElement el = args.Element;
                elementProperties = el.CreateProperties();

                string code;
                el.Json.TryGetValue("Code", out code);
                codeEditorService.SetText(code ?? String.Empty);
            }
            else
            {
                codeEditorService.SetText(String.Empty);
            }

            propGrid.SelectedObject = elementProperties;
            canvasController.Canvas.Focus();
        }

        private static void UpdateSelectedElement(object controller, ElementEventArgs args)
        {
            elementProperties.UpdateFrom(args.Element);
            propGrid.Refresh();
        }

        private static void OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            canvasController.SelectedElements.ForEach(sel =>
            {
                canvasController.Redraw(sel, el =>
                {
                    elementProperties.Update(el);
                    el.UpdateProperties();
                    el.UpdatePath();
                });
            });
        }

    }
}

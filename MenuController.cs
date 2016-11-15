/* 
* Copyright (c) Marc Clifton
* The Code Project Open License (CPOL) 1.02
* http://www.codeproject.com/info/cpol10.aspx
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;

using FlowSharpLib;
using FlowSharpCodeShapeInterfaces;

namespace FlowSharpCode
{
    public class MenuController
    {
        protected BaseController canvasController;
        protected Form form;

        protected CompilerResults results;
        protected Dictionary<string, string> tempToTextBoxMap = new Dictionary<string, string>();

        public MenuController(Form form, Menu menu, BaseController canvasController)
        {
            this.form = form;
            this.canvasController = canvasController;

            menu.mnuNew.Click += mnuNew_Click;
            menu.mnuOpen.Click += mnuOpen_Click;
            menu.mnuImport.Click += (sndr, args) =>
            {
                canvasController.DeselectCurrentSelectedElements();
                mnuImport_Click(sndr, args);
            };

            menu.mnuSave.Click += mnuSave_Click;
            menu.mnuSaveAs.Click += mnuSaveAs_Click;
            menu.mnuExit.Click += mnuExit_Click;
            menu.mnuCopy.Click += mnuCopy_Click;
            menu.mnuPaste.Click += mnuPaste_Click;
            menu.mnuDelete.Click += mnuDelete_Click;
            menu.mnuTopmost.Click += mnuTopmost_Click;
            menu.mnuBottommost.Click += mnuBottommost_Click;
            menu.mnuMoveUp.Click += mnuMoveUp_Click;
            menu.mnuMoveDown.Click += mnuMoveDown_Click;
            menu.mnuGroup.Click += mnuGroup_Click;
            menu.mnuUngroup.Click += mnuUngroup_Click;
            menu.mnuPlugins.Click += mnuPlugins_Click;

            menu.mnuCompile.Click += MnuCompile_Click;
            menu.mnuRun.Click += MnuRun_Click;
        }

        protected string filename;

        public void Copy()
        {
            if (canvasController.SelectedElements.Any())
            {
                List<GraphicElement> elementsToCopy = new List<GraphicElement>();
                // Include child elements of any groupbox, otherwise, on deserialization,
                // the ID's for the child elements aren't found.
                elementsToCopy.AddRange(canvasController.SelectedElements);
                elementsToCopy.AddRange(IncludeChildren(elementsToCopy));
                string copyBuffer = Persist.Serialize(elementsToCopy);
                Clipboard.SetData("FlowSharp", copyBuffer);
            }
            else
            {
                MessageBox.Show("Please select one or more shape(s).", "Nothing to copy.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        protected List<GraphicElement> IncludeChildren(List<GraphicElement> parents)
        {
            List<GraphicElement> els = new List<GraphicElement>();

            parents.ForEach(p =>
            {
                els.AddRange(p.GroupChildren);
                els.AddRange(IncludeChildren(p.GroupChildren));
            });

            return els;
        }

        public void Paste()
        {
            string copyBuffer = Clipboard.GetData("FlowSharp")?.ToString();

            if (copyBuffer == null)
            {
                MessageBox.Show("Clipboard does not contain a FlowSharp shape", "Paste Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    List<GraphicElement> els = Persist.Deserialize(canvasController.Canvas, copyBuffer);
                    canvasController.DeselectCurrentSelectedElements();

                    // After deserialization, only move and select elements without parents -
                    // children of group boxes should not be moved, as their parent will handle this,
                    // and children of group boxes cannot be selected.
                    List<GraphicElement> noParentElements = els.Where(e => e.Parent == null).ToList();

                    noParentElements.ForEach(el =>
                    {
                        el.Move(new Point(20, 20));
                        el.UpdateProperties();
                        el.UpdatePath();
                    });

                    List<GraphicElement> intersections = new List<GraphicElement>();

                    els.ForEach(el =>
                    {
                        intersections.AddRange(canvasController.FindAllIntersections(el));
                    });

                    IEnumerable<GraphicElement> distinctIntersections = intersections.Distinct();
                    canvasController.EraseTopToBottom(distinctIntersections);
                    els.ForEach(el => canvasController.Insert(0, el));
                    canvasController.DrawBottomToTop(distinctIntersections);
                    canvasController.UpdateScreen(distinctIntersections);
                    noParentElements.ForEach(el => canvasController.SelectElement(el));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error pasting shape:\r\n" + ex.Message, "Paste Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void Delete()
        {
            // TODO: Better implementation would be for the mouse controller to hook a shape deleted event?
            canvasController.SelectedElements.ForEach(el => canvasController.MouseController.ShapeDeleted(el));
            canvasController.DeleteSelectedElementsHierarchy();
        }

        private void mnuTopmost_Click(object sender, EventArgs e)
        {
            canvasController.Topmost();
        }

        private void mnuBottommost_Click(object sender, EventArgs e)
        {
            canvasController.Bottommost();
        }

        private void mnuMoveUp_Click(object sender, EventArgs e)
        {
            canvasController.MoveSelectedElementsUp();
        }

        private void mnuMoveDown_Click(object sender, EventArgs e)
        {
            canvasController.MoveSelectedElementsDown();
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (canvasController.SelectedElements.Count > 0)
            {
                Copy();
            }
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            // TODO: Check for changes before closing.
            canvasController.Clear();              // TODO: This is ugly!  Hook an event or decouple in some other way.
            canvasController.Canvas.Invalidate();
            filename = String.Empty;

            UpdateCaption();
            Program.csCodeEditorService.SetText("");          // TODO: This is ugly!  Hook event or decouple in some other way.
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            // TODO: Check for changes before closing.
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "FlowSharp (*.fsd)|*.fsd";
            DialogResult res = ofd.ShowDialog();

            if (res == DialogResult.OK)
            {
                filename = ofd.FileName;
            }
            else
            {
                return;
            }

            string data = File.ReadAllText(filename);
            List<GraphicElement> els = Persist.Deserialize(canvasController.Canvas, data);
            canvasController.Clear();              // TODO: This is ugly!  Hook an event or decouple in some other way.
            canvasController.AddElements(els);
            canvasController.Elements.ForEach(el => el.UpdatePath());
            canvasController.Canvas.Invalidate();

            UpdateCaption();
            Program.csCodeEditorService.SetText("");          // TODO: This is ugly!  Hook event or decouple in some other way.
        }

        private void mnuImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "FlowSharp (*.fsd)|*.fsd";
            DialogResult res = ofd.ShowDialog();

            if (res == DialogResult.OK)
            {
                string importFilename = ofd.FileName;
                string data = File.ReadAllText(importFilename);
                List<GraphicElement> els = Persist.Deserialize(canvasController.Canvas, data);
                canvasController.AddElements(els);
                canvasController.Elements.ForEach(el => el.UpdatePath());
                els.ForEach(el => canvasController.Canvas.Controller.SelectElement(el));
                canvasController.Canvas.Invalidate();
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            if (canvasController.Elements.Count > 0)
            {
                if (String.IsNullOrEmpty(filename))
                {
                    mnuSaveAs_Click(sender, e);
                }

                string data = Persist.Serialize(canvasController.Elements);
                File.WriteAllText(filename, data);
                UpdateCaption();
            }
            else
            {
                MessageBox.Show("Nothing to save.", "Empty Canvas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (canvasController.Elements.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "FlowSharp (*.fsd)|*.fsd|PNG (*.png)|*.png";
                DialogResult res = sfd.ShowDialog();

                if (res == DialogResult.OK)
                {
                    if (Path.GetExtension(sfd.FileName).ToLower() == ".png")
                    {
                        canvasController.SaveAsPng(sfd.FileName);
                    }
                    else
                    {
                        filename = sfd.FileName;
                        string data = Persist.Serialize(canvasController.Elements);
                        File.WriteAllText(filename, data);
                        UpdateCaption();
                    }
                }
            }
            else
            {
                MessageBox.Show("Nothing to save.", "Empty Canvas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            // TODO: Check for changes before closing.
            form.Close();
        }

        private void mnuGroup_Click(object sender, EventArgs e)
        {
            if (canvasController.SelectedElements.Any())
            {
                FlowSharpLib.GroupBox groupBox = new FlowSharpLib.GroupBox(canvasController.Canvas);
                canvasController.GroupShapes(groupBox);
                canvasController.DeselectCurrentSelectedElements();
                canvasController.SelectElement(groupBox);
            }
        }

        private void mnuUngroup_Click(object sender, EventArgs e)
        {
            canvasController.UngroupShapes(canvasController.SelectedElements[0] as FlowSharpLib.GroupBox);
            canvasController.Clear();
        }

        private void mnuPlugins_Click(object sender, EventArgs e)
        {
            // TODO:
            // new DlgPlugins().ShowDialog();
            // pluginManager.UpdatePlugins();
        }

        private void MnuCompile_Click(object sender, EventArgs e)
        {
            Compile();
        }

        protected void Compile()
        {
            tempToTextBoxMap.Clear();
            List<GraphicElement> compiledAssemblies = new List<GraphicElement>();

            bool ok = CompileAssemblies(compiledAssemblies);

            if (!ok)
            {
                DeleteTempFiles();
                return;
            }

            List<string> refs = new List<string>();
            List<string> sources = new List<string>();
            List<GraphicElement> rootSourceShapes = GetSources();
            rootSourceShapes.ForEach(root => GetReferencedAssemblies(root).Where(refassy => refassy is IAssemblyBox).ForEach(refassy => refs.Add(((IAssemblyBox)refassy).Filename)));

            // TODO: Better Linq!
            rootSourceShapes.Where(root => !String.IsNullOrEmpty(GetCode(root))).ForEach(root =>
            {
                string filename = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".cs";
                tempToTextBoxMap[filename] = root.Text;
                File.WriteAllText(filename, GetCode(root));
                sources.Add(filename);
            });

            Compile("foo.exe", sources, refs, true);
            DeleteTempFiles();
        }

        protected void DeleteTempFiles()
        {
            tempToTextBoxMap.ForEach(kvp => File.Delete(kvp.Key));
        }

        private void MnuRun_Click(object sender, EventArgs e)
        {
            // Ever compiled?
            if (results == null || results.Errors.HasErrors)
            {
                Compile();
            }

            // If no errors:
            if (!results.Errors.HasErrors)
            {
                ProcessStartInfo psi = new ProcessStartInfo("foo.exe");
                psi.UseShellExecute = true;     // must be true if we want to keep a console window open.
                Process p = Process.Start("foo.exe");
                //p.WaitForExit();
                //p.Close();
                //Type program = compiledAssembly.GetType("WebServerDemo.Program");
                //MethodInfo main = program.GetMethod("Main");
                //main.Invoke(null, null);
            }
        }

        protected void UpdateCaption()
        {
            form.Text = "FlowSharpCode" + (String.IsNullOrEmpty(filename) ? "" : " - ") + filename;
        }

        protected bool CompileAssemblies(List<GraphicElement> compiledAssemblies)
        {
            bool ok = true;

            // 1. First compile AssemblyBox shapes into their assemblies.  This is a recursive process.
            foreach (GraphicElement elAssy in canvasController.Elements.Where(el => el is IAssemblyBox))
            {
                CompileAssembly(elAssy, compiledAssemblies);
            }

            return ok;
        }

        protected string CompileAssembly(GraphicElement elAssy, List<GraphicElement> compiledAssemblies)
        {
            string assyFilename = ((IAssemblyBox)elAssy).Filename;

            if (!compiledAssemblies.Contains(elAssy))
            {
                // Add now, so we don't accidentally recurse infinitely.
                compiledAssemblies.Add(elAssy);

                List<GraphicElement> referencedAssemblies = GetReferencedAssemblies(elAssy);
                List<string> refs = new List<string>();

                // Recurse into referenced assemblies that need compiling first.
                foreach (GraphicElement el in referencedAssemblies)
                {
                    string refAssy = CompileAssembly(el, compiledAssemblies);
                    refs.Add(refAssy);
                }

                List<string> sources = GetSources(elAssy);
                Compile(assyFilename, sources, refs);
            }

            return assyFilename;
        }

        protected List<GraphicElement> GetReferencedAssemblies(GraphicElement elAssy)
        {
            List<GraphicElement> refs = new List<GraphicElement>();

            // TODO: Qualify EndConnectedShape as being IAssemblyBox
            elAssy.Connections.Where(c => ((Connector)c.ToElement).EndCap == AvailableLineCap.Arrow).ForEach(c =>
            {
                // Connector endpoint will reference ourselves, so exclude.
                if (((Connector)c.ToElement).EndConnectedShape != elAssy)
                {
                    GraphicElement toAssy = ((Connector)c.ToElement).EndConnectedShape;
                    refs.Add(toAssy);
                }
            });

            // TODO: Qualify EndConnectedShape as being IAssemblyBox
            elAssy.Connections.Where(c => ((Connector)c.ToElement).StartCap == AvailableLineCap.Arrow).ForEach(c =>
            {
                // Connector endpoint will reference ourselves, so exclude.
                if (((Connector)c.ToElement).StartConnectedShape != elAssy)
                {
                    GraphicElement toAssy = ((Connector)c.ToElement).StartConnectedShape;
                    refs.Add(toAssy);
                }
            });

            return refs;
        }

        protected bool Compile(string assyFilename, List<string> sources, List<string> refs, bool generateExecutable = false)
        {
            bool ok = false;

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            parameters.IncludeDebugInformation = true;
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = generateExecutable;

            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Data.Linq.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("System.Net.dll");
            parameters.ReferencedAssemblies.Add("System.Net.Http.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");
            parameters.ReferencedAssemblies.Add("Clifton.Core.dll");
            parameters.ReferencedAssemblies.AddRange(refs.ToArray());
            parameters.OutputAssembly = assyFilename;

            if (generateExecutable)
            {
                parameters.MainClass = "App.Program";
            }

            // results = provider.CompileAssemblyFromSource(parameters, sources.ToArray());

            results = provider.CompileAssemblyFromFile(parameters, sources.ToArray());
            ok = !results.Errors.HasErrors;

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0} - {1}): {2}", tempToTextBoxMap[Path.GetFileNameWithoutExtension(error.FileName) + ".cs"], error.Line, error.ErrorText));
                }

                MessageBox.Show(sb.ToString(), assyFilename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ok;
        }

        /// <summary>
        /// Returns only top level sources - those not contained within AssemblyBox shapes.
        /// </summary>
        protected List<GraphicElement> GetSources()
        {
            List<GraphicElement> sourceList = new List<GraphicElement>();

            foreach (GraphicElement srcEl in canvasController.Elements.Where(
                srcEl => !ContainedIn<IAssemblyBox>(srcEl) &&
                !(srcEl is IFileBox)))
            {
                sourceList.Add(srcEl);
            }

            return sourceList;
        }

        protected bool ContainedIn<T>(GraphicElement child)
        {
            return canvasController.Elements.Any(el => el is T && el.DisplayRectangle.Contains(child.DisplayRectangle));
        }

        /// <summary>
        /// Returns sources contained in an element (ie., AssemblyBox shape).
        /// </summary>
        protected List<string> GetSources(GraphicElement elAssy)
        {
            List<string> sourceList = new List<string>();

            foreach (GraphicElement srcEl in canvasController.Elements.Where(srcEl => elAssy.DisplayRectangle.Contains(srcEl.DisplayRectangle)))
            {
                string filename = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".cs";
                tempToTextBoxMap[filename] = srcEl.Text;
                File.WriteAllText(filename, GetCode(srcEl));
                sourceList.Add(filename);
            }

            return sourceList;
        }

        protected string GetCode(GraphicElement el)
        {
            string code;
            el.Json.TryGetValue("Code", out code);

            return code ?? "";
        }
    }
}

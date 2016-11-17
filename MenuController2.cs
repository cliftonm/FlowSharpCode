﻿/* 
* Copyright (c) Marc Clifton
* The Code Project Open License (CPOL) 1.02
* http://www.codeproject.com/info/cpol10.aspx
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;

using FlowSharpLib;
using FlowSharpCodeShapeInterfaces;

namespace FlowSharpCode
{
    public partial class MenuController
    {
        protected BaseController canvasController;
        protected Form form;
        protected Operations operations;
        protected string exeFilename;

        protected CompilerResults results;
        protected Dictionary<string, string> tempToTextBoxMap = new Dictionary<string, string>();

        public MenuController(Form form, Menu menu, BaseController canvasController, Operations operations)
        {
            this.form = form;
            this.canvasController = canvasController;
            this.operations = operations;

            traceListener = new TraceListener();
            Trace.Listeners.Add(traceListener);

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
            menu.mnuUndo.Click += mnuUndo_Click;
            menu.mnuRedo.Click += mnuRedo_Click;
            // mnuEdit.Click += (sndr, args) => EditText();
            // menu.mnuPlugins.Click += mnuPlugins_Click;
            menu.mnuDebugWindow.Click += mnuDebugWindow_Click;

            menu.mnuCompile.Click += MnuCompile_Click;
            menu.mnuRun.Click += MnuRun_Click;
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

        // Program.csCodeEditorService.SetText("");          // TODO: This is ugly!  Hook event or decouple in some other way.

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

            // Get code for workflow boxes first, as this code will then be included in the rootSourceShape code listing.
            IEnumerable<GraphicElement> workflowShapes = canvasController.Elements.Where(el => el is IWorkflowBox);
            workflowShapes.ForEach(wf =>
            {
                string code = GetWorkflowCode(wf);
                wf.Json["Code"] = code;
                // CreateCodeFile(wf, sources, code);
            });

            // TODO: Better Linq!
            rootSourceShapes.Where(root => !String.IsNullOrEmpty(GetCode(root))).ForEach(root =>
            {
                CreateCodeFile(root, sources, GetCode(root));
            });

            exeFilename = String.IsNullOrEmpty(filename) ? "temp.exe" : Path.GetFileNameWithoutExtension(filename) + ".exe";
            Compile(exeFilename, sources, refs, true);
            DeleteTempFiles();
        }

        protected void CreateCodeFile(GraphicElement root, List<string> sources, string code)
        {
            string filename = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".cs";
            tempToTextBoxMap[filename] = root.Text;
            File.WriteAllText(filename, GetCode(root));
            sources.Add(filename);
        }

        public string GetWorkflowCode(GraphicElement wf)
        {
            StringBuilder sb = new StringBuilder();

            // TODO: Hardcoded for now for POC.
            sb.AppendLine("namespace App");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic partial class " + wf.Text);
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tpublic static void Execute(" + Clifton.Core.ExtensionMethods.ExtensionMethods.LeftOf(wf.Text, "Workflow") + " packet)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\t" + wf.Text + " workflow = new " + wf.Text + "();");

            // Fill in the workflow steps.
            GraphicElement el = FindStartOfWorkflow(wf);

            while (el != null)
            {
                sb.AppendLine("\t\t\tworkflow." + el.Text + "(packet);");
                el = NextElementInWorkflow(el);
            }

            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            return sb.ToString();
        }

        protected GraphicElement FindStartOfWorkflow(GraphicElement wf)
        {
            GraphicElement start = null;

            foreach (GraphicElement srcEl in canvasController.Elements.Where(srcEl => wf.DisplayRectangle.Contains(srcEl.DisplayRectangle)))
            {
                if (!srcEl.IsConnector && srcEl != wf)
                {
                    // Special case for a 1 step workflow.  Untested.
                    if (srcEl.Connections.Count == 0)
                    {
                        start = srcEl;
                        break;
                    }

                    // start and end has only one connection.
                    if (srcEl.Connections.Count == 1 && ((Connection)srcEl.Connections[0]).ToConnectionPoint.Type == FlowSharpLib.GripType.Start)
                    {
                        start = srcEl;
                        break;
                    }
                }
            }

            return start;
        }

        protected GraphicElement NextElementInWorkflow(GraphicElement el)
        {
            GraphicElement ret = null;

            if (el.Connections.Count == 1)
            {
                if (((Connector)((Connection)el.Connections[0]).ToElement).EndConnectedShape != el)
                {
                    ret = ((Connector)((Connection)el.Connections[0]).ToElement).EndConnectedShape;
                }
            }
            else if (el.Connections.Count == 2)
            {
                if (((Connector)((Connection)el.Connections[0]).ToElement).StartConnectedShape == el)
                {
                    ret = ((Connector)((Connection)el.Connections[0]).ToElement).EndConnectedShape;
                }
                else if (((Connector)((Connection)el.Connections[1]).ToElement).StartConnectedShape == el)
                {
                    ret = ((Connector)((Connection)el.Connections[1]).ToElement).EndConnectedShape;
                }
            }

            return ret;
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
                //ProcessStartInfo psi = new ProcessStartInfo(exeFilename);
                //psi.UseShellExecute = true;     // must be true if we want to keep a console window open.
                Process p = Process.Start(exeFilename);
                //p.WaitForExit();
                //p.Close();
                //Type program = compiledAssembly.GetType("WebServerDemo.Program");
                //MethodInfo main = program.GetMethod("Main");
                //main.Invoke(null, null);
            }
        }

        protected bool CompileAssemblies(List<GraphicElement> compiledAssemblies)
        {
            bool ok = true;

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
                    try
                    {
                        sb.AppendLine(String.Format("Error ({0} - {1}): {2}", tempToTextBoxMap[Path.GetFileNameWithoutExtension(error.FileName) + ".cs"], error.Line, error.ErrorText));
                    }
                    catch
                    {
                        sb.AppendLine(error.ErrorText);     // other errors, like "process in use", do not have an associated filename, so general catch-all here.
                    }
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

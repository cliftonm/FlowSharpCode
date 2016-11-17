/* 
* Copyright (c) Marc Clifton
* The Code Project Open License (CPOL) 1.02
* http://www.codeproject.com/info/cpol10.aspx
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using FlowSharpLib;

namespace FlowSharpCode
{
    public class TraceListener : ConsoleTraceListener
    {
        public DlgDebugWindow DebugWindow { get; set; }

        public override void WriteLine(string msg)
        {
            if (DebugWindow != null)
            {
                DebugWindow.Trace(msg + "\r\n");
            }
        }
    }

    public partial class MenuController
    {
        protected string filename;
        protected DlgDebugWindow debugWindow;
        protected TraceListener traceListener;

        private void mnuTopmost_Click(object sender, EventArgs e)
        {
            List<ZOrderMap> originalZOrder = canvasController.GetZOrderOfSelectedElements();

            canvasController.UndoStack.UndoRedo("Z-Top",
                () =>
                {
                    canvasController.Topmost();
                },
                () =>
                {
                    canvasController.RestoreZOrder(originalZOrder);
                });
        }

        private void mnuBottommost_Click(object sender, EventArgs e)
        {
            List<ZOrderMap> originalZOrder = canvasController.GetZOrderOfSelectedElements();

            canvasController.UndoStack.UndoRedo("Z-Bottom",
                () =>
                {
                    canvasController.Bottommost();
                },
                () =>
                {
                    canvasController.RestoreZOrder(originalZOrder);
                });
        }

        private void mnuMoveUp_Click(object sender, EventArgs e)
        {
            List<ZOrderMap> originalZOrder = canvasController.GetZOrderOfSelectedElements();

            canvasController.UndoStack.UndoRedo("Z-Up",
                () =>
                {
                    canvasController.MoveSelectedElementsUp();
                },
                () =>
                {
                    canvasController.RestoreZOrder(originalZOrder);
                });
        }

        private void mnuMoveDown_Click(object sender, EventArgs e)
        {
            List<ZOrderMap> originalZOrder = canvasController.GetZOrderOfSelectedElements();

            canvasController.UndoStack.UndoRedo("Z-Down",
                () =>
                {
                    canvasController.MoveSelectedElementsDown();
                },
                () =>
                {
                    canvasController.RestoreZOrder(originalZOrder);
                });
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (canvasController.SelectedElements.Count > 0)
            {
                operations.Copy();
            }
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            operations.Paste();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            operations.Delete();
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            if (CheckForChanges()) return;
            canvasController.Clear();
            canvasController.UndoStack.ClearStacks();
            ElementCache.Instance.ClearCache();
            canvasController.MouseController.ClearState();
            canvasController.Canvas.Invalidate();
            filename = String.Empty;
            UpdateCaption();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            if (CheckForChanges()) return;
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
            canvasController.Clear();
            canvasController.UndoStack.ClearStacks();
            ElementCache.Instance.ClearCache();
            canvasController.MouseController.ClearState();
            canvasController.AddElements(els);
            canvasController.Elements.ForEach(el => el.UpdatePath());
            canvasController.Canvas.Invalidate();
            UpdateCaption();
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
                List<GraphicElement> selectedElements = canvasController.SelectedElements.ToList();

                canvasController.UndoStack.UndoRedo("Import",
                    () =>
                    {
                        canvasController.DeselectCurrentSelectedElements();
                        canvasController.AddElements(els);
                        canvasController.Elements.ForEach(el => el.UpdatePath());
                        canvasController.SelectElements(els);
                        canvasController.Canvas.Invalidate();
                    },
                    () =>
                    {
                        canvasController.DeselectCurrentSelectedElements();
                        els.ForEach(el => canvasController.DeleteElement(el));
                        canvasController.SelectElements(selectedElements);
                    });
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            if (canvasController.Elements.Count > 0)
            {
                SaveOrSaveAs();
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
                SaveOrSaveAs(true);
                UpdateCaption();
            }
            else
            {
                MessageBox.Show("Nothing to save.", "Empty Canvas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            if (CheckForChanges()) return;
            form.Close();
        }

        private void mnuGroup_Click(object sender, EventArgs e)
        {
            if (canvasController.SelectedElements.Any())
            {
                List<GraphicElement> selectedShapes = canvasController.SelectedElements.ToList();
                FlowSharpLib.GroupBox groupBox = new FlowSharpLib.GroupBox(canvasController.Canvas);

                canvasController.UndoStack.UndoRedo("Group",
                    () =>
                    {
                        ElementCache.Instance.Remove(groupBox);
                        canvasController.GroupShapes(groupBox);
                        canvasController.DeselectCurrentSelectedElements();
                        canvasController.SelectElement(groupBox);
                    },
                    () =>
                    {
                        ElementCache.Instance.Add(groupBox);
                        canvasController.UngroupShapes(groupBox, false);
                        canvasController.DeselectCurrentSelectedElements();
                        canvasController.SelectElements(selectedShapes);
                    });
            }
        }

        private void mnuUngroup_Click(object sender, EventArgs e)
        {
            // At this point, we can only ungroup one group.
            if (canvasController.SelectedElements.Count == 1)
            {
                FlowSharpLib.GroupBox groupBox = canvasController.SelectedElements[0] as FlowSharpLib.GroupBox;

                if (groupBox != null)
                {
                    List<GraphicElement> groupedShapes = new List<GraphicElement>(groupBox.GroupChildren);

                    canvasController.UndoStack.UndoRedo("Ungroup",
                    () =>
                    {
                        ElementCache.Instance.Add(groupBox);
                        canvasController.UngroupShapes(groupBox, false);
                        canvasController.DeselectCurrentSelectedElements();
                        canvasController.SelectElements(groupedShapes);
                    },
                    () =>
                    {
                        ElementCache.Instance.Remove(groupBox);
                        canvasController.GroupShapes(groupBox);
                        canvasController.DeselectCurrentSelectedElements();
                        canvasController.SelectElement(groupBox);
                    });
                }
            }
        }

        private void mnuDebugWindow_Click(object sender, EventArgs e)
        {
            if (debugWindow == null)
            {
                debugWindow = new DlgDebugWindow(canvasController);
                debugWindow.Show();
                List<string> undoEvents = canvasController.UndoStack.GetStackInfo();
                debugWindow.UpdateUndoStack(undoEvents);
                traceListener.DebugWindow = debugWindow;
                debugWindow.FormClosed += (sndr, args) =>
                {
                    debugWindow = null;
                    traceListener.DebugWindow = null;
                };
            }
        }

        //private void mnuPlugins_Click(object sender, EventArgs e)
        //{
        //    new DlgPlugins().ShowDialog();
        //    pluginManager.UpdatePlugins();
        //}

        private void mnuUndo_Click(object sender, EventArgs e)
        {
            operations.Undo();
        }

        private void mnuRedo_Click(object sender, EventArgs e)
        {
            operations.Redo();
        }

        /// <summary>
        /// Return true if operation should be cancelled.
        /// </summary>
        protected bool CheckForChanges()
        {
            bool ret = false;

            if (canvasController.UndoStack.HasChanges)
            {
                DialogResult res = MessageBox.Show("Do you wish to save changes to this drawing?", "Save Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                ret = res == DialogResult.Cancel;

                if (res == DialogResult.Yes)
                {
                    ret = !SaveOrSaveAs();   // override because of possible cancel in save operation.
                }
                else
                {
                    canvasController.UndoStack.ClearStacks();       // Prevents second "are you sure" when exiting with Ctrl+X
                }
            }

            return ret;
        }

        protected bool SaveOrSaveAs(bool forceSaveAs = false)
        {
            bool ret = true;

            if (String.IsNullOrEmpty(filename) || forceSaveAs)
            {
                ret = SaveAs();
            }
            else
            {
                string data = Persist.Serialize(canvasController.Elements);
                File.WriteAllText(filename, data);
            }

            return ret;
        }

        protected bool SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "FlowSharp (*.fsd)|*.fsd|PNG (*.png)|*.png";
            DialogResult res = sfd.ShowDialog();
            string ext = ".fsd";

            if (res == DialogResult.OK)
            {
                ext = Path.GetExtension(sfd.FileName).ToLower();

                if (ext == ".png")
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

            return res == DialogResult.OK && ext != ".png";
        }

        protected void UpdateCaption()
        {
            form.Text = "FlowSharp" + (String.IsNullOrEmpty(filename) ? "" : " - ") + filename;
        }
    }
}

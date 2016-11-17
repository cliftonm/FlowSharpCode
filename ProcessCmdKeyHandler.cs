using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Clifton.WinForm.ServiceInterfaces;

using FlowSharpLib;

namespace FlowSharpCode
{
    public class ProcessCmdKeyHandler
    {
        protected TextBox editBox;
        protected GraphicElement shapeBeingEdited;
        protected BaseController canvasController;
        protected Dictionary<Keys, Action> keyActions = new Dictionary<Keys, Action>();

        public ProcessCmdKeyHandler(BaseController controller)
        {
            canvasController = controller;
            keyActions[Keys.Up] = () => canvasController.DragSelectedElements(new Point(0, -1));
            keyActions[Keys.Down] = () => canvasController.DragSelectedElements(new Point(0, 1));
            keyActions[Keys.Left] = () => canvasController.DragSelectedElements(new Point(-1, 0));
            keyActions[Keys.Right] = () => canvasController.DragSelectedElements(new Point(1, 0));
            keyActions[Keys.Delete] = () => Program.operations.Delete();
            keyActions[Keys.F2] = EditText;
        }

        public void ProcessCmdKey(object sender, ProcessCmdKeyEventArgs args)
        {
            Keys keyData = args.KeyData;
            Action act;
            bool ret = false;

            if (editBox == null)
            {
                if (canvasController.Canvas.Focused && keyActions.TryGetValue(keyData, out act))
                {
                    act();
                    ret = true;
                }
                else
                {
                    if (canvasController.Canvas.Focused &&
                        canvasController.SelectedElements.Count == 1 &&
                        !canvasController.SelectedElements[0].IsConnector &&
                        CanStartEditing(keyData))
                    {
                        EditText();
                        // TODO: THIS IS SUCH A MESS!

                        // Will return upper case letter always, regardless of shift key....
                        string firstKey = ((char)keyData).ToString();

                        // ... so we have to fix it.  Sigh.
                        if ((keyData & Keys.Shift) != Keys.Shift)
                        {
                            firstKey = firstKey.ToLower();
                        }
                        else
                        {
                            // Handle shift of number keys on main keyboard
                            if (char.IsDigit(firstKey[0]))
                            {
                                // TODO: Probably doesn't handle non-American keyboards!
                                // Note index 0 is ")"
                                string key = ")!@#$%^&*(";
                                int n;

                                if (int.TryParse(firstKey, out n))
                                {
                                    firstKey = key[n].ToString();
                                }
                            }
                            // TODO: This is such a PITA.  Other symbols and shift combinations do not produce the correct first character!
                        }

                        editBox.Text = firstKey;
                        editBox.SelectionStart = 1;
                        editBox.SelectionLength = 0;
                        ret = true;
                    }
                }
            }

            args.Handled = ret;
        }

        protected bool CanStartEditing(Keys keyData)
        {
            bool ret = false;

            if (((keyData & Keys.Control) != Keys.Control) &&              // any control + key is not valid
                 ((keyData & Keys.Alt) != Keys.Alt))                       // any alt + key is not valid
            {
                Keys k2 = (keyData & ~(Keys.Control | Keys.Shift | Keys.ShiftKey | Keys.Alt | Keys.Menu));

                if ((k2 != Keys.None) && (k2 < Keys.F1 || k2 > Keys.F12))
                {
                    // Here we assume we have a viable character.
                    // TODO: Probably more logic is required here.
                    ret = true;
                }
            }

            return ret;
        }

        protected void EditText()
        {
            if (canvasController.SelectedElements.Count == 1)
            {
                // TODO: At the moment, connectors do not support text.
                if (!canvasController.SelectedElements[0].IsConnector)
                {
                    shapeBeingEdited = canvasController.SelectedElements[0];
                    editBox = CreateTextBox(shapeBeingEdited);
                    canvasController.Canvas.Controls.Add(editBox);
                    editBox.Visible = true;
                    editBox.Focus();
                    editBox.KeyPress += OnEditBoxKey;
                    editBox.LostFocus += (sndr, args) => TerminateEditing();
                }
            }
        }

        protected void OnEditBoxKey(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27 || e.KeyChar == 13)
            {
                TerminateEditing();
                e.Handled = true;       // Suppress beep.
            }
        }

        protected void TerminateEditing()
        {
            if (editBox != null)
            {
                editBox.KeyPress -= OnEditBoxKey;
                string oldVal = shapeBeingEdited.Text;
                string newVal = editBox.Text;
                TextBox tb = editBox;
                editBox = null;     // set editBox to null so the remove, which fires a LoseFocus event, doesn't call into TerminateEditing again!

                canvasController.UndoStack.UndoRedo("Inline edit",
                    () =>
                    {
                        shapeBeingEdited.Text = newVal;
                        canvasController.Redraw(shapeBeingEdited);
                        // Updates PropertyGrid:
                        canvasController.ElementSelected.Fire(this, new ElementEventArgs() { Element = shapeBeingEdited });
                    },
                    () =>
                    {
                        shapeBeingEdited.Text = oldVal;
                        canvasController.Redraw(shapeBeingEdited);
                        // Updates PropertyGrid:
                        canvasController.ElementSelected.Fire(this, new ElementEventArgs() { Element = shapeBeingEdited });
                    });

                canvasController.Canvas.Controls.Remove(tb);
            }
        }

        protected TextBox CreateTextBox(GraphicElement el)
        {
            TextBox tb = new TextBox();
            tb.Location = el.DisplayRectangle.LeftMiddle().Move(0, -10);
            tb.Size = new Size(el.DisplayRectangle.Width, 20);
            tb.Text = el.Text;

            return tb;
        }

    }
}

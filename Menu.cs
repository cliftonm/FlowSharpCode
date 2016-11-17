using System.Windows.Forms;

namespace FlowSharpCode
{
    public class Menu
    {
        public MenuStrip menuStrip1;
        public ToolStripMenuItem fileToolStripMenuItem;
        public ToolStripMenuItem mnuNew;
        public ToolStripMenuItem mnuOpen;
        public ToolStripMenuItem mnuSave;
        public ToolStripMenuItem mnuSaveAs;
        public ToolStripSeparator toolStripMenuItem1;
        public ToolStripMenuItem mnuExit;
        public ToolStripMenuItem editToolStripMenuItem;
        public ToolStripMenuItem mnuCopy;
        public ToolStripMenuItem mnuPaste;
        public ToolStripMenuItem mnuDelete;
        public ToolStripMenuItem orderToolStripMenuItem;
        public ToolStripMenuItem mnuTopmost;
        public ToolStripMenuItem mnuBottommost;
        public ToolStripMenuItem mnuMoveUp;
        public ToolStripMenuItem mnuMoveDown;
        public ToolStripSeparator toolStripMenuItem3;
        public ToolStripMenuItem mnuImport;
        public ToolStripSeparator toolStripMenuItem2;
        public ToolStripMenuItem viewToolStripMenuItem;
        public ToolStripMenuItem mnuDebugWindow;
        public ToolStripMenuItem groupToolStripMenuItem;
        public ToolStripMenuItem mnuGroup;
        public ToolStripMenuItem mnuUngroup;
        public ToolStripMenuItem mnuPlugins;
        public ToolStripSeparator toolStripMenuItem4;
        private ToolStripSeparator toolStripMenuItem5;
        public ToolStripMenuItem mnuUndo;
        public ToolStripMenuItem mnuRedo;

        public ToolStripMenuItem buildToolStripMenuItem;
        public ToolStripMenuItem mnuCompile;
        public ToolStripMenuItem mnuRun;

        public void Initialize()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            mnuNew = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            mnuOpen = new ToolStripMenuItem();
            mnuImport = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            mnuSave = new ToolStripMenuItem();
            mnuSaveAs = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            mnuPlugins = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            mnuExit = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            mnuCopy = new ToolStripMenuItem();
            mnuPaste = new ToolStripMenuItem();
            mnuDelete = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            this.mnuUndo = new ToolStripMenuItem();
            this.mnuRedo = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            mnuDebugWindow = new ToolStripMenuItem();
            orderToolStripMenuItem = new ToolStripMenuItem();
            mnuTopmost = new ToolStripMenuItem();
            mnuBottommost = new ToolStripMenuItem();
            mnuMoveUp = new ToolStripMenuItem();
            mnuMoveDown = new ToolStripMenuItem();
            groupToolStripMenuItem = new ToolStripMenuItem();
            mnuGroup = new ToolStripMenuItem();
            mnuUngroup = new ToolStripMenuItem();

            buildToolStripMenuItem = new ToolStripMenuItem();
            mnuCompile = new ToolStripMenuItem();
            mnuRun = new ToolStripMenuItem();

            menuStrip1.SuspendLayout();

            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            menuStrip1.Items.AddRange(new ToolStripItem[] {
            fileToolStripMenuItem,
            editToolStripMenuItem,
            viewToolStripMenuItem,
            orderToolStripMenuItem,
            groupToolStripMenuItem,
            buildToolStripMenuItem});
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(943, 25);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            mnuNew,
            toolStripMenuItem3,
            mnuOpen,
            mnuImport,
            toolStripMenuItem2,
            mnuSave,
            mnuSaveAs,
            toolStripMenuItem1,
            mnuPlugins,
            toolStripMenuItem4,
            mnuExit});
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 21);
            fileToolStripMenuItem.Text = "&File";
            // 
            // buildToolStripMenuItem
            // 
            buildToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            mnuCompile,
            mnuRun});
            buildToolStripMenuItem.Name = "buildToolStripMenuItem";
            buildToolStripMenuItem.Size = new System.Drawing.Size(37, 21);
            buildToolStripMenuItem.Text = "Bu&ild";
            // 
            // mnuNew
            // 
            mnuNew.Name = "mnuNew";
            this.mnuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            mnuNew.Size = new System.Drawing.Size(165, 24);
            mnuNew.Text = "&New";
            // 
            // mnuRun
            // 
            mnuRun.Name = "mnuRun";
            this.mnuRun.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            mnuRun.Size = new System.Drawing.Size(165, 24);
            mnuRun.Text = "&Run";
            // 
            // mnuCompile
            // 
            mnuCompile.Name = "mnuCompile";
            this.mnuCompile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            mnuCompile.Size = new System.Drawing.Size(165, 24);
            mnuCompile.Text = "&Compile";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(162, 6);
            // 
            // mnuOpen
            // 
            mnuOpen.Name = "mnuOpen";
            this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            mnuOpen.Size = new System.Drawing.Size(165, 24);
            mnuOpen.Text = "&Open...";
            // 
            // mnuImport
            // 
            mnuImport.Name = "mnuImport";
            mnuImport.Size = new System.Drawing.Size(165, 24);
            mnuImport.Text = "&Import...";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(162, 6);
            // 
            // mnuSave
            // 
            mnuSave.Name = "mnuSave";
            this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            mnuSave.Size = new System.Drawing.Size(165, 24);
            mnuSave.Text = "&Save";
            // 
            // mnuSaveAs
            // 
            mnuSaveAs.Name = "mnuSaveAs";
            mnuSaveAs.Size = new System.Drawing.Size(165, 24);
            mnuSaveAs.Text = "Save &as";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(162, 6);
            // 
            // mnuPlugins
            // 
            mnuPlugins.Name = "mnuPlugins";
            mnuPlugins.Size = new System.Drawing.Size(165, 24);
            mnuPlugins.Text = "&Plugins...";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(162, 6);
            // 
            // mnuExit
            // 
            mnuExit.Name = "mnuExit";
            this.mnuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            mnuExit.Size = new System.Drawing.Size(165, 24);
            mnuExit.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            mnuCopy,
            mnuPaste,
            mnuDelete,
            this.toolStripMenuItem5,
            this.mnuUndo,
            this.mnuRedo});
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // mnuCopy
            // 
            mnuCopy.Name = "mnuCopy";
            this.mnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            mnuCopy.Size = new System.Drawing.Size(110, 24);
            mnuCopy.Text = "&Copy";
            // 
            // mnuPaste
            // 
            mnuPaste.Name = "mnuPaste";
            this.mnuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            mnuPaste.Size = new System.Drawing.Size(110, 24);
            mnuPaste.Text = "&Paste";
            // 
            // mnuDelete
            // 
            mnuDelete.Name = "mnuDelete";
            this.mnuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            mnuDelete.Size = new System.Drawing.Size(110, 24);
            mnuDelete.Text = "&Delete";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            mnuDebugWindow});
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            viewToolStripMenuItem.Text = "&View";
            // 
            // mnuDebugWindow
            // 
            mnuDebugWindow.Name = "mnuDebugWindow";
            mnuDebugWindow.Size = new System.Drawing.Size(159, 24);
            mnuDebugWindow.Text = "&Debug Window";
            // 
            // orderToolStripMenuItem
            // 
            orderToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            mnuTopmost,
            mnuBottommost,
            mnuMoveUp,
            mnuMoveDown});
            orderToolStripMenuItem.Name = "orderToolStripMenuItem";
            orderToolStripMenuItem.Size = new System.Drawing.Size(49, 21);
            orderToolStripMenuItem.Text = "&Order";
            // 
            // mnuTopmost
            // 
            mnuTopmost.Name = "mnuTopmost";
            this.mnuTopmost.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            mnuTopmost.Size = new System.Drawing.Size(141, 24);
            mnuTopmost.Text = "To &Top";
            // 
            // mnuBottommost
            // 
            mnuBottommost.Name = "mnuBottommost";
            this.mnuBottommost.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.B)));
            mnuBottommost.Size = new System.Drawing.Size(141, 24);
            mnuBottommost.Text = "To &Bottom";
            // 
            // mnuMoveUp
            // 
            mnuMoveUp.Name = "mnuMoveUp";
            this.mnuMoveUp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.U)));
            mnuMoveUp.Size = new System.Drawing.Size(141, 24);
            mnuMoveUp.Text = "Move &Up";
            // 
            // mnuMoveDown
            // 
            mnuMoveDown.Name = "mnuMoveDown";
            this.mnuMoveDown.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            mnuMoveDown.Size = new System.Drawing.Size(141, 24);
            mnuMoveDown.Text = "Move &Down";
            // 
            // groupToolStripMenuItem
            // 
            groupToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            mnuGroup,
            mnuUngroup});
            groupToolStripMenuItem.Name = "groupToolStripMenuItem";
            groupToolStripMenuItem.Size = new System.Drawing.Size(52, 21);
            groupToolStripMenuItem.Text = "&Group";
            // 
            // mnuGroup
            // 
            mnuGroup.Name = "mnuGroup";
            this.mnuGroup.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            mnuGroup.Size = new System.Drawing.Size(124, 24);
            mnuGroup.Text = "&Group";
            // 
            // mnuUngroup
            // 
            mnuUngroup.Name = "mnuUngroup";
            this.mnuUngroup.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            mnuUngroup.Size = new System.Drawing.Size(124, 24);
            mnuUngroup.Text = "&Ungroup";
            // 
            // mnuUndo
            // 
            this.mnuUndo.Name = "mnuUndo";
            this.mnuUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mnuUndo.Size = new System.Drawing.Size(165, 24);
            this.mnuUndo.Text = "&Undo";
            // 
            // mnuRedo
            // 
            this.mnuRedo.Name = "mnuRedo";
            this.mnuRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.mnuRedo.Size = new System.Drawing.Size(165, 24);
            this.mnuRedo.Text = "&Redo";

            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
        }
    }
}

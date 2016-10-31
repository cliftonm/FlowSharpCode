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
            buildToolStripMenuItem.Text = "&Build";
            // 
            // mnuNew
            // 
            mnuNew.Name = "mnuNew";
            mnuNew.Size = new System.Drawing.Size(165, 24);
            mnuNew.Text = "&New";
            // 
            // mnuRun
            // 
            mnuRun.Name = "mnuRun";
            mnuRun.Size = new System.Drawing.Size(165, 24);
            mnuRun.Text = "&Run";
            // 
            // mnuCompile
            // 
            mnuCompile.Name = "mnuCompile";
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
            mnuExit.Size = new System.Drawing.Size(165, 24);
            mnuExit.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            mnuCopy,
            mnuPaste,
            mnuDelete});
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // mnuCopy
            // 
            mnuCopy.Name = "mnuCopy";
            mnuCopy.Size = new System.Drawing.Size(110, 24);
            mnuCopy.Text = "&Copy";
            // 
            // mnuPaste
            // 
            mnuPaste.Name = "mnuPaste";
            mnuPaste.Size = new System.Drawing.Size(110, 24);
            mnuPaste.Text = "&Paste";
            // 
            // mnuDelete
            // 
            mnuDelete.Name = "mnuDelete";
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
            mnuTopmost.Size = new System.Drawing.Size(141, 24);
            mnuTopmost.Text = "To &Top";
            // 
            // mnuBottommost
            // 
            mnuBottommost.Name = "mnuBottommost";
            mnuBottommost.Size = new System.Drawing.Size(141, 24);
            mnuBottommost.Text = "To &Bottom";
            // 
            // mnuMoveUp
            // 
            mnuMoveUp.Name = "mnuMoveUp";
            mnuMoveUp.Size = new System.Drawing.Size(141, 24);
            mnuMoveUp.Text = "Move &Up";
            // 
            // mnuMoveDown
            // 
            mnuMoveDown.Name = "mnuMoveDown";
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
            mnuGroup.Size = new System.Drawing.Size(124, 24);
            mnuGroup.Text = "&Group";
            // 
            // mnuUngroup
            // 
            mnuUngroup.Name = "mnuUngroup";
            mnuUngroup.Size = new System.Drawing.Size(124, 24);
            mnuUngroup.Text = "&Ungroup";

            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
        }
    }
}

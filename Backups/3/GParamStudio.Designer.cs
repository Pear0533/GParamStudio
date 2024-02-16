namespace GParamStudio
{
    partial class GParamStudio
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GParamStudio));
            this.ribbon = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupsParamsContainer = new System.Windows.Forms.SplitContainer();
            this.groupsBox = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.paramsPropertiesContainer = new System.Windows.Forms.SplitContainer();
            this.paramsBox = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.propertiesPanel = new System.Windows.Forms.Panel();
            this.versionStr = new System.Windows.Forms.Label();
            this.copyrightInfoStr = new System.Windows.Forms.Label();
            this.ribbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupsParamsContainer)).BeginInit();
            this.groupsParamsContainer.Panel1.SuspendLayout();
            this.groupsParamsContainer.Panel2.SuspendLayout();
            this.groupsParamsContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paramsPropertiesContainer)).BeginInit();
            this.paramsPropertiesContainer.Panel1.SuspendLayout();
            this.paramsPropertiesContainer.Panel2.SuspendLayout();
            this.paramsPropertiesContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ribbon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.themesToolStripMenuItem});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.Name = "ribbon";
            this.ribbon.Size = new System.Drawing.Size(800, 24);
            this.ribbon.TabIndex = 0;
            this.ribbon.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItemClick);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Visible = false;
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItemClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Visible = false;
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItemClick);
            // 
            // themesToolStripMenuItem
            // 
            this.themesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lightToolStripMenuItem,
            this.darkToolStripMenuItem});
            this.themesToolStripMenuItem.Name = "themesToolStripMenuItem";
            this.themesToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.themesToolStripMenuItem.Text = "Themes";
            // 
            // lightToolStripMenuItem
            // 
            this.lightToolStripMenuItem.Name = "lightToolStripMenuItem";
            this.lightToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.lightToolStripMenuItem.Text = "Light";
            this.lightToolStripMenuItem.Click += new System.EventHandler(this.LightToolStripMenuItemClick);
            // 
            // darkToolStripMenuItem
            // 
            this.darkToolStripMenuItem.Name = "darkToolStripMenuItem";
            this.darkToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.darkToolStripMenuItem.Text = "Dark";
            this.darkToolStripMenuItem.Click += new System.EventHandler(this.DarkToolStripMenuItemClick);
            // 
            // groupsParamsContainer
            // 
            this.groupsParamsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupsParamsContainer.Enabled = false;
            this.groupsParamsContainer.Location = new System.Drawing.Point(0, 22);
            this.groupsParamsContainer.Name = "groupsParamsContainer";
            // 
            // groupsParamsContainer.Panel1
            // 
            this.groupsParamsContainer.Panel1.Controls.Add(this.groupsBox);
            this.groupsParamsContainer.Panel1.Controls.Add(this.label1);
            // 
            // groupsParamsContainer.Panel2
            // 
            this.groupsParamsContainer.Panel2.Controls.Add(this.paramsPropertiesContainer);
            this.groupsParamsContainer.Size = new System.Drawing.Size(800, 428);
            this.groupsParamsContainer.SplitterDistance = 375;
            this.groupsParamsContainer.TabIndex = 3;
            // 
            // groupsBox
            // 
            this.groupsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupsBox.ItemHeight = 25;
            this.groupsBox.Location = new System.Drawing.Point(4, 26);
            this.groupsBox.Name = "groupsBox";
            this.groupsBox.Size = new System.Drawing.Size(368, 399);
            this.groupsBox.TabIndex = 5;
            this.groupsBox.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.GroupsBoxAfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Groups:";
            // 
            // paramsPropertiesContainer
            // 
            this.paramsPropertiesContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paramsPropertiesContainer.Location = new System.Drawing.Point(4, 3);
            this.paramsPropertiesContainer.Name = "paramsPropertiesContainer";
            this.paramsPropertiesContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // paramsPropertiesContainer.Panel1
            // 
            this.paramsPropertiesContainer.Panel1.Controls.Add(this.paramsBox);
            this.paramsPropertiesContainer.Panel1.Controls.Add(this.label2);
            // 
            // paramsPropertiesContainer.Panel2
            // 
            this.paramsPropertiesContainer.Panel2.Controls.Add(this.label3);
            this.paramsPropertiesContainer.Panel2.Controls.Add(this.propertiesPanel);
            this.paramsPropertiesContainer.Size = new System.Drawing.Size(408, 422);
            this.paramsPropertiesContainer.SplitterDistance = 205;
            this.paramsPropertiesContainer.TabIndex = 7;
            // 
            // paramsBox
            // 
            this.paramsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paramsBox.ItemHeight = 25;
            this.paramsBox.Location = new System.Drawing.Point(3, 19);
            this.paramsBox.Name = "paramsBox";
            this.paramsBox.Size = new System.Drawing.Size(403, 182);
            this.paramsBox.TabIndex = 6;
            this.paramsBox.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ParamsBoxBeforeLabelEdit);
            this.paramsBox.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ParamsBoxAfterLabelEdit);
            this.paramsBox.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ParamsBoxNodeMouseClick);
            this.paramsBox.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ParamsBoxNodeMouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Parameters:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, -3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Properties:";
            // 
            // propertiesPanel
            // 
            this.propertiesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.propertiesPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.propertiesPanel.Location = new System.Drawing.Point(3, 15);
            this.propertiesPanel.Name = "propertiesPanel";
            this.propertiesPanel.Size = new System.Drawing.Size(403, 188);
            this.propertiesPanel.TabIndex = 0;
            // 
            // versionStr
            // 
            this.versionStr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.versionStr.AutoSize = true;
            this.versionStr.BackColor = System.Drawing.SystemColors.Control;
            this.versionStr.ForeColor = System.Drawing.Color.DarkGray;
            this.versionStr.Location = new System.Drawing.Point(520, 24);
            this.versionStr.Name = "versionStr";
            this.versionStr.Size = new System.Drawing.Size(48, 15);
            this.versionStr.TabIndex = 8;
            this.versionStr.Text = "Version:";
            // 
            // copyrightInfoStr
            // 
            this.copyrightInfoStr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyrightInfoStr.AutoSize = true;
            this.copyrightInfoStr.BackColor = System.Drawing.SystemColors.Control;
            this.copyrightInfoStr.ForeColor = System.Drawing.Color.DarkGray;
            this.copyrightInfoStr.Location = new System.Drawing.Point(602, 24);
            this.copyrightInfoStr.Name = "copyrightInfoStr";
            this.copyrightInfoStr.Size = new System.Drawing.Size(174, 15);
            this.copyrightInfoStr.TabIndex = 7;
            this.copyrightInfoStr.Text = "© Pear, 2022 All rights reserved.";
            // 
            // GParamStudio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.versionStr);
            this.Controls.Add(this.copyrightInfoStr);
            this.Controls.Add(this.groupsParamsContainer);
            this.Controls.Add(this.ribbon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.ribbon;
            this.Name = "GParamStudio";
            this.Text = "GParam Studio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GParamStudioFormClosing);
            this.ribbon.ResumeLayout(false);
            this.ribbon.PerformLayout();
            this.groupsParamsContainer.Panel1.ResumeLayout(false);
            this.groupsParamsContainer.Panel1.PerformLayout();
            this.groupsParamsContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupsParamsContainer)).EndInit();
            this.groupsParamsContainer.ResumeLayout(false);
            this.paramsPropertiesContainer.Panel1.ResumeLayout(false);
            this.paramsPropertiesContainer.Panel1.PerformLayout();
            this.paramsPropertiesContainer.Panel2.ResumeLayout(false);
            this.paramsPropertiesContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paramsPropertiesContainer)).EndInit();
            this.paramsPropertiesContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip ribbon;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private SplitContainer groupsParamsContainer;
        private Label label1;
        private Label label2;
        private TreeView paramsBox;
        private TreeView groupsBox;
        private Label copyrightInfoStr;
        private ToolStripMenuItem themesToolStripMenuItem;
        private ToolStripMenuItem lightToolStripMenuItem;
        private ToolStripMenuItem darkToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private SplitContainer paramsPropertiesContainer;
        private Panel propertiesPanel;
        private Label label3;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private Label versionStr;
    }
}
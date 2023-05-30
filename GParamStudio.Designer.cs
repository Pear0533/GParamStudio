using JRINCCustomControls;

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GParamStudio));
            ribbon = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            themesToolStripMenuItem = new ToolStripMenuItem();
            lightToolStripMenuItem = new ToolStripMenuItem();
            darkToolStripMenuItem = new ToolStripMenuItem();
            groupsParamsContainer = new SplitContainer();
            groupsBox = new customTreeView();
            groupNodeColors = new ImageList(components);
            label1 = new Label();
            paramsPropertiesContainer = new SplitContainer();
            paramsBox = new customTreeView();
            label2 = new Label();
            label3 = new Label();
            propertiesPanel = new Panel();
            versionStr = new Label();
            applyMassEditCheckbox = new CheckBox();
            affectAllMapAreasCheckbox = new CheckBox();
            copyrightInfoStr = new Label();
            mapAreaIdNodeRightClickMenu = new ContextMenuStrip(components);
            addNewTimeToolStripMenuItem = new ToolStripMenuItem();
            paramsBoxAddParamRightClickMenu = new ContextMenuStrip(components);
            addNewParamToolStripMenuItem = new ToolStripMenuItem();
            paramNodeRightClickMenu = new ContextMenuStrip(components);
            deleteParamToolStripMenuItem = new ToolStripMenuItem();
            changeCommentToolStripMenuItem = new ToolStripMenuItem();
            groupNodeRightClickMenu = new ContextMenuStrip(components);
            assignCommentToolStripMenuItem = new ToolStripMenuItem();
            ribbon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)groupsParamsContainer).BeginInit();
            groupsParamsContainer.Panel1.SuspendLayout();
            groupsParamsContainer.Panel2.SuspendLayout();
            groupsParamsContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)paramsPropertiesContainer).BeginInit();
            paramsPropertiesContainer.Panel1.SuspendLayout();
            paramsPropertiesContainer.Panel2.SuspendLayout();
            paramsPropertiesContainer.SuspendLayout();
            mapAreaIdNodeRightClickMenu.SuspendLayout();
            paramsBoxAddParamRightClickMenu.SuspendLayout();
            paramNodeRightClickMenu.SuspendLayout();
            groupNodeRightClickMenu.SuspendLayout();
            SuspendLayout();
            // 
            // ribbon
            // 
            ribbon.ImageScalingSize = new Size(24, 24);
            ribbon.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, themesToolStripMenuItem });
            ribbon.Location = new Point(0, 0);
            ribbon.Name = "ribbon";
            ribbon.Size = new Size(944, 24);
            ribbon.TabIndex = 0;
            ribbon.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(190, 22);
            openToolStripMenuItem.Text = "Open (Ctrl+O)";
            openToolStripMenuItem.Click += OpenToolStripMenuItemClick;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(190, 22);
            saveToolStripMenuItem.Text = "Save (Ctrl+S)";
            saveToolStripMenuItem.Visible = false;
            saveToolStripMenuItem.Click += SaveToolStripMenuItemClick;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(190, 22);
            saveAsToolStripMenuItem.Text = "Save As (Ctrl+Shift+S)";
            saveAsToolStripMenuItem.Visible = false;
            saveAsToolStripMenuItem.Click += SaveAsToolStripMenuItemClick;
            // 
            // themesToolStripMenuItem
            // 
            themesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { lightToolStripMenuItem, darkToolStripMenuItem });
            themesToolStripMenuItem.Name = "themesToolStripMenuItem";
            themesToolStripMenuItem.Size = new Size(60, 20);
            themesToolStripMenuItem.Text = "Themes";
            // 
            // lightToolStripMenuItem
            // 
            lightToolStripMenuItem.Name = "lightToolStripMenuItem";
            lightToolStripMenuItem.Size = new Size(101, 22);
            lightToolStripMenuItem.Text = "Light";
            lightToolStripMenuItem.Click += LightToolStripMenuItemClick;
            // 
            // darkToolStripMenuItem
            // 
            darkToolStripMenuItem.Name = "darkToolStripMenuItem";
            darkToolStripMenuItem.Size = new Size(101, 22);
            darkToolStripMenuItem.Text = "Dark";
            darkToolStripMenuItem.Click += DarkToolStripMenuItemClick;
            // 
            // groupsParamsContainer
            // 
            groupsParamsContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupsParamsContainer.Enabled = false;
            groupsParamsContainer.Location = new Point(0, 22);
            groupsParamsContainer.Name = "groupsParamsContainer";
            // 
            // groupsParamsContainer.Panel1
            // 
            groupsParamsContainer.Panel1.Controls.Add(groupsBox);
            groupsParamsContainer.Panel1.Controls.Add(label1);
            // 
            // groupsParamsContainer.Panel2
            // 
            groupsParamsContainer.Panel2.Controls.Add(paramsPropertiesContainer);
            groupsParamsContainer.Size = new Size(944, 428);
            groupsParamsContainer.SplitterDistance = 442;
            groupsParamsContainer.TabIndex = 3;
            // 
            // groupsBox
            // 
            groupsBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupsBox.ImageIndex = 0;
            groupsBox.ImageList = groupNodeColors;
            groupsBox.ItemHeight = 25;
            groupsBox.Location = new Point(4, 22);
            groupsBox.Name = "groupsBox";
            groupsBox.PreseveTreeState = true;
            groupsBox.SelectedImageIndex = 0;
            groupsBox.Size = new Size(435, 403);
            groupsBox.TabIndex = 5;
            groupsBox.TreeState_dic = null;
            groupsBox.AfterSelect += GroupsBoxAfterSelect;
            groupsBox.MouseDown += GroupsBoxRightClick;
            // 
            // groupNodeColors
            // 
            groupNodeColors.ColorDepth = ColorDepth.Depth8Bit;
            groupNodeColors.ImageSize = new Size(16, 16);
            groupNodeColors.TransparentColor = Color.Transparent;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1, 4);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 4;
            label1.Text = "Groups:";
            // 
            // paramsPropertiesContainer
            // 
            paramsPropertiesContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            paramsPropertiesContainer.Location = new Point(4, 3);
            paramsPropertiesContainer.Name = "paramsPropertiesContainer";
            paramsPropertiesContainer.Orientation = Orientation.Horizontal;
            // 
            // paramsPropertiesContainer.Panel1
            // 
            paramsPropertiesContainer.Panel1.Controls.Add(paramsBox);
            paramsPropertiesContainer.Panel1.Controls.Add(label2);
            // 
            // paramsPropertiesContainer.Panel2
            // 
            paramsPropertiesContainer.Panel2.Controls.Add(label3);
            paramsPropertiesContainer.Panel2.Controls.Add(propertiesPanel);
            paramsPropertiesContainer.Size = new Size(485, 422);
            paramsPropertiesContainer.SplitterDistance = 205;
            paramsPropertiesContainer.TabIndex = 7;
            // 
            // paramsBox
            // 
            paramsBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            paramsBox.ItemHeight = 25;
            paramsBox.Location = new Point(3, 19);
            paramsBox.Name = "paramsBox";
            paramsBox.PreseveTreeState = true;
            paramsBox.Size = new Size(480, 182);
            paramsBox.TabIndex = 6;
            paramsBox.TreeState_dic = null;
            paramsBox.BeforeLabelEdit += ParamsBoxBeforeLabelEdit;
            paramsBox.AfterLabelEdit += ParamsBoxAfterLabelEdit;
            paramsBox.AfterExpand += ParamsBox_AfterExpand;
            paramsBox.AfterSelect += ParamsBox_AfterSelect;
            paramsBox.NodeMouseDoubleClick += ParamsBoxNodeMouseDoubleClick;
            paramsBox.MouseDown += ParamsBox_MouseDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1, 1);
            label2.Name = "label2";
            label2.Size = new Size(69, 15);
            label2.TabIndex = 5;
            label2.Text = "Parameters:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1, -3);
            label3.Name = "label3";
            label3.Size = new Size(63, 15);
            label3.TabIndex = 7;
            label3.Text = "Properties:";
            // 
            // propertiesPanel
            // 
            propertiesPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            propertiesPanel.BorderStyle = BorderStyle.FixedSingle;
            propertiesPanel.ForeColor = SystemColors.ControlText;
            propertiesPanel.Location = new Point(3, 15);
            propertiesPanel.Name = "propertiesPanel";
            propertiesPanel.Size = new Size(480, 195);
            propertiesPanel.TabIndex = 0;
            // 
            // versionStr
            // 
            versionStr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            versionStr.AutoSize = true;
            versionStr.BackColor = SystemColors.Control;
            versionStr.ForeColor = Color.DarkGray;
            versionStr.Location = new Point(680, 24);
            versionStr.Name = "versionStr";
            versionStr.Size = new Size(48, 15);
            versionStr.TabIndex = 8;
            versionStr.Text = "Version:";
            // 
            // applyMassEditCheckbox
            // 
            applyMassEditCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            applyMassEditCheckbox.Enabled = false;
            applyMassEditCheckbox.Location = new Point(685, 2);
            applyMassEditCheckbox.Name = "applyMassEditCheckbox";
            applyMassEditCheckbox.Size = new Size(115, 24);
            applyMassEditCheckbox.TabIndex = 9;
            applyMassEditCheckbox.Text = "Apply Mass Edit";
            // 
            // affectAllMapAreasCheckbox
            // 
            affectAllMapAreasCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            affectAllMapAreasCheckbox.Enabled = false;
            affectAllMapAreasCheckbox.Location = new Point(800, 2);
            affectAllMapAreasCheckbox.Name = "affectAllMapAreasCheckbox";
            affectAllMapAreasCheckbox.Size = new Size(155, 24);
            affectAllMapAreasCheckbox.TabIndex = 10;
            affectAllMapAreasCheckbox.Text = "Affect All Map Areas";
            // 
            // copyrightInfoStr
            // 
            copyrightInfoStr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            copyrightInfoStr.AutoSize = true;
            copyrightInfoStr.BackColor = SystemColors.Control;
            copyrightInfoStr.ForeColor = Color.DarkGray;
            copyrightInfoStr.Location = new Point(761, 24);
            copyrightInfoStr.Name = "copyrightInfoStr";
            copyrightInfoStr.Size = new Size(174, 15);
            copyrightInfoStr.TabIndex = 7;
            copyrightInfoStr.Text = "© Pear, 2023 All rights reserved.";
            // 
            // mapAreaIdNodeRightClickMenu
            // 
            mapAreaIdNodeRightClickMenu.Items.AddRange(new ToolStripItem[] { addNewTimeToolStripMenuItem });
            mapAreaIdNodeRightClickMenu.Name = "mapAreaIdNodeRightClickMenu";
            mapAreaIdNodeRightClickMenu.Size = new Size(163, 26);
            // 
            // addNewTimeToolStripMenuItem
            // 
            addNewTimeToolStripMenuItem.Name = "addNewTimeToolStripMenuItem";
            addNewTimeToolStripMenuItem.Size = new Size(162, 22);
            addNewTimeToolStripMenuItem.Text = "Add Time of Day";
            // 
            // paramsBoxAddParamRightClickMenu
            // 
            paramsBoxAddParamRightClickMenu.Items.AddRange(new ToolStripItem[] { addNewParamToolStripMenuItem });
            paramsBoxAddParamRightClickMenu.Name = "paramsBoxAddParamRightClickMenu";
            paramsBoxAddParamRightClickMenu.Size = new Size(161, 26);
            // 
            // addNewParamToolStripMenuItem
            // 
            addNewParamToolStripMenuItem.Name = "addNewParamToolStripMenuItem";
            addNewParamToolStripMenuItem.Size = new Size(160, 22);
            addNewParamToolStripMenuItem.Text = "Add New Param";
            // 
            // paramNodeRightClickMenu
            // 
            paramNodeRightClickMenu.Items.AddRange(new ToolStripItem[] { deleteParamToolStripMenuItem, changeCommentToolStripMenuItem });
            paramNodeRightClickMenu.Name = "paramNodeRightClickMenu";
            paramNodeRightClickMenu.Size = new Size(167, 48);
            // 
            // deleteParamToolStripMenuItem
            // 
            deleteParamToolStripMenuItem.Name = "deleteParamToolStripMenuItem";
            deleteParamToolStripMenuItem.Size = new Size(166, 22);
            deleteParamToolStripMenuItem.Text = "Delete Param";
            // 
            // changeCommentToolStripMenuItem
            // 
            changeCommentToolStripMenuItem.Name = "changeCommentToolStripMenuItem";
            changeCommentToolStripMenuItem.Size = new Size(166, 22);
            changeCommentToolStripMenuItem.Text = "Assign Comment";
            // 
            // groupNodeRightClickMenu
            // 
            groupNodeRightClickMenu.Items.AddRange(new ToolStripItem[] { assignCommentToolStripMenuItem });
            groupNodeRightClickMenu.Name = "groupNodeRightClickMenu";
            groupNodeRightClickMenu.Size = new Size(167, 26);
            // 
            // assignCommentToolStripMenuItem
            // 
            assignCommentToolStripMenuItem.Name = "assignCommentToolStripMenuItem";
            assignCommentToolStripMenuItem.Size = new Size(166, 22);
            assignCommentToolStripMenuItem.Text = "Assign Comment";
            // 
            // GParamStudio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 450);
            Controls.Add(versionStr);
            Controls.Add(applyMassEditCheckbox);
            Controls.Add(affectAllMapAreasCheckbox);
            Controls.Add(copyrightInfoStr);
            Controls.Add(groupsParamsContainer);
            Controls.Add(ribbon);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = ribbon;
            MinimumSize = new Size(700, 250);
            Name = "GParamStudio";
            Text = "GParam Studio";
            FormClosing += GParamStudioFormClosing;
            KeyDown += GParamStudio_KeyDown;
            ribbon.ResumeLayout(false);
            ribbon.PerformLayout();
            groupsParamsContainer.Panel1.ResumeLayout(false);
            groupsParamsContainer.Panel1.PerformLayout();
            groupsParamsContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)groupsParamsContainer).EndInit();
            groupsParamsContainer.ResumeLayout(false);
            paramsPropertiesContainer.Panel1.ResumeLayout(false);
            paramsPropertiesContainer.Panel1.PerformLayout();
            paramsPropertiesContainer.Panel2.ResumeLayout(false);
            paramsPropertiesContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)paramsPropertiesContainer).EndInit();
            paramsPropertiesContainer.ResumeLayout(false);
            mapAreaIdNodeRightClickMenu.ResumeLayout(false);
            paramsBoxAddParamRightClickMenu.ResumeLayout(false);
            paramNodeRightClickMenu.ResumeLayout(false);
            groupNodeRightClickMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip ribbon;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private SplitContainer groupsParamsContainer;
        private Label label1;
        private Label label2;
        private customTreeView groupsBox;
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
        private CheckBox applyMassEditCheckbox;
        private CheckBox affectAllMapAreasCheckbox;
        private ContextMenuStrip mapAreaIdNodeRightClickMenu;
        private ToolStripMenuItem addNewTimeToolStripMenuItem;
        private ContextMenuStrip paramsBoxAddParamRightClickMenu;
        private ToolStripMenuItem addNewParamToolStripMenuItem;
        private ContextMenuStrip paramNodeRightClickMenu;
        private ToolStripMenuItem deleteParamToolStripMenuItem;
        private customTreeView paramsBox;
        private ToolStripMenuItem changeCommentToolStripMenuItem;
        private ImageList groupNodeColors;
        private ContextMenuStrip groupNodeRightClickMenu;
        private ToolStripMenuItem assignCommentToolStripMenuItem;
    }
}
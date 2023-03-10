using System.Globalization;
using System.Numerics;
using AngleAltitudeControls;
using Cyotek.Windows.Forms;
using SoulsFormats;

#pragma warning disable SYSLIB0014

namespace GParamStudio;

public partial class GParamStudio : Form
{
    private const string version = "1.08";
    private static GPARAM gparam = new();
    private static TreeNode? prevSelectedNode;
    private static string gparamFileName = "";
    private static bool isGParamFileOpen;
    private static readonly List<int[]> paramValueInfoList = new();
    private static readonly List<GPARAM.Param> allParamsList = new();

    public GParamStudio()
    {
        InitializeComponent();
        SetVersionString();
        EnableDarkTheme();
    }

    private void SetVersionString()
    {
        versionStr.Text += $@" {version}";
    }

    private static DialogResult ShowQuestionDialog(string str)
    {
        return MessageBox.Show(str, @"Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    private int ShowAddParamDialog()
    {
        var form = new Form();
        form.Text = @"Add New Param";
        form.Icon = Icon;
        form.Width = 500;
        form.Height = 500;
        form.MinimumSize = new Size(300, 300);
        form.StartPosition = FormStartPosition.CenterScreen;
        form.MaximizeBox = false;
        var selectorBox = new TreeView();
        selectorBox.Width = 450;
        selectorBox.Height = 420;
        selectorBox.Location = new Point(selectorBox.Location.X + 15, selectorBox.Location.Y + 5);
        selectorBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
        var cancelButton = new Button
        {
            Text = @"Cancel",
            Size = new Size(65, 25),
            Location = new Point(selectorBox.Width - 105,
                selectorBox.Bottom + 5),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right
        };
        var okButton = new Button
        {
            Text = @"OK",
            Size = new Size(50, 25),
            Location = new Point(selectorBox.Width - 35,
                selectorBox.Bottom + 5),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            DialogResult = DialogResult.OK
        };

        void CloseDialogHandler(object? s, EventArgs e)
        {
            form.Close();
        }

        cancelButton.Click += CloseDialogHandler;
        okButton.Click += CloseDialogHandler;
        form.AcceptButton = okButton;
        form.Controls.Add(selectorBox);
        form.Controls.Add(cancelButton);
        form.Controls.Add(okButton);
        foreach (GPARAM.Param param in allParamsList) selectorBox.Nodes.Add(new TreeNode(param.Name2));
        return form.ShowDialog() == DialogResult.OK ? selectorBox.SelectedNode.Index : -1;
    }

    private static void ChangeTheme(Control control, Color backColor, Color foreColor)
    {
        foreach (Control subControl in control.Controls)
        {
            subControl.BackColor = backColor;
            subControl.ForeColor = foreColor;
            ChangeTheme(subControl, backColor, foreColor);
        }
    }

    private void EnableDarkTheme()
    {
        ChangeTheme(this, ColorTranslator.FromHtml("#323232"), ColorTranslator.FromHtml("#d9d9d9"));
    }

    private void EnableLightTheme()
    {
        ChangeTheme(this, DefaultBackColor, DefaultForeColor);
    }

    private void LoadParams()
    {
        groupsBox.SaveTreeState();
        groupsBox.Nodes.Clear();
        paramsBox.Nodes.Clear();
        propertiesPanel.Controls.Clear();
        foreach (GPARAM.Group group in gparam.Groups)
        {
            var groupNode = new TreeNode { Name = group.Name2, Text = group.Name2 };
            if (group.Params.Count > 0)
            {
                List<int> ids = new();
                List<float> times = new();
                foreach (GPARAM.Param param in group.Params)
                {
                    ids.AddRange(param.ValueIDs);
                    if (param.TimeOfDay != null) times.AddRange(param.TimeOfDay);
                    if (allParamsList.All(i => i.Name2 != param.Name2)) allParamsList.Add(param);
                }
                ids = ids.Distinct().ToList();
                times = times.Distinct().ToList();
                foreach (TreeNode? idNode in ids.Select(id => new TreeNode { Text = $@"? ID: {id}", Name = id.ToString() }))
                {
                    const float baseTime = 0;
                    foreach (TreeNode timeNode in from time in times
                             let newTime = new DateTime(TimeSpan.FromHours(baseTime + time).Ticks)
                             select new TreeNode
                             {
                                 Text = $@"? {newTime.ToString("h:mm tt", CultureInfo.InvariantCulture)}",
                                 Name = time.ToString(CultureInfo.InvariantCulture)
                             })
                    {
                        idNode.Nodes.Add(timeNode);
                    }
                    groupNode.Nodes.Add(idNode);
                }
            }
            groupsBox.Nodes.Add(groupNode);
        }
        groupsBox.RestoreTreeState();
    }

    private void OpenToolStripMenuItemClick(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog { Filter = @"GPARAM File (*.gparam.dcx, *.gparam)|*.gparam.dcx;*.gparam", Multiselect = false };
        if (dialog.ShowDialog() != DialogResult.OK) return;
        gparamFileName = dialog.FileName;
        byte[] fileData = File.ReadAllBytes(gparamFileName);
        if (Path.GetExtension(gparamFileName) == ".dcx") fileData = DCX.Decompress(fileData);
        fileData = fileData.Take(64).Concat(new byte[4]).Concat(fileData.Skip(68)).ToArray();
        gparam = GPARAM.Read(fileData);
        isGParamFileOpen = true;
        saveToolStripMenuItem.Visible = true;
        saveAsToolStripMenuItem.Visible = true;
        Program.window.Text = $@"{Program.windowTitle} - {Path.GetFileName(gparamFileName)}";
        groupsParamsContainer.Enabled = true;
        paramsBox.LabelEdit = true;
        LoadParams();
    }

    private void LightToolStripMenuItemClick(object sender, EventArgs e)
    {
        EnableLightTheme();
    }

    private void DarkToolStripMenuItemClick(object sender, EventArgs e)
    {
        EnableDarkTheme();
    }

    private void GroupsBoxAfterSelect(object sender, TreeViewEventArgs e)
    {
        if (paramsBox.SelectedNode is { Parent: { } }) prevSelectedNode = paramsBox.SelectedNode.Parent;
        paramsBox.SaveTreeState();
        paramsBox.Nodes.Clear();
        paramValueInfoList.Clear();
        propertiesPanel.Controls.Clear();
        var isTimeNodeSelected = false;
        foreach (TreeNode groupNode in groupsBox.Nodes)
        {
            foreach (TreeNode idNode in groupNode.Nodes)
            {
                foreach (TreeNode timeNode in idNode.Nodes)
                {
                    if (!timeNode.IsSelected) continue;
                    isTimeNodeSelected = true;
                    int wantedValueID = int.Parse(idNode.Name);
                    float wantedTimeValue = float.Parse(timeNode.Name);
                    GPARAM.Group group = gparam.Groups[groupNode.Index];
                    foreach (GPARAM.Param param in group.Params)
                    {
                        for (var i = 0; i < param.ValueIDs.Count; ++i)
                        {
                            int valueID = param.ValueIDs[i];
                            if (param.TimeOfDay == null) continue;
                            if (valueID != wantedValueID || !param.TimeOfDay[i].Equals(wantedTimeValue)) continue;
                            int shortNameIndex = param.Name1.IndexOf(param.Name2, StringComparison.Ordinal);
                            string paramNodeName = shortNameIndex != -1 ? param.Name1[shortNameIndex..] : param.Name2;
                            TreeNode paramNode = new() { Name = paramNodeName, Text = paramNodeName };
                            TreeNode valueNode = new() { Text = param.Values[i].ToString() };
                            paramValueInfoList.Add(new[] { groupNode.Index, group.Params.IndexOf(param), i });
                            paramNode.Nodes.Add(valueNode);
                            paramsBox.Nodes.Add(paramNode);
                            break;
                        }
                    }
                }
            }
        }
        paramsBox.RestoreTreeState();
        if (prevSelectedNode != null && prevSelectedNode.Nodes.Count > 0)
        {
            TreeNode? matchingParamNode = paramsBox.Nodes.Find(prevSelectedNode.Text, false).FirstOrDefault();
            if (matchingParamNode != null) paramsBox.SelectedNode = paramsBox.Nodes[prevSelectedNode.Text].Nodes[0];
        }
        if (isTimeNodeSelected && paramsBox.Nodes.Count > 0) return;
        paramsBox.Nodes.Clear();
        propertiesPanel.Controls.Clear();
        if (!isTimeNodeSelected)
            paramsBox.Nodes.Add(groupsBox.SelectedNode.Nodes.Count == 0 ? "There are no parameters for this group." : "Click the + button to expand the node.");
        else paramsBox.Nodes.Add("There are no parameters for this area and time of day.");
    }

    private void ParamsBoxNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Node.Parent != null) e.Node.BeginEdit();
    }

    private static int[] GetValueInfoFromParamValInfoList(TreeNode node)
    {
        return paramValueInfoList[node.Parent.Index];
    }

    private static GPARAM.Param GetParamFromValueNode(TreeNode node)
    {
        int[] valueInfo = GetValueInfoFromParamValInfoList(node);
        return gparam.Groups[valueInfo[0]].Params[valueInfo[1]];
    }

    private static string GetStringFromParamValue(string value)
    {
        dynamic valueStr = value.Replace("<", "").Replace(">", "");
        return valueStr;
    }

    private static float[] GetSplitValuesFromValueString(dynamic value)
    {
        string[] splitNewValue = CultureInfo.CurrentCulture.Equals(new CultureInfo("pt-PT")) ? value.split(".") : value.Split(",");
        try
        {
            float value1 = float.Parse(splitNewValue.ElementAtOrDefault(0) ?? "0");
            float value2 = float.Parse(splitNewValue.ElementAtOrDefault(1) ?? "0");
            float value3 = float.Parse(splitNewValue.ElementAtOrDefault(2) ?? "0");
            float value4 = float.Parse(splitNewValue.ElementAtOrDefault(3) ?? "0");
            return new[] { value1, value2, value3, value4 };
        }
        catch
        {
            return Array.Empty<float>();
        }
    }

    private void UpdateParamValueInfo(IReadOnlyList<int> valueInfo, object newValue)
    {
        if (applyMassEditCheckbox.Checked)
        {
            int mapAreaId = gparam.Groups[valueInfo[0]].Params[valueInfo[1]].ValueIDs[valueInfo[2]];
            for (var i = 0; i < gparam.Groups[valueInfo[0]].Params[valueInfo[1]].Values.Count; i++)
            {
                if (affectAllMapAreasCheckbox.Checked
                    || gparam.Groups[valueInfo[0]].Params[valueInfo[1]].ValueIDs[i] == mapAreaId)
                    gparam.Groups[valueInfo[0]].Params[valueInfo[1]].Values[i] = newValue;
            }
        }
        else gparam.Groups[valueInfo[0]].Params[valueInfo[1]].Values[valueInfo[2]] = newValue;
    }

    private void ParamsBoxAfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
        dynamic newValue = GetStringFromParamValue(e.Label is null or "" ? e.Node.Text : e.Label);
        float[] values = GetSplitValuesFromValueString(newValue);
        int[] valueInfo = GetValueInfoFromParamValInfoList(e.Node);
        GPARAM.Param param = GetParamFromValueNode(e.Node);
        try
        {
            newValue = param.Type switch
            {
                GPARAM.ParamType.Float2 => new Vector2(values[0], values[1]),
                GPARAM.ParamType.Float3 => new Vector3(values[0], values[1], values[2]),
                GPARAM.ParamType.Float4 => new Vector4(Math.Abs(values[0]), Math.Abs(values[1]), Math.Abs(values[2]), Math.Abs(values[3])),
                GPARAM.ParamType.Byte4 => new[] { (byte)values[0], (byte)values[1], (byte)values[2], (byte)values[3] },
                GPARAM.ParamType.Byte => byte.Parse(newValue),
                GPARAM.ParamType.Short => short.Parse(newValue),
                GPARAM.ParamType.IntA or GPARAM.ParamType.IntB => int.Parse(newValue),
                GPARAM.ParamType.BoolA or GPARAM.ParamType.BoolB => bool.Parse(newValue),
                GPARAM.ParamType.Float => float.Parse(newValue),
                _ => 0
            };
            UpdateParamValueInfo(valueInfo, newValue);
            e.CancelEdit = true;
            e.Node.Text = newValue.ToString();
            UpdateInteractiveControl(e.Node);
        }
        catch
        {
            e.CancelEdit = true;
        }
    }

    private static void SaveGPARAMFile(string fileName)
    {
        if (fileName.EndsWith(".gparam")) gparam.Write(fileName);
        else if (fileName.EndsWith(".dcx")) File.WriteAllBytes(fileName, DCX.Compress(gparam.Write(), DCX.Type.DCX_KRAK));
    }

    private void SaveToolStripMenuItemClick(object sender, EventArgs e)
    {
        SaveGPARAMFile(gparamFileName);
    }

    private void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
    {
        var dialog = new SaveFileDialog
            { Filter = @"GPARAM File (*.gparam)|*.gparam|BND File (*.gparam.dcx)|*.gparam.dcx", FileName = Path.GetFileNameWithoutExtension(gparamFileName) };
        if (dialog.ShowDialog() != DialogResult.OK) return;
        SaveGPARAMFile(dialog.FileName);
    }

    private void GParamStudioFormClosing(object sender, FormClosingEventArgs e)
    {
        if (!isGParamFileOpen) return;
        DialogResult result = MessageBox.Show(@"Do you want to save changes to the GPARAM file?", @"Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
        switch (result)
        {
            case DialogResult.Yes:
                SaveGPARAMFile(gparamFileName);
                break;
            case DialogResult.Cancel:
                e.Cancel = true;
                break;
            case DialogResult.None:
            case DialogResult.OK:
            case DialogResult.Abort:
            case DialogResult.Retry:
            case DialogResult.Ignore:
            case DialogResult.No:
            case DialogResult.TryAgain:
            case DialogResult.Continue:
            default:
                break;
        }
    }

    private static NumericUpDown CreateNumBoxWithSetValue(decimal value, decimal increment)
    {
        var numBox = new NumericUpDown
        {
            Size = new Size(120, 30),
            Increment = increment,
            DecimalPlaces = 2,
            Minimum = decimal.MinValue,
            Maximum = decimal.MaxValue,
            Value = value
        };
        numBox.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        };
        return numBox;
    }

    private static int ToDegrees(double radians)
    {
        return (int)(radians * (180 / Math.PI));
    }

    private static float ToRadians(double degrees)
    {
        return (float)(degrees * (Math.PI / 180));
    }

    private void UpdateInteractiveControl(TreeNode node)
    {
        if (node.Parent == null) return;
        int[] valueInfo = GetValueInfoFromParamValInfoList(node);
        GPARAM.Param param = GetParamFromValueNode(node);
        dynamic value = GetStringFromParamValue(node.Text);
        float[] values = GetSplitValuesFromValueString(value);
        propertiesPanel.Controls.Clear();
        switch (param.Type)
        {
            case GPARAM.ParamType.Float2:
                int angleValue = ToDegrees(values[0]);
                int altitudeValue = ToDegrees(values[1]);
                var angleSelector = new AngleAltitudeSelector
                {
                    Dock = DockStyle.Fill,
                    Angle = angleValue,
                    Altitude = altitudeValue
                };
                NumericUpDown angleBox = CreateNumBoxWithSetValue(angleValue, 1);
                NumericUpDown altitudeBox = CreateNumBoxWithSetValue(altitudeValue, 1);

                void AngleSelectorValueChanged()
                {
                    var angle = new Vector2(ToRadians(angleSelector.Angle), ToRadians(angleSelector.Altitude));
                    angleBox.Value = angleSelector.Angle;
                    altitudeBox.Value = angleSelector.Altitude;
                    UpdateParamValueInfo(valueInfo, angle);
                    node.Text = angle.ToString();
                }

                angleSelector.AngleChanged += AngleSelectorValueChanged;
                angleSelector.AltitudeChanged += AngleSelectorValueChanged;
                angleBox.ValueChanged += (_, _) => { angleSelector.Angle = (int)angleBox.Value; };
                altitudeBox.ValueChanged += (_, _) => { angleSelector.Altitude = (int)altitudeBox.Value; };
                var numBoxesContainer = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };
                numBoxesContainer.Controls.Add(angleBox);
                numBoxesContainer.Controls.Add(altitudeBox);
                var angleControlContainer = new SplitContainer { Dock = DockStyle.Fill };
                angleControlContainer.Panel1.Controls.Add(angleSelector);
                angleControlContainer.Panel2.Controls.Add(numBoxesContainer);
                propertiesPanel.Controls.Add(angleControlContainer);
                break;
            case GPARAM.ParamType.Float4:
                var wheel = new ColorWheel
                {
                    Dock = DockStyle.Fill,
                    Color = Color.FromArgb(
                        255, (int)(values[0] * 255), (int)(values[1] * 255), (int)(values[2] * 255))
                };
                var editor = new ColorEditor { Dock = DockStyle.Fill, Color = wheel.Color };
                wheel.ColorChanged += (_, _) =>
                {
                    Color color = wheel.Color;
                    var colorObj = new Vector4((float)(color.R / 255.0), (float)(color.G / 255.0), (float)(color.B / 255.0), values[3]);
                    editor.Color = wheel.Color;
                    UpdateParamValueInfo(valueInfo, colorObj);
                    node.Text = colorObj.ToString();
                };
                editor.ColorChanged += (_, _) => { wheel.Color = editor.Color; };
                foreach (Control control in editor.Controls)
                {
                    control.KeyDown += (_, e) =>
                    {
                        if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
                    };
                }
                var colorControlContainer = new SplitContainer { Dock = DockStyle.Fill };
                colorControlContainer.Panel1.Controls.Add(wheel);
                colorControlContainer.Panel2.Controls.Add(editor);
                propertiesPanel.Controls.Add(colorControlContainer);
                break;
            case GPARAM.ParamType.BoolA:
            case GPARAM.ParamType.BoolB:
                var checkbox = new CheckBox { Size = propertiesPanel.Size with { Height = 30 }, Text = param.Name2, Checked = bool.Parse(value) };
                checkbox.CheckStateChanged += (_, _) =>
                {
                    UpdateParamValueInfo(valueInfo, checkbox.Checked);
                    node.Text = checkbox.Checked.ToString();
                };
                propertiesPanel.Controls.Add(checkbox);
                break;
            case GPARAM.ParamType.Byte:
            case GPARAM.ParamType.Short:
            case GPARAM.ParamType.IntA:
            case GPARAM.ParamType.IntB:
            case GPARAM.ParamType.Float:
                NumericUpDown numBox = CreateNumBoxWithSetValue(decimal.Parse(value), (decimal)0.01);
                numBox.ValueChanged += (_, _) =>
                {
                    object convertedNumBoxVal = numBox.Value;
                    switch (param.Type)
                    {
                        case GPARAM.ParamType.Byte:
                            convertedNumBoxVal = Convert.ToByte(convertedNumBoxVal);
                            break;
                        case GPARAM.ParamType.Short:
                            convertedNumBoxVal = Convert.ToInt16(convertedNumBoxVal);
                            break;
                        case GPARAM.ParamType.IntA:
                        case GPARAM.ParamType.IntB:
                            convertedNumBoxVal = Convert.ToInt32(convertedNumBoxVal);
                            break;
                        case GPARAM.ParamType.Float:
                            convertedNumBoxVal = Convert.ToSingle(convertedNumBoxVal);
                            break;
                        case GPARAM.ParamType.BoolA:
                        case GPARAM.ParamType.BoolB:
                        case GPARAM.ParamType.Float2:
                        case GPARAM.ParamType.Float3:
                        case GPARAM.ParamType.Float4:
                        case GPARAM.ParamType.Byte4:
                        default:
                            break;
                    }
                    UpdateParamValueInfo(valueInfo, convertedNumBoxVal);
                    node.Text = numBox.Value.ToString(CultureInfo.InvariantCulture);
                };
                propertiesPanel.Controls.Add(numBox);
                break;
            case GPARAM.ParamType.Float3:
            case GPARAM.ParamType.Byte4:
            default:
                propertiesPanel.Controls.Clear();
                propertiesPanel.Controls.Add(new Label { Dock = DockStyle.Fill, Text = @"Interactive controls for this parameter are unavailable." });
                break;
        }
    }

    private void ParamsBoxBeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
        if (e.Node.Parent == null) e.CancelEdit = true;
    }

    private static string ShowInputDialog(string text, string caption)
    {
        var prompt = new Form
        {
            Width = 240,
            Height = 125,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = caption,
            StartPosition = FormStartPosition.CenterScreen,
            MaximizeBox = false
        };
        var textLabel = new Label { Left = 8, Top = 8, Width = 200, Text = text };
        var textBox = new TextBox { Left = 10, Top = 28, Width = 200 };
        var cancelButton = new Button { Text = @"Cancel", Left = 9, Width = 100, Top = 55, DialogResult = DialogResult.Cancel };
        cancelButton.Click += (_, _) => { prompt.Close(); };
        var confirmation = new Button { Text = @"OK", Left = 112, Width = 100, Top = 55, DialogResult = DialogResult.OK };
        confirmation.Click += (_, _) => { prompt.Close(); };
        prompt.Controls.Add(textBox);
        prompt.Controls.Add(cancelButton);
        prompt.Controls.Add(confirmation);
        prompt.Controls.Add(textLabel);
        prompt.AcceptButton = confirmation;
        return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
    }

    private static TreeNode? GetHoveredNodeOnRightClick(TreeView treeView, MouseEventArgs e)
    {
        TreeNode? hoveredNode = treeView.GetNodeAt(e.X, e.Y);
        bool isMouseWithinNodeBounds = hoveredNode.Bounds.Contains(e.Location);
        treeView.SelectedNode = isMouseWithinNodeBounds ? hoveredNode : null;
        return isMouseWithinNodeBounds ? hoveredNode : null;
    }

    private void OnMapAreaIdNodeAddTimeOfDay(TreeNode mapAreaIdNode)
    {
        string timeOfDayStr = ShowInputDialog("Enter a time of day:", "Add Time");
        if (timeOfDayStr == "") return;
        float.TryParse(timeOfDayStr, out float timeOfDay);
        List<GPARAM.Param>? paramList = gparam.Groups.ElementAtOrDefault(groupsBox.Nodes.IndexOf(mapAreaIdNode.Parent))?.Params;
        if (paramList == null) return;
        foreach (GPARAM.Param param in paramList)
        {
            int newTimeOfDayInsertLoc = param.TimeOfDay.IndexOf(param.TimeOfDay.LastOrDefault(i => i < timeOfDay)) + 1;
            param.TimeOfDay.Insert(newTimeOfDayInsertLoc, timeOfDay);
            param.ValueIDs.Insert(newTimeOfDayInsertLoc, int.Parse(mapAreaIdNode.Name));
            param.Values.Insert(newTimeOfDayInsertLoc, param.Values[^1]);
        }
        LoadParams();
    }

    private void OnParamsBoxAddNewParam()
    {
        int paramIndex = ShowAddParamDialog();
        if (paramIndex == -1) return;
        int[] valueInfo = GetValueInfoFromParamValInfoList(paramsBox.Nodes[0].Nodes[0]);
        GPARAM.Group group = gparam.Groups[valueInfo[0]];
        GPARAM.Param newParam = allParamsList[paramIndex];
        var maxTimeOfDay = new List<float>();
        var maxValueIDs = new List<int>();
        foreach (GPARAM.Param param in group.Params)
        {
            if (param.TimeOfDay.Count > maxTimeOfDay.Count) maxTimeOfDay = param.TimeOfDay;
            if (param.ValueIDs.Count > maxValueIDs.Count) maxValueIDs = param.ValueIDs;
        }
        newParam.TimeOfDay = maxTimeOfDay;
        newParam.ValueIDs = maxValueIDs;
        for (var i = 0; i < newParam.ValueIDs.Count; ++i)
            if (i > newParam.Values.Count - 1)
                newParam.Values.Add(newParam.Values[0]);
        GPARAM.Param? existingParam = group.Params.FirstOrDefault(i => i.Name2 == newParam.Name2);
        if (existingParam != null) gparam.Groups[valueInfo[0]].Params[group.Params.IndexOf(existingParam)] = newParam;
        else gparam.Groups[valueInfo[0]].Params.Add(newParam);
        LoadParams();
    }

    private void OnParamNodeDeleteParam(TreeNode paramNode)
    {
        DialogResult result = ShowQuestionDialog("Are you sure you want to delete this param?");
        if (result != DialogResult.Yes) return;
        int[] valueInfo = GetValueInfoFromParamValInfoList(paramNode.Nodes[0]);
        GPARAM.Param param = gparam.Groups[valueInfo[0]].Params[valueInfo[1]];
        param.TimeOfDay.RemoveAt(valueInfo[2]);
        param.ValueIDs.RemoveAt(valueInfo[2]);
        param.Values.RemoveAt(valueInfo[2]);
        LoadParams();
    }

    private static void ShowRightClickMenu(ToolStripDropDown rightClickMenu, Control treeView, ToolStripItemClickedEventHandler clickEvent,
        MouseEventArgs e)
    {
        rightClickMenu.Closed += (_, _) => { rightClickMenu.ItemClicked -= clickEvent; };
        rightClickMenu.ItemClicked += clickEvent;
        rightClickMenu.Show(treeView, e.X, e.Y);
    }

    private void GroupsBoxParamIdNodeClick(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        TreeNode? mapAreaIdNode = GetHoveredNodeOnRightClick(groupsBox, e);
        if (mapAreaIdNode is not { Level: 1 }) return;
        ShowRightClickMenu(mapAreaIdNodeRightClickMenu, groupsBox, (_, _) => OnMapAreaIdNodeAddTimeOfDay(mapAreaIdNode), e);
    }

    private void ParamsBox_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        TreeNode? paramNode = GetHoveredNodeOnRightClick(paramsBox, e);
        if (paramNode is not { Level: 0 }) ShowRightClickMenu(paramsBoxAddParamRightClickMenu, paramsBox, (_, _) => OnParamsBoxAddNewParam(), e);
        else ShowRightClickMenu(paramNodeRightClickMenu, paramsBox, (_, _) => OnParamNodeDeleteParam(paramNode), e);
    }

    private void ParamsBox_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (e.Node != null) UpdateInteractiveControl(e.Node);
    }

    private void ParamsBox_AfterExpand(object sender, TreeViewEventArgs e)
    {
        if (e.Node == null || e.Node.Nodes.Count <= 0) return;
        if (prevSelectedNode != null && e.Node != prevSelectedNode) return;
        UpdateInteractiveControl(e.Node.Nodes[0]);
        paramsBox.SelectedNode = e.Node.Nodes[0];
    }
}
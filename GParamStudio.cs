using System.Globalization;
using System.Numerics;
using System.Reflection;
using AngleAltitudeControls;
using Cyotek.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoulsFormats;
using GPARAM = WitchyFormats.GPARAM;

#pragma warning disable SYSLIB0014

namespace GParamStudio;

public partial class GParamStudio : Form
{
    private const string version = "1.15";
    private static GPARAM gparam = new();
    private static TreeNode? prevSelectedNode;
    private static string gparamFileName = "";
    private static readonly string? rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static readonly string commentsJsonFilePath = $"{rootPath}\\comments.json";
    private static JObject commentsJson = new();
    private static bool isGParamFileOpen;
    private static readonly List<int[]> paramValueInfoList = new();
    private static readonly List<GPARAM.Param> allParamsList = new();

    public GParamStudio()
    {
        InitializeComponent();
        CenterToScreen();
        SetVersionString();
        EnableDarkTheme();
    }

    private void SetVersionString()
    {
        versionStr.Text += $@" {version}";
    }

    private void PopulateGroupNodeColors()
    {
        groupNodeColors.Images.Clear();
        for (int i = 0; i < gparam.Groups.Count; ++i)
        {
            Random randomColorGenerator = new();
            Bitmap colorBitmap = new(16, 16);
            int r = randomColorGenerator.Next(256);
            int g = randomColorGenerator.Next(256);
            int b = randomColorGenerator.Next(256);
            Color rgb = Color.FromArgb(r, g, b);
            for (int y = 0; y < colorBitmap.Height; ++y)
            for (int x = 0; x < colorBitmap.Width; ++x)
                colorBitmap.SetPixel(x, y, rgb);
            groupNodeColors.Images.Add(colorBitmap);
        }
    }

    private static DialogResult ShowQuestionDialog(string str)
    {
        return MessageBox.Show(str, @"Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    private int ShowAddParamDialog()
    {
        Form? form = new();
        form.Text = @"Add New Param";
        form.Icon = Icon;
        form.Width = 500;
        form.Height = 500;
        form.MinimumSize = new Size(300, 300);
        form.StartPosition = FormStartPosition.CenterScreen;
        form.MaximizeBox = false;
        TreeView selectorBox = new();
        selectorBox.Width = 450;
        selectorBox.Height = 420;
        selectorBox.Location = new Point(selectorBox.Location.X + 15, selectorBox.Location.Y + 5);
        selectorBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
        Button cancelButton = new()
        {
            Text = @"Cancel",
            Size = new Size(65, 25),
            Location = new Point(selectorBox.Width - 105,
                selectorBox.Bottom + 5),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right
        };
        Button okButton = new()
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
        bool shouldRememberGroupsBoxState = groupsBox.SelectedNode != null;
        int groupNodeIndex = groupsBox.SelectedNode?.Parent?.Parent?.Index ?? 0;
        int mapAreaIdNodeIndex = groupsBox.SelectedNode?.Parent?.Index ?? 0;
        int timeNodeIndex = groupsBox.SelectedNode?.Index ?? 0;
        groupsBox.SaveTreeState();
        groupsBox.Nodes.Clear();
        paramsBox.Nodes.Clear();
        propertiesPanel.Controls.Clear();
        foreach (GPARAM.Group group in gparam.Groups)
        {
            int imageIndex = gparam.Groups.IndexOf(group);
            string? groupComment = commentsJson[group.Name2]?.ToString();
            string groupDispName = !string.IsNullOrEmpty(groupComment) ? $"{group.Name2} - {groupComment}" : group.Name2;
            TreeNode groupNode = new() { Name = group.Name2, Text = groupDispName, ImageIndex = imageIndex, SelectedImageIndex = imageIndex };
            if (group.Params.Count > 0)
            {
                List<int> ids = new();
                List<float> times = new();
                foreach (GPARAM.Param param in group.Params)
                {
                    ids.AddRange(param.ValueIDs);
                    times.AddRange(param.TimeOfDay);
                    if (allParamsList.All(i => i.Name2 != param.Name2)) allParamsList.Add(param);
                }
                ids = ids.Distinct().ToList();
                times = times.Distinct().ToList();
                times.Sort();
                foreach (TreeNode? idNode in ids.Select(id => new TreeNode
                         {
                             Text = $@"• ID: {id}",
                             Name = id.ToString(),
                             ImageIndex = groupNodeColors.Images.Count,
                             SelectedImageIndex = groupNodeColors.Images.Count
                         }))
                {
                    const float baseTime = 0;
                    foreach (TreeNode timeNode in from time in times
                             let newTime = new DateTime(TimeSpan.FromHours(baseTime + time).Ticks)
                             select new TreeNode
                             {
                                 Text = $@"• {newTime.ToString("h:mm tt", CultureInfo.InvariantCulture)}",
                                 Name = time.ToString(CultureInfo.InvariantCulture),
                                 ImageIndex = groupNodeColors.Images.Count,
                                 SelectedImageIndex = groupNodeColors.Images.Count
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
        if (shouldRememberGroupsBoxState)
            groupsBox.SelectedNode = groupsBox.Nodes[groupNodeIndex].Nodes[mapAreaIdNodeIndex].Nodes[timeNodeIndex];
    }

    private void OpenGPARAMFile()
    {
        OpenFileDialog dialog = new() { Filter = @"GPARAM File (*.gparam.dcx, *.gparam)|*.gparam.dcx;*.gparam", Multiselect = false };
        if (dialog.ShowDialog() != DialogResult.OK) return;
        gparamFileName = dialog.FileName;
        byte[] fileData = File.ReadAllBytes(gparamFileName);
        if (Path.GetExtension(gparamFileName) == ".dcx") fileData = DCX.Decompress(fileData);
        fileData = fileData.Take(64).Concat(new byte[4]).Concat(fileData.Skip(68)).ToArray();
        gparam = GPARAM.Read(fileData);
        isGParamFileOpen = true;
        applyMassEditCheckbox.Enabled = true;
        affectAllMapAreasCheckbox.Enabled = true;
        saveToolStripMenuItem.Visible = true;
        saveAsToolStripMenuItem.Visible = true;
        Program.window.Text = $@"{Program.windowTitle} - {Path.GetFileName(gparamFileName)}";
        groupsParamsContainer.Enabled = true;
        paramsBox.LabelEdit = true;
        PopulateGroupNodeColors();
        ReadCommentsJson();
        LoadParams();
    }

    private void OpenToolStripMenuItemClick(object sender, EventArgs e)
    {
        OpenGPARAMFile();
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
        bool isTimeNodeSelected = false;
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
                        for (int j = 0; j < param.ValueIDs.Count; ++j)
                        {
                            int valueID = param.ValueIDs[j];
                            if (param.TimeOfDay == null) continue;
                            if (valueID != wantedValueID || !param.TimeOfDay[j].Equals(wantedTimeValue)) continue;
                            int shortNameIndex = param.Name1.IndexOf(param.Name2, StringComparison.Ordinal);
                            string paramName = shortNameIndex != -1 ? param.Name1[shortNameIndex..] : param.Name2;
                            string? paramComment = commentsJson[param.Name1]?.ToString();
                            string paramDispName = $"{(!string.IsNullOrEmpty(paramComment) ? $"{paramName} - {paramComment}" : paramName)} ({param.Name1})";
                            TreeNode paramNode = new() { Name = param.Name1, Text = paramDispName };
                            TreeNode valueNode = new() { Text = param.Values[j].ToString() };
                            paramValueInfoList.Add(new[] { groupNode.Index, group.Params.IndexOf(param), j });
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
            TreeNode? matchingParamNode = paramsBox.Nodes.Find(prevSelectedNode.Name, false).FirstOrDefault();
            if (matchingParamNode != null) paramsBox.SelectedNode = paramsBox.Nodes[prevSelectedNode.Name].Nodes[0];
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
            for (int i = 0; i < gparam.Groups[valueInfo[0]].Params[valueInfo[1]].Values.Count; i++)
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

    private static void SaveGPARAMFileAs()
    {
        SaveFileDialog dialog = new()
            { Filter = @"GPARAM File (*.gparam)|*.gparam|BND File (*.gparam.dcx)|*.gparam.dcx", FileName = Path.GetFileNameWithoutExtension(gparamFileName) };
        if (dialog.ShowDialog() != DialogResult.OK) return;
        SaveGPARAMFile(dialog.FileName);
    }

    private void SaveToolStripMenuItemClick(object sender, EventArgs e)
    {
        SaveGPARAMFile(gparamFileName);
    }

    private void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
    {
        SaveGPARAMFileAs();
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
        NumericUpDown numBox = new()
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

    private void ActivateAngleControl(TreeNode node, IReadOnlyList<int> valueInfo, IReadOnlyList<float> values)
    {
        int angleValue = ToDegrees(values[0]);
        int altitudeValue = ToDegrees(values[1]);
        AngleAltitudeSelector angleSelector = new()
        {
            Dock = DockStyle.Fill,
            Angle = angleValue,
            Altitude = altitudeValue
        };
        NumericUpDown angleBox = CreateNumBoxWithSetValue(angleValue, 1);
        NumericUpDown altitudeBox = CreateNumBoxWithSetValue(altitudeValue, 1);

        void AngleSelectorValueChanged()
        {
            Vector2 angle = new(ToRadians(angleSelector.Angle), ToRadians(angleSelector.Altitude));
            angleBox.Value = angleSelector.Angle;
            altitudeBox.Value = angleSelector.Altitude;
            UpdateParamValueInfo(valueInfo, angle);
            node.Text = angle.ToString();
        }

        angleSelector.AngleChanged += AngleSelectorValueChanged;
        angleSelector.AltitudeChanged += AngleSelectorValueChanged;
        angleBox.ValueChanged += (_, _) => { angleSelector.Angle = (int)angleBox.Value; };
        altitudeBox.ValueChanged += (_, _) => { angleSelector.Altitude = (int)altitudeBox.Value; };
        FlowLayoutPanel numBoxesContainer = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };
        numBoxesContainer.Controls.Add(angleBox);
        numBoxesContainer.Controls.Add(altitudeBox);
        SplitContainer angleControlContainer = new() { Dock = DockStyle.Fill };
        angleControlContainer.Panel1.Controls.Add(angleSelector);
        angleControlContainer.Panel2.Controls.Add(numBoxesContainer);
        propertiesPanel.Controls.Add(angleControlContainer);
    }

    private void ActivateColorWheelControl(TreeNode node, IReadOnlyList<int> valueInfo, IReadOnlyList<float> values)
    {
        ColorWheel wheel = new()
        {
            Dock = DockStyle.Fill,
            Color = Color.FromArgb(
                255, (int)(values[0] * 255), (int)(values[1] * 255), (int)(values[2] * 255))
        };
        ColorEditor editor = new() { Dock = DockStyle.Fill, Color = wheel.Color };
        wheel.ColorChanged += (_, _) =>
        {
            Color color = wheel.Color;
            Vector4 colorObj = new((float)(color.R / 255.0), (float)(color.G / 255.0), (float)(color.B / 255.0), values[3]);
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
        SplitContainer colorControlContainer = new() { Dock = DockStyle.Fill };
        colorControlContainer.Panel1.Controls.Add(wheel);
        colorControlContainer.Panel2.Controls.Add(editor);
        propertiesPanel.Controls.Add(colorControlContainer);
    }

    private void ActivateCheckboxControl(TreeNode node, IReadOnlyList<int> valueInfo, GPARAM.Param param, dynamic value)
    {
        CheckBox checkbox = new() { Size = propertiesPanel.Size with { Height = 30 }, Text = param.Name2, Checked = bool.Parse(value) };
        checkbox.CheckStateChanged += (_, _) =>
        {
            UpdateParamValueInfo(valueInfo, checkbox.Checked);
            node.Text = checkbox.Checked.ToString();
        };
        propertiesPanel.Controls.Add(checkbox);
    }

    private void ActivateNumberBoxControl(TreeNode node, IReadOnlyList<int> valueInfo, GPARAM.Param param, dynamic value)
    {
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
                ActivateAngleControl(node, valueInfo, values);
                break;
            case GPARAM.ParamType.Float4:
                ActivateColorWheelControl(node, valueInfo, values);
                break;
            case GPARAM.ParamType.BoolA:
            case GPARAM.ParamType.BoolB:
                ActivateCheckboxControl(node, valueInfo, param, value);
                break;
            case GPARAM.ParamType.Byte:
            case GPARAM.ParamType.Short:
            case GPARAM.ParamType.IntA:
            case GPARAM.ParamType.IntB:
            case GPARAM.ParamType.Float:
                ActivateNumberBoxControl(node, valueInfo, param, value);
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

    private static string? ShowInputDialog(string text, string caption)
    {
        Form prompt = new()
        {
            Width = 240,
            Height = 125,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = caption,
            StartPosition = FormStartPosition.CenterScreen,
            MaximizeBox = false
        };
        Label textLabel = new() { Left = 8, Top = 8, Width = 200, Text = text };
        TextBox textBox = new() { Left = 10, Top = 28, Width = 200 };
        Button cancelButton = new() { Text = @"Cancel", Left = 9, Width = 100, Top = 55, DialogResult = DialogResult.Cancel };
        cancelButton.Click += (_, _) => { prompt.Close(); };
        Button confirmation = new() { Text = @"OK", Left = 112, Width = 100, Top = 55, DialogResult = DialogResult.OK };
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

    private static void ReadCommentsJson()
    {
        try
        {
            commentsJson = JObject.Parse(File.ReadAllText(commentsJsonFilePath));
        }
        catch
        {
            WriteCommentsJson();
        }
    }

    private static void WriteCommentsJson()
    {
        File.WriteAllText(commentsJsonFilePath, JsonConvert.SerializeObject(commentsJson, Formatting.Indented));
    }

    private void AddOverrideComment(string nameKey, string comment)
    {
        commentsJson[nameKey] = comment;
        WriteCommentsJson();
        LoadParams();
    }

    private void OnAssignComment(string nameKey)
    {
        string? comment = ShowCommentDialog();
        if (comment == null) return;
        AddOverrideComment(nameKey, comment);
    }

    private void OnClearComment(string nameKey)
    {
        AddOverrideComment(nameKey, "");
    }

    private void OnMapAreaIdNodeAddTimeOfDay(TreeNode mapAreaIdNode)
    {
        string? timeOfDayStr = ShowInputDialog("Enter a time of day:", "Add Time");
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
        List<float> maxTimeOfDay = new();
        List<int> maxValueIDs = new();
        foreach (GPARAM.Param param in group.Params)
        {
            if (param.TimeOfDay.Count > maxTimeOfDay.Count) maxTimeOfDay = param.TimeOfDay;
            if (param.ValueIDs.Count > maxValueIDs.Count) maxValueIDs = param.ValueIDs;
        }
        newParam.TimeOfDay = maxTimeOfDay;
        newParam.ValueIDs = maxValueIDs;
        for (int i = 0; i < newParam.ValueIDs.Count; ++i)
            if (i > newParam.Values.Count - 1)
                newParam.Values.Add(newParam.Values[0]);
        GPARAM.Param? existingParam = group.Params.FirstOrDefault(i => i.Name2 == newParam.Name2);
        if (existingParam != null) gparam.Groups[valueInfo[0]].Params[group.Params.IndexOf(existingParam)] = newParam;
        else gparam.Groups[valueInfo[0]].Params.Add(newParam);
        LoadParams();
    }

    private static string? ShowCommentDialog()
    {
        string? comment = ShowInputDialog("Enter a comment:", "Assign Comment");
        return string.IsNullOrEmpty(comment) ? null : comment;
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

    private static int GetSelectedOptionIndex(ToolStrip contextMenu, ToolStripItemClickedEventArgs args)
    {
        return contextMenu.Items.IndexOf(args.ClickedItem);
    }

    private void GroupsBoxRightClick(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        TreeNode? treeNode = GetHoveredNodeOnRightClick(groupsBox, e);
        switch (treeNode)
        {
            case { Level: 0 }:
                ShowRightClickMenu(groupNodeRightClickMenu, groupsBox, (_, args) =>
                {
                    switch (GetSelectedOptionIndex(groupNodeRightClickMenu, args))
                    {
                        case 0:
                            OnAssignComment(treeNode.Name);
                            break;
                        case 1:
                            OnClearComment(treeNode.Name);
                            break;
                    }
                }, e);
                break;
            case { Level: 1 }:
                ShowRightClickMenu(mapAreaIdNodeRightClickMenu, groupsBox, (_, _) => OnMapAreaIdNodeAddTimeOfDay(treeNode), e);
                break;
        }
    }

    private void ParamsBox_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right || paramsBox.Nodes.Cast<TreeNode>().All(i => string.IsNullOrEmpty(i.Name))) return;
        TreeNode? paramNode = GetHoveredNodeOnRightClick(paramsBox, e);
        if (paramNode is { Level: 0 })
        {
            ShowRightClickMenu(paramNodeRightClickMenu, paramsBox, (_, args) =>
            {
                switch (GetSelectedOptionIndex(paramNodeRightClickMenu, args))
                {
                    case 0:
                        OnParamNodeDeleteParam(paramNode);
                        break;
                    case 1:
                        OnAssignComment(paramNode.Name);
                        break;
                    case 2:
                        OnClearComment(paramNode.Name);
                        break;
                }
            }, e);
        }
        else if (paramNode is not { Level: 1 })
            ShowRightClickMenu(paramsBoxAddParamRightClickMenu, paramsBox, (_, _) => OnParamsBoxAddNewParam(), e);
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

    private void GParamStudio_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Control)
        {
            case true when e.KeyCode == Keys.O:
                e.SuppressKeyPress = true;
                OpenGPARAMFile();
                break;
            case true when !e.Shift && e.KeyCode == Keys.S:
                e.SuppressKeyPress = true;
                SaveGPARAMFile(gparamFileName);
                break;
            case true when e.Shift && e.KeyCode == Keys.S:
                e.SuppressKeyPress = true;
                SaveGPARAMFileAs();
                break;
        }
    }
}
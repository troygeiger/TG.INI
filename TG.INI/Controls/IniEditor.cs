using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TG.INI.Encryption;

namespace TG.INI.Controls
{
    /// <summary>
    /// A visual editor for INI files.
    /// </summary>
    public partial class IniEditor : Form
    {
        #region Fields

        private bool _dirty = false;

        private DisplayModes _displayMode = DisplayModes.Dialog;

        private string _documentPath = null;

        private string _title = "Ini Editor";

        private bool canEditKeys = false;

        private bool canEditValues = false;

        private bool cellChanged;

        private IEncryptionHandler encryptionHandler = null;

        private IniDocument ini = null;

        private bool isClearingKeyValue = false;

        private string lastValueAdded = null;

        private bool loading = false;

        private IniDocument original = null;

        private EditorPrivileges privileges = EditorPrivileges.All;

        private IniEntry selectedEntry = null;

        private IniSection selectedSection = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance of IniEditor.
        /// </summary>
        public IniEditor()
        {
            InitializeComponent();
            ApplyPrivileges();
        }

        /// <summary>
        /// Initializes a new IniEditor with a predefined <see cref="IEncryptionHandler"/>.
        /// </summary>
        /// <param name="encryption"></param>
        public IniEditor(IEncryptionHandler encryption) : this()
        {
            encryptionHandler = encryption;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// This event is invoked when the <see cref="Dirty"/> property is changed.
        /// </summary>
        public event EventHandler DirtyChanged;

        /// <summary>
        /// This event is invoked when the <see cref="DisplayMode"/> property is changed.
        /// </summary>
        public event EventHandler DisplayModeChanged;

        #endregion Events

        #region Enums

        /// <summary>
        /// Represents modes of the GUI.
        /// </summary>
        public enum DisplayModes
        {
            /// <summary>
            /// This will cause a top tool bar to be shown that includes a New, Open and Save button.
            /// </summary>
            Standalone,

            /// <summary>
            /// This will hide the top tool bar and show an Ok and Cancel button at the bottom of the window.
            /// </summary>
            Dialog
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// Gets whether an EncryptionKey property is set and the length is greater than zero.
        /// </summary>
        public bool CanEncrypt
        {
            get
            {
                return ini?.HasEncryptionHandler == true;
            }
        }

        /// <summary>
        /// Gets or sets if the document has changed.
        /// </summary>
        public bool Dirty
        {
            get
            {
                return _dirty;
            }
            set
            {
                _dirty = value;
                OnDirtyChanged();
            }
        }

        /// <summary>
        /// Gets or sets how the editor window is setup.
        /// </summary>
        /// <remarks>
        /// There are two options available for this property; Standalone and Dialog.
        /// When the Standalone option is selected, a tool bar is shown with the buttons New, Open and Save.
        /// When the Dialog option is shown, the tool bar is hidden and OK and Cancel button is shown at the bottom of the window.
        /// </remarks>
        public DisplayModes DisplayMode
        {
            get
            {
                return _displayMode;
            }
            set
            {
                _displayMode = value;
                OnDisplayModeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IniDocument"/> that is being viewed.
        /// </summary>
        public IniDocument Document
        {
            get
            {
                return ini;
            }
            set
            {
                LoadDocument(value);
            }
        }

        /// <summary>
        /// Gets or sets the file path to a INI file. This is only used in Standalone mode.
        /// </summary>
        public string DocumentPath
        {
            get { return _documentPath; }
            set
            {
                _documentPath = value;
                UpdateTitle();
            }
        }

        /// <summary>
        /// Get or set the <see cref="IEncryptionHandler"/> to use. This is mostly used while in <see cref="DisplayModes.Standalone"/> but can be used to change the handler for an existing <see cref="IniDocument"/>.
        /// </summary>
        public IEncryptionHandler EncryptionHandler
        {
            get
            {
                return encryptionHandler;
            }
            set
            {
                encryptionHandler = value;
                if (ini != null)
                {
                    ini.EncryptionHandler = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the user privileges of the editor.
        /// </summary>
        public EditorPrivileges Privileges
        {
            get
            {
                return privileges;
            }
            set
            {
                privileges = value;
                ApplyPrivileges();
            }
        }

        /// <summary>
        /// Gets or sets the Text title property.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                UpdateTitle();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads an <see cref="IniDocument"/>.
        /// </summary>
        /// <param name="iniDocument"></param>
        public void LoadDocument(IniDocument iniDocument)
        {
            ini = null;
            _dirty = false;
            DocumentPath = null;
            btnAdd.Enabled = iniDocument != null;
            if (iniDocument == null)
                return;
            loading = true;
            ini = iniDocument;
            txtCommentChar.Text = ini.CommentLineIndicator;
            txtCommentChar.TextChanged += TxtCommentChar_TextChanged;
            original = iniDocument.Clone();
            RefreshSections();
            loading = false;
        }

        /// <summary>
        /// This method is invoked when the <see cref="Dirty"/> property is modified.
        /// </summary>
        public virtual void OnDirtyChanged()
        {
            //btnSave.Enabled = Dirty;
            UpdateTitle();
            DirtyChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This method is invoked when the <see cref="DisplayMode"/> property is modified.
        /// </summary>
        public virtual void OnDisplayModeChanged()
        {
            barDialog.Visible = !(barStandalone.Visible =
                DisplayMode == DisplayModes.Standalone);
            DisplayModeChanged?.Invoke(this, new EventArgs());
        }

        private static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 18, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            form.Load += (s, e) =>
            {
                textBox.Select();
            };
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void ApplyPrivileges()
        {
            btnAdd.Visible =
                btnDelete.Visible =
                KeyValue.AllowUserToAddRows =
                KeyValue.AllowUserToDeleteRows =
                txtCommentChar.Enabled =
                canEditKeys = canEditValues = false;

            if (privileges == EditorPrivileges.All)
            {
                btnAdd.Visible =
                btnDelete.Visible =
                KeyValue.AllowUserToAddRows =
                KeyValue.AllowUserToDeleteRows =
                txtCommentChar.Enabled =
                canEditKeys = canEditValues = true;
            }
            else if (privileges == EditorPrivileges.ReadOnly)
            {
                // Do nothing
            }
            else
            {
                btnAdd.Visible = privileges.HasFlag(EditorPrivileges.AddSections);
                btnDelete.Visible = privileges.HasFlag(EditorPrivileges.RemoveSections);
                KeyValue.AllowUserToAddRows = privileges.HasFlag(EditorPrivileges.AddEntries);
                KeyValue.AllowUserToDeleteRows = privileges.HasFlag(EditorPrivileges.RemoveEntries);
                txtCommentChar.Enabled = canEditKeys = privileges.HasFlag(EditorPrivileges.ChangeKeys);
                canEditValues = privileges.HasFlag(EditorPrivileges.ChangeValues);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = "";
            if (InputBox("New Section", "Enter the name of the new section.", ref name) == DialogResult.OK)
            {
                if (ini.Sections.Contains(name))
                    return;
                ini.Sections.Add(name);
                lstSections.BeginUpdate();
                lstSections.Items.Add(name);
                lstSections.EndUpdate();
                Dirty = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetIni();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!ini.Sections.Contains(lstSections.SelectedItems[0].Text))
                return;
            if (MessageBox.Show("Are you sure you want to delete the selected section?"
                , "Deletion Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ini.Sections.Remove(lstSections.SelectedItems[0].Text);
                lstSections.SelectedItems[0].Remove();
                Dirty = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (CheckDirty())
                Document = new IniDocument(EncryptionHandler);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog() { Filter = "*.ini|*.ini" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    OpenDocument(dlg.FileName);
            }
        }

        private void btnSave_ButtonClick(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private bool CheckDirty()
        {
            if (Dirty)
            {
                var result = MessageBox.Show("The current document has changed. Would you like to save?", "Document Changed", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                    return SaveDocument();
                else if (result == DialogResult.No)
                    return true;
                else
                    return false;
            }
            return true;
        }

        private void encryptValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedEntry != null && CanEncrypt)
            {
                if (selectedEntry.EntryType == EntryTypes.KeyValue)
                {
                    var kv = selectedEntry as IniKeyValue;
                    kv.EncryptValue = !kv.EncryptValue;
                }
                KeyValue.Invalidate();
            }
        }

        private void IniEditor_Load(object sender, EventArgs e)
        {
            if (DisplayMode == DisplayModes.Standalone)
            {
                string[] args = Environment.GetCommandLineArgs();
                if (args.Length == 2)
                    OpenDocument(args[1]);
                else if (Document == null)
                    Document = new IniDocument();
            }
        }

        private bool IsNullOrWhitespace(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;
            for (int i = 0; i < value.Length; i++)
                if (value[i] != ' ')
                    return false;
            return true;
        }

        private void KeyValue_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (selectedEntry == null)
                return;
            if (!canEditKeys && e.ColumnIndex == 0)
                e.Cancel = true;
            else if (!canEditValues && e.ColumnIndex == 1)
                e.Cancel = true;
            else
                e.Cancel = selectedEntry.EntryType == EntryTypes.WhiteSpace
                                || (selectedEntry.EntryType == EntryTypes.Comment && e.ColumnIndex == 0);
        }

        private void KeyValue_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            var cell = KeyValue.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (!cell.Selected)
            {
                KeyValue.ClearSelection();
                cell.Selected = true;
            }
            if (selectedEntry == null)
                return;
            if (e.ColumnIndex == 0)
                return;
            if (selectedEntry.EntryType != EntryTypes.KeyValue || !canEditValues)
                return;
            var kv = selectedEntry as IniKeyValue;
            quoteValueToolStripMenuItem.Checked = kv.QuoteValue;
            encryptValueToolStripMenuItem.Checked = kv.EncryptValue;
            encryptValueToolStripMenuItem.Enabled = CanEncrypt;
            e.ContextMenuStrip = KeyValueMenu;
        }

        private void KeyValue_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = KeyValue.Rows[e.RowIndex];
            if (!cellChanged)
            {
                if (selectedEntry == null && !row.IsNewRow)
                    KeyValue.Rows.Remove(row);
                return;
            }
            cellChanged = false;

            if (selectedEntry == null)
            {
                if (e.ColumnIndex == 1)
                {
                    MessageBox.Show("You must assign the Key field first.", "Creating Failed");
                    if (!row.IsNewRow)
                        KeyValue.Rows.Remove(row);
                    lastValueAdded = null;
                    return;
                }

                if (IsNullOrWhitespace(lastValueAdded))
                {
                    if (row.IsNewRow)
                    {
                        row = KeyValue.Rows[KeyValue.Rows.Add()];
                        KeyValue.ClearSelection();
                        row.Cells[0].Selected = true;
                    }

                    row.Tag = selectedEntry = new IniWhiteSpace();
                    var cell = row.Cells[0];
                    cell.Style.Font = new Font(KeyValue.DefaultCellStyle.Font, FontStyle.Italic);
                    cell.Style.ForeColor = Color.Gray;
                    Dirty = true;
                }
                else if (lastValueAdded.StartsWith(ini.CommentLineIndicator))
                {
                    row.Tag = selectedEntry = new IniComment(lastValueAdded.Substring(ini.CommentLineIndicator.Length));
                    KeyValue.Invalidate();
                    Dirty = true;
                }
                else
                {
                    row.Tag = selectedEntry = new IniKeyValue();
                    Dirty = true;
                }

                selectedSection.Add(selectedEntry);
            }
            switch (e.ColumnIndex)
            {
                case 0:
                    if (selectedEntry.EntryType == EntryTypes.KeyValue)
                    {
                        (selectedEntry as IniKeyValue).Key = lastValueAdded;// row.Cells[0].Value as string;
                        Dirty = true;
                    }
                    break;

                case 1:
                    switch (selectedEntry.EntryType)
                    {
                        case EntryTypes.WhiteSpace:
                            break;

                        case EntryTypes.Comment:
                            selectedEntry.Value = lastValueAdded;
                            Dirty = true;
                            break;

                        case EntryTypes.KeyValue:
                            var kv = selectedEntry as IniKeyValue;
                            kv.Value = lastValueAdded;// row.Cells[1].Value as string;
                            Dirty = true;
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }
            lastValueAdded = null;
        }

        private void KeyValue_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.SetClip(e.CellBounds);
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                var row = KeyValue.Rows[e.RowIndex];
                IniEntry ie = row.Tag as IniEntry;
                if (ie != null)
                {
                    bool selected = row.Cells[e.ColumnIndex].Selected;

                    if (ie.EntryType == EntryTypes.KeyValue)
                    {
                        var kv = ie as IniKeyValue;

                        if (kv.EncryptValue || kv.QuoteValue)
                        {
                            e.PaintBackground(e.ClipBounds, selected);
                            if (kv.QuoteValue)
                            {
                                var txtSize = e.Graphics.MeasureString(kv.Value, e.CellStyle.Font);
                                e.Graphics.DrawString($"\"{kv.Value}\"", e.CellStyle.Font,
                                    selected ? Brushes.White : Brushes.Black, e.CellBounds.Left + 3,
                                    e.CellBounds.Top + ((e.CellBounds.Height / 2) - (txtSize.Height / 2)));
                            }
                            else
                                e.PaintContent(e.ClipBounds);

                            if (kv.EncryptValue)
                            {
                                e.Graphics.DrawImage(Properties.Resources.Lock16, e.CellBounds.Right - 19,
                                e.CellBounds.Top + ((e.CellBounds.Height / 2) - 8));
                            }

                            e.Handled = true;
                        }
                    }
                    else if (ie.EntryType == EntryTypes.Comment)
                    {
                        e.PaintBackground(e.ClipBounds, selected);
                        var txtSize = e.Graphics.MeasureString(ie.Value, e.CellStyle.Font);
                        e.Graphics.DrawString(ie.Value, e.CellStyle.Font,
                            selected ? Brushes.White : Brushes.DarkGreen, e.CellBounds.Left + 3,
                            e.CellBounds.Top + ((e.CellBounds.Height / 2) - (txtSize.Height / 2)));
                        e.Handled = true;
                    }
                }
            }
        }

        private void KeyValue_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!loading && e.RowIndex > -1 && e.ColumnIndex > -1)
                Dirty = cellChanged = true;
        }

        private void KeyValue_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            var row = KeyValue.Rows[e.RowIndex];
            IniEntry ie = row.Tag as IniEntry;
            if (ie != null)
            {
                switch (e.ColumnIndex)
                {
                    case 0:

                        if (ie.EntryType == EntryTypes.Comment)
                            e.Value = ie.ParentDocument.CommentLineIndicator;
                        else if (ie.EntryType == EntryTypes.KeyValue)
                            e.Value = (ie as IniKeyValue).Key;
                        else if (ie.EntryType == EntryTypes.WhiteSpace)
                            e.Value = "Whitespace";
                        break;

                    case 1:
                        if (ie.EntryType == EntryTypes.Comment)
                            e.Value = ie.Value;
                        else if (ie.EntryType == EntryTypes.KeyValue)
                        {
                            var kv = ie as IniKeyValue;
                            e.Value = kv.Value;
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        private void KeyValue_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            lastValueAdded = e.Value as string;
        }

        private void KeyValue_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (isClearingKeyValue || selectedSection == null)
                return;
            for (int r = 0; r < e.RowCount; r++)
            {
                if (e.RowIndex < selectedSection.Count)
                    selectedSection.RemoveAt(e.RowIndex);
            }
            Dirty = true;
        }

        private void KeyValue_SelectionChanged(object sender, EventArgs e)
        {
            selectedEntry = null;
            if (KeyValue.SelectedCells.Count == 1)
                selectedEntry = KeyValue.SelectedCells[0].OwningRow.Tag as IniEntry;
            else if (KeyValue.SelectedRows.Count == 1)
                selectedEntry = KeyValue.SelectedRows[0].Tag as IniEntry;
        }

        private void lstSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = lstSections.SelectedItems.Count > 0 && lstSections.SelectedItems[0].Text != "<Global>";
            RefreshKeyValues();
        }

        /// <summary>
        /// Opens a INI document from file.
        /// </summary>
        /// <param name="path">The file path do the document.</param>
        public void OpenDocument(string path)
        {
            try
            {
                Document = new IniDocument(path, EncryptionHandler);
                DocumentPath = path;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load INI file. " + ex.ToString());
            }
        }

        private void quoteValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedEntry != null && selectedEntry.EntryType == EntryTypes.KeyValue)
            {
                var kv = selectedEntry as IniKeyValue;
                kv.QuoteValue = !kv.QuoteValue;
                KeyValue.Invalidate();
            }
        }

        private void RefreshKeyValues()
        {
            isClearingKeyValue = true;
            KeyValue.Rows.Clear();
            isClearingKeyValue = false;
            selectedSection = null;
            if (lstSections.SelectedItems.Count == 0)
                return;

            string secText = lstSections.SelectedItems[0].Text;
            if (secText == "<Global>")
                selectedSection = ini.GlobalSection;
            else
                selectedSection = ini[secText];

            foreach (IniEntry item in selectedSection)
            {
                var row = KeyValue.Rows[KeyValue.Rows.Add()];
                if (item.EntryType == EntryTypes.Comment)
                {

                }
                else if (item.EntryType == EntryTypes.WhiteSpace)
                {
                    var cell = row.Cells[0];
                    cell.Style.Font = new Font(KeyValue.DefaultCellStyle.Font, FontStyle.Italic);
                    cell.Style.ForeColor = Color.Gray;
                }
                row.Tag = item;
            }
            lastValueAdded = null;
            if (KeyValue.SelectedCells.Count == 1)
                selectedEntry = KeyValue.SelectedCells[0].OwningRow.Tag as IniEntry;
            cellChanged = false;
        }

        private void RefreshSections()
        {
            lstSections.Items.Clear();
            if (ini == null)
                return;

            lstSections.BeginUpdate();
            lstSections.Items.Add("<Global>");
            for (int i = 0; i < ini.Sections.Count; i++)
                lstSections.Items.Add(ini.Sections[i].Name);

            lstSections.EndUpdate();

            if (lstSections.Items.Count > 0)
                lstSections.Items[0].Selected = true;
        }

        private void ResetIni()
        {
            ini.Sections.Clear();
            ini.GlobalSection.Clear();
            for (int i = 0; i < original.GlobalSection.Count; i++)
                ini.GlobalSection.Add(original.GlobalSection[i]);
            for (int i = 0; i < original.Sections.Count; i++)
                ini.Sections.Add(original.Sections[i]);
            original = null;
        }

        private bool SaveAsDocument()
        {
            try
            {
                using (var dlg = new SaveFileDialog() { Filter = "*.ini|*.ini" })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        DocumentPath = dlg.FileName;
                        Document.Write(DocumentPath);
                        Dirty = false;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving. " + ex.ToString());
            }
            return false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsDocument();
        }

        private bool SaveDocument()
        {
            try
            {
                if (string.IsNullOrEmpty(DocumentPath))
                    return SaveAsDocument();
                else
                {
                    Document.Write(DocumentPath);
                    Dirty = false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving. " + ex.ToString());
            }
            return false;
        }

        private void TxtCommentChar_TextChanged(object sender, EventArgs e)
        {
            ini.CommentLineIndicator = txtCommentChar.Text;
            KeyValue.Invalidate();
        }

        private void UpdateTitle()
        {
            if (string.IsNullOrEmpty(DocumentPath))
                this.Text = Title;
            else
                this.Text = $"{(Dirty ? "*" : "")}{System.IO.Path.GetFileName(DocumentPath)} - {Title}";
        }

        #endregion Methods

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using WD.XLC.Domain.Entities;
using WD.XLC.Domain.Helpers;



// namespace: WD.XLC.WIN.Controls
//
// summary:	.


namespace WD.XLC.WIN.Controls
{

    /// <summary>   A mapping control. </summary>
    ///
    /// <remarks>   Shahid K, 7/21/2017. </remarks>


    public partial class MappingControl : MetroUserControl
    {
        
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="max">  The maximum. </param>
        public MappingControl(int max)
        {
            this.max = max;
            InitializeComponent();
            this.treeImageList.Images.Add(global::WD.XLC.WIN.Properties.Resources.folderclose);
            this.treeImageList.Images.Add(global::WD.XLC.WIN.Properties.Resources.folderopen);
            this.Dock = DockStyle.Fill;
            

        }
        /// <summary>   Loads a tree. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="templates">    The templates. </param>
        public void LoadTree(List<AppConfig> templates)
        {
            treeView1.Nodes.Clear();
            treeView1.ImageList = null;
            this.treeView1.ImageList = treeImageList;
            this.treeView1.ImageIndex = 0;
            this.treeView1.SelectedImageIndex = 1;
             this.treeView1.Nodes.AddRange((from n in templates
                                select new TreeNode
                                {
                                    Name = n.Id,
                                    Text = n.RecId,
                                    ImageIndex = 0,
                                    SelectedImageIndex = 1,
                                }).ToArray());
             if (this.treeView1.Nodes.Count > 0)
                 this.treeView1.SelectedNode = this.treeView1.Nodes[0];
        }
        internal AppConfig input { get; set; }
        internal void LoadGrid()
        {
               dataGridView1.Rows.Clear();
               btnUp.Visible = btnDown.Visible = false;
               BindColumn();
               List<ColumnConfig> cols = (from c in Utility.ConvertJsonToObject<Mapping>(input.Config).Columns
                                          select c).ToList();
               foreach (var r in cols)
               {
                   DataGridViewRow row = new DataGridViewRow();
                   row.CreateCells(dataGridView1, new object[] { r.Index, r.ColumnName, r.FieldName, r.DataType.ToLower(), r.Format, r.Length, r.DefaultValue, r.IsPrimary });
                   dataGridView1.Rows.Add(row);

               }
               if (dataGridView1.Rows.Count == 0)
               {
                   ColumnConfig r = new ColumnConfig()
                   {
                       Index = 1,
                       ColumnName = "F1",
                       DataType = "string",
                       DefaultValue = string.Empty,
                       FieldName = "RecId",
                       Format = string.Empty,
                       IsPrimary = false,
                       Length = "0"
                   };
                   DataGridViewRow row = new DataGridViewRow();
                   row.CreateCells(dataGridView1, new object[] { r.Index, r.ColumnName, r.FieldName, r.DataType.ToLower(), r.Format, r.Length, r.DefaultValue, r.IsPrimary });
                   dataGridView1.Rows.Add(row);
                   r = null;
               }
               dataGridView1.Rows[0].Cells[1].ReadOnly = true;
               dataGridView1.Rows[0].Cells[2].ReadOnly = true;
               dataGridView1.Rows[0].Cells[3].ReadOnly = true;
               dataGridView1.Rows[0].Cells[4].ReadOnly = true;
               dataGridView1.Rows[0].Cells[5].ReadOnly = true;
               dataGridView1.Rows[0].Cells[6].ReadOnly = true;
               dataGridView1.Rows[0].Frozen = true;
               dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
               cols = null;
        }
        private void BindColumn()
        {
            if (this.dataGridView1.Columns[2].DataPropertyName == "FieldName")
            {
                this.dataGridView1.Columns.RemoveAt(2);
            }
            DataGridViewComboBoxColumn colType = new DataGridViewComboBoxColumn();
            colType.HeaderText = "Table Column";
            colType.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            colType.FlatStyle = FlatStyle.Flat;
            colType.DropDownWidth = 200;
            colType.Width = 200;
            colType.Items.Clear();
            colType.ValueMember = "ColumnName";
            colType.DisplayMember = "ColumnName";
            try
            {
                using (IDbConnection con = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(input.DbProvider))
                {

                    con.ConnectionString = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(input.ConnString, true);
                    using (IDbCommand com = con.CreateCommand())
                    {
                        con.Open();
                        com.CommandText = String.Format("select * from {0}", input.TargetTableName);
                        com.CommandType = CommandType.Text;
                        colType.DataSource = com.ExecuteReader(CommandBehavior.SchemaOnly).GetSchemaTable();
                    }
                }

            }
            catch
            {
                colType.Items.Add(string.Empty);
            }
            finally
            {

                colType.MaxDropDownItems = 100;
                if (this.dataGridView1.Columns[2].DataPropertyName != "FieldName")
                {
                    this.dataGridView1.Columns.Insert(2, colType);
                    this.dataGridView1.Columns[2].DataPropertyName = "FieldName";
                }
            }
        }
        /// <summary>   Event handler. Called by treeView1 for after select events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Tree view event information. </param>
        void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (AfterSelected != null)
            {
                input = AfterSelected(sender, e);
                LoadGrid();
            }
        }
        /// <summary>   Event handler. Called by btnUpdated for click events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        public Dictionary<string, int> StrToNDuplicates = null;
        private void btnUpdated_Click(object sender, EventArgs e)
        {
            if (SaveChanges != null)
            {
                    var input_rows = new List<ColumnConfig>();
                    int asc_count = 0;
                    int isF1 = 0;
                    string message = string.Empty;
                    StrToNDuplicates = new Dictionary<string, int>();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(row.Cells[2].Value.ToString())) {
                                MessageBox.Show("Table Column Cannot be null or empty");
                                dataGridView1.CurrentCell = row.Cells[1];
                                dataGridView1.CurrentCell = row.Cells[1];
                                row.Selected = true;
                                row.DefaultCellStyle.BackColor = Color.LightYellow;
                                isF1 = 0;
                                break;
                            }
                            if (!StrToNDuplicates.ContainsKey(row.Cells[1].Value.ToString().ToUpper()))
                            {
                                StrToNDuplicates.Add(row.Cells[1].Value.ToString().ToUpper(), row.Index);
                                if (!StrToNDuplicates.ContainsKey(row.Cells[2].Value.ToString().ToUpper()))
                                {
                                    StrToNDuplicates.Add(row.Cells[2].Value.ToString().ToUpper(), row.Index);
                                    ColumnConfig cc = new ColumnConfig()
                                    {
                                        Index = asc_count + 1,
                                        ColumnName = "F" + (asc_count + 1),//row.Cells[1].Value.ToString().ToUpper(),
                                        FieldName = row.Cells[2].Value.ToString(),
                                        DataType = row.Cells[3].Value.ToString(),
                                        Format = row.Cells[4].Value == null ? string.Empty : row.Cells[4].Value.ToString(),
                                        Length = row.Cells[5].Value == null ? "0" : row.Cells[5].Value.ToString(),
                                        DefaultValue = row.Cells[6].Value == null ? string.Empty : row.Cells[6].Value.ToString(),
                                        IsPrimary = row.Cells[7].Value == null ? false : Convert.ToBoolean(row.Cells[7].Value.ToString())
                                    };
                                    isF1 = (cc.ColumnName == ("F1")) ? (isF1 + 1) : isF1;
                                    if (isF1 > 1)
                                    {
                                        row.Selected = true;
                                        message = "Duplicate F1 column";
                                        row.Selected = true;
                                        break;
                                    }
                                    input_rows.Add(cc);
                                    row.Cells[1].Value = cc.ColumnName;
                                    row.Cells[0].Value = cc.Index;
                                    asc_count++;
                                    cc = null;
                                }
                                else
                                {
                                    isF1 = 0;
                                    message = "Duplicate " + row.Cells[2].Value.ToString() + " columns";
                                    dataGridView1.CurrentCell = row.Cells[2];
                                    dataGridView1.CurrentCell.Selected = true;
                                    row.Selected = true;
                                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                                    break;
                                }
                            }
                            else
                            {
                                isF1 = 0;
                                message = "Duplicate " + row.Cells[1].Value.ToString() + " columns";
                                dataGridView1.CurrentCell = row.Cells[1];
                                dataGridView1.CurrentCell.Selected = true;
                                row.Selected = true;
                                row.DefaultCellStyle.BackColor = Color.LightYellow;
                                break;
                            }
                        }
                        catch
                        {
                            isF1 = 0;
                            message = "Duplicate " + row.Cells[1].Value.ToString() + " columns";
                            dataGridView1.CurrentCell = row.Cells[1];
                            dataGridView1.CurrentCell = row.Cells[1];
                            row.Selected = true;
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                            break;
                        }
                    }
                    if (isF1 == 1)
                    {
                        SaveChanges(input_rows, treeView1.SelectedNode.Name);

                        dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1];
                        dataGridView1.CurrentCell.Selected = true;

                        dataGridView1.Rows[0].Selected = true;
                        dataGridView1.Rows[0].Cells[1].ReadOnly = true;
                        dataGridView1.Rows[0].Cells[2].ReadOnly = true;
                        dataGridView1.Rows[0].Cells[3].ReadOnly = true;
                        dataGridView1.Rows[0].Cells[4].ReadOnly = true;
                        dataGridView1.Rows[0].Cells[5].ReadOnly = true;
                        dataGridView1.Rows[0].Cells[6].ReadOnly = true;
                        dataGridView1.Rows[0].Frozen = true;
                        dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
                        ResetSort();
                    }
                    else
                        MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               
            }

        }
        /// <summary>   Event handler. Called by dataGridView1 for mouse click events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Mouse event information. </param>
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                btnDown.Visible = false;
                btnUp.Visible = false;
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Delete Row", btn_delete));
                m.MenuItems.Add(new MenuItem("Add Row Up", RowUp));
                m.MenuItems.Add(new MenuItem("Add Row Down", RowDown));
                m.Show(dataGridView1, new Point(e.X, e.Y));
            }
        }
        /// <summary>   Event handler. Called by btn for delete events. </summary>
        ///
        /// <remarks>   Shahid K, 7/21/2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>
        private void btn_delete(object sender, EventArgs e)
        {
            try
            {
                string value = dataGridView1.SelectedCells[0].OwningRow.Cells[2].Value == null ? string.Empty : dataGridView1.SelectedCells[0].OwningRow.Cells[2].Value.ToString();
                if (value.ToLower() != ("recid"))
                {
                    dataGridView1.Rows.Remove((dataGridView1.SelectedCells[0].OwningRow));
                    switch (sort)
                    {

                        case SortOrder.Ascending:
                        case SortOrder.None:
                            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                            {
                                dataGridView1.Rows[i].Cells[0].Value = i + 1;
                                dataGridView1.Rows[i].Cells[1].Value = "F" + (i + 1).ToString();

                            }
                            break;
                        case SortOrder.Descending:
                        default:
                            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                            {

                                dataGridView1.Rows[i].Cells[0].Value = dataGridView1.Rows.Count - i;
                                dataGridView1.Rows[i].Cells[1].Value = "F" + (dataGridView1.Rows.Count - i).ToString();

                            }
                            break;
                    }
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.SelectedCells[0].OwningRow.Index].Cells[0];
                    btnUp.Visible = btnDown.Visible = false;
                }
                else
                {
                    MessageBox.Show("Recid row cannot be deleted","Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {

            }
        }
        private void RowUp(object sender, EventArgs e)
        {

            try
            {
                string value = dataGridView1.SelectedCells[0].OwningRow.Cells[1].Value == null ? string.Empty : dataGridView1.SelectedCells[0].OwningRow.Cells[1].Value.ToString();
                if (value != ("F1"))
                {
                    using (DataGridViewRow row = new DataGridViewRow())
                    {
                        row.CreateCells(dataGridView1, new object[] { dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value, dataGridView1.SelectedCells[0].OwningRow.Cells[1].Value, string.Empty, "string", string.Empty, string.Empty, string.Empty, false });
                        dataGridView1.Rows.Insert(dataGridView1.SelectedCells[0].OwningRow.Index, row);
                    }
                    switch (sort)
                    {

                        case SortOrder.Ascending:
                        case SortOrder.None:
                            for (int i = 1; i < dataGridView1.Rows.Count; i++)
                            {
                                dataGridView1.Rows[i].Cells[0].Value = i + 1;
                                dataGridView1.Rows[i].Cells[1].Value = "F" + (i + 1).ToString();

                            }
                            break;
                        case SortOrder.Descending:
                        default:
                            for (int i = 1; i < dataGridView1.Rows.Count; i++)
                            {

                                dataGridView1.Rows[i].Cells[0].Value = dataGridView1.Rows.Count - i;
                                dataGridView1.Rows[i].Cells[1].Value = "F" + (dataGridView1.Rows.Count - i).ToString();

                            }
                            break;
                    }
                    btnUp.Visible = btnDown.Visible = false;
                }
                else
                {
                    MessageBox.Show("Row cannot be added above F1 row.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {

            }
        }
        private void RowDown(object sender, EventArgs e)
        {

            try
            {
                    using (DataGridViewRow row = new DataGridViewRow())
                    {
                        row.CreateCells(dataGridView1, new object[] { dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value, dataGridView1.SelectedCells[0].OwningRow.Cells[1].Value, string.Empty, "string", string.Empty, string.Empty, string.Empty, false });
                        dataGridView1.Rows.Insert(dataGridView1.SelectedCells[0].OwningRow.Index + 1, row);
                    }
                    switch (sort)
                    {
                       
                        case SortOrder.Ascending:
                        case SortOrder.None:
                            for (int i = 1; i < dataGridView1.Rows.Count; i++)
                            {
                                dataGridView1.Rows[i].Cells[0].Value = i + 1;
                                dataGridView1.Rows[i].Cells[1].Value = "F" + (i + 1).ToString();

                            }
                            break;
                        case SortOrder.Descending:
                        default:
                            for (int i = 1; i < dataGridView1.Rows.Count; i++)
                            {

                                dataGridView1.Rows[i].Cells[0].Value = dataGridView1.Rows.Count - i;
                                dataGridView1.Rows[i].Cells[1].Value = "F" + (dataGridView1.Rows.Count - i).ToString();

                            }
                            break;
                    }
                    btnUp.Visible = btnDown.Visible = false;
            }
            catch
            {

            }
        }
        private int rowIndex = 0;
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                rowIndex = e.RowIndex;
                btnUp.Visible = btnDown.Visible = true;
            }
            else
            {
                btnUp.Visible = btnDown.Visible = false;
            }
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            if ((rowIndex - 1) > 0 && rowIndex < dataGridView1.Rows.Count)
            {
                ColumnConfig previous = new ColumnConfig()
                {
                    Index = WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(dataGridView1.Rows[rowIndex - 1].Cells[0].Value.ToString(), 1),
                    ColumnName = dataGridView1.Rows[rowIndex - 1].Cells[1].Value.ToString(),
                    FieldName = dataGridView1.Rows[rowIndex - 1].Cells[2].Value.ToString(),
                    DataType = dataGridView1.Rows[rowIndex - 1].Cells[3].Value.ToString(),
                    Format = dataGridView1.Rows[rowIndex - 1].Cells[4].Value.ToString(),
                    Length = dataGridView1.Rows[rowIndex - 1].Cells[5].Value.ToString(),
                    DefaultValue = dataGridView1.Rows[rowIndex - 1].Cells[6].Value.ToString(),
                    IsPrimary = WD.DataAccess.Helpers.HelperUtility.ConvertTo<bool>(dataGridView1.Rows[rowIndex - 1].Cells[7].EditedFormattedValue.ToString(), false)
                };
                ColumnConfig current = new ColumnConfig()
                {
                    Index = WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(dataGridView1.Rows[rowIndex].Cells[0].Value.ToString(), 1),
                    ColumnName = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString(),
                    FieldName = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString(),
                    DataType = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString(),
                    Format = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString(),
                    Length = dataGridView1.Rows[rowIndex].Cells[5].Value.ToString(),
                    DefaultValue = dataGridView1.Rows[rowIndex].Cells[6].Value.ToString(),
                    IsPrimary = WD.DataAccess.Helpers.HelperUtility.ConvertTo<bool>(dataGridView1.Rows[rowIndex].Cells[7].EditedFormattedValue.ToString(), false)
                };

                //Current Grid
                dataGridView1.Rows[rowIndex].Cells[0].Value = current.Index;
                dataGridView1.Rows[rowIndex].Cells[1].Value = current.ColumnName;
                dataGridView1.Rows[rowIndex].Cells[2].Value = previous.FieldName;
                dataGridView1.Rows[rowIndex].Cells[3].Value = previous.DataType;
                dataGridView1.Rows[rowIndex].Cells[4].Value = previous.Format;
                dataGridView1.Rows[rowIndex].Cells[5].Value = previous.Length;
                dataGridView1.Rows[rowIndex].Cells[6].Value = previous.DefaultValue;
                dataGridView1.Rows[rowIndex].Cells[7].Value = previous.IsPrimary;

                //previous Grid
                dataGridView1.Rows[rowIndex - 1].Cells[0].Value = previous.Index;
                dataGridView1.Rows[rowIndex - 1].Cells[1].Value = previous.ColumnName;
                dataGridView1.Rows[rowIndex - 1].Cells[2].Value = current.FieldName;
                dataGridView1.Rows[rowIndex - 1].Cells[3].Value = current.DataType;
                dataGridView1.Rows[rowIndex - 1].Cells[4].Value = current.Format;
                dataGridView1.Rows[rowIndex - 1].Cells[5].Value = current.Length;
                dataGridView1.Rows[rowIndex - 1].Cells[6].Value = current.DefaultValue;
                dataGridView1.Rows[rowIndex - 1].Cells[7].Value = current.IsPrimary;


                dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex - 1].Cells[1];
                dataGridView1.Rows[rowIndex - 1].Selected = true;
                current = null;
                previous = null;
                rowIndex--;
            }
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            if (rowIndex > 0 && (rowIndex + 1) < dataGridView1.Rows.Count)
            {
                ColumnConfig next = new ColumnConfig()
                {
                    Index = WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(dataGridView1.Rows[rowIndex + 1].Cells[0].Value.ToString(), 1),
                    ColumnName = dataGridView1.Rows[rowIndex + 1].Cells[1].Value.ToString(),
                    FieldName = dataGridView1.Rows[rowIndex + 1].Cells[2].Value.ToString(),
                    DataType = dataGridView1.Rows[rowIndex + 1].Cells[3].Value.ToString(),
                    Format = dataGridView1.Rows[rowIndex + 1].Cells[4].Value.ToString(),
                    Length = dataGridView1.Rows[rowIndex + 1].Cells[5].Value.ToString(),
                    DefaultValue = dataGridView1.Rows[rowIndex + 1].Cells[6].Value.ToString(),
                    IsPrimary = WD.DataAccess.Helpers.HelperUtility.ConvertTo<bool>(dataGridView1.Rows[rowIndex + 1].Cells[7].EditedFormattedValue.ToString(), false)
                };
                ColumnConfig current = new ColumnConfig()
                {
                    Index = WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(dataGridView1.Rows[rowIndex].Cells[0].Value.ToString(), 1),
                    ColumnName = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString(),
                    FieldName = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString(),
                    DataType = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString(),
                    Format = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString(),
                    Length = dataGridView1.Rows[rowIndex].Cells[5].Value.ToString(),
                    DefaultValue = dataGridView1.Rows[rowIndex].Cells[6].Value.ToString(),
                    IsPrimary = WD.DataAccess.Helpers.HelperUtility.ConvertTo<bool>(dataGridView1.Rows[rowIndex].Cells[7].EditedFormattedValue.ToString(), false)
                };
                //Current Grid
                dataGridView1.Rows[rowIndex].Cells[0].Value = current.Index;
                dataGridView1.Rows[rowIndex].Cells[1].Value = current.ColumnName;
                dataGridView1.Rows[rowIndex].Cells[2].Value = next.FieldName;
                dataGridView1.Rows[rowIndex].Cells[3].Value = next.DataType;
                dataGridView1.Rows[rowIndex].Cells[4].Value = next.Format;
                dataGridView1.Rows[rowIndex].Cells[5].Value = next.Length;
                dataGridView1.Rows[rowIndex].Cells[6].Value = next.DefaultValue;
                dataGridView1.Rows[rowIndex].Cells[7].Value = next.IsPrimary;

                //next Grid
                dataGridView1.Rows[rowIndex + 1].Cells[0].Value = next.Index;
                dataGridView1.Rows[rowIndex + 1].Cells[1].Value = next.ColumnName;
                dataGridView1.Rows[rowIndex + 1].Cells[2].Value = current.FieldName;
                dataGridView1.Rows[rowIndex + 1].Cells[3].Value = current.DataType;
                dataGridView1.Rows[rowIndex + 1].Cells[4].Value = current.Format;
                dataGridView1.Rows[rowIndex + 1].Cells[5].Value = current.Length;
                dataGridView1.Rows[rowIndex + 1].Cells[6].Value = current.DefaultValue;
                dataGridView1.Rows[rowIndex + 1].Cells[7].Value = current.IsPrimary;

                dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex + 1].Cells[1];
                dataGridView1.Rows[rowIndex + 1].Selected = true;
                current = null;
                next = null;
                rowIndex++;
            }
        }
        private SortOrder sort = SortOrder.None;
        void ResetSort()
        {
            sort = SortOrder.None;
            dataGridView1.Columns[1].HeaderCell.SortGlyphDirection = sort;
        }
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                sort = sort == SortOrder.None ? SortOrder.Descending : sort == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
                dataGridView1.Sort(new RowComparer(sort));
                dataGridView1.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = sort;
                
            }
        }
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ResetSort();
            switch (dataGridView1.CurrentCell.ColumnIndex)
            {
                case 2:
                    try
                    {
                        ComboBox cmb = e.Control as ComboBox;
                        {
                            if (cmb != null)
                            {
                                
                                cmb.SelectionChangeCommitted += cmb_SelectionChangeCommitted;
                                List<string> excludeList = (from f in dataGridView1.Rows.Cast<DataGridViewRow>().Where(x => x.Index != dataGridView1.CurrentCell.RowIndex && !string.IsNullOrEmpty(x.Cells[2].Value.ToString()))
                                                            select "'" + f.Cells[2].Value.ToString() + "'").ToList();
                                using (IDbConnection con = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(input.DbProvider))
                                {
                                    con.ConnectionString = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(input.ConnString, true);
                                    using (IDbCommand com = con.CreateCommand())
                                    {
                                        con.Open();
                                        com.CommandText = String.Format("select * from {0}", input.TargetTableName);
                                        com.CommandType = CommandType.Text;
                                        DataTable dt = com.ExecuteReader(CommandBehavior.SchemaOnly).GetSchemaTable().Select("COLUMNNAME not in (" + string.Join(",", excludeList.ToArray()) + ")").CopyToDataTable();
                                        if (dt.Rows.Count > 0)
                                        {
                                            cmb.DataSource = dt;
                                        }
                                    }
                                }
                                cmb.MaxDropDownItems = 100;
                                excludeList = null;

                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("No Database columns to add", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
                default: break;
            }
        }
        private void cmb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridViewComboBoxEditingControl cmb = (System.Windows.Forms.DataGridViewComboBoxEditingControl)sender;
            if (cmb != null)
            {
                try
                {
                    using (IDbConnection con = WD.DataAccess.Configurations.AppConfiguration.CreateConnection(input.DbProvider))
                    {
                        con.ConnectionString = WD.DataAccess.Helpers.WebSecurityUtility.Decrypt(input.ConnString, true);
                        using (IDbCommand com = con.CreateCommand())
                        {
                            con.Open();
                            com.CommandText = String.Format("select * from {0}", input.TargetTableName);
                            com.CommandType = CommandType.Text;
                            DataTable dt = com.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly).GetSchemaTable();
                            if (dt.Rows.Count > 0)
                            {
                                TableInfo tbInfo = (from DataRow row in dt.Select("COLUMNNAME = '" + cmb.Text + "'")
                                                    select new TableInfo()
                                                    {

                                                        COLUMNNAME = row["COLUMNNAME"].ToString(),
                                                        DATATYPE = row["DATATYPE"].ToString(),
                                                        IsPrimary = (row["IsKey"].ToString() == "True") || (row["IsUnique"].ToString() == "True") ? true : false,
                                                        COLUMNSIZE = row["COLUMNSIZE"].ToString()
                                                       
                                                    }
                                                  ).FirstOrDefault();
                                if (tbInfo != null)
                                {

                                    dataGridView1.Rows[cmb.EditingControlRowIndex].Cells[3].Value = SQLHelpers.GetDataType(tbInfo.DATATYPE);
                                    dataGridView1.Rows[cmb.EditingControlRowIndex].Cells[5].Value = tbInfo.COLUMNSIZE;
                                    dataGridView1.Rows[cmb.EditingControlRowIndex].Cells[7].Value = tbInfo.IsPrimary;
                                }

                            }
                        }
                    }
                }
                catch
                {


                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    btn_delete(null, null);
                }
            }
        }

      
    }
    public class RowComparer : System.Collections.IComparer
    {
        private  int sortOrderModifier = 1;
        private readonly int cell = 0;

        public RowComparer(SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Descending)
            {
                sortOrderModifier = -1;
            }
            else if (sortOrder == SortOrder.Ascending)
            {
                sortOrderModifier = 1;
            }
            this.cell = 0;
        }
        public RowComparer(SortOrder sortOrder,int cell)
        {
            if (sortOrder == SortOrder.Descending)
            {
                sortOrderModifier = -1;
            }
            else if (sortOrder == SortOrder.Ascending)
            {
                sortOrderModifier = 1;
            }
            this.cell = cell;
        }
       
        public int Compare(object x, object y)
        {
            int CompareResult = sortOrderModifier;
            DataGridViewRow grdrow1 = (DataGridViewRow)x;
            DataGridViewRow grdRow2 = (DataGridViewRow)y;
            if (grdrow1.Index == 0)
            {
                CompareResult = -1;
            }
            else
            {
                // Try to sort based on the Last Name column.
                CompareResult = System.String.Compare(
                    grdrow1.Cells[this.cell].Value.ToString().PadLeft(3),
                    grdRow2.Cells[this.cell].Value.ToString().PadLeft(3), true);
                // If the Last Names are equal, sort based on the First Name.
                if (CompareResult == 0)
                {
                    CompareResult = System.String.Compare(
                        grdrow1.Cells[this.cell].Value.ToString().PadLeft(3),
                        grdRow2.Cells[this.cell].Value.ToString().PadLeft(3), true);
                }

                CompareResult = CompareResult * sortOrderModifier;
            }
            return CompareResult;
        }
    }
}

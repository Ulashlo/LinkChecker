using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrialProgram
{
    public partial class MainForm : Form
    {
        private DataTable table;
        private DataColumn linkColumn, NestingLevelColumn;

        public MainForm()
        {
            InitializeComponent();
            table = new DataTable("Список сайтов");
            linkColumn = new DataColumn("Ссылка");
            NestingLevelColumn = new DataColumn("Глубина рекурсии");
            table.Columns.AddRange(new DataColumn[] { linkColumn, NestingLevelColumn });
            SitesListGridView.DataSource = table;
            SitesListGridView.Columns[0].MinimumWidth = 445;
            SitesListGridView.Columns[1].MinimumWidth = 180;
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            if (textUrl.Text.Length == 0)
            {
                MessageBox.Show("Введите ссылку!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HttpStatusCode statusCode;
            try
            {
                statusCode = LinkInfo.GetStatusCode(textUrl.Text, true);
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Неверный формат URI!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if((int)statusCode > 399)
            {
                MessageBox.Show("Код ответа сайта: " + (int)statusCode + " " + statusCode.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int level = (int)recursionLevel.Value;
            PageLinksSQL pageLinks = new PageLinksSQL(textUrl.Text, level);
            Action visualize = () =>
            {
                LinksForm linksForm = new LinksForm(pageLinks, UpdateTable);
                linksForm.Show();
                UpdateTable();
            };
            ProgressForm progressForm = new ProgressForm(pageLinks, visualize, pageLinks.FindLinks);
            progressForm.Show();
        }

        private void SitesListGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            string link = SitesListGridView[0, e.RowIndex].Value.ToString();
            int level = int.Parse(SitesListGridView[1, e.RowIndex].Value.ToString());
            LinksForm linksForm = new LinksForm(new PageLinksSQL(link, level), UpdateTable);
            linksForm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void UpdateTable()
        {
            using(DataBaseContext db = new DataBaseContext())
            {
                table.Rows.Clear();
                foreach(var item in db.Sites.Select(x => new { uri = x.Uri, level = x.NestingLevel }))
                {
                    DataRow row = table.NewRow();
                    row[0] = item.uri;
                    row[1] = item.level;
                    table.Rows.Add(row);
                }
            }
            for (int i = 0; i < SitesListGridView.Rows.Count; i++)
            {
                SitesListGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }
    }
}

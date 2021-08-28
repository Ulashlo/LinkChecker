using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace TrialProgram
{
    [Flags]
    public enum Filtr
    {
        None = 0,
        Info = 1,
        Success = 2,
        Redirect = 4,
        Client = 8,
        Servis = 16,
        Inner = 32,
        Outer = 64,
        Exception = 128
    }

    public partial class LinksForm : Form
    {
        private DataTable table;
        private DataColumn linkColumn, statusColumn, typeColumn;

        private PageLinksSQL pageLinks;

        Action updateBaseTable;

        private Filtr filtrParams;

        public LinksForm(PageLinksSQL pageLinks, Action updateBaseTable)
        {
            InitializeComponent();
            table = new DataTable("Найденные ссылки");
            linkColumn = new DataColumn("Ссылка");
            statusColumn = new DataColumn("Статус ответа");
            typeColumn = new DataColumn("Тип");
            table.Columns.AddRange(new DataColumn[] { linkColumn, statusColumn, typeColumn });
            linksGridView.DataSource = table;
            linksGridView.Columns[0].MinimumWidth = 200;
            linksGridView.Columns[0].Width = 300;
            linksGridView.Columns[1].MinimumWidth = 90;
            linksGridView.Columns[1].Width = 160;
            linksGridView.Columns[2].MinimumWidth = 70;
            linksGridView.Columns[2].Width = 90;
            this.pageLinks = pageLinks;
            this.updateBaseTable = updateBaseTable;
            filtrParams = (Filtr)255;
            recursionLevel.Value = pageLinks.MaxLevel;
        }

        private void Find(object sender, EventArgs e)
        {
            int level = (int)recursionLevel.Value;
            if(level > pageLinks.MaxLevel)
            {
                DialogResult result = MessageBox.Show("Требуется догрузить новые ссылки, так как заявленный уровень рекурсии выше исходного. Хотите ли вы обновить уже скачанные ссылки?",
                    "Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if(result == DialogResult.Yes)
                {
                    Refresh(null, null);
                    return;
                }
                else if(result == DialogResult.No)
                {
                    IEnumerable<AboutLink> iner = null;
                    IEnumerable<AboutLink> outer = null;
                    Action update = () =>
                    {
                        outer = pageLinks.GetOuter(level, filtrParams);
                        iner = pageLinks.GetIner(level, filtrParams);
                    };
                    Action visualize = () =>
                    {
                        pageLinks.MaxLevel = level;
                        updateBaseTable();
                        SetTableInfo(new IEnumerable<AboutLink>[] { iner, outer }, new string[] { "Внутренняя", "Внешняя" });
                    };
                    ProgressForm progressForm = new ProgressForm(pageLinks, visualize, update);
                    progressForm.Show();
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                IEnumerable<AboutLink> iner, outer;
                iner = pageLinks.GetIner(level, filtrParams);
                outer = pageLinks.GetOuter(level, filtrParams);
                SetTableInfo(new IEnumerable<AboutLink>[] { iner, outer }, new string[] { "Внутренняя", "Внешняя" });
            }
        }

        private void Refresh(object sender, EventArgs e)
        {
            pageLinks.RemoveLinkes();
            int level = (int)recursionLevel.Value;
            pageLinks.MaxLevel = level;
            updateBaseTable();
            IEnumerable<AboutLink> iner = null;
            IEnumerable<AboutLink> outer = null;
            Action update = () =>
            {
                pageLinks.FindLinks();
                iner = pageLinks.GetIner(level, filtrParams);
                outer = pageLinks.GetOuter(level, filtrParams);
            };
            Action visualize = () =>
            {
                SetTableInfo(new IEnumerable<AboutLink>[] { iner, outer }, new string[] { "Внутренняя", "Внешняя" });
            };
            ProgressForm progressForm = new ProgressForm(pageLinks, visualize, update);
            progressForm.Show();
        }

        private void linksGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.RowIndex < 0)
            {
                return;
            }
            string link = linksGridView[0, e.RowIndex].Value.ToString();
            string type = linksGridView[2, e.RowIndex].Value.ToString();
            int k;
            AboutLink aboutLink;
            if (int.TryParse(linksGridView[1, e.RowIndex].Value.ToString().Substring(0,3), out k))
            {
                aboutLink = pageLinks.GetAboutLink(link, type == "Внутренняя");
            }
            else
            {
                aboutLink = pageLinks.GetExeptionLink(link, type == "Внутренняя");
            }
            AboutLinkForm aboutLinkForm = new AboutLinkForm(aboutLink, type);
            aboutLinkForm.Show();
        }

        private void linksGridView_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < linksGridView.Rows.Count; i++)
            {
                int status, secondStatus = 0;
                string stat = linksGridView.Rows[i].Cells[1].Value.ToString();
                if (!int.TryParse(stat.Substring(0,3), out status))
                {
                    status = -1;
                }
                if(status > 299 && status < 400)
                {
                    int.TryParse(Regex.Match(stat, @"(?<=- )\d{3}").Value, out secondStatus);
                }
                linksGridView.Rows[i].DefaultCellStyle.BackColor = GetColor(status, secondStatus);
                linksGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void SetTableInfo(IEnumerable<AboutLink>[] mas, string[] types)
        {
            table.Rows.Clear();
            int k = 0;
            for (int i = 0; i < mas.Length; i++)
            {
                foreach (AboutLink item in mas[i])
                {
                    DataRow row = table.NewRow();
                    AddRow(row, item, types[i], k);
                    k++;
                }
            }
            for (int i = 0; i < linksGridView.Rows.Count; i++)
            {
                linksGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            linksGridView_Sorted(null, null);
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            switch (checkBox.Tag)
            {
                case("Info"):
                    filtrParams ^= Filtr.Info;
                    return;
                case("Success"):
                    filtrParams ^= Filtr.Success;
                    return;
                case("Redirect"):
                    filtrParams ^= Filtr.Redirect;
                    return;
                case("Client"):
                    filtrParams ^= Filtr.Client;
                    return;
                case("Server"):
                    filtrParams ^= Filtr.Servis;
                    return;
                case("Inner"):
                    filtrParams ^= Filtr.Inner;
                    return;
                case("Outer"):
                    filtrParams ^= Filtr.Outer;
                    return;
                case ("Exception"):
                    filtrParams ^= Filtr.Exception;
                    return;
            }
        }

        private void AddRow(DataRow row, AboutLink item, string type, int rowIndex)
        {
            row[0] = item.Uri;
            StringBuilder stat = new StringBuilder();
            if(item.ExceptionMessage.Length == 0)
            {
                int status = (int)item.Status;
                stat.Append((status).ToString());
                stat.Append(" ");
                stat.Append(item.Status.ToString());
            }
            else
            {
                stat.Append(item.ExceptionMessage);
            }
            if (item.Redirects.Count > 1)
            {
                stat.Append(" - ");
                stat.Append(((int)item.Redirects.Last().Status).ToString());
                stat.Append(" ");
                stat.Append(item.Redirects.Last().Status.ToString());
            }
            row[1] = stat.ToString();
            row[2] = type;
            table.Rows.Add(row);
        }

        private Color GetColor(int status, int secondStatus = 0)
        {
            if(status == -1)
            {
                return Color.Brown;
            }
            Color color;
            if (status < 200)
            {
                color = Color.LightSkyBlue;
            }
            else if (status < 300)
            {
                color = Color.LightGreen;
            }
            else if (status < 400)
            {
                if (secondStatus < 400)
                {
                    color = Color.Khaki;
                }
                else
                {
                    color = Color.Orange;
                }
            }
            else if (status < 500)
            {
                color = Color.OrangeRed;
            }
            else
            {
                color = Color.Brown;
            }
            return color;
        }
    }
}

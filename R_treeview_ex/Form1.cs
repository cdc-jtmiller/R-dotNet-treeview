using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;
using RDotNet.NativeLibrary;

namespace R_treeview_ex
{

    public partial class R_Treeview : Form
    {
        public R_Treeview()
        {
            InitializeComponent();
            tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
        }

        private void R_Treeview_Load(object sender, EventArgs e)
        {
            // Takes the contents of console and sends it to a textbox
            Console.SetOut(new ControlWriter(tbLog));

            // Initializes RDotNet
            MyFunctions.InitializeRDotNet();

            // Populate the treeview
            ListDirectory(treeView1, @"C:\Work\VS_projects\Projects\R_treeview_ex\User_library");
        }

        // Used for treeview
        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var fileDirectory = new DirectoryInfo(path);

            treeView.Nodes.Add(CreateDirectory(fileDirectory));
        }

        // Used for treeview - creates list files
        private static TreeNode CreateDirectory(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectory(directory));
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                directoryNode.Nodes.Add(new TreeNode(file.Name));
            }

            return directoryNode;

        }

        private void btnChooseFileR_Click(object sender, EventArgs e)
        {
            try
            {
                MyFunctions._engine.Evaluate("dataset<-read.csv(file.choose(), sep = ',', stringsAsFactors = FALSE)");
                DataFrame df = MyFunctions._engine.Evaluate("dataset").AsDataFrame();

                //btnChooseFileNet.Enabled = false;
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();

                for (int i = 0; i < df.ColumnCount; ++i)
                {
                    dataGridView1.ColumnCount++;
                    dataGridView1.Columns[i].Name = df.ColumnNames[i];
                }

                for (int i = 0; i < df.RowCount; ++i)
                {
                    dataGridView1.RowCount++;
                    dataGridView1.Rows[i].HeaderCell.Value = df.RowNames[i];

                    for (int k = 0; k < df.ColumnCount; ++k)
                    {
                        dataGridView1[k, i].Value = df[i, k];
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error loading data using R: " + ex.Message);
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            switch ((sender as TabControl).SelectedIndex)
            {
                case 0:
                    btnSubmit.Enabled = true;
                    break;
                case 1:
                    btnSubmit.Enabled = false;
                    break;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                tbCode.Text = "";
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                tbLog.Text = "";
            }
            else
            {
                return;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SymbolicExpression expression = MyFunctions._engine.Evaluate(tbCode.Text.Trim());
                if (expression == null) return;

                CharacterVector vector = expression.AsCharacter();

                foreach (var sSubmitThis in vector)
                {
                    
                    {
                        Console.WriteLine(sSubmitThis);
                        tabControl1.SelectedIndex = 1;

                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Problem running your R program: " + ex.Message);
            }

        }


        private void btnChooseFileNet_Click(object sender, EventArgs e)
        {
            try
            {
                string strFileNameNet = "";
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();
                OpenFileDialog dialogNet = new OpenFileDialog() ;
                dialogNet.Title = "Open CSV file";
                dialogNet.Filter = "CSV Files (*.csv)|*.csv";
                if (dialogNet.ShowDialog() == DialogResult.OK)
                {
                    //btnChooseFileR.Enabled = false;
                    strFileNameNet = dialogNet.FileName;
                    using (StreamReader sr = new StreamReader(strFileNameNet))
                    {
                        DataTable netData = new DataTable();
                        string[] headers = sr.ReadLine().Split(',');
                        for (int i = 0; i < headers.Count(); i++)
                        {
                            netData.Columns.Add(headers[i]);
                        }
                        while (!sr.EndOfStream)
                        {
                            string[] rows = sr.ReadLine().Split(',');
                            DataRow dr = netData.NewRow();
                            for (int i = 0; i < rows.Count(); i++)
                            {
                                dr[i] = rows[i];
                            }
                            netData.Rows.Add(dr);
                        }
                    dataGridView1.DataSource = netData;
                    }

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data using .Net: " + ex.Message);
            }
        }
    }

    public static DataFrame ConvertDataTable(DataTable dt)
    {
        //REngine.SetEnvironmentVariables();
        //REngine engine = REngine.GetInstance();
        double?[,] stringData = new double?[dt.Rows.Count, dt.Columns.Count];
        DataFrame df = MyFunctions._engine.Evaluate("df=NULL").AsDataFrame();
        int irow = 0;
        foreach (DataRow row in dt.Rows)
        {
            NumericVector x = MyFunctions._engine.Evaluate("x=NULL").AsNumeric();
            int icol = 0;
            foreach (DataColumn col in dt.Columns)
            {
                if (row.Field<double?>(col) == null)
                {
                    x = MyFunctions._engine.Evaluate("x=c(x, NA) ").AsNumeric();
                }
                else
                {
                    x = MyFunctions._engine.Evaluate("x=c(x, " + row.Field<double?>(col) + ") ").AsNumeric();
                }
                icol++;
            }
            df = MyFunctions._engine.Evaluate("df= as.data.frame(rbind(df,x)) ").AsDataFrame();
            irow++;
        }
        return (df);
    }

    public static class MyFunctions
    {
        public static REngine _engine;

        public static void InitializeRDotNet()
        {
            try
            {
                REngine.SetEnvironmentVariables();
                _engine = REngine.GetInstance();
                _engine.Initialize();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error using RDotNet: " + ex.Message);
            }
        }
    }

    public class ControlWriter : TextWriter
    {
        private Control textbox;
        public ControlWriter(Control textbox)
        {
            this.textbox = textbox;
        }

        public override void Write(char value)
        {
            textbox.Text += value;
        }

        public override void Write(string value)
        {
            {
                textbox.Text += value;
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }

}

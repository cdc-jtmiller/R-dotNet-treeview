using System;
using System.Security;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using RDotNet;
using RDotNet.NativeLibrary;

namespace R_treeview_ex
{
    public partial class R_Treeview : Form
    {
        public string xmlLibPath = @"C:\\";
        public string xmlFileName = "";

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

            // Populate the treeview with 'Supplemental Path'
            // 64 bit
            //string folderPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\..\")) + @"User_library";

            // Any CPU
            string folderPath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\")) + @"User_library";
            xmlLibPath = folderPath;

            listDirectory(treeView1, xmlLibPath);
            //ListDirectory(treeView1, @"C:\Work\VS_projects\Projects\R_treeview_ex\User_library");
        }

        // Used for treeview - creates directory structure of user library
        private void listDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var fileDirectory = new DirectoryInfo(path);

            treeView.Nodes.Add(createDirectory(fileDirectory));
        }

        // Used for treeview - creates list of xml files
        private static TreeNode createDirectory(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);

            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(createDirectory(directory));
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                if (file.Extension.ToLower() == ".xml")
                {
                    directoryNode.Nodes.Add(new TreeNode(file.Name));
                }
            }
            return directoryNode;
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node.Text.Contains("xml"))
            {
                xmlFileName = xmlLibPath + "\\" + node.Text;
                //Console.WriteLine(xmlFileName);
            }

            // Call xml file reader
            loadXML();
        }

        private void loadXML()
        {
            // Must remove invalid XML chars
            // Read in XML
            string fixedXML = System.IO.File.ReadAllText(xmlFileName).Replace("<-", "&lt;-");

            // Encode original XML
            //string encodedXML = System.Security.SecurityElement.Escape(origXML);
            //Console.WriteLine(encodedXML);


            // For an XML file, use Load
            //XDocument doc = XDocument.Load(xmlFileName);

            // For an XML string, use Parse
            //XDocument doc = XDocument.Parse(encodedXML);
            XDocument doc = XDocument.Parse(fixedXML);

            //  To get individual node information
            var xNodes = from r in doc.Descendants("statistic")
                         select new
                         {
                             pgm_name = r.Element("name").Value,
                             description = r.Element("description").Value,
                             rcode = r.Element("rcode").Value
                         };

            //  To get a list of values from a repeating node
            List<string> paramList = doc.Element("statistic").Elements("parameters")
                .Where(p => p.Elements("parameters") != null)
                .Elements()
                .Select(p => p.Value).ToList();

            string sDropDown = string.Join(",", paramList);

            //  Clear tab for each click
            tbLog.Text = "";

            foreach (var r in xNodes)
            {
                Console.WriteLine(r.pgm_name + "\r\n" + r.description + "\r\n" + r.rcode + "\r\n");
            }

            //foreach (var l in xVars)
            //{
                Console.WriteLine("parameters: " + sDropDown + Environment.NewLine);
            //}
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
                        if (df[i, k] != null)
                        {
                            dataGridView1[k, i].Value = df[i, k];
                        }
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
            tabControl1.SelectedIndex = 1;  // Sets focus to the log tab
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
                string sFileNameNet = "";
                string sFilePath = "";
                string sPathToCSV = "";
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();
                DataTable netData = new DataTable();


                OpenFileDialog dialogNet = new OpenFileDialog
                {
                    Title = "Open CSV file",
                    Filter = "CSV Files (*.csv)|*.csv"
                };

                if (dialogNet.ShowDialog() == DialogResult.OK)
                {
                    //btnChooseFileR.Enabled = false;
                    sFilePath = dialogNet.FileName;
                    sPathToCSV = Path.GetDirectoryName(sFilePath);
                    sFileNameNet = Path.GetFileName(sFilePath);

                    string sConnString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sPathToCSV + ";Extended Properties='text;HDR=Yes';");
                    string sSqlSelect = String.Format("SELECT * FROM [" + sFileNameNet + "]");

                    using (OleDbConnection conn = new OleDbConnection(sConnString))

                    using (OleDbCommand command = new OleDbCommand(sSqlSelect, conn))

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        netData.Locale = CultureInfo.CurrentCulture;
                        adapter.Fill(netData);

                        for  (int i=0; i< netData.Rows.Count; i++)
                        {
                            for (int j = 0; j < netData.Columns.Count; j++)
                            { 
                                if (netData.Rows[i][j] == DBNull.Value)
                                    netData.Rows[i][j] = double.NaN;
                                netData.AcceptChanges();
                                //Console.WriteLine("Row: (" + (i+1) + ") Col: (" + (j+1) + ") is: " + netData.Rows[i][j].ToString());
                            }
                        }

                        dataGridView1.DataSource = netData.AsDataView();
                        conn.Close();
                    }

                    //ConvertDT(netData);    //Method 1
                    ConvertDT2(netData);   //Method 2
                    //ConvertDT3(netData);   //Method 3
                    //ConvertDT4(netData);



                    //using (StreamReader sr = new StreamReader(sFileNameNet))
                    //{
                    //    DataTable netData = new DataTable();
                    //    string[] headers = sr.ReadLine().Split(',');
                    //    for (int i = 0; i < headers.Count(); i++)
                    //    {
                    //        netData.Columns.Add(headers[i], typeof(string));
                    //    }
                    //    while (!sr.EndOfStream)
                    //    {
                    //        string[] rows = sr.ReadLine().Split(',');
                    //        DataRow dr = netData.NewRow();
                    //        for (int i = 0; i < rows.Count(); i++)
                    //        {
                    //            dr[i] = rows[i];
                    //        }
                    //        netData.Rows.Add(dr);
                    //    }
                    //    dataGridView1.DataSource = netData.AsDataView();
                    //    //ConvertDT(netData);    //Method 1
                    //    ConvertDT2(netData);   //Method 2
                    //    //ConvertDT3(netData);   //Method 3
                    //    //ConvertDT4(netData);
                    //}
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

        public static List<Dictionary<string, string>> GetDictionaryList(DataTable dt)
        {
            return dt.AsEnumerable().Select(
                row => dt.Columns.Cast<DataColumn>().ToDictionary(
                    column => column.ColumnName,
                    column => row[column].ToString()
                    )).ToList();      //  Returns list of dictionaries
        }

        public static Dictionary<string, object> GetDictA(DataTable dt)
        {
            return dt.AsEnumerable()
              .ToDictionary(row => row.Field<string>(0),
                                        row => row.Field<object>(1));
        }

        public static Dictionary<string, object> GetDictB(DataTable dt)
        {
            return dt.AsEnumerable().ToDictionary<DataRow, string, object>(row => row.Field<string>(0),
                                        row => row.Field<object>(1));
        }

        public static DataFrame ConvertDT(DataTable tab)
        // Converts the .Net datatable to an R dataframe
        // Method 1
        {
            //double?[,] stringData = new double?[tab.Rows.Count, tab.Columns.Count];
            //double?[,] result = (tab.Rows, tab.Columns).AsEnumerable()
            //        .Select(row => Convert.ToDouble(row.Field<string>("column1"), System.Globalization.CultureInfo.InvariantCulture)).ToArray();

            //DataFrame dframe = null;
            DataFrame dframe = MyFunctions._engine.Evaluate("dframe=NULL").AsDataFrame();

            int irow = 0;
            foreach (DataRow row in tab.Rows)
            {
                NumericVector x = MyFunctions._engine.Evaluate("x=NULL").AsNumeric();
                int icol = 0;
                foreach (DataColumn col in tab.Columns)
                {
                    if (col.DataType.Equals(typeof(string)))
                    {
                        object value = row[col];
                        if (value.ToString() == "")
                        {
                            x = MyFunctions._engine.Evaluate("x=c(x, NA) ").AsNumeric();
                        }
                        else 
                        {
                            x = MyFunctions._engine.Evaluate("x = c(x, " + Math.Round(Convert.ToDouble(value), 5) + ") ").AsNumeric();
                        }
                    }
                    else
                    {
                        x = MyFunctions._engine.Evaluate("x = c(x, " + row[col] + ") ").AsNumeric();
                    }
                    icol++;
                }
                dframe = MyFunctions._engine.Evaluate("dframe= as.data.frame(rbind(dframe,x)) ").AsDataFrame();
                irow++;
            }

            MyFunctions._engine.SetSymbol("ds1", dframe);

            return dframe;
        }


        public static DataFrame ConvertDT2(DataTable dt)
        // Converts the .Net datatable to an R dataframe
        // Method 2
        {
            //DataFrame df = null;
            DataFrame df = MyFunctions._engine.Evaluate("df=NULL").AsDataFrame();

            //double n;

            IEnumerable[] columns = new IEnumerable[dt.Columns.Count];
            string[] columnNames = dt.Columns.Cast<DataColumn>()
                                   .Select(x => x.ColumnName)
                                   .ToArray();


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                switch (Type.GetTypeCode(dt.Columns[i].DataType))
                {
                    case TypeCode.Double:
                        columns[i] = dt.Rows.Cast<DataRow>()
                            .Select(row => row.Field<double>(i))
                            .ToArray();
                        break;

                    case TypeCode.Int32:
                        columns[i] = dt.Rows.Cast<DataRow>()
                            .Select(row => row.Field<int>(i))
                            .ToArray();
                        break;

                    //case TypeCode.Decimal:
                    //    IEnumerable array = dt.Rows.Cast<DataRow>().Select(row => row.Field<object>(i)).ToArray();

                    case TypeCode.Int64:
                        columns[i] = dt.Rows.Cast<DataRow>()
                            .Select(row => row.Field<long>(i))
                            .ToArray();
                        break;

                    case TypeCode.String:
                        columns[i] = dt.Rows.Cast<DataRow>()
                            //.Select(row => row.Field<string>(i).Replace("\"", string.Empty))
                            .Select(row => row.Field<string>(i))
                            .ToArray();
                        break;

                    default:
                        //columns[i] = dt.Rows.Cast<DataRow>().Select(row => row[i]).ToArray();

                        //columns[i] = dt.Rows.Cast<DataRow>().Select(row => row.Field<long>(i)).ToArray();
                        //columns[i] = dt.Rows.Cast<DataRow>().Select(row => row.Field<decimal>(i)).ToArray();
                        columns[i] = dt.Rows.Cast<DataRow>().Select(row => row[i]).ToArray();
                        //columns[i] = ListToIenumerable(array);
                        break;

                        throw new InvalidOperationException(String.Format("Type {0} is not supported", dt.Columns[i].DataType.Name));
                }
            }

            try
            {

                df = MyFunctions._engine.CreateDataFrame(columns: columns.ToArray(), columnNames: columnNames, stringsAsFactors: false);
                MyFunctions._engine.SetSymbol("ds2", df);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating dataframe: " + ex.Message);
            }

            return df;

        }

        public static DataFrame ConvertDT3(DataTable dt)
        // Converts the .Net datatable to an R dataframe
        // Method 3
        {
            DataFrame df = MyFunctions._engine.Evaluate("df=NULL").AsDataFrame();

            var list = GetDictA(dt);

            var colNames = new List<string>() { "col1", "col2" };
            IEnumerable[] columns = new IEnumerable[2];

            columns[0] = list.Keys.ToArray();
            columns[1] = list.Values.ToArray();

            df = MyFunctions._engine.CreateDataFrame(columns, colNames.ToArray(), stringsAsFactors: false);
            MyFunctions._engine.SetSymbol("ds3", df);

            return df;
        }

        public static DataFrame ConvertDT4(DataTable dt)
        // Converts the .Net datatable to an R dataframe
        // Method 4
        {
            //DataFrame dframe = null;
            DataFrame dframe = MyFunctions._engine.Evaluate("dframe=NULL").AsDataFrame();

            for (int i = 0; i < dt.Rows.Count; i++)
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    object o = dt.Rows[i].ItemArray[j];
                    Console.WriteLine("Data item = {0}", o.ToString());
                }

            //dframe = MyFunctions._engine.Evaluate("dframe= as.data.frame(rbind(dframe,x)) ").AsDataFrame();
            return dframe;
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
}

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
        //public string xmlLibPath = @"C:\\";
        //public string xmlFileName = "";
        
        //public int xmlCount = 0;
        DataTable netData = new DataTable();
        private int currentTabPage { get; set; }

        public R_Treeview()
        {
            InitializeComponent();
            //tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            //btnCloseTab.Click += new EventHandler(btnCloseTab_Click);


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
            myGlobalVars.xmlLibPath = folderPath;

            listDirectory(treeView1, myGlobalVars.xmlLibPath);
            //ListDirectory(treeView1, @"C:\Work\VS_projects\Projects\R_treeview_ex\User_library");
            btnCreateR.Visible = false;
            btnCreateR.Enabled = false;

            checkTabCount();
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


        private void parseXML(out int cntParams, out string lstParams, out string RpgmName, out string Rpgm)
        {
            // Must remove invalid XML chars
            // Read in XML
            string fixedXML = System.IO.File.ReadAllText(myGlobalVars.xmlFileName).Replace("<-", "&lt;-");
            string list = "";

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
                             pgmName = r.Element("name"),
                             description = r.Element("description"),
                             rcode = r.Element("rcode")
                         };

            RpgmName = doc.Descendants("statistic").Elements("name").Single().Value.ToString();
            Rpgm = doc.Descendants("statistic").Elements("rcode").Single().Value.ToString();
            myGlobalVars.rCode = Rpgm;
            

            //  To get a list of values from the parameter node
            List<string> paramList = doc.Element("statistic").Elements("parameters")
                .Where(p => p.Elements("parameters") != null)
                .Elements()
                .Select(p => p.Value).ToList();

            //  Get the count for the number of drop-downs needed on the new tab
            cntParams = doc.Descendants("parameters")
                        .SelectMany(item => item.Descendants()).Count();

            list = string.Join(",", paramList);
            //lstParams = "'" + list.Replace(",", "','") + "'";
            lstParams = string.Join(",", list.Split(',').Select(x => string.Format("'{0}'", x)).ToList());

            //  Clear tab for each click
            tbLog.Text = "";

            foreach (var r in xNodes)
            {
                Console.WriteLine(r.pgmName + "\r\n" + r.description + "\r\n" + r.rcode + "\r\n");
            }

            Console.WriteLine("parameters: " + lstParams + Environment.NewLine + "Count: " + cntParams.ToString());
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node.Text.Contains("xml"))
            {
                myGlobalVars.xmlFileName = myGlobalVars.xmlLibPath + "\\" + node.Text;
                //Console.WriteLine(xmlFileName);
            }

            // Call xml file parser
            //   - get needed parameters
            parseXML(out int cntParams, out string lstParams, out string RpgmName, out string Rpgm);

            tabControl1.TabPages.Add(RpgmName);  // Create a new tab--.text is RpgmName. .Name is null
            
            // T A B S
            // Name tabs same as the text of the tabs
            int tabCnt = tabControl1.TabPages.Count;

            for (int i = 0; i < tabCnt;  i++ )
            {
                tabControl1.TabPages[i].Name = tabControl1.TabPages[i].Text;
            }

            tabControl1.SelectedTab = tabControl1.TabPages[RpgmName];

            //  C O M B O   B O X E S
            //  Add number of comboboxes found in XML 'parameter' node
            //  Use element tags for var names and elements to create labels for combos

            for (int p = 0; p < cntParams; p++) 
            {
                string ctlName = ("cboParams_" + p.ToString());
                string lblText = ("lblParams_"  + p.ToString());
                string[] lblNames = lstParams.Split(',');
                int Y = p * 27 + 10;

                ComboBox cboParams = new ComboBox();
                Label lblParams = new Label();

                cboParams.Name = ctlName;
                cboParams.Width = 300;
                cboParams.Height = 25;
                cboParams.Location = new Point(150, Y);

                lblParams.Name = lblText;
                string tmpLabel = lblNames[p].Trim(new char[] { (char)39 }).ToString();
                int lblLen = tmpLabel.Length;
                lblParams.Width = 100;
                lblParams.Height = 25;
                lblParams.Location = new Point(10, Y);
                lblParams.Text = tmpLabel;
               
                tabControl1.TabPages[RpgmName].Controls.Add(cboParams);
                tabControl1.TabPages[RpgmName].Controls.Add(lblParams);
            }

            btnCreateR.Visible = true;
            btnCreateR.Enabled = true;

            foreach (Control c in tabControl1.TabPages[RpgmName].Controls)
            {
                if (c is ComboBox)
                {
                    (c as ComboBox).BindingContext = new BindingContext();
                    (c as ComboBox).DataSource = myGlobalVars.colList;
                }
            }



        }

        private void btnCloseTab_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            tabControl1.SelectedIndex = 1;
            //checkTabCount();
            btnCreateR.Visible = false;
            btnCreateR.Enabled = false;
        }

        private void checkTabCount()
        {
            int tabCnt2 = tabControl1.TabPages.Count;
            if (tabCnt2 <= 3)
            {
                btnCloseTab.Visible = false;
            }
            else
            {
                btnCloseTab.Visible = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentTabPage = tabControl1.SelectedIndex;
            Console.WriteLine("The current tab page is: " + currentTabPage);

            switch ((sender as TabControl).SelectedIndex)
            {
                case 0:
                    btnSubmit.Enabled = true;
                    break;
                case 1:
                    btnSubmit.Enabled = false;
                    break;
                default:
                    tabControl1.SelectedIndex = currentTabPage;
                    break;
            }

            checkTabCount();

        }

        //private void tabControl1_Selected(object sender, TabControlEventArgs e)
        //{
        //    Console.WriteLine("The current tab page is: " + CurrentTabPage);
        //    switch ((sender as TabControl).SelectedIndex)
        //    {
        //        case 0:
        //            btnSubmit.Enabled = true;
        //            break;
        //        case 1:
        //            btnSubmit.Enabled = false;
        //            break;
        //        default:
        //            tabControl1.SelectedIndex = CurrentTabPage;
        //            break;
        //    }
        //}


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

                string[] columnNames = dataGridView1.Columns.Cast<DataGridViewColumn>()
                                       .Select(x => x.Name)
                                       .ToArray();

                // Must bind column names to global var for drop-downs later
                myGlobalVars.colList = columnNames;


                MyFunctions._engine.SetSymbol("df", df);

            }

            catch (Exception ex)
            {
                Console.WriteLine("Error loading data using R: " + ex.Message);
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
                //DataTable netData = new DataTable();


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

        public static DataFrame ConvertDT(DataTable dt)
        // Converts the .Net datatable to an R dataframe
        // Method 1
        {
            //double?[,] stringData = new double?[tab.Rows.Count, tab.Columns.Count];
            //double?[,] result = (tab.Rows, tab.Columns).AsEnumerable()
            //        .Select(row => Convert.ToDouble(row.Field<string>("column1"), System.Globalization.CultureInfo.InvariantCulture)).ToArray();

            //DataFrame dframe = null;
            DataFrame df = MyFunctions._engine.Evaluate("dframe=NULL").AsDataFrame();

            int irow = 0;
            foreach (DataRow row in dt.Rows)
            {
                NumericVector x = MyFunctions._engine.Evaluate("x=NULL").AsNumeric();
                int icol = 0;
                foreach (DataColumn col in dt.Columns)
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
                df = MyFunctions._engine.Evaluate("dframe= as.data.frame(rbind(dframe,x)) ").AsDataFrame();
                irow++;
            }

            MyFunctions._engine.SetSymbol("df", df);

            return df;
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

            // Adds column names to global list to be used when creating drop-downs
            myGlobalVars.colList = columnNames;

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
                MyFunctions._engine.SetSymbol("df", df);

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
            MyFunctions._engine.SetSymbol("df", df);

            return df;
        }

        public static DataFrame ConvertDT4(DataTable dt)
        // Converts the .Net datatable to an R dataframe
        // Method 4
        {
            //DataFrame dframe = null;
            DataFrame df = MyFunctions._engine.Evaluate("dframe=NULL").AsDataFrame();

            for (int i = 0; i < dt.Rows.Count; i++)
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    object o = dt.Rows[i].ItemArray[j];
                    Console.WriteLine("Data item = {0}", o.ToString());
                }

            //dframe = MyFunctions._engine.Evaluate("dframe= as.data.frame(rbind(dframe,x)) ").AsDataFrame();

            MyFunctions._engine.SetSymbol("df", df);

            return df;
        }

        public static class myGlobalVars
        {
            public static string[] colList = new string[] {};
            public static string xmlLibPath = @"C:\\";
            public static string xmlFileName = "";
            public static string rCode = "";

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

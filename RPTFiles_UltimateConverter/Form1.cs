using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPTFiles_UltimateConverter
{
    public partial class Form1 : Form
    {
        private string filename;
        private string filepath;
        private string file;
        private string filenameNoExtension;
        public Form1()
        {
            InitializeComponent();
            mywork.WorkerReportsProgress = true;
            // This event will be raised on the worker thread when the worker starts
            mywork.DoWork += new DoWorkEventHandler(Mywork_OnDoWork);
            // This event will be raised when we call ReportProgress
            mywork.ProgressChanged += new ProgressChangedEventHandler(Mywork_OnProgressChanged);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mywork.RunWorkerAsync();
        }
     

        private void Mywork_OnDoWork(object sender, DoWorkEventArgs e)
        {
            using (ReportClass rptH = new ReportClass())
            {
                CrystalDecisions.Shared.PdfRtfWordFormatOptions pdfOpts = CrystalDecisions.Shared.ExportOptions.CreatePdfRtfWordFormatOptions();
                CrystalDecisions.Shared.ExcelDataOnlyFormatOptions excelOptsDataOnly = CrystalDecisions.Shared.ExportOptions.CreateDataOnlyExcelFormatOptions();
                CrystalDecisions.Shared.ExcelFormatOptions excelOpts = CrystalDecisions.Shared.ExportOptions.CreateExcelFormatOptions();
                CrystalDecisions.Shared.MicrosoftMailDestinationOptions mailOpts = CrystalDecisions.Shared.ExportOptions.CreateMicrosoftMailDestinationOptions();
                CrystalDecisions.Shared.DiskFileDestinationOptions diskOpts = CrystalDecisions.Shared.ExportOptions.CreateDiskFileDestinationOptions();
                CrystalDecisions.Shared.ExportOptions exportOpts = new CrystalDecisions.Shared.ExportOptions();
                ReportDocument rpt = new ReportDocument();

                rpt.Load(file);

                pdfOpts.UsePageRange = false;
                exportOpts.ExportFormatOptions = pdfOpts;

                excelOptsDataOnly.UseWorksheetFunctionsForSummaries = true;
                excelOptsDataOnly.MaintainColumnAlignment = true;


                string MyRptName = rpt.FileName.ToString();
                //MyRptName = @"D:\ITProject\Tin\Tin2.xls";
                MyRptName = filepath + "\\" + filenameNoExtension + ".xls";
                //diskOpts.DiskFileName = "World Sales Report.pdf";
                diskOpts.DiskFileName = MyRptName;

                exportOpts.ExportDestinationOptions = diskOpts;

                //exportOpts.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.MicrosoftMail;
                exportOpts.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;

                //exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                exportOpts.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.ExcelRecord;

                rpt.Export(exportOpts);
                for (int i = 0; i <= 100; i=i+20)
                {
                    // Report progress to 'UI' thread
                    mywork.ReportProgress(i);
                    // Simulate long task
                    System.Threading.Thread.Sleep(100);
                }

            }
        }

        private void Mywork_OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Mywork_Onfinish(object sender, RunWorkerCompletedEventArgs e)
        {
            // TODO: do something with final calculation.
        }


        void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void panel1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var allowedExtensions = new[] {
                 ".rpt",".RPT"
                };
                string[] filess = (string[])e.Data.GetData(DataFormats.FileDrop);
                file = filess[0];
                var extension = Path.GetExtension(file);
                if (allowedExtensions.Contains(extension))
                {
                    filename = Path.GetFileName(file);
                    filepath = Path.GetDirectoryName(file);
                    filenameNoExtension = Path.GetFileNameWithoutExtension(file);
                }
                else
                {
                    DialogResult dialog1 = MessageBox.Show("Please choose .rpt file Tin???", "Are you seriously?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                DialogResult dialog = MessageBox.Show("might be something wrong", "Are you seriously?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
    }
}

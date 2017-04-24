using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using C1.WPF.DataGrid;
using System.IO;
using Microsoft.Win32;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;

namespace Infra.Wpf.Controls
{
    public partial class ExportExcel : Window
    {
        #region Methods
        
        public ExportExcel()
        {
            InitializeComponent();
        }
        
        public ExportExcel(C1DataGrid dataGrid) : this()
        {
            DataGrid = dataGrid;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ExportExecute();
        }
        
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }
        
        void ExportExecute()
        {
            rows = DataGrid.Rows.Where(x => x.Type == DataGridRowType.Group || x.Type == DataGridRowType.Item).ToList();
            if (rows.Count == 0)
                return;
            
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            sheet.CreateFreezePane(0, 2);
            if (DataGrid.FlowDirection == FlowDirection.RightToLeft)
                sheet.IsRightToLeft = true;
            
            for (int i = 0; i <= DataGrid.GroupedColumns.Count(); i++)           
                sheet.SetColumnWidth(i, 1024);
            
            PrepareStyle(workbook);
            
            CreateExcelHeader(sheet);
            
            worker = new BackgroundWorker();
            worker.DoWork += CreateExcelItem;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync(sheet);
        }
        
        private void PrepareStyle(IWorkbook workbook)
        {
            #region Header Style
            
            headerStyle = workbook.CreateCellStyle();
            headerStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;
            headerStyle.BorderTop = BorderStyle.Medium;
            headerStyle.BorderBottom = BorderStyle.Medium;
            headerStyle.BorderLeft = BorderStyle.Thin;
            headerStyle.BorderRight = BorderStyle.Thin;
            
            headerLStyle = workbook.CreateCellStyle();
            headerLStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerLStyle.FillPattern = FillPattern.SolidForeground;
            headerLStyle.BorderTop = BorderStyle.Medium;
            headerLStyle.BorderBottom = BorderStyle.Medium;
            headerLStyle.BorderLeft = BorderStyle.Medium;
            headerLStyle.BorderRight = BorderStyle.Thin;
            
            headerRStyle = workbook.CreateCellStyle();
            headerRStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerRStyle.FillPattern = FillPattern.SolidForeground;
            headerRStyle.BorderTop = BorderStyle.Medium;
            headerRStyle.BorderBottom = BorderStyle.Medium;
            headerRStyle.BorderLeft = BorderStyle.Thin;
            headerRStyle.BorderRight = BorderStyle.Medium;
            
            headerLRStyle = workbook.CreateCellStyle();
            headerLRStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            headerLRStyle.FillPattern = FillPattern.SolidForeground;
            headerLRStyle.BorderTop = BorderStyle.Medium;
            headerLRStyle.BorderBottom = BorderStyle.Medium;
            headerLRStyle.BorderLeft = BorderStyle.Medium;
            headerLRStyle.BorderRight = BorderStyle.Medium;
            
            #endregion

            #region Item Style
            
            itemStyle = workbook.CreateCellStyle();
            itemStyle.BorderBottom = BorderStyle.Thin;
            itemStyle.BorderLeft = BorderStyle.Thin;
            itemStyle.BorderRight = BorderStyle.Thin;
            
            itemLStyle = workbook.CreateCellStyle();
            itemLStyle.BorderBottom = BorderStyle.Thin;
            itemLStyle.BorderLeft = BorderStyle.Medium;
            itemLStyle.BorderRight = BorderStyle.Thin;
            
            itemRStyle = workbook.CreateCellStyle();
            itemRStyle.BorderBottom = BorderStyle.Thin;
            itemRStyle.BorderLeft = BorderStyle.Thin;
            itemRStyle.BorderRight = BorderStyle.Medium;
            
            itemLRStyle = workbook.CreateCellStyle();
            itemLRStyle.BorderBottom = BorderStyle.Thin;
            itemLRStyle.BorderLeft = BorderStyle.Medium;
            itemLRStyle.BorderRight = BorderStyle.Medium;
            
            itemBStyle = workbook.CreateCellStyle();
            itemBStyle.BorderBottom = BorderStyle.Medium;
            itemBStyle.BorderLeft = BorderStyle.Thin;
            itemBStyle.BorderRight = BorderStyle.Thin;
            
            itemBLStyle = workbook.CreateCellStyle();
            itemBLStyle.BorderBottom = BorderStyle.Medium;
            itemBLStyle.BorderLeft = BorderStyle.Medium;
            itemBLStyle.BorderRight = BorderStyle.Thin;
            
            itemBRStyle = workbook.CreateCellStyle();
            itemBRStyle.BorderBottom = BorderStyle.Medium;
            itemBRStyle.BorderLeft = BorderStyle.Thin;
            itemBRStyle.BorderRight = BorderStyle.Medium;
            
            itemBLRStyle = workbook.CreateCellStyle();
            itemBLRStyle.BorderBottom = BorderStyle.Medium;
            itemBLRStyle.BorderLeft = BorderStyle.Medium;
            itemBLRStyle.BorderRight = BorderStyle.Medium;

            #endregion
            
            #region Group Style
            
            groupStyle = workbook.CreateCellStyle();
            groupStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            groupStyle.FillPattern = FillPattern.SolidForeground;
            groupStyle.BorderTop = BorderStyle.Medium;
            groupStyle.BorderBottom = BorderStyle.Medium;
            groupStyle.BorderLeft = BorderStyle.Medium;
            groupStyle.BorderRight = BorderStyle.Medium;
            if (DataGrid.FlowDirection == FlowDirection.RightToLeft)
                groupStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;

            #endregion
        }
            
        private void CreateExcelHeader(ISheet sheet)
        {
            int groupColumnsCount = DataGrid.GroupedColumns.Count();
                                                  
            var columnIndex = DataGrid.Columns
                                      .Where(x => !(x is DataGridImageColumn) &&
                                                  !(x is CustomButtonColumn) &&
                                                  !(x is CustomHyperlinkColumn))
                                      .Select(x => x.Index);
            
            IRow header = sheet.CreateRow(1);          
                
            for (int i = (-1); i < columnIndex.Count(); i++)
            {
                ICell cell = header.CreateCell(i + groupColumnsCount + 2);
                if (i == (-1))
                    cell.SetCellValue("#");
                else
                    cell.SetCellValue(DataGrid.Columns[columnIndex.ElementAt(i)].Header.ToString());
                
                bool left = false;
                bool right = false;
                if (i == (-1))
                    left = true;
                if (i == columnIndex.Count() - 1)
                    right = true;
                
                if (left == true && right == true)
                    cell.CellStyle = headerLRStyle;
                else if (left == true)
                    cell.CellStyle = headerLStyle;
                else if (right == true)
                    cell.CellStyle = headerRStyle;
                else
                    cell.CellStyle = headerStyle;
            }
        }
            
        private void CreateExcelItem(object sender, DoWorkEventArgs e)
        {
            ISheet sheet = (ISheet)e.Argument;
            
            int groupColumnsCount = DataGrid.GroupedColumns.Count();
            
            int counter = 0;
                                                  
            var columnIndex = DataGrid.Columns
                                      .Where(x => !(x is DataGridImageColumn) &&
                                                  !(x is CustomButtonColumn) &&
                                                  !(x is CustomHyperlinkColumn))
                                      .Select(x => x.Index);
                
            for (int i = 0; i < rows.Count; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                    
                this.Dispatcher.Invoke(() =>
                {
                    if (rows[i].Type == DataGridRowType.Item)
                    {
                        counter++;
                        IRow rowItem = sheet.CreateRow(i + 2);
                        bool bottom = false;
                        if (i == rows.Count - 1)
                            bottom = true;
                            
                        for (int j = (-1); j < columnIndex.Count(); j++)
                        {
                            bool left = false;
                            bool right = false;
                            if (j == (-1))
                                left = true;
                            if (j == columnIndex.Count() - 1)
                                right = true;
                                
                            ICell cell = rowItem.CreateCell(j + groupColumnsCount + 2);
                            if (j == (-1))
                                cell.SetCellValue(counter.ToString());
                            else
                                cell.SetCellValue(DataGrid[rows[i], DataGrid.Columns[columnIndex.ElementAt(j)]].Text);
                                
                            if (bottom == true)
                            {
                                if (left == true && right == true)
                                    cell.CellStyle = itemBLRStyle;
                                else if (left == true)
                                    cell.CellStyle = itemBLStyle;
                                else if (right == true)
                                    cell.CellStyle = itemBRStyle;
                                else
                                    cell.CellStyle = itemBStyle;
                            }
                            else
                            {
                                if (left == true && right == true)
                                    cell.CellStyle = itemLRStyle;
                                else if (left == true)
                                    cell.CellStyle = itemLStyle;
                                else if (right == true)
                                    cell.CellStyle = itemRStyle;
                                else
                                    cell.CellStyle = itemStyle;
                            }
                        }
                    }
                    else
                    {
                        IRow rowItem = sheet.CreateRow(i + 2);
                        for (int t = rows[i].Level + 1; t <= columnIndex.Count() + groupColumnsCount + 1; t++)
                        {
                            ICell groupCell = rowItem.CreateCell(t);
                            if (t == rows[i].Level + 1)
                                groupCell.SetCellValue((rows[i] as DataGridGroupRow).GetGroupText());
                            rowItem.GetCell(t).CellStyle = groupStyle;
                        }
                    
                        sheet.AddMergedRegion(new CellRangeAddress(i + 2, i + 2, rows[i].Level + 1 ,
                            columnIndex.Count() + groupColumnsCount + 1));
                    }
                });
                (sender as BackgroundWorker).ReportProgress((int)(((float)(i + 1) / (float)(rows.Count)) * 100));
            }
            
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            dlg.DefaultExt = "xlsx";
            dlg.Filter = "Excel Workbook (*.xlsx) | *.xlsx";
                
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    using (FileStream sw = File.Create(dlg.FileName))
                        sheet.Workbook.Write(sw);
                
                    System.Diagnostics.Process.Start("Excel.exe", "\"" + dlg.FileName + "\"");
                }
                catch (Win32Exception)
                {
                }
                catch (Exception)
                {
                    MessageBox.Show("فایل مورد نظر در دسترس نیست.", "خطا", MessageBoxButton.OK,
                        MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
                }
            }

            this.Dispatcher.Invoke(() => this.Close());
        }
            
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
            
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            worker.CancelAsync();
        }
        
        #endregion
        
        #region Properties
        
        private C1DataGrid DataGrid;
        
        private BackgroundWorker worker;
        
        private List<C1.WPF.DataGrid.DataGridRow> rows;
        
        ICellStyle headerStyle;
        ICellStyle headerLStyle;
        ICellStyle headerRStyle;
        ICellStyle headerLRStyle;
        
        ICellStyle itemStyle;
        ICellStyle itemLStyle;
        ICellStyle itemRStyle;
        ICellStyle itemLRStyle;
        ICellStyle itemBStyle;
        ICellStyle itemBLStyle;
        ICellStyle itemBRStyle;
        ICellStyle itemBLRStyle;
    
        ICellStyle groupStyle;

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using NexusPOS.Models;

namespace NexusPOS.Services
{
    public class PrintService
    {
        public void PrintInvoice(Invoice invoice, IEnumerable<InvoiceItem> items)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Create FlowDocument
                FlowDocument doc = new FlowDocument();
                doc.PageWidth = 302; // ~80mm (approx 3.15 inches * 96 dpi)
                doc.FontFamily = new FontFamily("Segoe UI");
                doc.FontSize = 12;
                doc.FlowDirection = FlowDirection.RightToLeft;
                doc.TextAlignment = TextAlignment.Center;
                doc.PagePadding = new Thickness(5);

                // Header
                Paragraph header = new Paragraph();
                header.Inlines.Add(new Run("فروشگاه نکسوس") { FontWeight = FontWeights.Bold, FontSize = 16 });
                header.Inlines.Add(new LineBreak());
                header.Inlines.Add(new Run($"تاریخ: {invoice.Date:yyyy/MM/dd HH:mm}"));
                header.Inlines.Add(new LineBreak());
                header.Inlines.Add(new Run($"شماره فاکتور: {invoice.InvoiceNo}"));
                doc.Blocks.Add(header);

                // Separator
                doc.Blocks.Add(new Paragraph(new Run("--------------------------------")) { TextAlignment = TextAlignment.Center, Margin = new Thickness(0) });

                // Table
                Table table = new Table();
                table.CellSpacing = 0;
                // Columns: Item (2*), Qty (0.8*), Price (1.5*)
                table.Columns.Add(new TableColumn() { Width = new GridLength(2, GridUnitType.Star) });
                table.Columns.Add(new TableColumn() { Width = new GridLength(0.8, GridUnitType.Star) });
                table.Columns.Add(new TableColumn() { Width = new GridLength(1.5, GridUnitType.Star) });

                TableRowGroup rowGroup = new TableRowGroup();

                // Header Row
                TableRow headerRow = new TableRow();
                headerRow.Cells.Add(new TableCell(new Paragraph(new Run("کالا")) { FontWeight = FontWeights.Bold }));
                headerRow.Cells.Add(new TableCell(new Paragraph(new Run("تعداد")) { FontWeight = FontWeights.Bold }));
                headerRow.Cells.Add(new TableCell(new Paragraph(new Run("قیمت")) { FontWeight = FontWeights.Bold }));
                rowGroup.Rows.Add(headerRow);

                // Data Rows
                foreach (var item in items)
                {
                    TableRow row = new TableRow();
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Product?.Name ?? "-"))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Quantity.ToString()))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.TotalPrice.ToString("N0")))));
                    rowGroup.Rows.Add(row);
                }

                table.RowGroups.Add(rowGroup);
                doc.Blocks.Add(table);

                // Separator
                doc.Blocks.Add(new Paragraph(new Run("--------------------------------")) { TextAlignment = TextAlignment.Center, Margin = new Thickness(0) });

                // Footer
                Paragraph footer = new Paragraph();
                footer.Inlines.Add(new Run($"جمع کل: {invoice.TotalAmount:N0}"));
                footer.Inlines.Add(new LineBreak());
                footer.Inlines.Add(new Run($"تخفیف: {invoice.Discount:N0}"));
                footer.Inlines.Add(new LineBreak());
                footer.Inlines.Add(new Run($"قابل پرداخت: {invoice.FinalAmount:N0}") { FontWeight = FontWeights.Bold, FontSize = 14 });
                footer.Inlines.Add(new LineBreak());
                footer.Inlines.Add(new LineBreak());
                footer.Inlines.Add(new Run("از خرید شما سپاسگزاریم"));
                doc.Blocks.Add(footer);

                // Print
                IDocumentPaginatorSource idpSource = doc;
                try {
                    printDialog.PrintDocument(idpSource.DocumentPaginator, "NexusPOS Receipt");
                } catch { /* Handle print errors */ }
            }
        }
    }
}
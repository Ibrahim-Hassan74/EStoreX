using EStoreX.Core.ServiceContracts.Common;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;
using System.Reflection;
using OfficeOpenXml;
using System.Text;

namespace EStoreX.Core.Services.Common
{
    public class ExportService : IExportService
    {
        private string FormatValue(object value)
        {
            if (value == null) return "";

            if (value is string s)
                return s;

            if (value is System.Collections.IEnumerable enumerable && !(value is string))
            {
                var items = new List<string>();
                foreach (var v in enumerable)
                {
                    if (v == null) continue;

                    var type = v.GetType();
                    if (type.IsPrimitive || v is string || v is decimal || v is DateTime || v is Guid)
                    {
                        items.Add(v.ToString() ?? "");
                    }
                    else
                    {
                        var nestedProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        var nestedValues = nestedProps.Select(np => $"{np.Name}: {np.GetValue(v) ?? ""}");
                        items.Add(string.Join(", ", nestedValues));
                    }
                }
                return string.Join(" | ", items);
            }

            return value.ToString();
        }

        /// <inheritdoc/>
        public byte[] ExportToCsv<T>(IEnumerable<T> data)
        {
            var sb = new StringBuilder();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            sb.AppendLine(string.Join(";", props.Select(p => p.Name)));

            // Rows
            foreach (var item in data)
            {
                var values = props.Select(p =>
                {
                    var value = FormatValue(p.GetValue(item, null));
                    if (value.Contains(";") || value.Contains("\"") || value.Contains("\n"))
                    {
                        value = "\"" + value.Replace("\"", "\"\"") + "\"";
                    }
                    return value;
                });

                sb.AppendLine(string.Join(";", values));
            }

            var utf8Bom = new UTF8Encoding(true);
            return utf8Bom.GetBytes(sb.ToString());
        }

        /// <inheritdoc/>
        public byte[] ExportToExcel<T>(IEnumerable<T> data)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");
                var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // Header
                for (int i = 0; i < props.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = props[i].Name;
                }

                using (var range = worksheet.Cells[1, 1, 1, props.Length])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Rows
                int row = 2;
                foreach (var item in data)
                {
                    for (int i = 0; i < props.Length; i++)
                    {
                        worksheet.Cells[row, i + 1].Value = FormatValue(props[i].GetValue(item, null));
                    }
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        /// <inheritdoc/>
        public byte[] ExportToPdf<T>(IEnumerable<T> data)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new PdfWriter(ms))
                {
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // Table with number of columns = properties count
                    var table = new Table(props.Length, true);

                    // Header
                    foreach (var prop in props)
                    {
                        table.AddHeaderCell(new Cell()
                            .Add(new Paragraph(prop.Name))
                            .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE)
                            .SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE)
                            .SetTextAlignment(TextAlignment.CENTER));
                    }

                    // Rows
                    foreach (var item in data)
                    {
                        foreach (var prop in props)
                        {
                            table.AddCell(new Cell()
                                .Add(new Paragraph(FormatValue(prop.GetValue(item, null))))
                                .SetTextAlignment(TextAlignment.LEFT));
                        }
                    }

                    document.Add(table);
                    document.Close();
                }

                return ms.ToArray();
            }
        }
    }
}

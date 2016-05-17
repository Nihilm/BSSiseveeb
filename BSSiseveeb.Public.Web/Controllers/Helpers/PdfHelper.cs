using BSSiseveeb.Core.Domain;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Collections.Generic;
using System.IO;

namespace BSSiseveeb.Public.Web.Controllers.Helpers
{
    public static class PdfHelper
    {
        public static MemoryStream BuildRequestsPdf(IEnumerable<Request> requests, string start, string end, RequestStatus status)
        {
            var document = new Document();
            document.Info.Title = $"{status} requests from {start} to {end}";

            var style = document.Styles["Normal"];
            style.Font.Name = "Verdana";
            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style = document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;
            style = document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);

            var section = document.AddSection();

            var paragraph = section.AddParagraph();
            paragraph.Style = "Reference";
            paragraph.AddFormattedText($"{status} requests from {start} to {end}", TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddDateField("dd.MM.yyyy");

            var table = section.AddTable();
            table.Style = "Table";
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            var column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            var row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("Töötarve");
            row.Cells[1].AddParagraph("Lisa info");
            row.Cells[2].AddParagraph("Taotluse kuupäev");
            row.Cells[3].AddParagraph("Taotleja");

            table.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);

            foreach (var request in requests)
            {
                var itemrow = table.AddRow();
                var employee = request.Employee.Name;
                itemrow.TopPadding = 1.5;
                itemrow.Cells[0].AddParagraph(request.Req);
                itemrow.Cells[1].AddParagraph(request.Description);
                itemrow.Cells[2].AddParagraph(request.TimeStamp.ToString("d"));
                itemrow.Cells[3].AddParagraph(employee);
                table.SetEdge(0, table.Rows.Count - 1, 4, 1, Edge.Box, BorderStyle.Single, 0.75);
            }

            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "requests.mdddl");
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;

            renderer.RenderDocument();
            var stream = new MemoryStream();
            renderer.PdfDocument.Save(stream, false);

            return stream;
        }
    }
}
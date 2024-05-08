using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace VehicleAlert.Core.PdfCreator
{
    public class RaportDocument : IDocument
    {
        public RaportModel Model { get; }


        public RaportDocument(RaportModel model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;


        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.MarginTop(50);

                page.Header().BorderBottom(5).BorderColor(Colors.White).Element(ComposeHeader);

                page.Content().Element(ComposeContent);

            });
        }

        public void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().Italic();

            container.Row(row =>
            {

                row.RelativeItem().Column(column =>
                {
                    column.Item().AlignCenter().Text($"Wykaz czynności warsztatowych #").Style(titleStyle);

                    column.Item().AlignRight().Text(text =>
                    {
                        text.Span($"{Model.GeneratingDate}");
                    });
                    column.Item().Text(text =>
                    {
                        text.Span("Marka: ").SemiBold();
                        text.Span($"{Model.Name}");

                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Numer rej.: ").SemiBold();
                        text.Span($"{Model.Plate}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Przebieg: ").SemiBold();
                        text.Span($"{Model.Course}");
                    });


                });

                //row.ConstantItem(100).Height(50);
            });

        }

        public void ComposeContent(IContainer container)
        {
            container
                .PaddingVertical(20)
                .Height(192)
                .Background(Colors.Grey.Lighten3)
                .AlignCenter()
                .Element(ComposeTable);

        }
        void ComposeTable(IContainer container)
        {
            var i = 0;

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });


                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).AlignCenter().Text("#");
                    header.Cell().Element(CellStyle).Text("Opis");
                    header.Cell().Element(CellStyle).AlignCenter().Text("Ostatni serwis");
                    header.Cell().Element(CellStyle).AlignCenter().Text("Interwał");
                    header.Cell().Element(CellStyle).AlignCenter().Text("Pozostało");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });


                foreach (var item in Model.Actions)
                {
                    table.Cell().Element(CellStyle).AlignCenter().Text(i + 1);
                    table.Cell().Element(CellStyle).PaddingLeft(5).Text($"{item.Description}");
                    table.Cell().Element(CellStyle).AlignCenter().Text($"{item.LastActionCourse}");
                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Interval);
                    table.Cell().Element(CellStyle).AlignCenter().Text($"{item.Interval}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderVertical(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                    i++;
                }
            });
        }
    }
}

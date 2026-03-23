using BackEnd.ModelClasses;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BackEnd.Reports
{
    public static class ReportGenerator
    {
        public static void GenerateListReport(RootManager _ReportDataSource, string _FilePath, byte[] _ImageData)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page( page =>
                {
                    page.Margin(25);
                    page.MarginTop(10);
                    page.Size(PageSizes.Letter);

                    page.Header().ShowOnce().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });

                void ComposeHeader(IContainer _Container)
                {
                    int ImageHeight = 110;

                    _Container.Column(TitleColumn => 
                    {
                        TitleColumn.Item().Height(50).BorderBottom(3).BorderAlignmentInside().AlignCenter().AlignTop().Text("HomeStash Inventory Report").SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                        TitleColumn.Item().Row(InformationRow =>
                        {
                            InformationRow.RelativeItem().Column(InformationColumn =>
                            {
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text("Building : Home");
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text("Report Date : 03/22/2026");
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text("Building Item Count : 100");
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text("Building Value : $150.00");
                            });

                            InformationRow.AutoItem().Width(200).Height(ImageHeight).AlignCenter().AlignMiddle().Image(_ImageData).FitArea();
                        });
                    });
                }

                void ComposeContent(IContainer _Container)
                {
                    _Container
                        .PaddingTop(15)
                        .AlignCenter()
                        .Table(CurrenTable => 
                        {
                            CurrenTable.ColumnsDefinition(CurrentColumns => 
                            {
                                CurrentColumns.ConstantColumn(100);
                                CurrentColumns.ConstantColumn(100);
                                CurrentColumns.RelativeColumn();
                                CurrentColumns.ConstantColumn(100);
                                CurrentColumns.ConstantColumn(100);
                            });

                            CurrenTable.Header( TableHeader =>
                            {
                                TableHeader.Cell().Element(HeaderStyle).Text("Name").SemiBold();
                                TableHeader.Cell().Element(HeaderStyle).Text("Location").SemiBold();
                                TableHeader.Cell().Element(HeaderStyle).Text("Description").SemiBold();
                                TableHeader.Cell().Element(HeaderStyle).Text("Value").SemiBold();
                                TableHeader.Cell().Element(HeaderStyle).Text("Qty").SemiBold();

                                IContainer HeaderStyle(IContainer _Container)
                                {
                                    return _Container
                                        .Background(Colors.Blue.Darken2)
                                        .DefaultTextStyle(x => x.FontColor(Colors.White).Bold())
                                        .PaddingVertical(8)
                                        .PaddingHorizontal(16)
                                        .AlignCenter();
                                }
                            });

                            for (uint CurrentDataRow = 1; CurrentDataRow < 50; CurrentDataRow++)
                            {
                                CurrenTable.Cell().Row(CurrentDataRow).Column(1).Element(CellStyle).Text(CurrentDataRow.ToString());
                                CurrenTable.Cell().Row(CurrentDataRow).Column(2).Element(CellStyle).Text((CurrentDataRow + 1).ToString());
                                CurrenTable.Cell().Row(CurrentDataRow).Column(3).Element(CellStyle).Text((CurrentDataRow + 2).ToString());
                                CurrenTable.Cell().Row(CurrentDataRow).Column(4).Element(CellStyle).Text((CurrentDataRow + 3).ToString());
                                CurrenTable.Cell().Row(CurrentDataRow).Column(5).Element(CellStyle).Text((CurrentDataRow + 4).ToString());

                                IContainer CellStyle(IContainer _Container)
                                {
                                    Color BackgroundColor = CurrentDataRow % 2 == 0
                                        ? Colors.Blue.Lighten5
                                        : Colors.Blue.Lighten4;

                                    return _Container
                                        .DefaultTextStyle(x => x.FontSize(10))
                                        .BorderVertical(1)
                                        .BorderColor(Colors.Blue.Accent1)
                                        .Background(BackgroundColor)
                                        .PaddingVertical(1)
                                        .AlignCenter();

                                }
                            }

                        });
                }
            })
            .GeneratePdf(_FilePath);
        }
    }
}

using BackEnd.ModelClasses;
using BackEnd.ModelInterfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BackEnd.Reports
{
    public static class ReportGenerator
    {
        public static void GenerateListReport(RootManager _ReportDataSource, string _FilePath, byte[] _ImageData)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
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
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text($"Building : {_ReportDataSource.ActiveUser.ActiveBuilding.Name}");
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text($"Report Date : {DateTime.Today.ToString("dd/MM/yyyy")}");
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text($"Building Item Count : {_ReportDataSource.ActiveUser.ActiveBuilding.TotalItemCount()}");
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text($"Building Value : {_ReportDataSource.ActiveUser.ActiveBuilding.TotalItemValue():C2}");
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
                        .Table(CurrentTable =>
                        {
                            CurrentTable.ColumnsDefinition(CurrentColumns =>
                           {
                               CurrentColumns.RelativeColumn();
                               CurrentColumns.RelativeColumn();
                               CurrentColumns.RelativeColumn();
                               CurrentColumns.RelativeColumn(0.5f);
                               CurrentColumns.RelativeColumn(0.5f);
                           });

                            CurrentTable.Header(TableHeader =>
                            {
                                TableHeader.Cell().Element(HeaderStyle).Text("Name").SemiBold();
                                TableHeader.Cell().Element(HeaderStyle).Text("Location").SemiBold();
                                TableHeader.Cell().Element(HeaderStyle).Text("Description").SemiBold();
                                TableHeader.Cell().Element(HeaderStyle).Text(text =>
                                {
                                    text.Span("Value\n").SemiBold();
                                    text.Span("(Unit)").FontSize(10);
                                });


                                TableHeader.Cell().Element(HeaderStyle).Text(text =>
                                {
                                    text.Span("Qty").SemiBold();
                                    text.Span("\n(Per Location)").FontSize(9);
                                });

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

                            List<ReportObject> ReportList = GetReportList(_ReportDataSource);

                            uint i = 0;

                            foreach (ReportObject CurrentReportObject in ReportList)
                            {
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Name);
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Location);
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Description);
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Value.ToString("C2"));
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Quantity.ToString());

                                i++;

                                IContainer CellStyle(IContainer _Container)
                                {
                                    Color BackgroundColor = i % 2 == 0
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

        private static List<ReportObject> GetReportList(RootManager _ReportDataSource)
        {
            List<ReportObject> ReturnList = new List<ReportObject>();

            Building CurrentBuilding = _ReportDataSource.ActiveUser.ActiveBuilding;

            PopulateStorageReportObjects(CurrentBuilding.CurrentStorage as Storage, ref ReturnList);

            foreach (Room CurrentRoom in CurrentBuilding.RoomList)
            {
                PopulateStorageReportObjects(CurrentRoom.CurrentStorage as Storage, ref ReturnList);
            }


            return ReturnList;
        }
        private static void PopulateStorageReportObjects(Storage _SourceStorage, ref List<ReportObject> _ReportObjectList)
        {
            foreach (IStored CurrentItem in _SourceStorage.StoredItems)
            {
                ReportObject NewReportObject = new ReportObject
                {
                    Name = CurrentItem.Name,
                    Description = CurrentItem.Description,
                    Quantity = CurrentItem.Quantity,
                    Value = CurrentItem.Value,
                    Location = CurrentItem.ImmediateParent.Name
                };

                _ReportObjectList.Add(NewReportObject);

                if (CurrentItem.GetType() == typeof(Container))
                {
                    PopulateStorageReportObjects((CurrentItem as Container).CurrentStorage as Storage, ref _ReportObjectList);
                }
            }
        }
    }
}

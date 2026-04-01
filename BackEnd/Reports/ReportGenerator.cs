using BackEnd.ModelClasses;
using BackEnd.ModelInterfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BackEnd.Reports
{
    /// <summary>
    /// Static Class Responsible For Generating Inventory Reports For A Building
    /// </summary>
    public static class ReportGenerator
    {
        /// <summary>
        /// Generates A PDF Report Of The Active Building, For The Active User From The Given RootManager Data Source
        /// </summary>
        /// <param name="_ReportDataSource">The RootManager Instance Containing The Report Data</param>
        /// <param name="_FilePath">The Output File Path For The Generated PDF</param>
        /// <param name="_ImageData">Byte Array Of The Image To Include In The Header</param>
        public static void GenerateListReport(RootManager _ReportDataSource, string _FilePath, byte[] _ImageData)
        {
            // Setting The License Setting. Required To Use QuestPDF. Community Is The Valid Option For My Context
            QuestPDF.Settings.License = LicenseType.Community;

            // Create The Document Object To Be Converted To A PDF
            Document.Create(container =>
            {
                // Creates A Page Set For The Document Object
                container.Page(page =>
                {
                    // Set The Page Settings
                    page.Margin(25);
                    page.MarginTop(10);
                    page.Size(PageSizes.Letter);

                    // Create A Header And Specify That Is Only Shown Once (Only Showing On The First Page)
                    // Using "Element" Allows Defining More Complex Header Settings In A Separate Method, Instead Of Inline
                    page.Header().ShowOnce().Element(ComposeHeader);

                    // Create The Content Block That Contains The Report List
                    // Using "Element" Allows Defining More Complex Content Settings In A Separate Method, Instead Of Inline
                    page.Content().Element(ComposeContent);

                    // Create A Footer
                    // Specifies To Footer Details Inline
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });

                /// <summary>
                /// Composes The Header Of The Report With Title, Information, And Image
                /// </summary>
                /// <param name="_Container">The Container For Header Elements</param>
                void ComposeHeader(IContainer _Container)
                {
                    int ImageHeight = 110;

                    _Container.Column(TitleColumn =>
                    {
                        // Top Banner Text
                        TitleColumn.Item().Height(50).BorderBottom(3).BorderAlignmentInside().AlignCenter().AlignTop().Text("HomeStash Inventory Report").SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                        TitleColumn.Item().Row(InformationRow =>
                        {
                            // Report Summary Information
                            InformationRow.RelativeItem().Column(InformationColumn =>
                            {
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text($"Building : {_ReportDataSource.ActiveUser.ActiveBuilding.Name}");
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text($"Report Date : {DateTime.Today.ToString("dd/MM/yyyy")}");
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text($"Building Item Count : {_ReportDataSource.ActiveUser.ActiveBuilding.TotalItemCount()}");
                                InformationColumn.Item().Height(ImageHeight / 4).AlignMiddle().Text($"Building Value : {_ReportDataSource.ActiveUser.ActiveBuilding.TotalItemValue():C2}");
                            });

                            // Header Image
                            InformationRow.AutoItem().Width(200).Height(ImageHeight).AlignCenter().AlignMiddle().Image(_ImageData).FitArea();
                        });
                    });
                }

                /// <summary>
                /// Composes The Main Table Content Of The Report
                /// </summary>
                /// <param name="_Container">The Container For Table Elements</param>
                void ComposeContent(IContainer _Container)
                {
                    _Container
                        .PaddingTop(15)
                        .AlignCenter()
                        .Table(CurrentTable =>
                        {
                            // Define the Size Of The Report Columns
                            CurrentTable.ColumnsDefinition(CurrentColumns =>
                           {
                               CurrentColumns.RelativeColumn();
                               CurrentColumns.RelativeColumn();
                               CurrentColumns.RelativeColumn();
                               CurrentColumns.RelativeColumn(0.5f);
                               CurrentColumns.RelativeColumn(0.5f);
                           });

                            // Define The Header Row For The Table
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

                                // Reusable Function To Define Header Style
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

                            // Generate Report Objects From Live RootManager Instance
                            List<ReportObject> ReportList = GetReportList(_ReportDataSource);

                            uint i = 0;

                            // Populate Table Rows
                            foreach (ReportObject CurrentReportObject in ReportList)
                            {
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Name);
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Location);
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Description);
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Value.ToString("C2"));
                                CurrentTable.Cell().Element(CellStyle).Text(CurrentReportObject.Quantity.ToString());

                                i++;

                                // Reusable Function To Define Row Style
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
            .GeneratePdf(_FilePath); // Generates The PDF From The Document Object At The Specified File Path
        }

        /// <summary>
        /// Generates A Flat List Of Report Objects From The Live RootManager Instance
        /// </summary>
        /// <param name="_ReportDataSource">The RootManager Containing The Report Data</param>
        /// <returns>List Of ReportObject Representing All Items In The Report</returns>
        private static List<ReportObject> GetReportList(RootManager _ReportDataSource)
        {
            List<ReportObject> ReturnList = new List<ReportObject>();

            // Set The Building The Report Will Be Generated For
            Building CurrentBuilding = _ReportDataSource.ActiveUser.ActiveBuilding;

            // Add All Items Stored In The Building To The List Of Report Objects
            PopulateStorageReportObjects(CurrentBuilding.CurrentStorage as Storage, ref ReturnList);

            foreach (Room CurrentRoom in CurrentBuilding.RoomList)
            {
                // Add All Items Stored In The Room To The List Of Report Objects
                PopulateStorageReportObjects(CurrentRoom.CurrentStorage as Storage, ref ReturnList);
            }

            return ReturnList;
        }

        /// <summary>
        /// Recursively Populates The Report Object List From The Given Storage
        /// </summary>
        /// <param name="_SourceStorage">The Source Storage Containing Items Or Containers</param>
        /// <param name="_ReportObjectList">The List To Populate With Report Objects</param>
        private static void PopulateStorageReportObjects(Storage _SourceStorage, ref List<ReportObject> _ReportObjectList)
        {
            foreach (IStored CurrentItem in _SourceStorage.StoredItems)
            {
                // Create A Report Object For The Current Item
                ReportObject NewReportObject = new ReportObject
                {
                    Name = CurrentItem.Name,
                    Description = CurrentItem.Description,
                    Quantity = CurrentItem.Quantity,
                    Value = CurrentItem.Value,
                    Location = CurrentItem.ImmediateParent.Name
                };

                // Add The Report Object To The List
                _ReportObjectList.Add(NewReportObject);

                // If The Current Item Is A Container, Recursively Populate Its Contents
                if (CurrentItem.GetType() == typeof(Container))
                {
                    PopulateStorageReportObjects((CurrentItem as Container).CurrentStorage as Storage, ref _ReportObjectList);
                }
            }
        }
    }
}

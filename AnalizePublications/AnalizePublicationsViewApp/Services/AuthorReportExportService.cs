using AnalyzePublicationsContracts.ViewModels;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using QuestDocument = QuestPDF.Fluent.Document;
using WordDocument = DocumentFormat.OpenXml.Wordprocessing.Document;

namespace AnalyzePublicationsWeb.Services
{
    public class AuthorReportExportService
    {
        public byte[] ExportToPdf(AuthorReportViewModel report)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return QuestDocument.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Отчет по автору: {report.Author.FullName}")
                        .SemiBold().FontSize(16).AlignCenter();

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(20);

                            // Информация об авторе
                            column.Item().Text("Информация об авторе").Bold().FontSize(14);
                            column.Item().Text($"Полное имя: {report.Author.FullName}");
                            column.Item().Text($"Организация: {report.Author.Affiliation}");
                            column.Item().Text($"Email: {report.Author.Email}");
                            column.Item().Text($"Дата формирования отчета: {report.ReportDate:dd.MM.yyyy HH:mm}");

                            // Метрики
                            column.Item().Text("Метрики").Bold().FontSize(14);
                            column.Item().Text($"Количество публикаций: {report.Author.PublicationsCount}");
                            column.Item().Text($"Количество соавторов: {report.Author.CoauthorsCount}");
                            column.Item().Text($"Общее количество цитирований: {report.Author.CitationCount}");
                            column.Item().Text($"H-индекс: {report.Author.HIndex}");
                            column.Item().Text($"i10-индекс: {report.Author.I10Index}");
                            column.Item().Text($"Средние цитирования на публикацию: {report.Author.AverageCitationsPerPublication:F2}");
                            column.Item().Text($"Среднее число страниц: {report.Author.AveragePages:F2}");
                            column.Item().Text($"Общее количество страниц: {report.Author.TotalPages}");
                            column.Item().Text($"Индекс продуктивности: {report.Author.ProductivityIndex:F2}");
                            column.Item().Text($"Максимальное количество цитирований: {report.Author.MaxCitations}");
                            column.Item().Text($"Минимальное количество цитирований: {report.Author.MinCitations}");

                            // Статистика по типам
                            if (report.PublicationsByType.Any())
                            {
                                column.Item().Text("Статистика по типам публикаций").Bold().FontSize(14);
                                foreach (var item in report.PublicationsByType)
                                {
                                    column.Item().Text($"{item.Key}: {item.Value}");
                                }
                            }

                            // Статистика по годам
                            if (report.PublicationsByYear.Any())
                            {
                                column.Item().Text("Статистика по годам").Bold().FontSize(14);
                                foreach (var item in report.PublicationsByYear.OrderBy(x => x.Key))
                                {
                                    column.Item().Text($"{item.Key}: {item.Value}");
                                }
                            }

                            // Публикации
                            if (report.Publications.Any())
                            {
                                column.Item().Text("Публикации").Bold().FontSize(14);
                                foreach (var pub in report.Publications)
                                {
                                    column.Item().Text($"{pub.Title} - {pub.PublicationDate:dd.MM.yyyy} - Цитирования: {pub.CitationCount}");
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Страница ");
                            x.CurrentPageNumber();
                            x.Span(" из ");
                            x.TotalPages();
                        });
                });
            })
            .GeneratePdf();
        }

        public byte[] ExportToWord(AuthorReportViewModel report)
        {
            using var stream = new MemoryStream();
            using (var wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                
                var body = new Body();
                mainPart.Document = new WordDocument(body);

                // Заголовок
                var title = new Paragraph(
                    new ParagraphProperties(
                        new Justification() { Val = JustificationValues.Center },
                        new SpacingBetweenLines() { After = "200" }
                    ),
                    new Run(
                        new RunProperties(new Bold()),
                        new Text($"Отчет по автору: {report.Author.FullName}")
                    )
                );
                body.AppendChild(title);
                
                // Пустая строка
                body.AppendChild(new Paragraph(new Run(new Text(""))));

                // Информация об авторе
                body.AppendChild(CreateHeading("Информация об авторе"));
                body.AppendChild(CreateParagraph($"Полное имя: {report.Author.FullName}"));
                body.AppendChild(CreateParagraph($"Организация: {report.Author.Affiliation}"));
                body.AppendChild(CreateParagraph($"Email: {report.Author.Email}"));
                body.AppendChild(CreateParagraph($"Дата формирования отчета: {report.ReportDate:dd.MM.yyyy HH:mm}"));

                // Метрики
                body.AppendChild(CreateHeading("Метрики"));
                body.AppendChild(CreateParagraph($"Количество публикаций: {report.Author.PublicationsCount}"));
                body.AppendChild(CreateParagraph($"Количество соавторов: {report.Author.CoauthorsCount}"));
                body.AppendChild(CreateParagraph($"Общее количество цитирований: {report.Author.CitationCount}"));
                body.AppendChild(CreateParagraph($"H-индекс: {report.Author.HIndex}"));
                body.AppendChild(CreateParagraph($"i10-индекс: {report.Author.I10Index}"));
                body.AppendChild(CreateParagraph($"Средние цитирования на публикацию: {report.Author.AverageCitationsPerPublication:F2}"));
                body.AppendChild(CreateParagraph($"Среднее число страниц: {report.Author.AveragePages:F2}"));
                body.AppendChild(CreateParagraph($"Общее количество страниц: {report.Author.TotalPages}"));
                body.AppendChild(CreateParagraph($"Индекс продуктивности: {report.Author.ProductivityIndex:F2}"));
                body.AppendChild(CreateParagraph($"Максимальное количество цитирований: {report.Author.MaxCitations}"));
                body.AppendChild(CreateParagraph($"Минимальное количество цитирований: {report.Author.MinCitations}"));

                // Статистика по типам
                if (report.PublicationsByType.Any())
                {
                    body.AppendChild(CreateHeading("Статистика по типам публикаций"));
                    foreach (var item in report.PublicationsByType)
                    {
                        body.AppendChild(CreateParagraph($"{item.Key}: {item.Value}"));
                    }
                }

                // Статистика по годам
                if (report.PublicationsByYear.Any())
                {
                    body.AppendChild(CreateHeading("Статистика по годам"));
                    foreach (var item in report.PublicationsByYear.OrderBy(x => x.Key))
                    {
                        body.AppendChild(CreateParagraph($"{item.Key}: {item.Value}"));
                    }
                }

                // Публикации
                if (report.Publications.Any())
                {
                    body.AppendChild(CreateHeading("Публикации"));
                    foreach (var pub in report.Publications)
                    {
                        var pubDate = pub.PublicationDate?.ToString("dd.MM.yyyy") ?? "Не указана";
                        body.AppendChild(CreateParagraph($"{pub.Title} - {pubDate} - Цитирования: {pub.CitationCount}"));
                    }
                }

                mainPart.Document.Save();
            }
            
            stream.Position = 0;
            return stream.ToArray();
        }

        private Paragraph CreateHeading(string text)
        {
            var paragraph = new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines() { Before = "240", After = "120" }
                ),
                new Run(
                    new RunProperties(new Bold()),
                    new Text(text)
                )
            );
            return paragraph;
        }

        private Paragraph CreateParagraph(string text)
        {
            return new Paragraph(
                new ParagraphProperties(
                    new SpacingBetweenLines() { After = "120" }
                ),
                new Run(new Text(text))
            );
        }

        public byte[] ExportToExcel(AuthorReportViewModel report)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Отчет");

            int row = 1;

            // Заголовок
            worksheet.Cells[row, 1].Value = $"Отчет по автору: {report.Author.FullName}";
            worksheet.Cells[row, 1, row, 3].Merge = true;
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            worksheet.Cells[row, 1].Style.Font.Size = 14;
            row += 2;

            // Информация об авторе
            worksheet.Cells[row, 1].Value = "Информация об авторе";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            row++;
            worksheet.Cells[row, 1].Value = "Полное имя:";
            worksheet.Cells[row, 2].Value = report.Author.FullName;
            row++;
            worksheet.Cells[row, 1].Value = "Организация:";
            worksheet.Cells[row, 2].Value = report.Author.Affiliation;
            row++;
            worksheet.Cells[row, 1].Value = "Email:";
            worksheet.Cells[row, 2].Value = report.Author.Email;
            row++;
            worksheet.Cells[row, 1].Value = "Дата формирования отчета:";
            worksheet.Cells[row, 2].Value = report.ReportDate.ToString("dd.MM.yyyy HH:mm");
            row += 2;

            // Метрики
            worksheet.Cells[row, 1].Value = "Метрики";
            worksheet.Cells[row, 1].Style.Font.Bold = true;
            row++;
            worksheet.Cells[row, 1].Value = "Количество публикаций:";
            worksheet.Cells[row, 2].Value = report.Author.PublicationsCount;
            row++;
            worksheet.Cells[row, 1].Value = "Количество соавторов:";
            worksheet.Cells[row, 2].Value = report.Author.CoauthorsCount;
            row++;
            worksheet.Cells[row, 1].Value = "Общее количество цитирований:";
            worksheet.Cells[row, 2].Value = report.Author.CitationCount;
            row++;
            worksheet.Cells[row, 1].Value = "H-индекс:";
            worksheet.Cells[row, 2].Value = report.Author.HIndex;
            row++;
            worksheet.Cells[row, 1].Value = "i10-индекс:";
            worksheet.Cells[row, 2].Value = report.Author.I10Index;
            row++;
            worksheet.Cells[row, 1].Value = "Средние цитирования на публикацию:";
            worksheet.Cells[row, 2].Value = report.Author.AverageCitationsPerPublication;
            row++;
            worksheet.Cells[row, 1].Value = "Среднее число страниц:";
            worksheet.Cells[row, 2].Value = report.Author.AveragePages;
            row++;
            worksheet.Cells[row, 1].Value = "Общее количество страниц:";
            worksheet.Cells[row, 2].Value = report.Author.TotalPages;
            row++;
            worksheet.Cells[row, 1].Value = "Индекс продуктивности:";
            worksheet.Cells[row, 2].Value = report.Author.ProductivityIndex;
            row++;
            worksheet.Cells[row, 1].Value = "Максимальное количество цитирований:";
            worksheet.Cells[row, 2].Value = report.Author.MaxCitations;
            row++;
            worksheet.Cells[row, 1].Value = "Минимальное количество цитирований:";
            worksheet.Cells[row, 2].Value = report.Author.MinCitations;
            row += 2;

            // Статистика по типам
            if (report.PublicationsByType.Any())
            {
                worksheet.Cells[row, 1].Value = "Статистика по типам публикаций";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                row++;
                worksheet.Cells[row, 1].Value = "Тип";
                worksheet.Cells[row, 2].Value = "Количество";
                worksheet.Cells[row, 1, row, 2].Style.Font.Bold = true;
                row++;
                foreach (var item in report.PublicationsByType)
                {
                    worksheet.Cells[row, 1].Value = item.Key.ToString();
                    worksheet.Cells[row, 2].Value = item.Value;
                    row++;
                }
                row++;
            }

            // Статистика по годам
            if (report.PublicationsByYear.Any())
            {
                worksheet.Cells[row, 1].Value = "Статистика по годам";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                row++;
                worksheet.Cells[row, 1].Value = "Год";
                worksheet.Cells[row, 2].Value = "Количество";
                worksheet.Cells[row, 1, row, 2].Style.Font.Bold = true;
                row++;
                foreach (var item in report.PublicationsByYear.OrderBy(x => x.Key))
                {
                    worksheet.Cells[row, 1].Value = item.Key;
                    worksheet.Cells[row, 2].Value = item.Value;
                    row++;
                }
                row++;
            }

            // Публикации
            if (report.Publications.Any())
            {
                worksheet.Cells[row, 1].Value = "Публикации";
                worksheet.Cells[row, 1].Style.Font.Bold = true;
                row++;
                worksheet.Cells[row, 1].Value = "Название";
                worksheet.Cells[row, 2].Value = "Дата";
                worksheet.Cells[row, 3].Value = "Тип";
                worksheet.Cells[row, 4].Value = "Цитирования";
                worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;
                row++;
                foreach (var pub in report.Publications)
                {
                    worksheet.Cells[row, 1].Value = pub.Title;
                    worksheet.Cells[row, 2].Value = pub.PublicationDate?.ToString("dd.MM.yyyy") ?? "";
                    worksheet.Cells[row, 3].Value = pub.PublicationType.ToString();
                    worksheet.Cells[row, 4].Value = pub.CitationCount;
                    row++;
                }
            }

            worksheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}


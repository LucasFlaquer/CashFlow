using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;
using System.Security.AccessControl;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase: IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "$";
    private readonly IExpensesReadOnlyRepository _repository;
    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);
        if (expenses.Count == 0) return [];
        var document = CreateDocument(month);
        var page = CreatePage(document);
        CreateHeaderWithProfilePhotoAndName(page);
        var totalExpenses = expenses.Sum(expense => expense.Amount);
        CreateTotalSpentSection(page, month, totalExpenses);
        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month) 
    {
        var document = new Document();
        document.Info.Title = "Expenses Report";
        document.Info.Subject = $"Expenses for {month:MMMM yyyy}";
        document.Info.Author = "CashFlow Application";
        // Add styles, sections, and content to the document here
        // ...
        
        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;
        return section;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        }; 
        renderer.RenderDocument();
        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);
        return file.ToArray();
    }

    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        // Page header with image
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");
        var row = table.AddRow();
        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "spyro.speg");
        var img = row.Cells[0].AddImage(pathFile);
        img.Width = "62";
        img.Height = "62";
        img.LockAspectRatio = true;
        row.Cells[1].AddParagraph("Hey, Lucas Flaquer");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RALLEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        var title = string.Format(ResourceReportGenerationMessage.TOTAL_SPENT_IN, month.ToString("Y"));
        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALLEWAY_REGULAR, Size = 15 });
        paragraph.AddLineBreak();
        
        paragraph.AddFormattedText($"{totalExpenses}{CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
    }
}

using CashFlow.Domain.Reports;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase: IGenerateExpensesReportExcelUseCase
{
    public Task<byte[]> Execute(DateOnly month)
    {
        var workbook = new XLWorkbook();
        workbook.Author = "Lucas Flaquer - Cashflow";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";
        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));
        InsertHeader(worksheet);
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessage.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessage.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessage.AMOUNT;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessage.PAYMENT_TYPE;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessage.DESCRIPTION;
        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");
        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

    }
}

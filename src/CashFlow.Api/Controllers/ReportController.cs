﻿using System.Net.Mime;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController: ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromHeader] DateOnly Month)
    {
        byte[] file = new byte[1];
        if(file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
        return NoContent();
    }
}

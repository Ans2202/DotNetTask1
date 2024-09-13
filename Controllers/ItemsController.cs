using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using TaskTry.Mappers;
using TaskTry.Helpers;


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;
using TaskTry.Mappers;
using TaskTry.Models;
using Google;

namespace GoogleSheet_DotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        const string SPREADSHEET_ID = "16zQjvx2H5XtDEjyTGjtpKWIEGa9uDW9p4taYJrnSQe0";
        const string SHEET_NAME = "Sheet1";

        SpreadsheetsResource.ValuesResource _googleSheetValues;

        public ItemsController(GoogleSheetsHelper googleSheetsHelper)
        {
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var range = $"{SHEET_NAME}!A:K";

            var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
            var response = request.Execute();
            var values = response.Values;

            return Ok(ItemsMapper.MapFromRangeData(values));
        }
        [HttpPost]
        public IActionResult Post([FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest("Item object is null");
            }

            var range = $"{SHEET_NAME}!A:k"; // Specify the range where the new data should be appended
            var valueRange = new ValueRange
            {
                Values = ItemsMapper.MapToRangeData(item) // Map the Item object to the range format
            };

            try
            {
                // Create the append request
                var appendRequest = _googleSheetValues.Append(valueRange, SPREADSHEET_ID, range);
                appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;

                // Execute the request asynchronously
                var appendResponse = appendRequest.Execute();

                // Check if the append operation was successful and respond accordingly
                if (appendResponse != null)
                {
                    return CreatedAtAction(nameof(Get), new { id = 1 }, item); // Adjust the id as needed
                }

                return StatusCode(500, "Failed to add data to the Google Sheet");
            }
            catch (GoogleApiException ex)
            {
                // Log the error details
                // You can use a logging framework or simply output to the console
                Console.WriteLine($"Google API exception: {ex.Message}");
                return StatusCode((int)ex.HttpStatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine($"General exception: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred");
            }
        }



        [HttpGet("{rowId}")]
        public IActionResult Get(int rowId)
        {
            var range = $"{SHEET_NAME}!A{rowId}:K{rowId}";
            var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
            var response = request.Execute();
            var values = response.Values;

            return Ok(ItemsMapper.MapFromId(values).FirstOrDefault());
        }


        [HttpPut("{rowId}")]
        public IActionResult Put(int rowId, Item item)
        {
            var range = $"{SHEET_NAME}!A{rowId}:K{rowId}";
            var valueRange = new ValueRange
            {
                Values = ItemsMapper.MapToRangeData(item)
            };

            var updateRequest = _googleSheetValues.Update(valueRange, SPREADSHEET_ID, range);
            updateRequest.ValueInputOption = UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();

            return NoContent();
        }

        [HttpDelete("{rowId}")]
        public IActionResult Delete(int rowId)
        {
            var range = $"{SHEET_NAME}!A{rowId}:K{rowId}";
            var requestBody = new ClearValuesRequest();

            var deleteRequest = _googleSheetValues.Clear(requestBody, SPREADSHEET_ID, range);
            deleteRequest.Execute();

            return NoContent();
        }
    }
}
// excel -> c# output json -> express server (local)

/*
 * 1. Read data from csv into C# app
 * 2. Convert csv data to json object
 * 3. Send json object to express server on localhost
 */

using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;

try
{
    
    var data = new List<FinancialData>();
    string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"data\annual-enterprise-survey-2020-financial-year-provisional-csv.csv");
    using (var reader = new StreamReader(path))
    {
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            data = csv.GetRecords<FinancialData>().ToList<FinancialData>();
        }
    }

    string json = JsonSerializer.Serialize(data);

    await SendData(json);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

async Task SendData(string data)
{
    try
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:3000/data");
        var response = await client.PostAsync("http://localhost:3000/data", new StringContent(data, Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("success\n");
            Console.WriteLine(response.ToString());
        }
        else
        {
            Console.WriteLine("fail\n");
            Console.WriteLine(response.ToString());
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

public class FinancialData
{
    public string Year { get; set; }

    [Name("Industry_aggregation_NZSIOC")]
    public string IndustryAggregationNZSIOC { get; set; }

    [Name("Industry_code_NZSIOC")]
    public string IndustryCodeNZSIOC { get; set; }

    [Name("Industry_name_NZSIOC")]
    public string IndustryNameNZSIOC { get; set; }

    public string Units { get; set; }

    [Name("Variable_code")]
    public string VariableCode { get; set; }

    [Name("Variable_name")]
    public string VariableName { get; set; }

    [Name("Variable_category")]
    public string VariableCategory { get; set; }

    public string Value { get; set; }

    [Name("Industry_code_ANZSIC06")]
    public string IndustryCodeANZSIC06 { get; set; }
}
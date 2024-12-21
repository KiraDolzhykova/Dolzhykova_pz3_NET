using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using System.Globalization;
using System.IO;

public class RecommendationService
{
    private readonly MLContext _mlContext;
    private readonly ITransformer _model;
    private IEnumerable<Laptop> _data;

    public RecommendationService()
    {
        _mlContext = new MLContext();
        _data = LoadData();
        var data = _mlContext.Data.LoadFromTextFile<Laptop>(
            path: "Data/laptops.csv",
            hasHeader: true,
            separatorChar: ','
        );
    }

    public List<Laptop> Recommend(float priceUAH, string? brand, float? ram, float? storage, string? processorBrand, string? processor, float? numCores, string? os, float? screen, int k = 5)
    {
        var filteredLaptops = _data
            .Where(laptop => laptop.PriceUAH <= priceUAH)
            .ToList();
        if(!filteredLaptops.Any())
        {
            Console.WriteLine("Немає ноутбуків, які відповідають суворим критеріям.");
            return new List<Laptop>();
        }
        var distances = _data
            .Select(laptop => new
            {
                Laptop = laptop,
                Distance = ComputeDistance(laptop, priceUAH, brand, ram, storage, processorBrand, processor, numCores, os, screen)
            })
            .OrderBy(x => x.Distance)
            .Take(k)
            .Select(x => x.Laptop)
            .ToList();

        return distances;
    }

    private double ComputeDistance(Laptop laptop, float priceUAH, string? brand, float? ram, float? storage, string? processorBrand, string? processor, float? numCores, string? os, float? screen, int k = 5)
    {
        double distance = 0;
        distance += Math.Pow((laptop.PriceUAH - priceUAH) / priceUAH, 2);

        if (ram.HasValue)
            distance += Math.Pow((laptop.RAM - ram.Value) / ram.Value, 2);

        if (storage.HasValue)
            distance += Math.Pow((laptop.Storage - storage.Value) / storage.Value, 2);

        if (numCores.HasValue)
            distance += Math.Pow((laptop.NumCores - numCores.Value) / numCores.Value, 2);

        if (screen.HasValue)
            distance += Math.Pow((laptop.Screen - screen.Value) / screen.Value, 2);

        if (!string.IsNullOrEmpty(processor) && !string.IsNullOrEmpty(laptop.Processor))
            distance += laptop.Processor.Contains(processor, StringComparison.OrdinalIgnoreCase) ? 0 : 1;

        if (!string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(laptop.Brand))
            distance += laptop.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase) ? 0 : 1;

        if (!string.IsNullOrEmpty(processorBrand) && !string.IsNullOrEmpty(laptop.ProcessorBrand))
            distance += laptop.ProcessorBrand.Contains(processorBrand, StringComparison.OrdinalIgnoreCase) ? 0 : 1;

        if (!string.IsNullOrEmpty(os) && !string.IsNullOrEmpty(laptop.OS))
            distance += laptop.OS.Contains(os, StringComparison.OrdinalIgnoreCase) ? 0 : 1;

        return Math.Sqrt(distance);
    }


    private IEnumerable<Laptop> LoadData()
    {
        var filePath = "Data/laptops.csv"; 
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("CSV файл не знайдено", filePath);
        }
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var laptops = csv.GetRecords<Laptop>().ToList();
            return laptops;
        }
    }
}

using Microsoft.ML.Data;

public class Laptop
{
    [LoadColumn(0)]
    public string Brand { get; set; }
    [LoadColumn(1)]
    public string Model { get; set; }
    [LoadColumn(2)]
    public float Price { get; set; }
    [LoadColumn(3)]
    public string ProcessorBrand { get; set; }
    [LoadColumn(4)]
    public string Processor { get; set; }
    [LoadColumn(5)]
    public float NumCores { get; set; }
    [LoadColumn(6)]
    public float NumThreads { get; set; }
    [LoadColumn(7)]
    public float RAM { get; set; }
    [LoadColumn(8)]
    public string StorageType { get; set; }
    [LoadColumn(9)]
    public float Storage { get; set; }
    [LoadColumn(10)]
    public bool TouchScreen { get; set; }
    [LoadColumn(11)]
    public float Screen { get; set; }
    [LoadColumn(12)]
    public float ResolutionWidth { get; set; }
    [LoadColumn(13)]
    public float ResolutionHeight { get; set; }
    [LoadColumn(14)]
    public string OS { get; set; }
    [LoadColumn(15)]
    public float Guarantee { get; set; }
    [LoadColumn(16)]
    public float PriceUAH { get; set; }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using LaptopRecommend.Models;

namespace LaptopRecommend.Controllers;

public class HomeController : Controller
{
    private readonly RecommendationService _recommendationService;

    public HomeController()
    {
        _recommendationService = new RecommendationService();
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(float priceUAH, string? brand, float? ram, float? storage, string? processorBrand, string? processor, float? numCores, string? os, float? screen)
    {
        Console.WriteLine($"Запит: Ціна = {priceUAH}, Brand={brand}, RAM = {ram}, Storage = {storage}, ProcessorBrand = {processorBrand}, Processor = {processor}, NumCores={numCores}, OS = {os}, Screen={screen}");
        
        var recommendations = _recommendationService.Recommend(priceUAH, brand, ram, storage, processorBrand, processor, numCores, os, screen, k: 5);
        Console.WriteLine($"Рекомендовано {recommendations.Count} ноутбуків");

        return View("Recommendations", recommendations);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

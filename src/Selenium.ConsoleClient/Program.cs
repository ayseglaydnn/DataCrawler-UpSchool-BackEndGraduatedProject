using AngleSharp.Text;
using Application.Common.Models.CrawlerLog;
using Domain.Entities;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using System;
using System.Net.Http.Json;
using WebDriverManager.DriverConfigs.Impl;
using Domain.Utilities;
using OpenQA.Selenium.DevTools;


//Thread.Sleep(5000);
//List<Product> Product = new List<Product>();
Console.WriteLine("UpSchool Crawler");
Console.ReadKey();
string WebsiteUrl = "https://finalproject.dotnet.gg/";

new DriverManager().SetUpDriver(new ChromeConfig());
IWebDriver driver = new ChromeDriver();


var hubConnection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7005/Hubs/CrawlerLogHub")
    .WithAutomaticReconnect()
    .Build();

await hubConnection.StartAsync();

await hubConnection.InvokeAsync(SignalRMethodKeys.CreawlerLog.NewCrawlerLogAdded, CreateLog("Bot started."));//ilk log yöntemi
//await hubConnection.InvokeAsync("SendLogNotificationAsync", new CrawlerLogDto("Bot started."));//ilk log yöntemi

try
{
     

    //var httpClient = new HttpClient();

    //var apiSendNotificationDto = new SendLogNotificationApiDto(CreateLog("Bot started."), hubConnection.ConnectionId);

    //await httpClient.PostAsJsonAsync("https://localhost:7005/api/CrawlerLogs", apiSendNotificationDto);


    driver.Navigate().GoToUrl($"{WebsiteUrl}");
    int currentPage = 1;
    bool isLastPage = false;
    //IReadOnlyCollection<IWebElement> pageItems = driver.FindElements(By.CssSelector(".pagination"));

    //// is it last page
    while (isLastPage == false)
    {

        driver.Navigate().GoToUrl($"{WebsiteUrl}?currentPage={currentPage.ToString()}");
        IReadOnlyCollection<IWebElement> products = driver.FindElements(By.CssSelector(".card.h-100"));
        IReadOnlyCollection<IWebElement> pageItems = driver.FindElements(By.CssSelector(".page-item"));
        foreach (IWebElement pageItem in pageItems)
        {
            //int pageIndex = 1;

            try
            {
                pageItem.FindElement(By.CssSelector(".page-link.next-page"));
                isLastPage = false;
            }
            catch
            {
                isLastPage = true;
            }

        }



        //// is it last page - end
        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("veri kazınıyor."));//ilk log yöntemi 


        foreach (IWebElement webProduct in products)
        {

            IWebElement priceElement = webProduct.FindElement(By.CssSelector(".price"));
            string classValue = priceElement.GetAttribute("class");

            Console.WriteLine($"class value = {classValue}");

            if (classValue.Contains("text-muted"))
            {
                Console.WriteLine("this is a discounted product");
            }


            bool IsDiscount;
            try
            {
                webProduct.FindElement(By.CssSelector(".sale-price"));
                IsDiscount = true;
            }
            catch (NoSuchElementException e)
            {
                IsDiscount = false;
            }
            if (IsDiscount == true)
            {
                var product = new Product()
                {
                    Name = webProduct.FindElement(By.CssSelector(".fw-bolder.product-name")).Text,
                    //Price = product.FindElement(By.CssSelector(".text-muted.text-decoration-line-through.price")).Text,
                    Price = Convert.ToDecimal(webProduct.FindElement(By.CssSelector(".price")).Text.Replace("$", "")),
                    SalePrice = Convert.ToDecimal(webProduct.FindElement(By.CssSelector(".sale-price")).Text.Replace("$", "")),
                    Picture = webProduct.FindElement(By.CssSelector(".card-img-top")).GetAttribute("src"),
                    IsOnSale = true,
                };
                Thread.Sleep(1000);
                Console.WriteLine($"Name = {product.Name}");
                Console.WriteLine($"Price = {product.Price}");
                Console.WriteLine($"DiscountPrice = {product.SalePrice}");
                Console.WriteLine($"DiscountStatus = {product.IsOnSale}");
                Console.WriteLine($"PhotoUrl = {product.Picture}");
            }
            else
            {
                var product = new Product()
                {
                    Name = webProduct.FindElement(By.CssSelector(".fw-bolder.product-name")).Text,
                    //Price = product.FindElement(By.CssSelector(".text-muted.text-decoration-line-through.price")).Text,
                    Price = Convert.ToDecimal(webProduct.FindElement(By.CssSelector(".price")).Text.Replace("$", "")),
                    SalePrice = null,
                    Picture = webProduct.FindElement(By.CssSelector(".card-img-top")).GetAttribute("src"),
                    IsOnSale = false,
                };
                Thread.Sleep(1000);
                Console.WriteLine($"Name = {product.Name}");
                Console.WriteLine($"Price = {product.Price}");
                Console.WriteLine($"DiscountPrice = {product.SalePrice}");
                Console.WriteLine($"DiscountStatus = {product.IsOnSale}");
                Console.WriteLine($"PhotoUrl = {product.Picture}");
            }

        }
        currentPage++;
        //await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("sonraki sayfaya geçiliyor."));//ilk log yöntemi 

    }
    driver.Quit();
}
catch (Exception exception)
{
    Console.WriteLine(exception.ToString());
    //driver.Quit();
}

CrawlerLogDto CreateLog(string message) => new CrawlerLogDto(message);

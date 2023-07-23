using Application.Common.Interfaces;
using Domain.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using Application.Common.Models.CrawlerLog;
using Application.Common.Models.Order;
using System.Xml.Linq;
using System.Diagnostics;
using System.Threading;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Services
{
	public class CrawlerManager : ICrawlerService
	{
		private readonly ICrawlerLogHubService _crawlerLogHubService;

		public CrawlerManager(ICrawlerLogHubService crawlerLogHubService)
		{
			_crawlerLogHubService = crawlerLogHubService;
		}

		public async Task<IEnumerable<ProductDto>> ScrapeWebsiteAsync(string websiteUrl,string orderId, int requestedAmount, int crawlType, CancellationToken cancellationToken)
		{
			await Task.Delay(5000);
			new DriverManager().SetUpDriver(new ChromeConfig());
			

			await _crawlerLogHubService.SendLogNotificationAsync(CreateLog($"Bot started.Id:{orderId}"), cancellationToken);

			using (var driver = new ChromeDriver())
			{
				try
				{
					await _crawlerLogHubService.SendLogNotificationAsync(CreateLog("Go to the url."), cancellationToken);
					//await driver.Navigate().GoToUrlAsync(websiteUrl);
					await Task.Run(() => driver.Navigate().GoToUrl(websiteUrl));
					int currentPage = 1;
					bool isLastPage = false;

					List<ProductDto> scrapedProducts = new List<ProductDto>();
					//var totalFountAmount = scrapedProducts.Count;

					while (!isLastPage && requestedAmount > scrapedProducts.Count)
					{
						
						IReadOnlyCollection<IWebElement> products = driver.FindElements(By.CssSelector(".card.h-100"));

						isLastPage = IsLastPage(driver);

						foreach (IWebElement webProduct in products)
						{

							ProductDto productDto = ScrapeProductDetails(webProduct);

							switch (crawlType)
							{
								case 0://All products
									scrapedProducts.Add(productDto);
									break;

								case 1://OnDiscount Products
									if (productDto.IsOnSale)
										scrapedProducts.Add(productDto);
									break;

								case 2://NonDiscount Products
									if (!productDto.IsOnSale)
										scrapedProducts.Add(productDto);
									break;

								default:
									break;
							}

							await Task.Delay(1000);

                            if (scrapedProducts.Count >= requestedAmount)
                            {
                                break; // Exit the loop if the requested amount is reached
                            }

                        }

						await _crawlerLogHubService.SendLogNotificationAsync(CreateLog($"Crawling of page {currentPage} is done"), cancellationToken);

						//if (scrapedProducts.Count >= requestedAmount)
						//{
						//	break; // Exit the loop if the requested amount is reached
						//}

						NextPageAsync(driver, websiteUrl, currentPage, cancellationToken);

						await Task.Delay(5000);
						currentPage++;

					}
					await _crawlerLogHubService.SendLogNotificationAsync(CreateLog($"Scraping is done."), cancellationToken);

					return scrapedProducts;
				}
				catch (Exception exception)
				{
					//var orderEvent = new OrderEvent()
					//{
					//	OrderId = orderId,
					//	Status = OrderStatus.CrawlingFailed,
					//	CreatedOn = DateTimeOffset.Now,
					//};

					throw;
				}
				finally
				{
					driver.Quit();
				}
			}
		}
		CrawlerLogDto CreateLog(string message) => new CrawlerLogDto(message);

		private async void NextPageAsync(ChromeDriver driver, string websiteUrl, int currentPage, CancellationToken cancellationToken)
		{
			try
			{
				//IWebElement nextButton = driver.FindElement(By.CssSelector(".page-link.next-page"));
				//IWebElement nextButton = driver.FindElement(By.XPath("//li[@class='next-page']/a"));
				//nextButton.Click();

				var logTask = Task.Run(() => _crawlerLogHubService.SendLogNotificationAsync(CreateLog($"Going to the page {currentPage+1}."), cancellationToken));
				//var navigateTask = Task.Delay(2000);
				var navigateTask = Task.Run(() => driver.Navigate().GoToUrl($"{websiteUrl}?currentPage={(currentPage+1).ToString()}"));

				await Task.WhenAll(logTask, navigateTask);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private bool IsLastPage(IWebDriver driver)
		{
			IReadOnlyCollection<IWebElement> pageItems = driver.FindElements(By.CssSelector(".page-item"));
			return !pageItems.Any(item => item.FindElements(By.CssSelector(".page-link.next-page")).Any());
		}

		private ProductDto ScrapeProductDetails(IWebElement product)
		{
			string name = product.FindElement(By.CssSelector(".fw-bolder.product-name")).Text;
			decimal price = Convert.ToDecimal(product.FindElement(By.CssSelector(".price")).Text.Replace("$", ""));
			string picture = product.FindElement(By.CssSelector(".card-img-top")).GetAttribute("src");
			bool isDiscount = product.FindElements(By.CssSelector(".sale-price")).Any();
			decimal? salePrice = isDiscount ? Convert.ToDecimal(product.FindElement(By.CssSelector(".sale-price")).Text.Replace("$", "")) : null;


			return new ProductDto()
			{
				Name = name,
				Price = price,
				SalePrice = salePrice,
				IsOnSale = isDiscount,
				Picture = picture,
			};
		}
	}
}

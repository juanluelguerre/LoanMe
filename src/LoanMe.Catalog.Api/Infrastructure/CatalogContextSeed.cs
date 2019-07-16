﻿using LoanMe.Catalog.Api.Application.Entities;
using LoanMe.Catalog.Api.Extensions;
using LoanMe.Catalog.Api.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LoanMe.Catalog.Api.Infrastructure
{
	public class CatalogContextSeed
	{
		public async Task SeedAsync(CatalogContext context, IHostingEnvironment env, IOptions<CatalogSettings> settings, ILogger<CatalogContextSeed> logger)
		{
			var policy = CreatePolicy(logger, nameof(CatalogContextSeed));

			await policy.ExecuteAsync(async () =>
			{
				var useCustomizationData = settings.Value.UseCustomizationData;
				var contentRootPath = env.ContentRootPath;
				var picturePath = env.WebRootPath;

				if (!context.CatalogBrands.Any())
				{
					await context.CatalogBrands.AddRangeAsync(useCustomizationData
						? GetCatalogBrandsFromFile(contentRootPath, logger)
						: GetPreconfiguredCatalogBrands());

					await context.SaveChangesAsync();
				}

				if (!context.CatalogTypes.Any())
				{
					await context.CatalogTypes.AddRangeAsync(useCustomizationData
						? GetCatalogTypesFromFile(contentRootPath, logger)
						: GetPreconfiguredCatalogTypes());

					await context.SaveChangesAsync();
				}

				if (!context.CatalogItems.Any())
				{
					await context.CatalogItems.AddRangeAsync(useCustomizationData
						? GetCatalogItemsFromFile(contentRootPath, context, logger)
						: GetPreconfiguredItems());

					await context.SaveChangesAsync();

					GetCatalogItemPictures(contentRootPath, picturePath);
				}
			});
		}

		private IEnumerable<CatalogBrand> GetCatalogBrandsFromFile(string contentRootPath, ILogger<CatalogContextSeed> logger)
		{
			string csvFileCatalogBrands = Path.Combine(contentRootPath, "Setup", "CatalogBrands.csv");

			if (!File.Exists(csvFileCatalogBrands))
			{
				return GetPreconfiguredCatalogBrands();
			}

			string[] csvheaders;
			try
			{
				string[] requiredHeaders = { "catalogbrand" };
				csvheaders = GetHeaders(csvFileCatalogBrands, requiredHeaders);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
				return GetPreconfiguredCatalogBrands();
			}

			return File.ReadAllLines(csvFileCatalogBrands)
										.Skip(1) // skip header row
										.SelectTry(x => CreateCatalogBrand(x))
										.OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
										.Where(x => x != null);
		}

		private CatalogBrand CreateCatalogBrand(string brand)
		{
			brand = brand.Trim('"').Trim();

			if (String.IsNullOrEmpty(brand))
			{
				throw new Exception("catalog Brand Name is empty");
			}

			return new CatalogBrand
			{
				Brand = brand,
			};
		}

		private IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
		{
			return new List<CatalogBrand>()
			{
				new CatalogBrand() { Brand = "House"},
				new CatalogBrand() { Brand = "Car" },
				new CatalogBrand() { Brand = "Truck" },
				new CatalogBrand() { Brand = "Air Plain" },
				new CatalogBrand() { Brand = "Caravan" },
				new CatalogBrand() { Brand = "4x4" },
				new CatalogBrand() { Brand = "Boat" }
			};
		}

		private IEnumerable<CatalogType> GetCatalogTypesFromFile(string contentRootPath, ILogger<CatalogContextSeed> logger)
		{
			string csvFileCatalogTypes = Path.Combine(contentRootPath, "Setup", "CatalogTypes.csv");

			if (!File.Exists(csvFileCatalogTypes))
			{
				return GetPreconfiguredCatalogTypes();
			}

			string[] csvheaders;
			try
			{
				string[] requiredHeaders = { "catalogtype" };
				csvheaders = GetHeaders(csvFileCatalogTypes, requiredHeaders);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
				return GetPreconfiguredCatalogTypes();
			}

			return File.ReadAllLines(csvFileCatalogTypes)
										.Skip(1) // skip header row
										.SelectTry(x => CreateCatalogType(x))
										.OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
										.Where(x => x != null);
		}

		private CatalogType CreateCatalogType(string type)
		{
			type = type.Trim('"').Trim();

			if (String.IsNullOrEmpty(type))
			{
				throw new Exception("catalog Type Name is empty");
			}

			return new CatalogType
			{
				Type = type,
			};
		}

		private IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
		{
			return new List<CatalogType>()
			{
				new CatalogType() { Type = "Property" },
				new CatalogType() { Type = "Vehicle"},
				new CatalogType() { Type = "Air Plain" }
			};
		}

		private IEnumerable<CatalogItem> GetCatalogItemsFromFile(string contentRootPath, CatalogContext context, ILogger<CatalogContextSeed> logger)
		{
			string csvFileCatalogItems = Path.Combine(contentRootPath, "Setup", "CatalogItems.csv");

			if (!File.Exists(csvFileCatalogItems))
			{
				return GetPreconfiguredItems();
			}

			string[] csvheaders;
			try
			{
				string[] requiredHeaders = { "catalogtypename", "catalogbrandname", "description", "name", "price", "pictureFileName" };
				string[] optionalheaders = { "availablestock", "restockthreshold", "maxstockthreshold", "onreorder" };
				csvheaders = GetHeaders(csvFileCatalogItems, requiredHeaders, optionalheaders);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
				return GetPreconfiguredItems();
			}

			var catalogTypeIdLookup = context.CatalogTypes.ToDictionary(ct => ct.Type, ct => ct.Id);
			var catalogBrandIdLookup = context.CatalogBrands.ToDictionary(ct => ct.Brand, ct => ct.Id);

			return File.ReadAllLines(csvFileCatalogItems)
						.Skip(1) // skip header row
						.Select(row => Regex.Split(row, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
						.SelectTry(column => CreateCatalogItem(column, csvheaders, catalogTypeIdLookup, catalogBrandIdLookup))
						.OnCaughtException(ex => { logger.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message); return null; })
						.Where(x => x != null);
		}

		private CatalogItem CreateCatalogItem(string[] column, string[] headers, Dictionary<String, int> catalogTypeIdLookup, Dictionary<String, int> catalogBrandIdLookup)
		{
			if (column.Count() != headers.Count())
			{
				throw new Exception($"column count '{column.Count()}' not the same as headers count'{headers.Count()}'");
			}

			string catalogTypeName = column[Array.IndexOf(headers, "catalogtypename")].Trim('"').Trim();
			if (!catalogTypeIdLookup.ContainsKey(catalogTypeName))
			{
				throw new Exception($"type={catalogTypeName} does not exist in catalogTypes");
			}

			string catalogBrandName = column[Array.IndexOf(headers, "catalogbrandname")].Trim('"').Trim();
			if (!catalogBrandIdLookup.ContainsKey(catalogBrandName))
			{
				throw new Exception($"type={catalogTypeName} does not exist in catalogTypes");
			}

			string priceString = column[Array.IndexOf(headers, "price")].Trim('"').Trim();
			if (!Decimal.TryParse(priceString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out Decimal price))
			{
				throw new Exception($"price={priceString}is not a valid decimal number");
			}

			var catalogItem = new CatalogItem()
			{
				CatalogTypeId = catalogTypeIdLookup[catalogTypeName],
				CatalogBrandId = catalogBrandIdLookup[catalogBrandName],
				Description = column[Array.IndexOf(headers, "description")].Trim('"').Trim(),
				Name = column[Array.IndexOf(headers, "name")].Trim('"').Trim(),
				Price = price,
				PictureUri = column[Array.IndexOf(headers, "pictureuri")].Trim('"').Trim(),
			};

			int availableStockIndex = Array.IndexOf(headers, "availablestock");
			if (availableStockIndex != -1)
			{
				string availableStockString = column[availableStockIndex].Trim('"').Trim();
				if (!String.IsNullOrEmpty(availableStockString))
				{
					if (int.TryParse(availableStockString, out int availableStock))
					{
						catalogItem.AvailableStock = availableStock;
					}
					else
					{
						throw new Exception($"availableStock={availableStockString} is not a valid integer");
					}
				}
			}

			int restockThresholdIndex = Array.IndexOf(headers, "restockthreshold");
			if (restockThresholdIndex != -1)
			{
				string restockThresholdString = column[restockThresholdIndex].Trim('"').Trim();
				if (!String.IsNullOrEmpty(restockThresholdString))
				{
					if (int.TryParse(restockThresholdString, out int restockThreshold))
					{
						catalogItem.RestockThreshold = restockThreshold;
					}
					else
					{
						throw new Exception($"restockThreshold={restockThreshold} is not a valid integer");
					}
				}
			}

			int maxStockThresholdIndex = Array.IndexOf(headers, "maxstockthreshold");
			if (maxStockThresholdIndex != -1)
			{
				string maxStockThresholdString = column[maxStockThresholdIndex].Trim('"').Trim();
				if (!String.IsNullOrEmpty(maxStockThresholdString))
				{
					if (int.TryParse(maxStockThresholdString, out int maxStockThreshold))
					{
						catalogItem.MaxStockThreshold = maxStockThreshold;
					}
					else
					{
						throw new Exception($"maxStockThreshold={maxStockThreshold} is not a valid integer");
					}
				}
			}

			int onReorderIndex = Array.IndexOf(headers, "onreorder");
			if (onReorderIndex != -1)
			{
				string onReorderString = column[onReorderIndex].Trim('"').Trim();
				if (!String.IsNullOrEmpty(onReorderString))
				{
					if (bool.TryParse(onReorderString, out bool onReorder))
					{
						catalogItem.OnReorder = onReorder;
					}
					else
					{
						throw new Exception($"onReorder={onReorderString} is not a valid boolean");
					}
				}
			}

			return catalogItem;
		}

		private IEnumerable<CatalogItem> GetPreconfiguredItems()
		{
			return new List<CatalogItem>()
			{
				new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 1, Description = "The big house", Name = "The big house", Price = 19.5M, PictureFileName = "1.png" },
				new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 1, Description = "The big house in the middle on the country", Name = "The big house in the middle on the country", Price= 8.50M, PictureFileName = "2.png" },
				new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 1, AvailableStock = 1, Description = "Great square house", Name = "Great square house", Price = 12, PictureFileName = "3.png" },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 1, Description = "Bugatti, the World's Most Expensive New Car In Motion", Name = "Bugatti the most expensive car.", Price = 12, PictureFileName = "4.png" },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 1, Description = "It’s a very exciting time for the TA2 Muscle Car Series at the moment, we are shaping up to have grids of over 20 cars at every championship round in 2019, which could be as high as 24 or 26 at some events,” said category manager Craig Denyer.", Name = "TA2 Ford Mustang", Price = 8.5M, PictureFileName = "5.png" },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 6, AvailableStock = 1, Description = "For big girls and boys who like to play with even bigger toys, this is the perfect experience. Take the chance to get behind the wheel of a truly colossal monster truck and rampage around a rocky and muddy course. Upon arrival your instructor will give you a rundown of the history of this massive beast and an idea of the sort of thing you can expect from the course. You will then head out together to the monster truck. Your experienced instructor will give you a thorough introduction to the controls so you can make the most of your drive, then it’s over to you. Let rip around the course in as you run over hills, down steep drops and around tight corners, putting the truck through its paces. For the ultimate finale, finish by driving over two cars! ", Name = "The Big One - Monster Truck Driving Experience from Buyagift", Price = 12, PictureFileName = "6.png" },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 1, Description = "The design space for a machine that can roll out of a standard parking spot, unfurl a pair of wings, and hurl itself into the air has been explored as thoroughly as any. Stefan Klein, an engineer from Austria, has been working on such machines for the last 20 years. His latest concept, the Aeromobile 2.5, has some impressive specs, and seems to have little trouble getting airborne. The question that comes to mind when watching footage of the prototype is — can it stay there?", Name = "Terrafugia TF-X: The vertical take-off flying car", Price = 12, PictureFileName = "7.png" },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 1, Description = "Hobby Premium 70 Q 2015 Motorhome", Name = "Hobby Premium 70 Q 2015 Motorhome", Price = 8.5M, PictureFileName = "8.png" },
				new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 5, AvailableStock = 1, Description = "Honeywell SPZ-8000 IFCS Autopilot with DFZ-800", Name = "Honeywell SPZ-8000 IFCS Autopilot with DFZ-800", Price = 12, PictureFileName = "9.png" },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 3, AvailableStock = 1, Description = "You can choose from a range of UK locations to live out your transformers dream, where extreme power, exhilaration, and speed await you. Your day will begin with a safety briefing before you take on the Decepticons from behind the wheel of the great Optimus Prime. You’ll get 20 minutes of driving time which is just enough to feel the adrenaline of this beast, and you can see your driving skills in action with a photo and video viewing session. As Optimus Prime once said – “You are not alone. We are here. We are waiting.” Book your experience now!", Name = "Optimus Prime American Truck Driving Experience from Buyagift", Price = 12, PictureFileName = "10.png" },
				new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 7, AvailableStock = 1, Description = "Old Boat", Name = "Old Boat", Price = 8.5M, PictureFileName = "11.png" },
				new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 7, AvailableStock = 1, Description = "BAVARIA R55", Name = "BAVARIA R55", Price = 12, PictureFileName = "12.png" },
			};
		}

		private string[] GetHeaders(string csvfile, string[] requiredHeaders, string[] optionalHeaders = null)
		{
			string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

			if (csvheaders.Count() < requiredHeaders.Count())
			{
				throw new Exception($"requiredHeader count '{ requiredHeaders.Count()}' is bigger then csv header count '{csvheaders.Count()}' ");
			}

			if (optionalHeaders != null)
			{
				if (csvheaders.Count() > (requiredHeaders.Count() + optionalHeaders.Count()))
				{
					throw new Exception($"csv header count '{csvheaders.Count()}'  is larger then required '{requiredHeaders.Count()}' and optional '{optionalHeaders.Count()}' headers count");
				}
			}

			foreach (var requiredHeader in requiredHeaders)
			{
				if (!csvheaders.Contains(requiredHeader))
				{
					throw new Exception($"does not contain required header '{requiredHeader}'");
				}
			}

			return csvheaders;
		}

		private void GetCatalogItemPictures(string contentRootPath, string picturePath)
		{
			if (picturePath != null)
			{
				DirectoryInfo directory = new DirectoryInfo(picturePath);
				foreach (FileInfo file in directory.GetFiles())
				{
					file.Delete();
				}

				string zipFileCatalogItemPictures = Path.Combine(contentRootPath, "Setup", "CatalogItems.zip");
				ZipFile.ExtractToDirectory(zipFileCatalogItemPictures, picturePath);
			}
		}

		private AsyncRetryPolicy CreatePolicy(ILogger<CatalogContextSeed> logger, string prefix, int retries = 3)
		{
			var policy = Policy.Handle<SqlException>().
				WaitAndRetryAsync(
					retryCount: retries,
					sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
					onRetry: (exception, timeSpan, retry, ctx) =>
					{
						logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
					}
				);

			return policy;
		}
	}
}

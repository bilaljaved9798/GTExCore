using BettingServiceReference;
using Census.API.Controllers;
using GTCore.Models;
using GTCore.ViewModel;
using GTExCore.HelperClass;
using GTExCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using UserServiceReference;

namespace GTExCore.Controllers
{
	public class FancyController : Controller
	{
		private BettingServiceClient objBettingClient = new BettingServiceClient();
		UserServicesClient objUsersServiceCleint = new UserServicesClient();
		private readonly IHttpContextAccessor _httpContextAccessor;
		public FancyController(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}
			public IActionResult Index()
		{
			return View();
		}

		public async Task<PartialViewResult> GetFancyMarket(string MarketBookID, DateTime EventOpendate)
		{

			var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBookID);
			int UserIDforLinevmarkets = 0;
			if (LoggedinUserDetail.GetUserTypeID() == 1)
			{
				UserIDforLinevmarkets = 73;
			}
			else
			{
				UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
			}
			var linevmarkets = JsonConvert.DeserializeObject<List<BettingServiceReference.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));
			HttpContext.Session.SetObject("linevmarkets", linevmarkets);

			List<string> lstIds = linevmarkets.Select(item => item.MarketCatalogueIDk__BackingField).ToList();


			if (LoggedinUserDetail.GetCricketDataFrom == "Live")
			{
				var list = objBettingClient.GetAllMarketsFancyAsync(lstIds);

				if (list.Result.Count() > 0)
				{
					bool BettingAllowedoverall =await CheckForAllowedBettingOverAll("Cricket", "Line");
					List<BettingServiceReference.MarketBook> LastloadedLinMarkets1 = new List<BettingServiceReference.MarketBook>();
					foreach (var bfobject in linevmarkets)
					{
						try
						{
                            BettingServiceReference.MarketBook objmarketbookBF1 = list.Result.Where(item => item.MarketId == bfobject.MarketCatalogueIDk__BackingField).First();
							LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectLive(objmarketbookBF1, bfobject.MarketCatalogueIDk__BackingField, EventOpendate, bfobject.MarketCatalogueNamek__BackingField, "Cricket", bfobject.BettingAllowedk__BackingField, BettingAllowedoverall));
						}
						catch (System.Exception ex)
						{

						}
					}
					return PartialView("FancyMarketBook", LastloadedLinMarkets1);
				}
				else
				{
					return PartialView("FancyMarketBook", new List<BettingServiceReference.MarketBook>());
				}
			}
			else
			{


				var list = (objBettingClient.GetAllMarketsOthersFancyAsync(lstIds));

				if (list.Result.Count() > 0)
				{

					bool BettingAllowedoverall =await CheckForAllowedBettingOverAll("Cricket", "Line");
					List<BettingServiceReference.MarketBook> LastloadedLinMarkets1 = new List<BettingServiceReference.MarketBook>();
					foreach (var bfobject in linevmarkets)
					{
						try
						{
							MarketBookString objmarketbookBF1 = list.Result.Where(item => item.MarketBookId == bfobject.MarketCatalogueIDk__BackingField).First();
							LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFNewSource(objmarketbookBF1, bfobject.MarketCatalogueIDk__BackingField, EventOpendate, bfobject.MarketCatalogueNamek__BackingField, "Cricket", bfobject.BettingAllowedk__BackingField, BettingAllowedoverall));
						}
						catch (System.Exception ex)
						{

						}
					}
					return PartialView("FancyMarketBook", LastloadedLinMarkets1);
				}
				else
				{
					return PartialView("FancyMarketBook", new List<BettingServiceReference.MarketBook>());
				}
			}

		}

		public async Task<bool> CheckForAllowedBettingOverAll(string categoryname, string marketbookname)
		{
			AllowedMarketWeb AllowedMarketsForUser = JsonConvert.DeserializeObject<AllowedMarketWeb>(objUsersServiceCleint.GetAllowedMarketsbyUserID(LoggedinUserDetail.GetUserID()));
			bool AllowedBet = false;

			if (marketbookname.Contains("Line"))
			{
				AllowedBet = AllowedMarketsForUser.isFancyMarketAllowed;
				return AllowedBet;
			}

			if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
			{
				AllowedBet = AllowedMarketsForUser.isHorseRaceWinAllowedForBet;
			}
			else
			{
				if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
				{
					AllowedBet = AllowedMarketsForUser.isHorseRacePlaceAllowedForBet;
				}
				else
				{
					if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
					{
						AllowedBet = AllowedMarketsForUser.isGrayHoundRacePlaceAllowedForBet;
					}
					else
					{
						if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
						{
							AllowedBet = AllowedMarketsForUser.isGrayHoundRaceWinAllowedForBet;
						}
						else
						{
							if (marketbookname.Contains("Completed Match"))
							{
								AllowedBet = AllowedMarketsForUser.isCricketCompletedMatchAllowedForBet;
							}
							else
							{
								if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
								{
									AllowedBet = AllowedMarketsForUser.isCricketInningsRunsAllowedForBet;
								}
								else
								{
									if (categoryname == "Tennis")
									{
										AllowedBet = AllowedMarketsForUser.isTennisAllowedForBet;
									}
									else
									{
										if (categoryname == "Soccer")
										{
											AllowedBet = AllowedMarketsForUser.isSoccerAllowedForBet;
										}
										else
										{
											if (marketbookname.Contains("Tied Match"))
											{
												AllowedBet = AllowedMarketsForUser.isCricketTiedMatchAllowedForBet;
											}
											else
											{
												if (marketbookname.Contains("Winner"))
												{
													AllowedBet = AllowedMarketsForUser.isWinnerMarketAllowedForBet;

												}
												else
												{
													AllowedBet = AllowedMarketsForUser.isCricketMatchOddsAllowedForBet;
												}
											}

										}
									}

								}
							}
						}
					}
				}
			}
			return AllowedBet;
		}

		public BettingServiceReference.MarketBook ConvertJsontoMarketObjectBFNewSource(BettingServiceReference.MarketBookString BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BettingAllowed, bool BettingAllowedoverall)
		{

			if (1 == 1)
			{
				var marketbook = new BettingServiceReference.MarketBook();
				string[] newres = BFMarketbook.MarketBookData.Split(':').Select(tag => tag.Trim()).ToArray();
				string[] BFMarketBookDetail = newres[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();

				marketbook.MarketId = BFMarketbook.MarketBookId;
				marketbook.SheetName = "";
				marketbook.IsMarketDataDelayed = false;
				marketbook.PoundRate = LoggedinUserDetail.GetPoundRate();
				marketbook.NumberOfWinners = Convert.ToInt32(BFMarketBookDetail[6]);
				marketbook.MarketBookName = sheetname;
				marketbook.MainSportsname = MainSportsCategory;
				marketbook.OrignalOpenDate = marketopendate;
				marketbook.BettingAllowed = BettingAllowed;
				marketbook.BettingAllowedOverAll = BettingAllowedoverall;
				try
				{
					marketbook.TotalMatched = Convert.ToInt64(Convert.ToDouble(BFMarketBookDetail[10]) * Convert.ToDouble(marketbook.PoundRate));
				}
				catch (System.Exception ex)
				{


				}
				DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
				DateTime CurrentDate = DateTime.Now;
				TimeSpan remainingdays = (CurrentDate - OpenDate);
				if (OpenDate < CurrentDate)
				{
					marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
				}
				else
				{
					marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
				}

				if (Convert.ToInt32(BFMarketBookDetail[5]) == 1 && BFMarketBookDetail[2].Trim().ToString() == "OPEN")
				{

					marketbook.MarketStatusstr = "In Play";
				}
				else
				{
					if (BFMarketBookDetail[2].Trim().ToString() == "CLOSED")
					{
						marketbook.MarketStatusstr = "Closed";
					}
					else
					{
						if (BFMarketBookDetail[2].Trim().ToString() == "SUSPENDED")
						{
							marketbook.MarketStatusstr = "Suspended";
						}
						else
						{
							marketbook.MarketStatusstr = "Active";
						}

					}

				}

				List<BettingServiceReference.Runner> lstRunners = new List<BettingServiceReference.Runner>();


				for (int i = 1; i < newres.Count(); i++)
				{
					string[] runnerdetails = newres[i].Split(new string[] { "|" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();
					string[] runnerinfo = runnerdetails[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();
					string[] runnerbackdata = runnerdetails[1].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
					string[] runnerlaydata = runnerdetails[2].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
					var runner = new BettingServiceReference.Runner();
					runner.Handicap = 0;
					runner.StatusStr = runnerinfo[6].Trim();
					runner.SelectionId = runnerinfo[0].Trim().ToString();
					runner.RunnerName = sheetname;
					try
					{
						runner.LastPriceTraded = Convert.ToDouble(runnerinfo[3].Trim().ToString());
						runner.TotalMatchedStr = APIConfig.FormatNumber(Convert.ToInt64(Convert.ToDouble(runnerinfo[2]) * Convert.ToDouble(marketbook.PoundRate)));
					}
					catch (System.Exception ex)
					{


					}
					var lstpricelist = new List<BettingServiceReference.PriceSize>();
					if (runnerbackdata.Count() > 0)
					{
						if (newres.Count() == 2)
						{
							try
							{


								if (runnerbackdata[0].ToString().Contains("."))
								{
									for (int j = 0; j < runnerlaydata.Count();)
									{
										if (j < runnerlaydata.Count())
										{
											var pricesize = new BettingServiceReference.PriceSize();
											pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
											pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
											pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
											pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j]) + 0.5).ToString("F2"));

											lstpricelist.Add(pricesize);
											j = j + 4;

										}

									}
								}
								else
								{
									for (int j = 0; j < runnerbackdata.Count();)
									{
										if (j < runnerbackdata.Count())
										{
											var pricesize = new BettingServiceReference.PriceSize();
											pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
											pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
											pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
											pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j])).ToString("F2"));

											lstpricelist.Add(pricesize);
											j = j + 4;

										}

									}
								}
							}
							catch (System.Exception ex)
							{

							}
						}
						else
						{
							for (int j = 0; j < runnerbackdata.Count();)
							{
								if (j < runnerbackdata.Count())
								{
									var pricesize = new BettingServiceReference.PriceSize();
									pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
									pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
									pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
									pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j])).ToString("F2"));

									lstpricelist.Add(pricesize);
									j = j + 4;

								}

							}
						}
					}
					else
					{
						for (int ii = 0; ii < 3; ii++)
						{
							var pricesize = new BettingServiceReference.PriceSize();
							pricesize.OrignalSize = 0;
							pricesize.Size = 0;
							pricesize.SizeStr = "0";
							pricesize.Price = 0;

							lstpricelist.Add(pricesize);
						}
					}

					runner.ExchangePrices = new BettingServiceReference.ExchangePrices();
					runner.ExchangePrices.AvailableToBack = lstpricelist;
					lstpricelist = new List<BettingServiceReference.PriceSize>();
					if (runnerlaydata.Count() > 0)
					{
						if (newres.Count() == 2)
						{
							try
							{


								if (runnerlaydata[0].ToString().Contains("."))
								{
									for (int j = 0; j < runnerbackdata.Count();)
									{
										if (j < runnerbackdata.Count())
										{
											var pricesize = new BettingServiceReference.PriceSize();
											pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
											pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
											pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
											pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j]) + 0.5).ToString("F2"));

											lstpricelist.Add(pricesize);
											j = j + 4;

										}

									}
								}
								else
								{
									for (int j = 0; j < runnerlaydata.Count();)
									{
										if (j < runnerlaydata.Count())
										{
											var pricesize = new BettingServiceReference.PriceSize();
											pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
											pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
											pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
											pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j])).ToString("F2"));

											lstpricelist.Add(pricesize);
											j = j + 4;

										}

									}
								}
							}
							catch (System.Exception ex)
							{

							}
						}
						else
						{
							for (int j = 0; j < runnerlaydata.Count();)
							{
								if (j < runnerlaydata.Count())
								{
									var pricesize = new BettingServiceReference.PriceSize();
									pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
									pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
									pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
									pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j])).ToString("F2"));

									lstpricelist.Add(pricesize);
									j = j + 4;

								}

							}
						}
					}
					else
					{
						for (int ii = 0; ii < 3; ii++)
						{
							var pricesize = new BettingServiceReference.PriceSize();
							pricesize.OrignalSize = 0;
							pricesize.Size = 0;
							pricesize.SizeStr = "0";
							pricesize.Price = 0;

							lstpricelist.Add(pricesize);
						}
					}

					runner.ExchangePrices.AvailableToLay = new List<BettingServiceReference.PriceSize>();
					runner.ExchangePrices.AvailableToLay = lstpricelist;
					lstRunners.Add(runner);
				}


				marketbook.Runners = new List<BettingServiceReference.Runner>(lstRunners);

				return marketbook;

			}
			else
			{
				return new BettingServiceReference.MarketBook();
			}
		}

		public BettingServiceReference.MarketBook ConvertJsontoMarketObjectLive(BettingServiceReference.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BettingAllowed, bool BettingAllowedoverall)
		{
			try
			{



				if (1 == 1)
				{
					var marketbook = new BettingServiceReference.MarketBook();

					marketbook.MarketId = BFMarketbook.MarketId;
					marketbook.SheetName = "";
					marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
					marketbook.PoundRate = LoggedinUserDetail.GetPoundRate();
					marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
					marketbook.MarketBookName = sheetname;
					marketbook.MainSportsname = MainSportsCategory;
					marketbook.OrignalOpenDate = marketopendate;
					marketbook.BettingAllowed = BettingAllowed;
					marketbook.Version = BFMarketbook.Version;
					marketbook.TotalMatched = BFMarketbook.TotalMatched;
					marketbook.BettingAllowedOverAll = BettingAllowedoverall;
					DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
					DateTime CurrentDate = DateTime.Now;
					TimeSpan remainingdays = (CurrentDate - OpenDate);
					if (OpenDate < CurrentDate)
					{
						marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
					}
					else
					{
						marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
					}

					if (BFMarketbook.IsInplay == true && BFMarketbook.Status.ToString() == "OPEN")
					{

						marketbook.MarketStatusstr = "In Play";
					}
					else
					{
						if (BFMarketbook.Status.ToString() == "CLOSED")
						{
							marketbook.MarketStatusstr = "Closed";
						}
						else
						{
							if (BFMarketbook.Status.ToString() == "SUSPENDED")
							{
								marketbook.MarketStatusstr = "Suspended";
							}
							else
							{
								marketbook.MarketStatusstr = "Active";
							}

						}

					}

					List<BettingServiceReference.Runner> lstRunners = new List<BettingServiceReference.Runner>();
					foreach (var runneritem in BFMarketbook.Runners)
					{
						var runner = new BettingServiceReference.Runner();
						runner.Handicap = runneritem.Handicap;
						runner.StatusStr = runneritem.Status.ToString();
						runner.SelectionId = runneritem.SelectionId.ToString();
						runner.LastPriceTraded = runneritem.LastPriceTraded;
						runner.RunnerName = sheetname;
						var lstpricelist = new List<BettingServiceReference.PriceSize>();
						if (runneritem.ExchangePrices.AvailableToBack != null && runneritem.ExchangePrices.AvailableToBack.Count() > 0)
						{
							if (BFMarketbook.Runners.Count() == 1)
							{
								try
								{


									if (runneritem.ExchangePrices.AvailableToBack[0].Price.ToString().Contains("."))
									{
										foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
										{
											var pricesize = new BettingServiceReference.PriceSize();

											pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
											pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
											pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

											lstpricelist.Add(pricesize);
										}
									}
									else
									{
										foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
										{
											var pricesize = new BettingServiceReference.PriceSize();

											pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
											pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
											pricesize.Price = backitems.Price;

											lstpricelist.Add(pricesize);
										}
									}
								}
								catch (System.Exception ex)
								{

								}
							}
							else
							{
								foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
								{
									var pricesize = new BettingServiceReference.PriceSize();

									pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
									pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
									pricesize.Price = backitems.Price;

									lstpricelist.Add(pricesize);
								}
							}

						}
						else
						{
							for (int i = 0; i < 3; i++)
							{
								var pricesize = new BettingServiceReference.PriceSize();

								pricesize.Size = 0;

								pricesize.Price = 0;

								lstpricelist.Add(pricesize);
							}
						}

						runner.ExchangePrices = new BettingServiceReference.ExchangePrices();
						runner.ExchangePrices.AvailableToBack = lstpricelist;
						lstpricelist = new List<BettingServiceReference.PriceSize>();
						if (runneritem.ExchangePrices.AvailableToLay != null && runneritem.ExchangePrices.AvailableToLay.Count() > 0)
						{
							if (BFMarketbook.Runners.Count() == 1)
							{
								if (runneritem.ExchangePrices.AvailableToLay[0].Price.ToString().Contains("."))
								{
									foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
									{
										var pricesize = new BettingServiceReference.PriceSize();

										pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));
										pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
										pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

										lstpricelist.Add(pricesize);
									}
								}
								else
								{
									foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
									{
										var pricesize = new BettingServiceReference.PriceSize();

										pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
										pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
										pricesize.Price = backitems.Price;

										lstpricelist.Add(pricesize);
									}
								}
							}
							else
							{
								foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
								{
									var pricesize = new BettingServiceReference.PriceSize();

									pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
									pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
									pricesize.Price = backitems.Price;

									lstpricelist.Add(pricesize);
								}
							}
						}
						else
						{
							for (int i = 0; i < 3; i++)
							{
								var pricesize = new BettingServiceReference.PriceSize();

								pricesize.Size = 0;

								pricesize.Price = 0;

								lstpricelist.Add(pricesize);
							}
						}

						runner.ExchangePrices.AvailableToLay = new List<BettingServiceReference.PriceSize>();
						runner.ExchangePrices.AvailableToLay = lstpricelist;
						lstRunners.Add(runner);
					}
					marketbook.Runners = new List<BettingServiceReference.Runner>(lstRunners);


					return marketbook;

				}
				else
				{
					return new BettingServiceReference.MarketBook();
				}
			}
			catch (System.Exception ex)
			{
				APIConfig.LogError(ex);
				return new BettingServiceReference.MarketBook();
			}

		}


	}
}

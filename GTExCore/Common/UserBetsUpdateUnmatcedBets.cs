using BettingServiceReference;
using GTCore.Models;
using GTExCore.Common;
using GTExCore.Models;

namespace Global.API
{
	public class UserBetsUpdateUnmatcedBets
	{
		BettingServiceClient objBettingClient = new BettingServiceClient();
		UserServiceReference.UserServicesClient objUsersServiceCleint = new UserServiceReference.UserServicesClient();
		public List<DebitCredit> ceckProfitandLoss(MarketBook marketbookstatus, List<UserBets> lstUserBets)
		{

			List<DebitCredit> lstDebitCredit = new List<DebitCredit>();
			var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);

			if (marketbookstatus.Runners.Count() == 1)
			{
				foreach (var userbet in lstUserbetsbyMarketID)
				{
					var objDebitCredit = new DebitCredit();
					if (userbet.BetType == "back")
					{
						objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
						objDebitCredit.Credit = 0;
						objDebitCredit.SelectionID = userbet.SelectionID;
						lstDebitCredit.Add(objDebitCredit);
					}
					else
					{
						objDebitCredit.Debit = 0;
						objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
						objDebitCredit.SelectionID = userbet.SelectionID;
						lstDebitCredit.Add(objDebitCredit);
					}
				}
				return lstDebitCredit;
			}

			if (marketbookstatus.Runners[0].Handicap < 0)
			{
				foreach (var userbet in lstUserbetsbyMarketID)
				{

					var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
					var objDebitCredit = new DebitCredit();
					if (userbet.BetType == "back")
					{
						double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
						objDebitCredit.SelectionID = userbet.SelectionID;
						objDebitCredit.Debit = totamount;
						objDebitCredit.Credit = 0;
						lstDebitCredit.Add(objDebitCredit);
						foreach (var runneritem in marketbookstatus.Runners)
						{
							if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
							{
								objDebitCredit = new DebitCredit();
								objDebitCredit.SelectionID = runneritem.SelectionId;
								objDebitCredit.Debit = totamount;
								objDebitCredit.Credit = 0;
								lstDebitCredit.Add(objDebitCredit);
							}
						}
						foreach (var runneritem in marketbookstatus.Runners)
						{
							if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
							{
								objDebitCredit = new DebitCredit();
								objDebitCredit.SelectionID = runneritem.SelectionId;
								objDebitCredit.Debit = 0;
								objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
								lstDebitCredit.Add(objDebitCredit);
							}
						}

					}
					else
					{
						double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
						objDebitCredit.SelectionID = userbet.SelectionID;
						objDebitCredit.Debit = 0;
						objDebitCredit.Credit = totamount;
						lstDebitCredit.Add(objDebitCredit);
						foreach (var runneritem in marketbookstatus.Runners)
						{
							if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
							{
								objDebitCredit = new DebitCredit();
								objDebitCredit.SelectionID = runneritem.SelectionId;
								objDebitCredit.Debit = 0;
								objDebitCredit.Credit = totamount;
								lstDebitCredit.Add(objDebitCredit);
							}
						}
						foreach (var runneritem in marketbookstatus.Runners)
						{
							if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
							{
								objDebitCredit = new DebitCredit();
								objDebitCredit.SelectionID = runneritem.SelectionId;
								objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
								objDebitCredit.Credit = 0;
								lstDebitCredit.Add(objDebitCredit);
							}
						}


					}

					//userbet.lstDebitCredit = new List<DebitCredit>();
					//userbet.lstDebitCredit = lstDebitCredit;

				}
				return lstDebitCredit;
			}
			else
			{
				if (lstUserbetsbyMarketID.Count() > 0)
				{
					if (lstUserbetsbyMarketID.FirstOrDefault().MarketBookname.Contains("To Be Placed"))
					{
						foreach (var userbet in lstUserbetsbyMarketID)
						{

							var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
							var objDebitCredit = new DebitCredit();
							if (userbet.BetType == "back")
							{

								objDebitCredit.SelectionID = userbet.SelectionID;
								objDebitCredit.Debit = totamount;
								objDebitCredit.Credit = -1 * Convert.ToDecimal(userbet.Amount);
								lstDebitCredit.Add(objDebitCredit);


							}
							else
							{
								objDebitCredit.SelectionID = userbet.SelectionID;
								objDebitCredit.Debit = -1 * totamount;
								objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
								lstDebitCredit.Add(objDebitCredit);
							}
						}
						return lstDebitCredit;

					}
				}
				foreach (var userbet in lstUserbetsbyMarketID)
				{

					var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
					var objDebitCredit = new DebitCredit();
					if (userbet.BetType == "back")
					{

						objDebitCredit.SelectionID = userbet.SelectionID;
						objDebitCredit.Debit = totamount;
						objDebitCredit.Credit = 0;
						lstDebitCredit.Add(objDebitCredit);
						foreach (var runneritem in marketbookstatus.Runners)
						{
							if (runneritem.SelectionId != userbet.SelectionID)
							{
								objDebitCredit = new DebitCredit();
								objDebitCredit.SelectionID = runneritem.SelectionId;
								objDebitCredit.Debit = 0;
								objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
								lstDebitCredit.Add(objDebitCredit);
							}
						}

					}
					else
					{
						objDebitCredit.SelectionID = userbet.SelectionID;
						objDebitCredit.Debit = 0;
						objDebitCredit.Credit = totamount;
						lstDebitCredit.Add(objDebitCredit);
						foreach (var runneritem in marketbookstatus.Runners)
						{
							if (runneritem.SelectionId != userbet.SelectionID)
							{
								objDebitCredit = new DebitCredit();
								objDebitCredit.SelectionID = runneritem.SelectionId;
								objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
								objDebitCredit.Credit = 0;
								lstDebitCredit.Add(objDebitCredit);
							}
						}
					}
				}
				return lstDebitCredit;
			}
		}
		public MarketBookForindianFancy GetBookPositionINNew(string selectionID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforSamiadmin> CurrentSamiadminBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBets> CurrentUserBets)
		{

			MarketBook objmarketbook = new MarketBook();
			MarketBookForindianFancy objmarketbookin = new MarketBookForindianFancy();
			List<BettingServiceReference.DebitCredit> lstDebitCredit = new List<BettingServiceReference.DebitCredit>();
			if (LoggedinUserDetail.GetUserTypeID() == 1)
			{

				List<UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.SelectionID == selectionID).ToList();
				if (lstCurrentBetsAdmin.Count > 0)
				{
					lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

					objmarketbook.MarketId = selectionID;
					objmarketbook.Runners = new List<BettingServiceReference.Runner>();
					BettingServiceReference.Runner objRunner1 = new BettingServiceReference.Runner();
					objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
					objmarketbook.Runners.Add(objRunner1);
					foreach (var userbet in lstCurrentBetsAdmin)
					{
						if (objmarketbook.Runners != null)
						{
							BettingServiceReference.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								BettingServiceReference.Runner objRunner = new BettingServiceReference.Runner();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

								objmarketbook.Runners.Add(objRunner);
							}
						}
						else
						{
							BettingServiceReference.Runner objRunner = new BettingServiceReference.Runner();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							objmarketbook.Runners = new List<BettingServiceReference.Runner>();
							objmarketbook.Runners.Add(objRunner);
						}



					}
					BettingServiceReference.Runner objRunnerlast = new BettingServiceReference.Runner();
					objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
					objmarketbook.Runners.Add(objRunnerlast);

					var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
					foreach (var userid in lstUsers)
					{
						var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
						decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
						decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
						bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
						var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
						decimal superpercent = 0;
						if (superrate > 0)
						{
							superpercent = superrate - agentrate;
						}
						else
						{
							superpercent = 0;
						}
						agentrate = agentrate + superpercent;
						foreach (var userbet in lstCurrentBetsbyUser)
						{
							var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
							double num = Convert.ToDouble(userbet.BetSize) / 100;
							var objDebitCredit = new BettingServiceReference.DebitCredit();
							if (userbet.BetType == "back")
							{
								double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
								objDebitCredit.Credit = 0;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbook.Runners)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbook.Runners)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = totamount;
										lstDebitCredit.Add(objDebitCredit);
									}
								}

							}
							else
							{
								double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = 0;
								objDebitCredit.Credit = totamount;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbook.Runners)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = totamount;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbook.Runners)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
							}
						}
					}

					objmarketbook.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbook.Runners)
					{
						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
						runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
					}




				}
			}

			if (LoggedinUserDetail.GetUserTypeID() == 2)
			{
				int a, b;
				List<UserBetsforAgent> lstCurrentBetsAdmin = CurrentAgentBets.Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();
				if (lstCurrentBetsAdmin.Count > 0)
				{
					lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();


					double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
					a = Convert.ToInt32(aa);
					objmarketbookin.MarketId = selectionID;
					objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
					BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
					objRunner1.SelectionId = (a - 1).ToString();
					objRunner1.StallDraw = (lstCurrentBetsAdmin[0].SelectionID).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunner1);
					foreach (var userbet in lstCurrentBetsAdmin)
					{
						if (objmarketbookin.RunnersForindianFancy != null)
						{
							BettingServiceReference.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
								objRunner.StallDraw = userbet.SelectionID;
								objmarketbookin.RunnersForindianFancy.Add(objRunner);
							}
						}
						else
						{
							BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							objRunner.StallDraw = userbet.SelectionID;
							objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
							objmarketbookin.RunnersForindianFancy.Add(objRunner);
						}

					}

					BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
					double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
					b = Convert.ToInt32(bb);
					objRunnerlast.SelectionId = ((b) + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
					objRunnerlast.StallDraw = lstCurrentBetsAdmin.Last().SelectionID;
					objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
					///calculation
					foreach (var userbet in lstCurrentBetsAdmin)
					{
						decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate));
						double num = Convert.ToDouble(userbet.BetSize) / 100;

						var objDebitCredit = new BettingServiceReference.DebitCredit();
						if (userbet.BetType == "back")
						{
							double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
							objDebitCredit.SelectionID = userbet.UserOdd;
							objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
							objDebitCredit.Credit = 0;

							lstDebitCredit.Add(objDebitCredit);
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new BettingServiceReference.DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);

									objDebitCredit.Credit = 0;

									lstDebitCredit.Add(objDebitCredit);
								}
							}
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new BettingServiceReference.DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									objDebitCredit.Debit = 0;
									objDebitCredit.Credit = totamount;

									lstDebitCredit.Add(objDebitCredit);
								}
							}

						}
						else
						{
							double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
							objDebitCredit.SelectionID = userbet.UserOdd;
							objDebitCredit.Debit = 0;
							objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
							lstDebitCredit.Add(objDebitCredit);
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new BettingServiceReference.DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									objDebitCredit.Debit = 0;
									objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									lstDebitCredit.Add(objDebitCredit);
								}
							}
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new BettingServiceReference.DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									objDebitCredit.Debit = totamount;
									objDebitCredit.Credit = 0;
									lstDebitCredit.Add(objDebitCredit);
								}
							}
						}
					}

					objmarketbookin.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
					{
						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
						runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
					}
				}
			}

			if (LoggedinUserDetail.GetUserTypeID() == 3)
			{
				int a, b;

				List<UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();
				if (lstCurrentBets.Count > 0)
				{
					lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
					double aa = Convert.ToDouble(lstCurrentBets[0].UserOdd);

					a = Convert.ToInt32(aa);
					objmarketbookin.MarketId = selectionID;
					objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
					BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
					objRunner1.SelectionId = (a - 1).ToString();
					objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunner1);
					foreach (var userbet in lstCurrentBets)
					{
						if (objmarketbookin.RunnersForindianFancy != null)
						{
							BettingServiceReference.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
								objRunner.StallDraw = userbet.SelectionID;
								objmarketbookin.RunnersForindianFancy.Add(objRunner);
							}
						}
						else
						{
							BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							objRunner.StallDraw = userbet.SelectionID;
							objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
							objmarketbookin.RunnersForindianFancy.Add(objRunner);
						}

					}
					BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
					double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
					b = Convert.ToInt32(bb);
					objRunnerlast.SelectionId = ((b) + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
					objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
					objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
					///calculation
					foreach (var userbet in lstCurrentBets)
					{
						double num = Convert.ToDouble(userbet.BetSize) / 100;
						var totamount = (Convert.ToDecimal(userbet.Amount));
						var objDebitCredit = new BettingServiceReference.DebitCredit();
						if (userbet.BetType == "back")
						{
							double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
							objDebitCredit.SelectionID = userbet.UserOdd;
							objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
							objDebitCredit.Credit = 0;

							lstDebitCredit.Add(objDebitCredit);
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new BettingServiceReference.DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;

									objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									// objDebitCredit.Debit = totamount;
									objDebitCredit.Credit = 0;

									lstDebitCredit.Add(objDebitCredit);
								}
							}
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new BettingServiceReference.DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;

									objDebitCredit.Debit = 0;
									objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
									//objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									lstDebitCredit.Add(objDebitCredit);
								}
							}

						}
						else
						{
							double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
							objDebitCredit.SelectionID = userbet.UserOdd;
							objDebitCredit.Debit = 0;
							objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num); ;
							lstDebitCredit.Add(objDebitCredit);
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new BettingServiceReference.DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;

									objDebitCredit.Debit = 0;
									// objDebitCredit.Credit = totamount;
									objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									lstDebitCredit.Add(objDebitCredit);
								}
							}
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new BettingServiceReference.DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;

									objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
									// objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									objDebitCredit.Credit = 0;
									lstDebitCredit.Add(objDebitCredit);
								}
							}
						}
					}
					objmarketbookin.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
					{
						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
					}

				}
			}

			if (LoggedinUserDetail.GetUserTypeID() == 8)
			{
				List<UserBetsforSuper> lstCurrentBetsSuper = CurrentSuperBets.Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();

				if (lstCurrentBetsSuper.Count > 0)
				{
					lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

					objmarketbookin.MarketId = selectionID;
					objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
					BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
					objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunner1);
					foreach (var userbet in lstCurrentBetsSuper)
					{
						if (objmarketbookin.RunnersForindianFancy != null)
						{
							BettingServiceReference.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

								objmarketbookin.RunnersForindianFancy.Add(objRunner);
							}
						}
						else
						{
							BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
							objmarketbookin.RunnersForindianFancy.Add(objRunner);
						}



					}
					BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
					objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

					///calculation
					var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
					foreach (var userid in lstUsers)
					{
						var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
						decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
						decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
						bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
						var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
						decimal superpercent = superrate - agentrate;

						foreach (var userbet in lstCurrentBetsbyUser)
						{
							//decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
							double num = Convert.ToDouble(userbet.BetSize) / 100;
							var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
							var totamount = (superpercent / 100) * (totamount1); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
							var objDebitCredit = new BettingServiceReference.DebitCredit();
							if (userbet.BetType == "back")
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = totamount;
								objDebitCredit.Credit = 0;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = totamount;
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
										lstDebitCredit.Add(objDebitCredit);
									}
								}

							}
							else
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = 0;
								objDebitCredit.Credit = totamount;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = totamount;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
							}

							//userbet.lstDebitCredit = new List<DebitCredit>();
							//userbet.lstDebitCredit = lstDebitCredit;

						}
					}

					objmarketbookin.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
					{
						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

						runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

					}

				}
			}
			if (LoggedinUserDetail.GetUserTypeID() == 9)
			{
				List<UserBetsforSamiadmin> lstCurrentBetsSuper = CurrentSamiadminBets.Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();

				if (lstCurrentBetsSuper.Count > 0)
				{
					lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

					objmarketbookin.MarketId = selectionID;
					objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
					BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
					objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunner1);
					foreach (var userbet in lstCurrentBetsSuper)
					{
						if (objmarketbookin.RunnersForindianFancy != null)
						{
							BettingServiceReference.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

								objmarketbookin.RunnersForindianFancy.Add(objRunner);
							}
						}
						else
						{
							BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
							objmarketbookin.RunnersForindianFancy.Add(objRunner);
						}



					}
					BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
					objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

					///calculation
					var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
					foreach (var userid in lstUsers)
					{
						var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
						decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
						decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
						decimal samiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SamiAdminRate);
						bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
						var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
						decimal superpercent = superrate - agentrate;
						decimal samiadminpercent = samiadminrate - (superpercent + agentrate);

						foreach (var userbet in lstCurrentBetsbyUser)
						{
							//decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
							double num = Convert.ToDouble(userbet.BetSize) / 100;
							var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
							var totamount = (samiadminpercent / 100) * (totamount1); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
							var objDebitCredit = new BettingServiceReference.DebitCredit();
							if (userbet.BetType == "back")
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = totamount;
								objDebitCredit.Credit = 0;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = totamount;
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
										lstDebitCredit.Add(objDebitCredit);
									}
								}

							}
							else
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = 0;
								objDebitCredit.Credit = totamount;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new BettingServiceReference.DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = totamount;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
							}
						}
					}

					objmarketbookin.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
					{
						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
						runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
					}

				}
			}
			return objmarketbookin;
		}

        public BettingServiceReference.MarketBookForindianFancy GetBookPositionIN(string marketBookID, string selectionID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforSamiadmin> CurrentSamiadminBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBets> CurrentUserBets)
        {

            BettingServiceReference.MarketBook objmarketbook = new BettingServiceReference.MarketBook();
            BettingServiceReference.MarketBookForindianFancy objmarketbookin = new BettingServiceReference.MarketBookForindianFancy();
            List<BettingServiceReference.DebitCredit> lstDebitCredit = new List<BettingServiceReference.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                List<UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbook.MarketId = marketBookID;
                    objmarketbook.Runners = new List<BettingServiceReference.Runner>();
                    BettingServiceReference.Runner objRunner1 = new BettingServiceReference.Runner();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbook.Runners.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbook.Runners != null)
                        {
                            BettingServiceReference.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                BettingServiceReference.Runner objRunner = new BettingServiceReference.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbook.Runners.Add(objRunner);
                            }
                        }
                        else
                        {
                            BettingServiceReference.Runner objRunner = new BettingServiceReference.Runner();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbook.Runners = new List<BettingServiceReference.Runner>();
                            objmarketbook.Runners.Add(objRunner);
                        }



                    }
                    BettingServiceReference.Runner objRunnerlast = new BettingServiceReference.Runner();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbook.Runners.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = 0;
                        if (superrate > 0)
                        {
                            superpercent = superrate - agentrate;
                        }
                        else
                        {
                            superpercent = 0;
                        }
                        agentrate = agentrate + superpercent;
                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                            var objDebitCredit = new BettingServiceReference.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }

                            }
                            else
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }


                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbook.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbook.Runners)
                    {


                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;


                    }




                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                int a, b;
                List<UserBetsforAgent> lstCurrentBetsAdmin = CurrentAgentBets.Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();


                    double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
                    BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBetsAdmin[0].SelectionID).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            BettingServiceReference.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objRunner.StallDraw = userbet.SelectionID;
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }

                    }

                    BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBetsAdmin.Last().SelectionID;
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                    ///calculation
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate));
                        double num = Convert.ToDouble(userbet.BetSize) / 100;
                        //var totamount = (Convert.ToDecimal(userbet.Amount));

                        var objDebitCredit = new BettingServiceReference.DebitCredit();
                        if (userbet.BetType == "back")
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                            objDebitCredit.Credit = 0;

                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new BettingServiceReference.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);

                                    objDebitCredit.Credit = 0;

                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new BettingServiceReference.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;

                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }

                        }
                        else
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new BettingServiceReference.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new BettingServiceReference.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }


                        }

                        //userbet.lstDebitCredit = new List<DebitCredit>();
                        //userbet.lstDebitCredit = lstDebitCredit;

                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }
                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
                int a, b;

                List<UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID).ToList();
                if (lstCurrentBets.Count > 0)
                {
                    lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                    double aa = Convert.ToDouble(lstCurrentBets[0].UserOdd);

                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
                    BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBets)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            BettingServiceReference.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objRunner.StallDraw = userbet.SelectionID;
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }

                    }
                    BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                    ///calculation
                    foreach (var userbet in lstCurrentBets)
                    {
                        decimal num = Convert.ToDecimal(userbet.BetSize) / 100;
                        var totamount = (Convert.ToDecimal(userbet.Amount) * num);

                        var objDebitCredit = new BettingServiceReference.DebitCredit();
                        if (userbet.BetType == "back")
                        {

                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;

                            objDebitCredit.Debit = totamount;
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new BettingServiceReference.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new BettingServiceReference.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }

                        }
                        else
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = totamount;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new BettingServiceReference.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new BettingServiceReference.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;

                                    objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }


                        }


                    }
                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                    }

                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 8)
            {
                List<UserBetsforSuper> lstCurrentBetsSuper = CurrentSuperBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();

                if (lstCurrentBetsSuper.Count > 0)
                {
                    lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
                    BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsSuper)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            BettingServiceReference.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }



                    }
                    BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = superrate - agentrate;

                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            //decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                            var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
                            var totamount = (superpercent / 100) * (totamount1); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            var objDebitCredit = new BettingServiceReference.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }

                            }
                            else
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }

                }
            }
            if (LoggedinUserDetail.GetUserTypeID() == 9)
            {
                List<UserBetsforSamiadmin> lstCurrentBetsSuper = CurrentSamiadminBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();

                if (lstCurrentBetsSuper.Count > 0)
                {
                    lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
                    BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsSuper)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            BettingServiceReference.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }



                    }
                    BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        decimal samiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SamiAdminRate);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = superrate - agentrate;
                        decimal samiadminpercent = samiadminrate - (superpercent + agentrate);

                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            //decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                            var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
                            var totamount = (samiadminpercent / 100) * (totamount1); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            var objDebitCredit = new BettingServiceReference.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }

                            }
                            else
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new BettingServiceReference.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }
                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }

                }
            }
            return objmarketbookin;
        }

		public MarketBookForindianFancy GetBookPositioninKJ(string marketBookID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforSamiadmin> CurrentSamiadminBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBets> CurrentUserBets)
		{

			MarketBook objmarketbook = new MarketBook();
			MarketBookForindianFancy objmarketbookin = new MarketBookForindianFancy();
			List<DebitCredit> lstDebitCredit = new List<DebitCredit>();
			if (LoggedinUserDetail.GetUserTypeID() == 1)
			{
				int a, b;
				List<UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
				if (lstCurrentBetsAdmin.Count > 0)
				{
					lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

					double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
					a = Convert.ToInt32(aa);
					objmarketbookin.MarketId = marketBookID;
					objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
					RunnerForIndianFancy objRunner1 = new RunnerForIndianFancy();
					objRunner1.SelectionId = (a - 1).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunner1);
					foreach (var userbet in lstCurrentBetsAdmin)
					{
						if (objmarketbookin.RunnersForindianFancy != null)
						{
							RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

								objmarketbookin.RunnersForindianFancy.Add(objRunner);
							}
						}
						else
						{
							RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
							objmarketbookin.RunnersForindianFancy.Add(objRunner);
						}
					}
					RunnerForIndianFancy objRunnerlast = new RunnerForIndianFancy();
					objRunnerlast.SelectionId = (a + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

					///calculation
					var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
					foreach (var userid in lstUsers)
					{
						var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
						decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
						decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
						bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
						var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
						decimal superpercent = 0;
						if (superrate > 0)
						{
							superpercent = superrate - agentrate;
						}
						else
						{
							superpercent = 0;
						}
						agentrate = agentrate + superpercent;
						foreach (var userbet in lstCurrentBetsbyUser)
						{
							var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));

							var objDebitCredit = new DebitCredit();
							if (userbet.BetType == "back")
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = totamount;
								objDebitCredit.Credit = 0;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = totamount;
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = totamount;
										lstDebitCredit.Add(objDebitCredit);
									}
								}

							}
							else
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = 0;
								objDebitCredit.Credit = totamount;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = totamount;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = totamount;
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}


							}

							//userbet.lstDebitCredit = new List<DebitCredit>();
							//userbet.lstDebitCredit = lstDebitCredit;

						}
					}

					objmarketbookin.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
					{
						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
						runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
					}
				}
			}

			if (LoggedinUserDetail.GetUserTypeID() == 2)
			{
				int a, b;
				List<UserBetsforAgent> lstCurrentBetsAdmin = CurrentAgentBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
				if (lstCurrentBetsAdmin.Count > 0)
				{
					lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();


					double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
					a = Convert.ToInt32(aa);
					objmarketbookin.MarketId = marketBookID;
					objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
					RunnerForIndianFancy objRunner1 = new RunnerForIndianFancy();
					objRunner1.SelectionId = (a - 1).ToString();
					// objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunner1);
					foreach (var userbet in lstCurrentBetsAdmin)
					{
						if (objmarketbookin.RunnersForindianFancy != null)
						{
							RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
								// objRunner.StallDraw = userbet.SelectionID;
								objmarketbookin.RunnersForindianFancy.Add(objRunner);
							}
						}
						else
						{
							RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							//objRunner.StallDraw = userbet.SelectionID;
							objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
							objmarketbookin.RunnersForindianFancy.Add(objRunner);
						}

					}

					RunnerForIndianFancy objRunnerlast = new RunnerForIndianFancy();
					double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
					b = Convert.ToInt32(bb);
					objRunnerlast.SelectionId = ((b) + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
					// objRunnerlast.StallDraw = lstCurrentBetsAdmin.Last().SelectionID;
					objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
					///calculation
					foreach (var userbet in lstCurrentBetsAdmin)
					{
						//decimal totamount = GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
						decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate));
						// var totamount = (Convert.ToDecimal(userbet.Amount));
						var objDebitCredit = new DebitCredit();
						if (userbet.BetType == "back")
						{
							double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
							objDebitCredit.SelectionID = userbet.UserOdd;
							objDebitCredit.Debit = 0;
							objDebitCredit.Credit = totamount;

							lstDebitCredit.Add(objDebitCredit);
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									double num = Convert.ToDouble(userbet.BetSize) / 100;
									objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									// objDebitCredit.Debit = totamount;
									objDebitCredit.Credit = 0;

									lstDebitCredit.Add(objDebitCredit);
								}
							}
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									double num = Convert.ToDouble(userbet.BetSize) / 100;
									objDebitCredit.Debit = 0;
									objDebitCredit.Credit = totamount;
									//objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									lstDebitCredit.Add(objDebitCredit);
								}
							}

						}
						else
						{
							double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
							objDebitCredit.SelectionID = userbet.UserOdd;
							objDebitCredit.Debit = totamount;
							objDebitCredit.Credit = 0;
							lstDebitCredit.Add(objDebitCredit);
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									double num = Convert.ToDouble(userbet.BetSize) / 100;
									objDebitCredit.Debit = 0;
									// objDebitCredit.Credit = totamount;
									objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									lstDebitCredit.Add(objDebitCredit);
								}
							}
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									double num = Convert.ToDouble(userbet.BetSize) / 100;
									objDebitCredit.Debit = totamount;
									// objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									objDebitCredit.Credit = 0;
									lstDebitCredit.Add(objDebitCredit);
								}
							}


						}

						//userbet.lstDebitCredit = new List<DebitCredit>();
						//userbet.lstDebitCredit = lstDebitCredit;

					}

					objmarketbookin.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
					{

						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

						runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

					}
				}
			}

			if (LoggedinUserDetail.GetUserTypeID() == 3)
			{
				int a, b;

				List<UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
				if (lstCurrentBets.Count > 0)
				{
					lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
					double aa = Convert.ToDouble(lstCurrentBets[0].UserOdd);

					a = Convert.ToInt32(aa);
					objmarketbookin.MarketId = marketBookID;
					objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
					RunnerForIndianFancy objRunner1 = new RunnerForIndianFancy();
					objRunner1.SelectionId = (a - 1).ToString();
					objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunner1);
					foreach (var userbet in lstCurrentBets)
					{
						if (objmarketbookin.RunnersForindianFancy != null)
						{
							RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
								objRunner.StallDraw = userbet.SelectionID;
								objmarketbookin.RunnersForindianFancy.Add(objRunner);
							}
						}
						else
						{
							RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							objRunner.StallDraw = userbet.SelectionID;
							objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
							objmarketbookin.RunnersForindianFancy.Add(objRunner);
						}

					}
					RunnerForIndianFancy objRunnerlast = new RunnerForIndianFancy();
					double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
					b = Convert.ToInt32(bb);
					objRunnerlast.SelectionId = ((b) + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
					objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
					objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
					///calculation
					foreach (var userbet in lstCurrentBets)
					{

						var totamount = (Convert.ToDecimal(userbet.Amount));
						var objDebitCredit = new DebitCredit();
						if (userbet.BetType == "back")
						{
							double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
							objDebitCredit.SelectionID = userbet.UserOdd;
							objDebitCredit.Debit = 0;
							objDebitCredit.Credit = totamount;

							lstDebitCredit.Add(objDebitCredit);
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									double num = Convert.ToDouble(userbet.BetSize) / 100;
									objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									// objDebitCredit.Debit = totamount;
									objDebitCredit.Credit = 0;

									lstDebitCredit.Add(objDebitCredit);
								}
							}
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									double num = Convert.ToDouble(userbet.BetSize) / 100;
									objDebitCredit.Debit = 0;
									objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
									//objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									lstDebitCredit.Add(objDebitCredit);
								}
							}

						}
						else
						{
							double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
							objDebitCredit.SelectionID = userbet.UserOdd;
							objDebitCredit.Debit = totamount;
							objDebitCredit.Credit = 0;
							lstDebitCredit.Add(objDebitCredit);
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									double num = Convert.ToDouble(userbet.BetSize) / 100;
									objDebitCredit.Debit = 0;
									// objDebitCredit.Credit = totamount;
									objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									lstDebitCredit.Add(objDebitCredit);
								}
							}
							foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
							{
								if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
								{
									objDebitCredit = new DebitCredit();
									objDebitCredit.SelectionID = runneritem.SelectionId;
									double num = Convert.ToDouble(userbet.BetSize) / 100;
									objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
									// objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
									objDebitCredit.Credit = 0;
									lstDebitCredit.Add(objDebitCredit);
								}
							}

						}

						//userbet.lstDebitCredit = new List<DebitCredit>();
						//userbet.lstDebitCredit = lstDebitCredit;

					}
					objmarketbookin.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
					{

						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

					}


				}
			}


			if (LoggedinUserDetail.GetUserTypeID() == 8)
			{
				int a, b;
				List<UserBetsforSuper> lstCurrentBetsAdmin = CurrentSuperBets.ToList().Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
				if (lstCurrentBetsAdmin.Count > 0)
				{
					lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

					double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
					a = Convert.ToInt32(aa);
					objmarketbookin.MarketId = marketBookID;
					objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
					RunnerForIndianFancy objRunner1 = new RunnerForIndianFancy();
					objRunner1.SelectionId = (a - 1).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunner1);

					foreach (var userbet in lstCurrentBetsAdmin)
					{
						if (objmarketbookin.RunnersForindianFancy != null)
						{
							RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
								objmarketbookin.RunnersForindianFancy.Add(objRunner);
							}
						}

						else
						{
							RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
							objmarketbookin.RunnersForindianFancy.Add(objRunner);
						}

					}


					RunnerForIndianFancy objRunnerlast = new RunnerForIndianFancy();
					double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
					b = Convert.ToInt32(bb);
					objRunnerlast.SelectionId = ((b) + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
					///calculation
					var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
					foreach (var userid in lstUsers)
					{
						var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
						decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
						decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
						bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
						var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
						decimal superpercent = superrate - agentrate;

						foreach (var userbet in lstCurrentBetsAdmin)
						{
							var totamount = (superpercent / 100) * ((Convert.ToDecimal(userbet.Amount)) * (Convert.ToDecimal(userbet.BetSize) / 100)); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
																																					   //double num = Convert.ToDouble(userbet.BetSize) / 100;
																																					   //var totamount1 = (Convert.ToDecimal(userbet.Amount)* Convert.ToDecimal(num));
																																					   //var totamount = totamount1 * (superpercent / 100);
							var objDebitCredit = new DebitCredit();
							if (userbet.BetType == "back")
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = totamount;
								objDebitCredit.Credit = 0;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = totamount;
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
										lstDebitCredit.Add(objDebitCredit);
									}
								}
							}
							else
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = 0;
								objDebitCredit.Credit = totamount;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = totamount;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
							}

						}
					}
					objmarketbookin.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
					{
						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
						runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
					}
				}
			}
			if (LoggedinUserDetail.GetUserTypeID() == 9)
			{
				int a, b;
				List<UserBetsforSamiadmin> lstCurrentBetsAdmin = CurrentSamiadminBets.ToList().Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
				if (lstCurrentBetsAdmin.Count > 0)
				{
					lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

					double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
					a = Convert.ToInt32(aa);
					objmarketbookin.MarketId = marketBookID;
					objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
					RunnerForIndianFancy objRunner1 = new RunnerForIndianFancy();
					objRunner1.SelectionId = (a - 1).ToString();
					objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunner1);

					foreach (var userbet in lstCurrentBetsAdmin)
					{
						if (objmarketbookin.RunnersForindianFancy != null)
						{
							RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
							if (objexistingrunner == null)
							{
								RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
								objRunner.SelectionId = userbet.UserOdd;
								objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
								objmarketbookin.RunnersForindianFancy.Add(objRunner);
							}
						}

						else
						{
							RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
							objRunner.SelectionId = userbet.UserOdd;
							objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
							objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
							objmarketbookin.RunnersForindianFancy.Add(objRunner);
						}

					}


					RunnerForIndianFancy objRunnerlast = new RunnerForIndianFancy();
					double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
					b = Convert.ToInt32(bb);
					objRunnerlast.SelectionId = ((b) + 1).ToString();
					objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
					objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
					///calculation
					var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
					foreach (var userid in lstUsers)
					{
						var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
						decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
						decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
						decimal semiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SamiAdminRate);
						bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
						var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
						decimal superpercent = superrate - agentrate;
						decimal semiadminpercent = semiadminrate - (superpercent + agentrate);

						foreach (var userbet in lstCurrentBetsAdmin)
						{
							var totamount = (semiadminpercent / 100) * ((Convert.ToDecimal(userbet.Amount)) * (Convert.ToDecimal(userbet.BetSize) / 100)); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
																																						   //double num = Convert.ToDouble(userbet.BetSize) / 100;
																																						   //var totamount1 = (Convert.ToDecimal(userbet.Amount)* Convert.ToDecimal(num));
																																						   //var totamount = totamount1 * (superpercent / 100);
							var objDebitCredit = new DebitCredit();
							if (userbet.BetType == "back")
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = totamount;
								objDebitCredit.Credit = 0;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = totamount;
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (semiadminpercent / 100);
										lstDebitCredit.Add(objDebitCredit);
									}
								}
							}
							else
							{
								double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
								objDebitCredit.SelectionID = userbet.UserOdd;
								objDebitCredit.Debit = 0;
								objDebitCredit.Credit = totamount;
								lstDebitCredit.Add(objDebitCredit);
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = 0;
										objDebitCredit.Credit = totamount;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
								foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
								{
									if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
									{
										objDebitCredit = new DebitCredit();
										objDebitCredit.SelectionID = runneritem.SelectionId;
										objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (semiadminpercent / 100);
										objDebitCredit.Credit = 0;
										lstDebitCredit.Add(objDebitCredit);
									}
								}
							}

						}
					}
					objmarketbookin.DebitCredit = lstDebitCredit;
					foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
					{
						runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
						runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
					}
				}
			}

			return objmarketbookin;
		}

        public List<DebitCredit> ceckProfitandLossFig(MarketBook marketbookstatus, List<UserBets> lstUserBets)
        {
            List<DebitCredit> lstDebitCredit = new List<DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);

            foreach (var userbet in lstUserbetsbyMarketID)
            {
                var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.BetSize) / 100);
                var objDebitCredit = new DebitCredit();
                if (userbet.BetType == "back")
                {
                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = totamount;
                    objDebitCredit.Credit = 0;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
                        {
                            objDebitCredit = new DebitCredit();
                            objDebitCredit.SelectionID = runneritem.SelectionId;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                            lstDebitCredit.Add(objDebitCredit);
                        }
                    }

                }
                else
                {
                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = 0;
                    objDebitCredit.Credit = totamount;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
                        {
                            objDebitCredit = new DebitCredit();
                            objDebitCredit.SelectionID = runneritem.SelectionId;
                            objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                        }
                    }

                }
            }
            return lstDebitCredit;
        }

    }


}
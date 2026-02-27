using BettingServiceReference;
using GTCore.Models;
using GTExCore.Models;
using Newtonsoft.Json;
using NuGet.Packaging;

namespace Global.API
{
    public class UserBetsUpdateUnmatcedBets
    {
        BettingServiceClient objBettingClient = new BettingServiceClient();
        UserServiceReference.UserServicesClient objUsersServiceCleint = new UserServiceReference.UserServicesClient();
        public DebitCredit[] ceckProfitandLoss(MarketBook marketbookstatus, List<UserBets> lstUserBets)
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
                return lstDebitCredit.ToArray();
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
                return lstDebitCredit.ToArray();
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
                        return lstDebitCredit.ToArray();

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
                return lstDebitCredit.ToArray();
            }
        }
        public MarketBookForindianFancy GetBookPositionINNew(string selectionID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforSamiadmin> CurrentSamiadminBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBets> CurrentUserBets)
        {

            MarketBook objmarketbook = new MarketBook();
            MarketBookForindianFancy objmarketbookin = new MarketBookForindianFancy();
            List<BettingServiceReference.DebitCredit> lstDebitCredit = new List<BettingServiceReference.DebitCredit>();
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
                    objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>().ToArray();
                    var runnerList = objmarketbookin.RunnersForindianFancy?.ToList()
                 ?? new List<BettingServiceReference.RunnerForIndianFancy>();
                    BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                    runnerList.Add(objRunner1);
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
                                runnerList.Add(objRunner);
                            }
                        }
                        else
                        {
                            BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>().ToArray();
                            runnerList.Add(objRunner);
                        }

                    }
                    BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
                    runnerList.Add(objRunnerlast);
                    objmarketbookin.RunnersForindianFancy = runnerList.ToArray();
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

                    objmarketbookin.DebitCredit = lstDebitCredit.ToArray();
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
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
                    var runnerList = objmarketbookin.RunnersForindianFancy?.ToList()
                 ?? new List<BettingServiceReference.RunnerForIndianFancy>();
                    objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>().ToArray();
                    BettingServiceReference.RunnerForIndianFancy objRunner1 = new BettingServiceReference.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                    runnerList.Add(objRunner1);
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
                                runnerList.Add(objRunner);
                            }
                        }
                        else
                        {
                            BettingServiceReference.RunnerForIndianFancy objRunner = new BettingServiceReference.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<BettingServiceReference.RunnerForIndianFancy>().ToArray();
                            runnerList.Add(objRunner);
                        }

                    }
                    BettingServiceReference.RunnerForIndianFancy objRunnerlast = new BettingServiceReference.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
                    runnerList.Add(objRunnerlast);
                    objmarketbookin.RunnersForindianFancy = runnerList.ToArray();
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
                    objmarketbookin.DebitCredit = lstDebitCredit.ToArray();
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

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
                    var runnerList = objmarketbookin.RunnersForindianFancy?.ToList()
                 ?? new List<BettingServiceReference.RunnerForIndianFancy>();
                    objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>().ToArray();
                    RunnerForIndianFancy objRunner1 = new RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                    runnerList.Add(objRunner1);
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
                                runnerList.Add(objRunner);
                            }
                        }
                        else
                        {
                            RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>().ToArray();
                            runnerList.Add(objRunner);
                        }

                    }
                    RunnerForIndianFancy objRunnerlast = new RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
                    runnerList.Add(objRunnerlast);
                    objmarketbookin.RunnersForindianFancy = runnerList.ToArray();
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
                    objmarketbookin.DebitCredit = lstDebitCredit.ToArray();
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                    }


                }
            }

            return objmarketbookin;
        }
        public DebitCredit[] ceckProfitandLossFig(MarketBook marketbookstatus, List<UserBets> lstUserBets)
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
            return lstDebitCredit.ToArray();
        }
        public long GetLiabalityofCurrentUser(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true && item.location != "8").ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();

            foreach (var item in lstMarketIDS)
            {
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                MarketBook objMarketbook = new MarketBook();
                var runnerList = objMarketbook.Runners?.ToList() ?? new List<BettingServiceReference.Runner>();
                objMarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                foreach (var selectionitem in selectionIDs)
                {
                    BettingServiceReference.Runner objrunner = new BettingServiceReference.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    runnerList.Add(objrunner);                    
                }
                objMarketbook.Runners = runnerList.ToArray();
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLoss(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                if (marketbookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketbook.Runners)
                    {
                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (Profit > Loss)
                            profitandloss += Loss;
                        else
                            profitandloss += Profit;
                    }

                    LastProfitandLoss = profitandloss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
                else
                {

                    if (objMarketbook.Runners.Count() == 1)
                    {
                        long ProfitandLoss = 0;
                        MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);

                        if (CurrentMarketProfitandloss.Runners != null)
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);

                        LastProfitandLoss = ProfitandLoss;
                    }
                    else
                    {
                        foreach (var runner in objMarketbook.Runners)
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            if (LastProfitandLoss > ProfitandLoss)
                            {
                                LastProfitandLoss = ProfitandLoss;
                            }
                        }
                    }
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
            }
            return OverAllLiabality;
        }
        public long GetLiabalityofCurrentUserfancy(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();

            foreach (var item in lstMarketIDS)
            {
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                MarketBook objMarketbook = new MarketBook();
                objMarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                var runnerList = objMarketbook.Runners?.ToList() ?? new List<BettingServiceReference.Runner>();
                objMarketbook.MarketId = item;
                foreach (var selectionitem in selectionIDs)
                {
                    BettingServiceReference.Runner objrunner = new BettingServiceReference.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    runnerList.Add(objrunner);
                }
                objMarketbook.Runners = runnerList.ToArray();
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLossfancy(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                long ProfitandLoss = 0;
                lstUserBets = lstUserBets.Where(s => s.location == "9").ToList();
                var slections = lstUserBets.Select(d => d.SelectionID).Distinct().ToList();
                foreach (var item2 in slections)
                {
                    MarketBookForindianFancy CurrentMarketProfitandloss = GetBookPositionIN(objMarketbook.MarketId, item2, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<UserBetsforAgent>(), lstUserBets);
                    if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                    {
                        ProfitandLoss += Convert.ToInt64(CurrentMarketProfitandloss.RunnersForindianFancy.Min(t => t.ProfitandLoss));
                    }
                    CurrentMarketProfitandloss = new MarketBookForindianFancy();
                }
                LastProfitandLoss = ProfitandLoss;
                OverAllLiabality += LastProfitandLoss;
                LastProfitandLoss = 0;
            }
            return OverAllLiabality;
        }
        public MarketBook GetBookPosition(string marketBookID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBets> CurrentUserBets)
        {
            MarketBook objmarketbook = new MarketBook();
            var runnerList = objmarketbook.Runners?.ToList() ?? new List<BettingServiceReference.Runner>();
            List<DebitCredit> lstDebitCredit = new List<DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                List<UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                    objmarketbook.MarketId = marketBookID;
                    objmarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                    BettingServiceReference.Runner objRunner1 = new BettingServiceReference.Runner();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    runnerList.Add(objRunner1);
                    objmarketbook.Runners = runnerList.ToArray();
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
                                runnerList.Add(objRunner);
                            }
                        }
                        else
                        {
                            BettingServiceReference.Runner objRunner = new BettingServiceReference.Runner();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();

                            objmarketbook.Runners = runnerList.ToArray();
                        }
                    }
                    BettingServiceReference.Runner objRunnerlast = new BettingServiceReference.Runner();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    runnerList.Add(objRunnerlast);

                    objmarketbook.Runners = runnerList.ToArray();
                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        decimal superpercent = 0;
                        if (superrate > 0)
                            superpercent = superrate - agentrate;
                        else
                            superpercent = 0;

                        agentrate = agentrate + superpercent;
                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((0) / 100));
                            var objDebitCredit = new DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
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
                                foreach (var runneritem in objmarketbook.Runners)
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
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
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
                                foreach (var runneritem in objmarketbook.Runners)
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
                        }
                    }

                    objmarketbook.DebitCredit = lstDebitCredit.ToArray();
                    foreach (var runneritem in objmarketbook.Runners)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                    }
                }
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                    List<UserBetsforSuper> lstCurrentBetsAdmin = CurrentSuperBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                    if (lstCurrentBetsAdmin.Count > 0)
                    {
                        lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                        objmarketbook.MarketId = marketBookID;
                        objmarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                        BettingServiceReference.Runner objRunner1 = new BettingServiceReference.Runner();
                        objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                        objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                        runnerList.Add(objRunner1);
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
                                    runnerList.Add(objRunner);

                                }
                            }
                            else
                            {
                                BettingServiceReference.Runner objRunner = new BettingServiceReference.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objmarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                                runnerList.Add(objRunner);
                            }
                        }
                        BettingServiceReference.Runner objRunnerlast = new BettingServiceReference.Runner();
                        objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                        objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                        runnerList.Add(objRunnerlast);
                        objmarketbook.Runners = runnerList.ToArray();
                        ///calculation
                        var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                        foreach (var userid in lstUsers)
                        {
                            var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                            decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                            decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                            bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                            decimal superpercent = superrate - agentrate;
                            foreach (var userbet in lstCurrentBetsbyUser)
                            {
                                var totamount = (superpercent / 100) * (Convert.ToDecimal(userbet.Amount));
                                var objDebitCredit = new DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbook.Runners)
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
                                    foreach (var runneritem in objmarketbook.Runners)
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
                                    double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbook.Runners)
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
                                    foreach (var runneritem in objmarketbook.Runners)
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
                            }
                        }

                        objmarketbook.DebitCredit = lstDebitCredit.ToArray();
                        foreach (var runneritem in objmarketbook.Runners)
                        {
                            runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                        }
                    }
                }
                else
                {
                    
                        if (LoggedinUserDetail.GetUserTypeID() == 3)
                        {

                            List<UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                            if (lstCurrentBets.Count > 0)
                            {
                                objmarketbook.MarketId = marketBookID;
                                objmarketbook.MarketBookName = lstCurrentBets[0].MarketBookname;
                                objmarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                                BettingServiceReference.Runner objRunner1 = new BettingServiceReference.Runner();
                                objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBets[0].UserOdd) - 1).ToString();
                                objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                                runnerList.Add(objRunner1);
                                foreach (var userbet in lstCurrentBets)
                                {
                                    if (objmarketbook.Runners != null)
                                    {
                                        BettingServiceReference.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                        if (objexistingrunner == null)
                                        {
                                            BettingServiceReference.Runner objRunner = new BettingServiceReference.Runner();
                                            objRunner.SelectionId = userbet.UserOdd;
                                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                            runnerList.Add(objRunner);
                                        }
                                    }
                                    else
                                    {
                                        BettingServiceReference.Runner objRunner = new BettingServiceReference.Runner();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                        objmarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                                        runnerList.Add(objRunner);
                                    }



                                }
                                BettingServiceReference.Runner objRunnerlast = new BettingServiceReference.Runner();
                                objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBets.Last().UserOdd) + 1).ToString();
                                objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                                runnerList.Add(objRunnerlast);
                            objmarketbook.Runners = runnerList.ToArray();
                                 
                                // calculation
                                foreach (var userbet in lstCurrentBets)
                                {

                                    var totamount = (Convert.ToDecimal(userbet.Amount));
                                    var objDebitCredit = new DebitCredit();
                                    if (userbet.BetType == "back")
                                    {
                                        double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbook.Runners)
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
                                        foreach (var runneritem in objmarketbook.Runners)
                                        {
                                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
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
                                        double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbook.Runners)
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
                                        foreach (var runneritem in objmarketbook.Runners)
                                        {
                                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
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
                                objmarketbook.DebitCredit = lstDebitCredit.ToArray();
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }
                        }
                    

                }
            }
            return objmarketbook;
        }
        public DebitCredit[] ceckProfitandLossfancy(MarketBook marketbookstatus, List<UserBets> lstUserBets)
        {

            List<DebitCredit> lstDebitCredit = new List<DebitCredit>();

            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId).ToList();
            if (lstUserbetsbyMarketID != null)
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
                return lstDebitCredit.ToArray();
            }
            else
            {
                return lstDebitCredit.ToArray();
            }

        }
        public long GetLiabalityofCurrentUserActual(int userID, string selectionID, string BetType, string marketbookID, string Marketbookname, string _password)
        {
            long OverAllLiabality = 0;
            string LastProfitandLoss = "";
            List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID, _password));
            lstUserBets = lstUserBets.Where(item => item.location != "9").ToList();
            List<string> lstIDS = new List<string>();
            lstIDS.Add(marketbookID);
            var lstMarketIDS = lstIDS;
            foreach (var item in lstMarketIDS)
            {
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                var runnerList = new List<BettingServiceReference.Runner>();
                MarketBook objMarketbook = new MarketBook();
                objMarketbook.MarketBookName = Marketbookname;
                objMarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                foreach (var selectionitem in selectionIDs)
                {
                    BettingServiceReference.Runner objrunner = new BettingServiceReference.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    runnerList.Add(objrunner);
                }
                objMarketbook.Runners = runnerList.ToArray();
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLoss(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                if (marketbookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketbook.Runners)
                    {
                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (Profit > Loss)
                            profitandloss += Loss;
                        else
                            profitandloss += Profit;

                    }
                    LastProfitandLoss = profitandloss.ToString();
                }
                else
                {
                    if (objMarketbook.Runners.Length == 1)
                    {
                        if (BetType == "back")
                        {
                            MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);
                            if (CurrentMarketProfitandloss.Runners != null)
                            {
                                CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item1 => Convert.ToInt32(item1.SelectionId) <= Convert.ToInt32(selectionID)).ToArray();
                                LastProfitandLoss = CurrentMarketProfitandloss.Runners.Min(item2 => item2.ProfitandLoss).ToString();
                            }
                        }
                        else
                        {
                            MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);
                            if (CurrentMarketProfitandloss.Runners != null)
                            {
                                CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item1 => Convert.ToInt32(item1.SelectionId) >= Convert.ToInt32(selectionID)).ToArray();
                                LastProfitandLoss = CurrentMarketProfitandloss.Runners.Min(item2 => item2.ProfitandLoss).ToString();
                            }
                        }
                    }
                    else
                    {
                        if (BetType == "back")
                        {
                            foreach (var runner in objMarketbook.Runners)
                            {
                                if (runner.SelectionId != selectionID)
                                {
                                    long ProfitandLoss = 0;
                                    ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                    if (LastProfitandLoss == "")
                                    {
                                        LastProfitandLoss = ProfitandLoss.ToString();
                                    }
                                    else
                                    {
                                        if (objMarketbook.Runners.Length == 1)
                                        {
                                            if (ProfitandLoss > 0)
                                            {
                                                ProfitandLoss = ProfitandLoss * 1;
                                            }
                                        }
                                        if (Convert.ToInt64(LastProfitandLoss) > ProfitandLoss)
                                        {
                                            LastProfitandLoss = ProfitandLoss.ToString();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == selectionID).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == selectionID).Sum(item2 => item2.Credit));
                            if (objMarketbook.Runners.Length == 1)
                            {
                                if (ProfitandLoss > 0)
                                {
                                    ProfitandLoss = ProfitandLoss * 1;
                                }
                            }
                            LastProfitandLoss = ProfitandLoss.ToString();
                        }
                    }
                }
            }
            if (LastProfitandLoss == "")
            {
                OverAllLiabality += 0;
            }
            else
            {
                OverAllLiabality += Convert.ToInt64(LastProfitandLoss);
            }
            LastProfitandLoss = "";
            return OverAllLiabality;
        }
        public long GetLiabalityofCurrentUserActualforOtherMarkets(int userID, string selectionID, string BetType, string marketbookID, string Marketbookname, List<UserBets> lstUserBets)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            var lstMarketIDSOthers = lstUserBets.Where(item2 => item2.MarketBookID != marketbookID).Select(item => item.MarketBookID).Distinct().ToArray();
            foreach (var item in lstMarketIDSOthers)
            {
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                MarketBook objMarketbook = new MarketBook();
                var runnerList = new List<BettingServiceReference.Runner>();
                objMarketbook.MarketBookName = Marketbookname;
                objMarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                foreach (var selectionitem in selectionIDs)
                {
                    BettingServiceReference.Runner objrunner = new BettingServiceReference.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    runnerList.Add(objrunner);
                }
                objMarketbook.Runners = runnerList.ToArray();
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    if (objMarketbook.MarketId.Length > 8)
                    {
                        objMarketbook.DebitCredit = ceckProfitandLoss(objMarketbook, lstUserBets);
                    }
                }
                List<UserBets> marketbooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                if (marketbookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketbook.Runners)
                    {
                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (Profit > Loss)
                            profitandloss += Loss;
                        else
                            profitandloss += Profit;
                    }
                    LastProfitandLoss = profitandloss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
                else
                {
                    if (objMarketbook.Runners.Length == 1)
                    {
                        long ProfitandLoss = 0;
                        MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);
                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }
                        LastProfitandLoss = ProfitandLoss;
                    }
                    else
                    {
                        foreach (var runner in objMarketbook.Runners)
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            if (objMarketbook.Runners.Length == 1)
                            {
                                if (ProfitandLoss > 0)
                                {
                                    ProfitandLoss = ProfitandLoss * 1;
                                }
                            }
                            if (LastProfitandLoss > ProfitandLoss)
                            {
                                LastProfitandLoss = ProfitandLoss;
                            }
                        }
                    }
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
            }
            return OverAllLiabality;
        }
        public List<LiabalitybyMarket> GetLiabalityofCurrentUserbyMarkets(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
            List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
            foreach (var item in lstMarketIDS)
            {
                LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                MarketBook objMarketbook = new MarketBook();
                var runnerList = new List<BettingServiceReference.Runner>();
                objMarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                foreach (var selectionitem in selectionIDs)
                {
                    BettingServiceReference.Runner objrunner = new BettingServiceReference.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    runnerList.Add(objrunner);
                }
                objMarketbook.Runners = runnerList.ToArray();
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLoss(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                if (marketbookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketbook.Runners)
                    {
                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (Profit > Loss)
                            profitandloss += Loss;
                        else
                            profitandloss += Profit;
                    }
                    objMarketLiabality.MarketBookID = item;
                    objMarketLiabality.MarketBookName = marketbookname;
                    objMarketLiabality.Liabality = profitandloss;
                    lstLiabalitybyMarket.Add(objMarketLiabality);
                    LastProfitandLoss = profitandloss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
                else
                {
                    foreach (var runner in objMarketbook.Runners)
                    {
                        long ProfitandLoss = 0;
                        ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (objMarketbook.Runners.Length == 1)
                        {
                            if (ProfitandLoss > 0)
                                ProfitandLoss = -1 * ProfitandLoss;
                        }
                        if (LastProfitandLoss > ProfitandLoss)
                            LastProfitandLoss = ProfitandLoss;
                    }
                    objMarketLiabality.MarketBookID = item;
                    objMarketLiabality.MarketBookName = marketbookname;
                    objMarketLiabality.Liabality = LastProfitandLoss;
                    lstLiabalitybyMarket.Add(objMarketLiabality);
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
            }
            return lstLiabalitybyMarket;
        }
        public List<LiabalitybyMarket> GetLiabalityofCurrentAgentbyMarkets(List<UserBetsforAgent> lstUserBet)
        {
            try
            {
                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<MarketBook> lstmarketbooks = new List<MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    MarketBook objMarketbook = new MarketBook();
                    var runnerList = new List<BettingServiceReference.Runner>();
                    objMarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                    foreach (var selectionitem in selectionIDs)
                    {
                        BettingServiceReference.Runner objrunner = new BettingServiceReference.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        runnerList.Add(objrunner);
                    }
                    objMarketbook.Runners = runnerList.ToArray();

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsforAgent> marketbooknames = new List<UserBetsforAgent>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }
                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.Loss += Convert.ToInt64(-1 * profit);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (objMarketbook.Runners.Length == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = profitorloss * -1;
                                            }
                                        }
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                    }
                                }
                            }
                        }
                    }
                    lstmarketbooks.Add(objMarketbook);
                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var marketbook in lstmarketbooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {
                            long Profit = 0;
                            long Loss = 0;
                            Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                            if (Profit < Loss)
                                profitandloss += Loss;
                            else
                                profitandloss += Profit;
                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                }
                return lstLiabalitybyMarket;
            }
            catch (System.Exception ex)
            {
                return new List<LiabalitybyMarket>();
            }
        }
        public List<LiabalitybyMarket> GetLiabalityofAdminbyMarkets(List<UserBetsForAdmin> lstUserBet)
        {
            try
            {
                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<MarketBook> lstmarketbooks = new List<MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    MarketBook objMarketbook = new MarketBook();
                    var runnerList = new List<BettingServiceReference.Runner>();
                    objMarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                    foreach (var selectionitem in selectionIDs)
                    {
                        BettingServiceReference.Runner objrunner = new BettingServiceReference.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        runnerList.Add(objrunner);
                    }
                    objMarketbook.Runners = runnerList.ToArray();
                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsForAdmin> marketbooknames = new List<UserBetsForAdmin>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }
                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAdmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAdmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (objMarketbook.Runners.Length == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = profitorloss * -1;
                                            }
                                        }
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                    }
                                }
                            }
                        }
                    }
                    lstmarketbooks.Add(objMarketbook);
                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var marketbook in lstmarketbooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {
                            long Profit = 0;
                            long Loss = 0;
                            Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                            if (Profit < Loss)
                                profitandloss += Loss;
                            else
                                profitandloss += Profit;
                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {
                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                }

                return lstLiabalitybyMarket;
            }
            catch (System.Exception ex)
            {
                return new List<LiabalitybyMarket>();
            }
        }
        public List<LiabalitybyMarket> GetLiabalityofSuperbyMarkets(List<UserBetsforSuper> lstUserBet)
        {
            try
            {
                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsForAdmin());
                List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<MarketBook> lstmarketbooks = new List<MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    MarketBook objMarketbook = new MarketBook();
                    var runnerList = new List<BettingServiceReference.Runner>();
                    objMarketbook.Runners = new List<BettingServiceReference.Runner>().ToArray();
                    foreach (var selectionitem in selectionIDs)
                    {
                       BettingServiceReference.Runner objrunner = new BettingServiceReference.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                            runnerList.Add(objrunner);
                    }
                    objMarketbook.Runners = runnerList.ToArray();

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsforSuper> marketbooknames = new List<UserBetsforSuper>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }
                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSuper(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSuper(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (objMarketbook.Runners.Length == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = profitorloss * -1;
                                            }
                                        }
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                    }
                                }
                            }
                        }
                    }
                    lstmarketbooks.Add(objMarketbook);

                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var marketbook in lstmarketbooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {
                            long Profit = 0;
                            long Loss = 0;
                            Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                            if (Profit < Loss)
                                profitandloss += Loss;
                            else
                                profitandloss += Profit;
                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                }
                return lstLiabalitybyMarket;
            }
            catch (System.Exception ex)
            {
                return new List<LiabalitybyMarket>();
            }
        }
        public DebitCredit[] ceckProfitandLossAgent(MarketBook marketbookstatus, List<UserBetsforAgent> lstUserBets)
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
                return lstDebitCredit.ToArray();
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
                }
                return lstDebitCredit.ToArray();
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
                        return lstDebitCredit.ToArray();
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
                return lstDebitCredit.ToArray();
            }
        }
        public DebitCredit[] ceckProfitandLossAdmin(MarketBook marketbookstatus, List<UserBetsForAdmin> lstUserBets)
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
                return lstDebitCredit.ToArray();
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
                }
                return lstDebitCredit.ToArray();
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
                        return lstDebitCredit.ToArray();
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
                return lstDebitCredit.ToArray();
            }
        }

        public DebitCredit[] ProfitandLossSuper(MarketBook marketbookstatus, List<UserBetsforSuper> lstUserBets)
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
                return lstDebitCredit.ToArray();
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
                }
                return lstDebitCredit.ToArray();
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
                        return lstDebitCredit.ToArray();

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
                return lstDebitCredit.ToArray();
            }
        }
        public List<DebitCredit> ceckProfitandLossSamiadmin(MarketBook marketbookstatus, List<UserBetsforSamiadmin> lstUserBets)
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
        public DebitCredit[] ceckProfitandLossSuper(MarketBook marketbookstatus, List<UserBetsforSuper> lstUserBets)
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
                return lstDebitCredit.ToArray();
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
                return lstDebitCredit.ToArray();
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

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                        return lstDebitCredit.ToArray();

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

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit.ToArray();
            }


        }
    }
}
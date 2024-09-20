using DataAccess.BLL;
using ExternalAPI.TO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Test.wrBetEasyNG;

namespace GTCore.ViewModel
{
    public class GetDataFromOtherSource
    {
        //BettingService objBettingService; //= new BettingService();
        public HttpResponseMessage ReturnPureJson(object responseModel)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            string jsonClient = JsonConvert.SerializeObject(responseModel);

            byte[] resultBytes = Encoding.UTF8.GetBytes(jsonClient);
            response.Content = new StreamContent(new MemoryStream(resultBytes));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

            return response;
        }
        public void GetUserSessionForData(string Username, string Password)
        {
            try
            {
                UserLoginObject objuser = new UserLoginObject();
                objuser.Username = Username;
                objuser.Password = Password;
                objuser.SoftwareId = "BetPro";
                objuser.DeviceId = "AEBD9416 IQBALPRACHA@DESKTOP-SN5MV98";

                var client = new RestClient(ConfigurationManager.AppSettings["URLForData"]);
                var request = new RestRequest("/UserLogin", Method.Post);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(objuser);

                var response = client.Execute(request);

                var obj = JObject.Parse(response.Content);
                BetterInfo objBetter = JsonConvert.DeserializeObject<BetterInfo>(obj.ToString());


                APIConfig.objCurrSession.id = objBetter.BetterId;
                APIConfig.objCurrSession.token = objBetter.PrivateKey;
                APIConfig.objCurrSession.Username = objBetter.BetterName;
                APIConfig.objCurrSession.lastReq = Convert.ToDateTime(DateTime.Now.Date.ToString());
                APIConfig.objCurrSession.CurrentReq = Convert.ToDateTime(DateTime.Now.Date.ToString());
                APIConfig.objCurrSession.isAllowedBetting = false;
                APIConfig.objCurrSession.active = false;
                APIConfig.objCurrSession.anouncement = "";
                ///
                //APIConfig.objCurrSessionNew.id = objResult.BetterId;
                //APIConfig.objCurrSessionNew.token = objResult.PrivateKey;
                //APIConfig.objCurrSessionNew.username = objResult.BetterName;
                //APIConfig.objCurrSessionNew.lastReq = Convert.ToDateTime(DateTime.Now.Date.ToString());
                //APIConfig.objCurrSessionNew.CurrentReq = Convert.ToDateTime(DateTime.Now.Date.ToString());
                //APIConfig.objCurrSessionNew.isAllowedBetting = false;
                //APIConfig.objCurrSessionNew.active = false;
                //APIConfig.objCurrSessionNew.anouncement = "";
            }
            catch (System.Exception ex)
            {
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }
        public void GetCurrentMarketBookCricket(string Password, string marketIDs, bool isFancyMarkets)
        {
            try
            {
                if (APIConfig.uRLsData.Count == 0)
                {
                    //objBettingService.SetURLsData();
                }
                if (APIConfig.GetCricketDataFrom == "Live")
                {

                }
            }
            catch (System.Exception ex)
            {
                //  GetUserSessionForData(ConfigurationManager.AppSettings["UserNameforData"], ConfigurationManager.AppSettings["UserPasswordforData"]);
                APIConfig.WriteErrorToDB(ex.Message);
                

            }
        }
        public string convertdata(string string_0, string string_1, string string_3)
        {
            string str = "";
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            try
            {
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.KeySize = 128;
                rijndaelManaged.BlockSize = 128;
                byte[] numArray = Convert.FromBase64String(string_1);
                byte[] bytes = Encoding.UTF8.GetBytes(string.Concat(string_0, "1.", (string_3), ("BetEzy")));
                byte[] numArray1 = new byte[16];
                int length = (int)bytes.Length;
                if (length > (int)numArray1.Length)
                {
                    length = (int)numArray1.Length;
                }
                Array.Copy(bytes, numArray1, length);
                rijndaelManaged.Key = numArray1;
                rijndaelManaged.IV = numArray1;
                byte[] numArray2 = rijndaelManaged.CreateDecryptor().TransformFinalBlock(numArray, 0, (int)numArray.Length);
                str = Encoding.UTF8.GetString(numArray2);
            }
            catch (System.Exception exception)
            {

            }
            return str;
        }
        public static beteasyng wsBetEasyNGNew = new beteasyng();
        public void GetSessionfromothersource()
        {
            try
            {

                //wsBetEasyNGNew.Url = ConfigurationManager.AppSettings["URLForDataother"];
                wsBetEasyNGNew.Url = "https://inforeborn.com/UnitedBet/beteasynewng.asmx";
                string text = wsBetEasyNGNew.NG(0, 25, "sfs5646", "", ConfigurationManager.AppSettings["UserNameforDataOther"], ConfigurationManager.AppSettings["UserNameforDataOther"], ConfigurationManager.AppSettings["UserPasswordforDataOther"], "7FAISALSHAH2", ConfigurationManager.AppSettings["VendorforDataOther"], "", "");
                string[] results = text.Split('~');
                APIConfig.SessionforOtherdata = results[64].ToString().Trim();
            }
            catch (System.Exception ex)
            {
                
            }

        }
        public void GetCurrentMarketBooksOther123(string Password, string MarketIDs, TestBF.wsnew ws1, int EventTypeID, bool isFancyMarket)
        {
            try
            {
                if (APIConfig.SessionforOtherdata == "")
                {
                    GetSessionfromothersource();
                }
                //wsBetEasyNGNew.Url = ConfigurationManager.AppSettings["URLForDataother"];
                wsBetEasyNGNew.Url = "https://inforeborn.com/UnitedBet/beteasynewng.asmx";
                wsBetEasyNGNew.UserAgent = ".\".$.avb&.(.*.,..";
                string[] marketIds = MarketIDs.Split(new string[] { ", " }, StringSplitOptions.None);
                foreach (var item in marketIds)
                {
                    string text1 = wsBetEasyNGNew.NG(1, 606060, "sdflsdlfs09sd7f0-sd9sd", APIConfig.SessionforOtherdata, ConfigurationManager.AppSettings["UserNameforDataOther"], item, EventTypeID.ToString(), "", ("Computer"), EventTypeID.ToString(), "");

                    if (text1.Contains("session") || text1.Contains("Session"))
                    {
                        GetSessionfromothersource();
                        text1 = wsBetEasyNGNew.NG(1, 606060, "sdflsdlfs09sd7f0-sd9sd", APIConfig.SessionforOtherdata, ConfigurationManager.AppSettings["UserNameforDataOther"], item, EventTypeID.ToString(), "", ("Computer"), EventTypeID.ToString(), "");
                    }
                    string[] marketid = item.Split('.');
                    string string_22 = convertdata("@#$%^j", text1, marketid[1]);
                    ExternalAPI.TO.MarketBookString marketres = new ExternalAPI.TO.MarketBookString();
                    marketres.MarketBookId = item;
                    marketres.MarketBookData = string_22;
                    if (APIConfig.BFMarketBooksOther123 != null)
                    {
                        if (APIConfig.BFMarketBooksOther123.ToList().Count() > 0)
                        {
                            var currbpmarket = APIConfig.BFMarketBooksOther123.ToList().Where(item2 => item2.MarketBookId == item).FirstOrDefault();
                            if (currbpmarket != null)
                            {
                                var index = APIConfig.BFMarketBooksOther123.ToList().IndexOf(currbpmarket);

                                if (index != -1)
                                    APIConfig.BFMarketBooksOther123[index] = marketres;

                            }
                            else
                            {
                                APIConfig.BFMarketBooksOther123.Add(marketres);
                            }
                        }
                        else
                        {
                            APIConfig.BFMarketBooksOther123.Add(marketres);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

                APIConfig.WriteErrorToDB("Get Data from server " + ex.Message.ToString());
            }
            //string[] newres = string_22.Split(':');
            //string[] selection = newres[1].Split('|');
            //string[] selectiondata = selection[1].Split('~');
        }
        public void GetCurrentMarketBooksOther(string Password, string MarketIDs, TestBF.wsnew ws1, int EventTypeID, bool isFancyMarket)
        {
            string[] marketIds = MarketIDs.Split(new string[] { ", " }, StringSplitOptions.None);

            IList<wrBF.MarketBook> list;

            try
            {
                list = ws1.GD(APIConfig.SecurityCode, marketIds, EventTypeID);
                if (list != null)
                {
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            if (item.MarketId != null)
                            {
                                try
                                {

                                    if (isFancyMarket == false)
                                    {


                                        if (APIConfig.BFMarketBooksOther != null)
                                        {
                                            if (APIConfig.BFMarketBooksOther.Count() > 0)
                                            {
                                                var currbpmarket = APIConfig.BFMarketBooksOther.Where(item2 => item2.MarketId == item.MarketId).FirstOrDefault();
                                                if (currbpmarket != null)
                                                {
                                                    var index = APIConfig.BFMarketBooksOther.IndexOf(currbpmarket);

                                                    if (index != -1)
                                                        APIConfig.BFMarketBooksOther[index] = item;

                                                }
                                                else
                                                {
                                                    APIConfig.BFMarketBooksOther.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                APIConfig.BFMarketBooksOther.Add(item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (APIConfigforResults.BFMarketBooksOtherFancy != null)
                                        {
                                            if (APIConfigforResults.BFMarketBooksOtherFancy.Count() > 0)
                                            {
                                                var currbpmarket = APIConfigforResults.BFMarketBooksOtherFancy.Where(item2 => item2.MarketId == item.MarketId).FirstOrDefault();
                                                if (currbpmarket != null)
                                                {
                                                    var index = APIConfigforResults.BFMarketBooksOtherFancy.IndexOf(currbpmarket);

                                                    if (index != -1)
                                                        APIConfigforResults.BFMarketBooksOtherFancy[index] = item;
                                                }
                                                else
                                                {
                                                    APIConfigforResults.BFMarketBooksOtherFancy.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                APIConfigforResults.BFMarketBooksOtherFancy.Add(item);
                                            }
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }



        public void GetCurrentMarketFancy(List<UserMarket> markets)
        {
            try
            {
                //string[] marketIds = MarketIDs.Split(new string[] { ", " }, StringSplitOptions.None);
                foreach (var item in markets)
                {
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://royalold.com/api/MatchOdds/GetOddslite/4/" + item.MarketCatalogueID + "/" + item.EventID + ""));

                    WebReq.Method = "GET";

                    HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

                    Console.WriteLine(WebResp.StatusCode);
                    Console.WriteLine(WebResp.Server);

                    string jsonString;
                    using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
                    {
                        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                        jsonString = reader.ReadToEnd();
                    }
                    try
                    {

                        GetDataFancy myDeserializedClass = JsonConvert.DeserializeObject<GetDataFancy>(jsonString);
                                             
                        if (APIConfig.MarketBookForindianFancy != null)
                        {
                            string marketid = myDeserializedClass.market[0].marketId;
                            myDeserializedClass.MarketID = marketid;
                            myDeserializedClass.LinevMarkets = APIConfig.LinevMarkets;
                            if (APIConfig.MarketBookForindianFancy.ToList().Count() > 0)
                            {
                                var currbpmarket = APIConfig.MarketBookForindianFancy.ToList().Where(item2 => item2.market[0].marketId == marketid).FirstOrDefault();
                                if (currbpmarket != null)
                                {
                                    var index = APIConfig.MarketBookForindianFancy.ToList().IndexOf(currbpmarket);

                                    if (index != -1)
                                        APIConfig.MarketBookForindianFancy[index] = myDeserializedClass;

                                }
                                else
                                {
                                    APIConfig.MarketBookForindianFancy.Add(myDeserializedClass);
                                }
                            }
                            else
                            {
                                APIConfig.MarketBookForindianFancy.Add(myDeserializedClass);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        APIConfig.WriteErrorToDB("Get Data from server " + ex.Message.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.WriteErrorToDB("Get Data from server " + ex.Message.ToString());
            }
        }
    }
}
                
                

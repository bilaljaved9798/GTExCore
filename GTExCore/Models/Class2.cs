using BettingServiceReference;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GTExCore.Models
{
    public class wsnew
    {
        private readonly HttpClient _httpClient;

        public wsnew()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://109.169.22.130/theService/MainSvc.asmx")
            };
            _httpClient.DefaultRequestHeaders.Add("SOAPAction", "\"http://forexdatawsnew.org/\"");
        }

        public async Task<MarketBook[]> GDLBFAsync(string Scd, string[] marketIds, int n)
        {
            var soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <GDLBF xmlns=""http://forexdatawsnew.org/"">
                  <Scd>{Scd}</Scd>
                  <marketIds>{string.Join("", marketIds)}</marketIds>
                  <n>{n}</n>
                </GDLBF>
              </soap:Body>
            </soap:Envelope>";

            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

            var response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();

            // Deserialize the response
            var serializer = new XmlSerializer(typeof(MarketBook[]), new XmlRootAttribute("GDLBFResponse"));
            return (MarketBook[])serializer.Deserialize(responseStream);
        }

        public async Task<MarketBook[]> GDAsync(string Scd, string[] marketIds, int n)
        {
            var soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <GD xmlns=""http://forexdatawsnew.org/"">
                  <Scd>{Scd}</Scd>
                  <marketIds>{string.Join("", marketIds)}</marketIds>
                  <n>{n}</n>
                </GD>
              </soap:Body>
            </soap:Envelope>";

            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

            var response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();

            // Deserialize the response
            var serializer = new XmlSerializer(typeof(MarketBook[]), new XmlRootAttribute("GDResponse"));
            return (MarketBook[])serializer.Deserialize(responseStream);
        }

        public async Task<string> GetMarketBookAsync(string marketId)
        {
            var soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <getMarketBook xmlns=""http://bfbetpro.com/"">
                  <marketId>{marketId}</marketId>
                </getMarketBook>
              </soap:Body>
            </soap:Envelope>";

            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

            var response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public string Url
        {
            get => _httpClient.BaseAddress.ToString();
            set => _httpClient.BaseAddress = new System.Uri(value);
        }
    }

    // Example of a MarketBook class for deserialization.
  
}

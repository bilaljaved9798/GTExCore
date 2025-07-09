using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTExCore.Models
{
    public class LiveTVChannels
    {
        public int ID { get; set; }
        public string ChanelURL { get; set; }
        public string ChanelName { get; set; }
    }
    public class DimondRoot
    {
        public ScoreData scoreData { get; set; }
        public TvData tvData { get; set; }
    }

    public class ScoreData
    {
        public bool message { get; set; }
        public string iframeUrl { get; set; }
    }

    public class TvData
    {
        public bool message { get; set; }
        public string beventId { get; set; }
        public int eventid { get; set; }
        public string iframeUrl { get; set; }
    }
    public partial class TVlink
    {
        public int ID { get; set; }
        public string tvlink1 { get; set; }
        public string scorecard { get; set; }
        public string EventID { get; set; }
        public string Name { get; set; }
        public string Eventtypeid { get; set; }
        public string CompName { get; set; }
        public Nullable<System.DateTime> Opendate { get; set; }
        public string DimondID { get; set; }
    }
}
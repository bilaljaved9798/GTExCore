using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class Root
    {
       
        public List<List<T1ItemItem>> t1 { get; set; }
      
        public List<T2Item> t2 { get; set; }
       
        public List<T3Item> t3 { get; set; }
       
        public List<T4Item> t4 { get; set; }
    }

    public class T1ItemItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mstatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string iplay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string srno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gtype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string utime { get; set; }
    }

    public class Bm1Item
    {
        /// <summary>
        /// 
        /// </summary>
        public string mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string min { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string max { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string s { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gtype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string utime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b1s { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b2s { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b3s { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l1s { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l2s { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l3s { get; set; }
    }

    public class T2Item
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Bm1Item> bm1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> bm2 { get; set; }
    }

    public class T3Item
    {
        /// <summary>
        /// 
        /// </summary>
        public string mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gtype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string utime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gvalid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gstatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string min { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string max { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string srno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string s1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string s2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ballsess { get; set; }
    }

    public class T4Item
    {
        /// <summary>
        /// 
        /// </summary>
        public string mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bs1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string l1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ls1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gtype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string utime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gvalid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gstatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string min { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string max { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string srno { get; set; }
    }
}
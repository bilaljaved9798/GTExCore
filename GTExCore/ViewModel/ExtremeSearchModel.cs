using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTCore.ViewModel
{
    public class ExtremeSearchModel
    {
        public List<ExtremeFilter> filters;
        public List<ExtremeParam> defaultParam;
        public string orderBy;
        public string orderDirection;
        public string isExport;
        public string groupBy;
        
        public long? skip;
        public long? take;
       
    }
    public class ExtremeFilter
    {
        public string columnName;
        public string OperatorName;
        public string filterValue;
    }
    public class ExtremeParam
    {
        public string paramName;       
        public string paramValue;
    }
}

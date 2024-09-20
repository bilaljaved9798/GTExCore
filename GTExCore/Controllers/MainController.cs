using Census.API.Common;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace Census.API.Controllers
{

	public class MainController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public MainController(HttpContextAccessor httpContextAccessor)
        {
            
        }
    }
}

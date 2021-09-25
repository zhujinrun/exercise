using IDCM.Contract.Client.Extension;
using IDCM.Contract.IGrains;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using System.Threading.Tasks;

namespace IDCM.Contract.Client.Controllers
{
    [ApiController]
    [Route("api/baseinfo/[action]")]
    public class BaseInfoController: ControllerBase
    {
        private readonly ILogger<BaseInfoController> _logger;
        private readonly IBaseDataGrains _baseDataGrains;

        public BaseInfoController()
        {
            //_logger = logger;
            var serviceProvider = GlobalConfigure.ServiceLocatorInstance.CreateScope().ServiceProvider;

          
            this._baseDataGrains = serviceProvider.GetRequiredService<IOrleansClient>().GetGrain<IBaseDataGrains>(1);
        }
        [HttpPost]
     
        public async Task<bool> GetError()
        {
            return await this._baseDataGrains.GetError();
        }

        [HttpGet]
        
        public string RtnString()
        {
            return "OK";
        }
        [HttpGet]

        public Task<bool> SaveOrder()
        {
            return _baseDataGrains.SaveOrder();
        }
    }
}

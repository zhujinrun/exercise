using IDCM.Contract.IGrains;
using Orleans;
using System;
using System.Threading.Tasks;

namespace IDCM.Contract.WebApi.Orleans
{
    public class BaseInfoGrains : Grain, IBaseDataGrains
    {

        public BaseInfoGrains()
        {

        }
        public Task<bool> GetError()
        {
            throw new Exception("error");
        }
    }
}

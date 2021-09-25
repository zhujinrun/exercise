using IDCM.Contract.IBusiness;
using IDCM.Contract.IGrains;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;

namespace IDCM.Contract.WebApi.Orleans
{
    public class BaseInfoGrains : Grain, IBaseDataGrains
    {
        private IOrderBusiness _orderBusiness;
        public BaseInfoGrains(IOrderBusiness orderBusiness)
        {
            _orderBusiness =  orderBusiness;
        }
        public Task<bool> GetError()
        {
            throw new Exception("error");
        }
        public Task<bool> SaveOrder()
        {
            return _orderBusiness.Save(new { });
        }


    }
}

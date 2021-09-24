using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDCM.Contract.IGrains
{
    public interface IBaseDataGrains : Orleans.IGrainWithIntegerKey
    {
        Task<bool> GetError();
    }
}

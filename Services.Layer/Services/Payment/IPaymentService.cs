using Common.Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Services.Payment
{
    public interface IPaymentService
    {
        public Task<Response<object>> Pay();
    }
}

using Integrators.Abstractions;
using Integrators.Dispatcher;
using MoneyEntity.Logic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic
{
    public class MoneyDispatcher : HostDispatcher
    {
        public MoneyDispatcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<string> DispatchSpecified<TIn>(string key, TIn request)
            where TIn : MoneyCommandRequest
        {
            var result = await base.DispatchSimple<TIn, MoneyCommandResponse>(key, request);
            return (result?.IsSuccess ?? false) ?
                result.ToBotString() : 
                (result?.ErrorText ?? "Ошибка обработки запроса");
        }

    }
}

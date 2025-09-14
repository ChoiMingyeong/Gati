using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ServerLib;

namespace GameApi.Filters
{
    public class SafeHubFilter : IHubFilter
    {
        public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext context, Func<HubInvocationContext, ValueTask<object?>> next)
        {
            try
            {
                return await next(context);
            }
            catch (Exception ex)
            {
                Logger.Default.LogWarning(ex, "[HubFilter] Bad payload or handler error: {0}", context.HubMethodName);
                context.Context.Abort();
                throw;
            }
        }
    }
}



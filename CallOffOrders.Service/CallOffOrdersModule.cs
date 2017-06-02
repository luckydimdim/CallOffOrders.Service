using System;
using System.Collections.Generic;
using Cmas.BusinessLayers.CallOffOrders.Entities;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Validation;
using Cmas.Infrastructure.ErrorHandler;
using System.Threading;
using System.Threading.Tasks;
using Cmas.Infrastructure.Security;
using Cmas.Services.CallOffOrders.Dtos.Responses;

namespace Cmas.Services.CallOffOrders
{
    public class CallOffOrdersModule : NancyModule
    { 
        private IServiceProvider _serviceProvider;

        private CallOffOrdersService callOffOrdersService;

        private CallOffOrdersService _callOffOrdersService
        {
            get
            {
                if (callOffOrdersService == null)
                    callOffOrdersService = new CallOffOrdersService(_serviceProvider, Context);

                return callOffOrdersService;
            }
        }

        public CallOffOrdersModule(IServiceProvider serviceProvider) : base("/call-off-orders")
        {
            this.RequiresAnyRole(new[] { Role.Contractor,Role.Customer});
            _serviceProvider = serviceProvider;
            
            /// <summary>
            /// /call-off-orders - получить наряд заказы
            /// </summary>
            Get<IEnumerable<SimpleCallOffOrderResponse>>("/", GetCallOffOrdersAsync,
                (ctx) => !ctx.Request.Query.ContainsKey("contractId"));

            /// <summary>
            /// /call-off-orders?contractId={id} - получить наряд заказы по указанному договору
            /// </summary>
            Get<IEnumerable<SimpleCallOffOrderResponse>>("/",
                GetCallOffOrdersByContractIdAsync,
                (ctx) => ctx.Request.Query.ContainsKey("contractId"));

            /// <summary>
            /// /call-off-orders/{id} - получить наряд заказ по указанному ID
            /// </summary>
            Get<DetailedCallOffOrderResponse>("/{id}", GetCallOffOrderHandlerAsync);

            /// <summary>
            /// Создать наряд заказ
            /// </summary>
            Post<string>("/", CreateCallOffOrderHandlerAsync);

            /// <summary>
            /// Получить данные для создания наряд заказа
            /// </summary>
            Get<CallOffOrderToCreateResponse>("/to-create/{contractId}", CallOffOrderToCreateHandlerAsync);

            /// <summary>
            /// Обновить наряд заказ
            /// </summary>
            Put<string>("/{id}", UpdateCallOffOrderHandlerAsync);

            /// <summary>
            /// Удалить наряд заказ
            /// </summary>
            Delete<string>("/{id}", DeleteCallOffOrderHandlerAsync);
        }

        #region Обработчики

        private async Task<DetailedCallOffOrderResponse> GetCallOffOrderHandlerAsync(dynamic args, CancellationToken ct)
        {
            return await _callOffOrdersService.GetCallOffOrderAsync((string) args.id);
        }
         
        private async Task<CallOffOrderToCreateResponse> CallOffOrderToCreateHandlerAsync(dynamic args, CancellationToken ct)
        {
            this.RequiresAnyRole(new[] { Role.Customer });
            
            return await _callOffOrdersService.CallOffOrderToCreateAsync(args.contractId);
        }

        private async Task<string> CreateCallOffOrderHandlerAsync(dynamic args, CancellationToken ct)
        {
            this.RequiresAnyRole(new[] { Role.Customer });

            CreateCallOffOrderRequest request = this.Bind();

            var validationResult = this.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationErrorException(validationResult.FormattedErrors);
            }

            return await _callOffOrdersService.CreateCallOffOrderAsync(request);
        }

        private async Task<string> UpdateCallOffOrderHandlerAsync(dynamic args, CancellationToken ct)
        {
            this.RequiresAnyRole(new[] { Role.Customer });

            UpdateCallOffOrderRequest request = this.Bind<UpdateCallOffOrderRequest>();

            var validationResult = this.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationErrorException(validationResult.FormattedErrors);
            }

            return await _callOffOrdersService.UpdateCallOffOrderAsync(args.Id, request);
        }

        private async Task<string> DeleteCallOffOrderHandlerAsync(dynamic args, CancellationToken ct)
        {
            this.RequiresAnyRole(new[] { Role.Customer });

            return await _callOffOrdersService.DeleteCallOffOrderAsync(args.id);
        }

        private async Task<IEnumerable<SimpleCallOffOrderResponse>> GetCallOffOrdersAsync(dynamic args, CancellationToken ct)
        {
            return await _callOffOrdersService.GetCallOffOrdersAsync();
        }

        private async Task<IEnumerable<SimpleCallOffOrderResponse>> GetCallOffOrdersByContractIdAsync(dynamic args,
            CancellationToken ct)
        {
            return await _callOffOrdersService.GetCallOffOrdersByContractIdAsync(Request.Query["contractId"]);
        }

        #endregion
    }
}
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
            this.RequiresRoles(new[] { Role.Contractor,Role.Customer});
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
            /// Обновить наряд заказ
            /// </summary>
            Put<string>("/{id}", UpdateCallOffOrderHandlerAsync);

            /// <summary>
            /// Создать ставку в наряд заказе
            /// </summary>
            Post<Rate>("/{id}/rates", AddRateHandlerAsync);

            /// <summary>
            /// Удалить ставку в наряд заказе
            /// </summary>
            Delete<Negotiator>("/{callOffOrderId}/rates/{rateId}", DeleteRateHandlerAsync);

            /// <summary>
            /// Обновить ставку в наряд заказе
            /// </summary>
            Put<Negotiator>("/{callOffOrderId}/rates/{rateId}", UpdateRateHandlerAsync);

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

        private async Task<string> CreateCallOffOrderHandlerAsync(dynamic args, CancellationToken ct)
        {
            this.RequiresRoles(new[] { Role.Customer });

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
            this.RequiresRoles(new[] { Role.Customer });

            UpdateCallOffOrderRequest request = this.Bind<UpdateCallOffOrderRequest>();

            var validationResult = this.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationErrorException(validationResult.FormattedErrors);
            }

            return await _callOffOrdersService.UpdateCallOffOrderAsync(args.Id, request);
        }

        private async Task<Rate> AddRateHandlerAsync(dynamic args, CancellationToken ct)
        {
            this.RequiresRoles(new[] { Role.Customer });

            CreateRateRequest request = this.Bind();

            var validationResult = this.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationErrorException(validationResult.FormattedErrors);
            }

            return await _callOffOrdersService.AddRateAsync(args.id, request);
        }

        private async Task<Negotiator> DeleteRateHandlerAsync(dynamic args, CancellationToken ct)
        {
            this.RequiresRoles(new[] { Role.Customer });

            await _callOffOrdersService.DeleteRateAsync(args.callOffOrderId, args.rateId);

            return Negotiate.WithStatusCode(HttpStatusCode.OK);
        }

        private async Task<Negotiator> UpdateRateHandlerAsync(dynamic args, CancellationToken ct)
        {
            this.RequiresRoles(new[] { Role.Customer });

            UpdateRateRequest request = this.Bind<UpdateRateRequest>(new BindingConfig {BodyOnly = true});

            var validationResult = this.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationErrorException(validationResult.FormattedErrors);
            }

            await _callOffOrdersService.UpdateRateAsync(args.callOffOrderId, args.rateId, request);

            return Negotiate.WithStatusCode(HttpStatusCode.OK);
        }

        private async Task<string> DeleteCallOffOrderHandlerAsync(dynamic args, CancellationToken ct)
        {
            this.RequiresRoles(new[] { Role.Customer });

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
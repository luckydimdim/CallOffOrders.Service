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

namespace Cmas.Services.CallOffOrders
{
    public class CallOffOrdersModule : NancyModule
    {
        private readonly CallOffOrdersService _callOffOrdersService;

        public CallOffOrdersModule(IServiceProvider serviceProvider) : base("/call-off-orders")
        {
            this.RequiresAuthentication();

            _callOffOrdersService = new CallOffOrdersService(serviceProvider);

            /// <summary>
            /// /call-off-orders - получить наряд заказы
            /// </summary>
            Get<IEnumerable<CallOffOrder>>("/", GetCallOffOrdersAsync,
                (ctx) => !ctx.Request.Query.ContainsKey("contractId"));

            /// <summary>
            /// /call-off-orders?contractId={id} - получить наряд заказы по указанному договору
            /// </summary>
            Get<IEnumerable<CallOffOrder>>("/",
                GetCallOffOrdersByContractIdAsync,
                (ctx) => ctx.Request.Query.ContainsKey("contractId"));

            /// <summary>
            /// /call-off-orders/{id} - получить наряд заказ по указанному ID
            /// </summary>
            Get<CallOffOrder>("/{id}", GetCallOffOrderHandlerAsync);

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

        private async Task<CallOffOrder> GetCallOffOrderHandlerAsync(dynamic args, CancellationToken ct)
        {
            return await _callOffOrdersService.GetCallOffOrderAsync((string) args.id);
        }

        private async Task<string> CreateCallOffOrderHandlerAsync(dynamic args, CancellationToken ct)
        {
            CallOffOrder request = this.Bind();

            var validationResult = this.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationErrorException(validationResult.FormattedErrors);
            }

            return await _callOffOrdersService.CreateCallOffOrderAsync(request);
        }

        private async Task<string> UpdateCallOffOrderHandlerAsync(dynamic args, CancellationToken ct)
        {
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
            await _callOffOrdersService.DeleteRateAsync(args.callOffOrderId, args.rateId);

            return Negotiate.WithStatusCode(HttpStatusCode.OK);
        }

        private async Task<Negotiator> UpdateRateHandlerAsync(dynamic args, CancellationToken ct)
        {
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
            return await _callOffOrdersService.DeleteCallOffOrderAsync(args.id);
        }

        private async Task<IEnumerable<CallOffOrder>> GetCallOffOrdersAsync(dynamic args, CancellationToken ct)
        {
            return await _callOffOrdersService.GetCallOffOrdersAsync();
        }

        private async Task<IEnumerable<CallOffOrder>> GetCallOffOrdersByContractIdAsync(dynamic args,
            CancellationToken ct)
        {
            return await _callOffOrdersService.GetCallOffOrdersByContractIdAsync(Request.Query["contractId"]);
        }

        #endregion
    }
}
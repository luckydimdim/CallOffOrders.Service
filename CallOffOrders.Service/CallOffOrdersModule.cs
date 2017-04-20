using System;
using AutoMapper;
using Cmas.BusinessLayers.CallOffOrders;
using Cmas.BusinessLayers.CallOffOrders.Entities;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.Infrastructure.Domain.Queries;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Validation;
using Cmas.Infrastructure.ErrorHandler;

namespace Cmas.Services.CallOffOrders
{
    public class CallOffOrdersModule : NancyModule
    {
        private readonly ICommandBuilder _commandBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly CallOffOrdersBusinessLayer _callOffOrdersBusinessLayer;
        private readonly IMapper _autoMapper;

        public CallOffOrdersModule(ICommandBuilder commandBuilder, IQueryBuilder queryBuilder,
            IServiceProvider serviceProvider) : base("/call-off-orders")
        {
            _commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;

            _callOffOrdersBusinessLayer = new CallOffOrdersBusinessLayer(_commandBuilder, _queryBuilder);
            _autoMapper = (IMapper) serviceProvider.GetService(typeof(IMapper));

            Get("/", async (args, ct) =>
            {
                string contractId = Request.Query["contractId"];

                if (contractId == null)
                {
                    return await _callOffOrdersBusinessLayer.GetCallOffOrders();
                }
                else
                {
                    return await _callOffOrdersBusinessLayer.GetCallOffOrders(contractId);
                }
            });


            Get("/{id}", async args => await _callOffOrdersBusinessLayer.GetCallOffOrder(args.id));


            Post("/", async (args, ct) =>
            {
                CallOffOrder form = this.Bind();
                string result = await _callOffOrdersBusinessLayer.CreateCallOffOrder(form);

                return result.ToString();
            });

            Put("/", async (args, ct) =>
            {
                CallOffOrder request = this.Bind();

                var validationResult = this.Validate(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationErrorException(validationResult.FormattedErrors);
                }

                string result = await _callOffOrdersBusinessLayer.UpdateCallOffOrder(request.Id, request);

                return result.ToString();
            });

            /// <summary>
            /// Создать ставку в наряд заказе
            /// </summary>
            Post<Rate>("/{id}/rates", async (args, ct) =>
            {
                CreateRateRequest request = this.Bind();

                Rate rateForCreate = _autoMapper.Map<Rate>(request);

                return await _callOffOrdersBusinessLayer.AddRate(args.id, rateForCreate);
            });

            /// <summary>
            /// Удалить ставку в наряд заказе
            /// </summary>
            Delete<Negotiator>("/{callOffOrderId}/rates/{rateId}", async (args, ct) =>
            {
                await _callOffOrdersBusinessLayer.DeleteRate(args.callOffOrderId, args.rateId);

                return Negotiate.WithStatusCode(HttpStatusCode.OK);
            });

            /// <summary>
            /// Обновить ставку в наряд заказе
            /// </summary>
            Put<Negotiator>("/{callOffOrderId}/rates/{rateId}", async (args, ct) =>
            {
                UpdateRateRequest request = this.Bind<UpdateRateRequest>(new BindingConfig {BodyOnly = true});

                var validationResult = this.Validate(request);

                if (!validationResult.IsValid)
                {
                    throw new ValidationErrorException(validationResult.FormattedErrors);
                }

                Rate rateForUpdate = _autoMapper.Map<Rate>(request);
                rateForUpdate.Id = args.rateId;

                await _callOffOrdersBusinessLayer.UpdateRate(args.callOffOrderId, rateForUpdate);

                return Negotiate.WithStatusCode(HttpStatusCode.OK);
            });

            Delete("/{id}", async args => { return await _callOffOrdersBusinessLayer.DeleteCallOffOrder(args.id); });
        }
    }
}
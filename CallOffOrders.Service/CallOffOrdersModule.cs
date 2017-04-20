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
                CallOffOrder form = this.Bind();

                string result = await _callOffOrdersBusinessLayer.UpdateCallOffOrder(form.Id, form);

                return result.ToString();
            });

            /// <summary>
            /// Создать ставку в наряд заказе
            /// </summary>
            Post<Rate>("/{id}/rate", async (args, ct) =>
            {
                CreateRateRequest request = this.Bind();

                Rate newRate = _autoMapper.Map<Rate>(request);

                return await _callOffOrdersBusinessLayer.AddRate(args.id, newRate);
            });

            /// <summary>
            /// Создать ставку в наряд заказе
            /// </summary>
            Delete<Negotiator>("/{callOffOrderId}/rate/{rateId}", async (args, ct) =>
            {
                var result = await _callOffOrdersBusinessLayer.DeleteRate(args.callOffOrderId, args.rateId);

                return Negotiate.WithStatusCode(HttpStatusCode.OK);
            });

            Delete("/{id}", async args => { return await _callOffOrdersBusinessLayer.DeleteCallOffOrder(args.id); });
        }
    }
}
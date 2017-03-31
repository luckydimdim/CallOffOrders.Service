using Cmas.BusinessLayers.CallOffOrders;
using Cmas.BusinessLayers.CallOffOrders.Entities;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.Infrastructure.Domain.Queries;
using Nancy;
using Nancy.ModelBinding;

namespace Cmas.Services.CallOffOrders
{

     
    public class CallOffOrdersModule : NancyModule
    {
        private readonly ICommandBuilder _commandBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly CallOffOrdersBusinessLayer _callOffOrdersBusinessLayer;

        public CallOffOrdersModule(ICommandBuilder commandBuilder, IQueryBuilder queryBuilder) : base("/call-off-orders")
        {

           _commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;

            _callOffOrdersBusinessLayer = new CallOffOrdersBusinessLayer(_commandBuilder, _queryBuilder);

            Get("/", async (args, ct)  =>
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

            Delete("/{id}", async args =>
            {
                return await _callOffOrdersBusinessLayer.DeleteCallOffOrder(args.id);
            });
        }

         
    }
}

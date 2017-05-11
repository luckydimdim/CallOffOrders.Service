using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cmas.BusinessLayers.CallOffOrders;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.Infrastructure.Domain.Queries;
using Cmas.BusinessLayers.CallOffOrders.Entities;
using System.Threading.Tasks;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using Nancy;

namespace Cmas.Services.CallOffOrders
{
    public class CallOffOrdersService
    {
        private readonly CallOffOrdersBusinessLayer _callOffOrdersBusinessLayer;
        private readonly IMapper _autoMapper;

        public CallOffOrdersService(IServiceProvider serviceProvider, NancyContext ctx)
        {
               _autoMapper = (IMapper) serviceProvider.GetService(typeof(IMapper));

            _callOffOrdersBusinessLayer = new CallOffOrdersBusinessLayer(serviceProvider, ctx.CurrentUser);
        }

        public async Task<CallOffOrder> GetCallOffOrderAsync(string callOffOrderId)
        {
            return await _callOffOrdersBusinessLayer.GetCallOffOrder(callOffOrderId);
        }

        public async Task<string> CreateCallOffOrderAsync(CallOffOrder request)
        {
            return await _callOffOrdersBusinessLayer.CreateCallOffOrder(request);
        }

        public async Task<string> UpdateCallOffOrderAsync(string callOffOrderId, UpdateCallOffOrderRequest request)
        {
            CallOffOrder orderForUpdate = await _callOffOrdersBusinessLayer.GetCallOffOrder(callOffOrderId);

            orderForUpdate = _autoMapper.Map<UpdateCallOffOrderRequest, CallOffOrder>(request, orderForUpdate);

            string result = await _callOffOrdersBusinessLayer.UpdateCallOffOrder(callOffOrderId, orderForUpdate);

            return result;
        }

        public async Task<Rate> AddRateAsync(string callOffOrderId, CreateRateRequest request)
        {
            Rate rateForCreate = _autoMapper.Map<Rate>(request);

            return await _callOffOrdersBusinessLayer.AddRate(callOffOrderId, rateForCreate);
        }

        public async Task DeleteRateAsync(string callOffOrderId, string rateId)
        {
            await _callOffOrdersBusinessLayer.DeleteRate(callOffOrderId, rateId);
        }

        public async Task UpdateRateAsync(string callOffOrderId, string rateId, UpdateRateRequest request)
        {
            Rate rateForUpdate = _autoMapper.Map<Rate>(request);
            rateForUpdate.Id = rateId;

            await _callOffOrdersBusinessLayer.UpdateRate(callOffOrderId, rateForUpdate);
        }

        public async Task<string> DeleteCallOffOrderAsync(string callOffOrderId)
        {
            return await _callOffOrdersBusinessLayer.DeleteCallOffOrder(callOffOrderId);
        }

        public async Task<IEnumerable<CallOffOrder>> GetCallOffOrdersAsync()
        {
            return await _callOffOrdersBusinessLayer.GetCallOffOrders();
        }

        public async Task<IEnumerable<CallOffOrder>> GetCallOffOrdersByContractIdAsync(string contractId)
        {
            return await _callOffOrdersBusinessLayer.GetCallOffOrders(contractId);
        }
    }
}
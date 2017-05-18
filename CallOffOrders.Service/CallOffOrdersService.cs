using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cmas.BusinessLayers.CallOffOrders;
using Cmas.BusinessLayers.CallOffOrders.Entities;
using System.Threading.Tasks;
using Cmas.Services.CallOffOrders.Dtos.Requests;
using Nancy;
using Cmas.Services.CallOffOrders.Dtos.Responses;
using Cmas.BusinessLayers.Contracts;
using Cmas.Infrastructure.ErrorHandler;

namespace Cmas.Services.CallOffOrders
{
    public class CallOffOrdersService
    {
        private readonly CallOffOrdersBusinessLayer _callOffOrdersBusinessLayer;
        private readonly ContractsBusinessLayer _contractsBusinessLayer;
        private readonly IMapper _autoMapper;

        public CallOffOrdersService(IServiceProvider serviceProvider, NancyContext ctx)
        {
               _autoMapper = (IMapper) serviceProvider.GetService(typeof(IMapper));

            _callOffOrdersBusinessLayer = new CallOffOrdersBusinessLayer(serviceProvider, ctx.CurrentUser);
            _contractsBusinessLayer = new ContractsBusinessLayer(serviceProvider, ctx.CurrentUser);
        }

        public async Task<DetailedCallOffOrderResponse> GetCallOffOrderAsync(string callOffOrderId)
        {
            CallOffOrder callOffOrder = await _callOffOrdersBusinessLayer.GetCallOffOrder(callOffOrderId);

            var result = new DetailedCallOffOrderResponse();

            // FIXME: плохо по производительности. Необходимо реализовать функцию _contractsBusinessLayer.GetCurrencies(callOffOrder.ContractId)
            var contract = await _contractsBusinessLayer.GetContract(callOffOrder.ContractId);

            if (contract == null)
            {
                throw new Exception($"contract with id {callOffOrder.ContractId} not found");
            } 
            
            result.Currencies = contract.Amounts.Select(a => a.CurrencySysName).Distinct().ToList();

            return _autoMapper.Map<CallOffOrder,DetailedCallOffOrderResponse>(callOffOrder, result);
        }

        public async Task<string> CreateCallOffOrderAsync(CreateCallOffOrderRequest request)
        { 
            var contract = await _contractsBusinessLayer.GetContract(request.ContractId);

            if (contract == null)
            {
                throw new Exception($"contract with id {request.ContractId} not found");
            }

            var currencies = contract.Amounts.Select(a => a.CurrencySysName).Distinct().ToList();

            return await _callOffOrdersBusinessLayer.CreateCallOffOrder(request.ContractId, contract.TemplateSysName, currencies.FirstOrDefault());
        }

        public async Task<string> UpdateCallOffOrderAsync(string callOffOrderId, UpdateCallOffOrderRequest request)
        {
            CallOffOrder orderForUpdate = await _callOffOrdersBusinessLayer.GetCallOffOrder(callOffOrderId);

            if (orderForUpdate == null)
            {
                throw new NotFoundErrorException($"Call Off Order with id {callOffOrderId} not found");
            }

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

        public async Task<IEnumerable<SimpleCallOffOrderResponse>> GetCallOffOrdersAsync()
        {
            var result = new List<SimpleCallOffOrderResponse>();

            var callOffOrders = await _callOffOrdersBusinessLayer.GetCallOffOrders();

            foreach (var callOffOrder in callOffOrders)
            {
                result.Add(_autoMapper.Map<SimpleCallOffOrderResponse>(callOffOrder));
            }

            return result;
        }

        public async Task<IEnumerable<SimpleCallOffOrderResponse>> GetCallOffOrdersByContractIdAsync(string contractId)
        {
            var result = new List<SimpleCallOffOrderResponse>();

            var callOffOrders = await _callOffOrdersBusinessLayer.GetCallOffOrders(contractId);

            foreach (var callOffOrder in callOffOrders)
            {
                result.Add(_autoMapper.Map<SimpleCallOffOrderResponse>(callOffOrder));
            }

            return result;
        }
    }
}
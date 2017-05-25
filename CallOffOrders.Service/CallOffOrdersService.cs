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
using Cmas.BusinessLayers.Contracts.Entities;

namespace Cmas.Services.CallOffOrders
{
    /// <summary>
    /// Сервис наряд заказов
    /// </summary>
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

        /// <summary>
        /// Получить детализированный наряд заказ
        /// </summary>
        /// <param name="callOffOrderId">ID наряд заказа</param>
        public async Task<DetailedCallOffOrderResponse> GetCallOffOrderAsync(string callOffOrderId)
        {
            CallOffOrder callOffOrder = await _callOffOrdersBusinessLayer.GetCallOffOrder(callOffOrderId);

            if (callOffOrder == null)
                throw new NotFoundErrorException();
             
            // FIXME: плохо по производительности. Необходимо реализовать функцию _contractsBusinessLayer.GetCurrencies(callOffOrder.ContractId)
            Contract contract = await _contractsBusinessLayer.GetContract(callOffOrder.ContractId);

            if (contract == null)
            {
                throw new Exception($"contract with id {callOffOrder.ContractId} not found");
            }

            var result = new DetailedCallOffOrderResponse();
            result.Currencies = contract.Currencies;
            result.MinDate = contract.StartDate;
            result.MaxDate = contract.FinishDate;

            return _autoMapper.Map<CallOffOrder,DetailedCallOffOrderResponse>(callOffOrder, result);
        }

        /// <summary>
        /// Создать наряд заказ
        /// </summary>
        /// <returns>ID созданного наряд заказа</returns>
        public async Task<string> CreateCallOffOrderAsync(CreateCallOffOrderRequest request)
        { 
            var contract = await _contractsBusinessLayer.GetContract(request.ContractId);

            if (contract == null)
            {
                throw new Exception($"contract with id {request.ContractId} not found");
            }

            string defaultCurrency = contract.Currencies.FirstOrDefault();

            return await _callOffOrdersBusinessLayer.CreateCallOffOrder(request.ContractId, contract.TemplateSysName, defaultCurrency);
        }

        /// <summary>
        /// Обновить наряд заказ
        /// </summary>
        /// <param name="callOffOrderId">ID наряд заказа</param>
        /// <param name="request">Данные для обновления</param>
        /// <returns>ID наряд заказа</returns>
        public async Task<string> UpdateCallOffOrderAsync(string callOffOrderId, UpdateCallOffOrderRequest request)
        {
            // FIXME: Продумать что делать если по наряд заказу есть табели

            CallOffOrder orderForUpdate = await _callOffOrdersBusinessLayer.GetCallOffOrder(callOffOrderId);

            if (orderForUpdate == null)
            {
                throw new NotFoundErrorException($"Call Off Order with id {callOffOrderId} not found");
            }

            orderForUpdate = _autoMapper.Map<UpdateCallOffOrderRequest, CallOffOrder>(request, orderForUpdate);

            string result = await _callOffOrdersBusinessLayer.UpdateCallOffOrder(callOffOrderId, orderForUpdate);

            return result;
        }

        /// <summary>
        /// Добавить ставку/группу
        /// </summary>
        public async Task<Rate> AddRateAsync(string callOffOrderId, CreateRateRequest request)
        {
            // FIXME: Продумать что делать если по наряд заказу есть табели

            Rate rateForCreate = _autoMapper.Map<Rate>(request);

            return await _callOffOrdersBusinessLayer.AddRate(callOffOrderId, rateForCreate);
        }

        /// <summary>
        /// Удалить ставку/группу
        /// </summary>
        public async Task DeleteRateAsync(string callOffOrderId, string rateId)
        {
            // FIXME: Продумать что делать если по наряд заказу есть табели

            await _callOffOrdersBusinessLayer.DeleteRate(callOffOrderId, rateId);
        }

        /// <summary>
        /// Обновить ставку/группу
        /// </summary>
        public async Task UpdateRateAsync(string callOffOrderId, string rateId, UpdateRateRequest request)
        {
            // FIXME: Продумать что делать если по наряд заказу есть табели

            Rate rateForUpdate = _autoMapper.Map<Rate>(request);
            rateForUpdate.Id = rateId;

            await _callOffOrdersBusinessLayer.UpdateRate(callOffOrderId, rateForUpdate);
        }

        /// <summary>
        /// Удалить наряд заказ
        /// </summary>
        /// <param name="callOffOrderId"></param>
        /// <returns></returns>
        public async Task<string> DeleteCallOffOrderAsync(string callOffOrderId)
        {
            // FIXME: Продумать что делать если по наряд заказу есть табели
            return await _callOffOrdersBusinessLayer.DeleteCallOffOrder(callOffOrderId);
        }

        /// <summary>
        /// Получить все наряд заказы
        /// </summary>
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

        /// <summary>
        /// Получить наряд заказы по договору
        /// </summary>
        /// <param name="contractId">ID договора</param>
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
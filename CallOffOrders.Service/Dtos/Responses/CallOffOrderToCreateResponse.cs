using System;
using System.Collections.Generic;

namespace Cmas.Services.CallOffOrders.Dtos.Responses
{
    public class CallOffOrderToCreateResponse
    {
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public string ContractId;
         
        /// <summary>
        /// 
        /// </summary>
        public DateTime MinDate;

        /// <summary>
        /// 
        /// </summary>
        public DateTime MaxDate;

        /// <summary>
        /// Доступные валюты
        /// </summary>
        public IEnumerable<string> Currencies;

        public string CurrencySysName;

    }
}

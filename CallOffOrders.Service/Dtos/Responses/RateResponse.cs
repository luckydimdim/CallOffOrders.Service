using System;
using System.Collections.Generic;
using System.Text;

namespace Cmas.Services.CallOffOrders.Dtos.Responses
{
    public class RateResponse
    {
        public string Id;

        public String Name;

        public bool IsRate;

        public string ParentId;

        /// <summary>
        /// Ставка
        /// </summary>
        public double Amount;

        /// <summary>
        /// Валюта
        /// </summary>
        public String Currency;

        /// <summary>
        /// Ед. изм.
        /// </summary>
        public int Unit;
    }
}

﻿using System; 
namespace Cmas.Services.CallOffOrders.Dtos.Requests
{
    /// <summary>
    /// Модель создания новой ставки
    /// </summary>
    public class UpdateRateRequest
    {
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
        public String UnitName;
    }
}
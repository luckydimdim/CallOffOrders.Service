using System;

namespace Cmas.Services.CallOffOrders.Dtos.Requests
{
    /// <summary>
    /// Ставка / группа ставок
    /// </summary>
    public class RateRequest
    {
        /// <summary>
        /// ID ставки/группы
        /// </summary>
        public string Id;

        /// <summary>
        /// Наименование ставки/группы
        /// </summary>
        public String Name;

        /// <summary>
        /// true, если ставка. Иначе - группа
        /// </summary>
        public bool IsRate;

        /// <summary>
        /// ID родительской группы
        /// </summary>
        public string ParentId;

        /// <summary>
        /// Ставка
        /// </summary>
        public double Amount;

        /// <summary>
        /// Ед. изм.
        /// </summary>
        public int Unit;
    }
}

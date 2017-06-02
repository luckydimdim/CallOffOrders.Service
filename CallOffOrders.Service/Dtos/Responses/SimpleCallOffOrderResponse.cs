using System;

namespace Cmas.Services.CallOffOrders.Dtos.Responses
{
    public class SimpleCallOffOrderResponse
    {
        /// <summary>
        /// Уникальный внутренний идентификатор
        /// </summary>
        public string Id;
         
        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public string ContractId;

        /// <summary>
        /// Номер наряд заказа
        /// </summary>
        public string Number;

        /// <summary>
        /// ФИО
        /// </summary>
        public string Assignee;

        /// <summary>
        /// Дата начала действия наряд-заказа
        /// </summary>
        public DateTime StartDate;

        /// <summary>
        /// Дата окончания действия наряд-заказа
        /// </summary>
        public DateTime FinishDate;

        /// <summary>
        /// Наименование заказа (по сути - работы)
        /// </summary>
        public string Name;

        /// <summary>
        /// Системное имя шаблона НЗ
        /// </summary>
        public string TemplateSysName;  // 'Default', 'Annotech'
        
        /// <summary>
        /// Системное имя валюты
        /// </summary>
        public string CurrencySysName;
         
    }
}

using System;
using System.Collections.Generic;

namespace Cmas.Services.CallOffOrders.Dtos.Requests
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCallOffOrderRequest
    {
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
        /// Должность
        /// </summary>
        public string Position;

        /// <summary>
        /// Место работы
        /// </summary>
        public string Location;

        /// <summary>
        /// Табельный номер
        /// </summary>
        public string EmployeeNumber;

        /// <summary>
        /// Номер позиции
        /// </summary>
        public string PositionNumber;

        /// <summary>
        /// Происхождение персонала
        /// </summary>
        public string PersonnelSource;

        /// <summary>
        /// Номер PAAF
        /// </summary>
        public string Paaf;

        /// <summary>
        /// Ссылка плана мобилизации
        /// </summary>
        public string MobPlanReference;

        /// <summary>
        /// Дата мобилизации
        /// </summary>
        public DateTime? MobDate;

        /// <summary>
        /// Валюта
        /// </summary>
        public string CurrencySysName;

        /// <summary>
        /// Ставки
        /// </summary>
        public IList<RateRequest> Rates;
    }
}

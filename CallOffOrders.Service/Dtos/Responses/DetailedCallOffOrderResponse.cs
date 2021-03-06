﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cmas.Services.CallOffOrders.Dtos.Responses
{
    public class DetailedCallOffOrderResponse
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
        /// 
        /// </summary>
        public DateTime MinDate;

        /// <summary>
        /// 
        /// </summary>
        public DateTime MaxDate;

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
        /// Системное имя шаблона НЗ
        /// </summary>
        public string TemplateSysName;  // 'Default', 'Annotech'

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
        /// Ставки
        /// </summary>
        public List<RateResponse> Rates;

        /// <summary>
        /// Доступные валюты
        /// </summary>
        public IEnumerable<string> Currencies;

        public string CurrencySysName;

        /// <summary>
        /// по наряд заказу есть табели
        /// </summary>
        public bool HasTimeSheets;

    }
}

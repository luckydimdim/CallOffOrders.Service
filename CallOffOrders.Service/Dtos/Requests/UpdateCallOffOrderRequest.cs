namespace Cmas.Services.CallOffOrders.Dtos.Requests
{
    public class UpdateCallOffOrderRequest
    { 
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
        public string StartDate;

        /// <summary>
        /// Дата окончания действия наряд-заказа
        /// </summary>
        public string FinishDate;

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
        public string MobDate;
        
    }
}

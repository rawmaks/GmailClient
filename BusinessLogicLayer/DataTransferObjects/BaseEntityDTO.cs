using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DataTransferObjects
{
    public class BaseEntityDTO
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PresentationLayer.Model.Entities
{
    public class BaseEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        /// 
        public int ID { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        /// 
        public DateTime CreateDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskMoloko.Models
{
    /// <summary>
    /// Класс для  сохранения критических ошибок от апи
    /// </summary>
    public class CriticalError
    {
        #region Properties

        [Column("id")] public long Id { get; set; }

        [Column("error")] public string Error { get; set; }

        #endregion

        #region Constructors

        public CriticalError()
        {

        }

        public CriticalError(string error)
        {
            this.Error = error;
        }

        #endregion
    }
}
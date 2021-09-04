using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tarea.Function.Entities
{
    public class ConsolidatedEntity : TableEntity
    { 
        public int IdEmployed { get; set; }
        public DateTime WorkDate { get; set; }
        public int WorkMinutes { get; set; }
    }
}

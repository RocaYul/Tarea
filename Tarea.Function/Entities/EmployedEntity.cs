using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Tarea.Function.Entities
{
    public class EmployedEntity : TableEntity
    {
        public EmployedEntity() { }
        public int IdEmployed { get; set; }
        public DateTime InputOutput { get; set; }
        public int Type { get; set; }
        public bool Consolidated { get; set; }
    }
}

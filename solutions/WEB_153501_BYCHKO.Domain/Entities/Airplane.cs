using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_153501_BYCHKO.Domain.Entities
{
    public class Airplane
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public EngineTypeCategory? Category { get; set; }

        public int Price { get; set; }

        public string? PhotoPath { get; set; }

        public string? MIMEType { get; set; }
    }
}
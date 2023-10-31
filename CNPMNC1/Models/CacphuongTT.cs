using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CNPMNC1.Models
{
    public class CacphuongTT
    {
        public string ID { get; set; }
        public string idtt {  get; set; }
        public string PhuongthucTT { get; set; }
        [NotMapped]
        public List<CacphuongTT> Listtt { get; set; }
    }
}
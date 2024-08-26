using System;
using System.Collections.Generic;
using System.Text;

namespace destapp.apiClient.Models
{
    public class BarcodeResponse
    {
        public List<Datum_bc> data { get; set; }
    }


    public class CreatedBy_bc
    {
        public int id { get; set; }
    }

    public class ModifiedBy_bc
    {
        public int id { get; set; }
    }

    public class Brand_bc
    {
        public int id { get; set; }
        public string status { get; set; }
        public object sort { get; set; }
        public int created_by { get; set; }
        public string created_on { get; set; }
        public int modified_by { get; set; }
        public string modified_on { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public object logo { get; set; }
    }

    public class ProductFormat_bc
    {
        public int id { get; set; }
        public string status { get; set; }
        public object sort { get; set; }
        public int created_by { get; set; }
        public string created_on { get; set; }
        public int modified_by { get; set; }
        public string modified_on { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Datum_bc
    {
        public int id { get; set; }
        public string status { get; set; }
        public object sort { get; set; }
        public CreatedBy_bc created_by { get; set; }
        public string created_on { get; set; }
        public ModifiedBy_bc modified_by { get; set; }
        public string modified_on { get; set; }
        public string name { get; set; }
        public List<string> type { get; set; }
        public int coins_to_add { get; set; }
        public object tickets_to_add { get; set; }
        public object image { get; set; }
        public Brand_bc brand { get; set; }
        public ProductFormat_bc product_format { get; set; }
        public string value { get; set; }
    }

}
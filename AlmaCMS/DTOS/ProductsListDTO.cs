using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmaCMS.DTOS
{
    public  class ProductsListDTO
    {
        public  List<ProductListItemDTO> data { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public int start { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class SliderRepository : GenericRepository<Slider>
    {
        public SliderRepository(DB_AlmaCmsEntities DB)
            : base(DB)
        {

        }
    }
}
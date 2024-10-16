using System.Collections.Generic;

namespace MyKP_работа_с_БД.DataBase
{
    public partial class Categories
    {
        public Categories()
        {
            Products = [];
        }

        public int ID_Category { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}

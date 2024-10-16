namespace MyKP_работа_с_БД.DataBase
{
    public partial class Products
    {
        public int ID_Product { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int ID_Category { get; set; }
        public double Price { get; set; }
        public virtual Categories Categories { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApp1.Model
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int ID_Item { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}

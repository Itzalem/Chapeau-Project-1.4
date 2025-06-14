﻿using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
    public class MenuViewModel
    {
        public ECardOptions CardFilter { get; set; }
        public ECategoryOptions CategoryFilter { get; set; }
        public Dictionary<ECardOptions, List<ECategoryOptions>> CategoriesDictionary { get; set; }

        public List<MenuItem> MenuItems { get; set; }      

    }
}

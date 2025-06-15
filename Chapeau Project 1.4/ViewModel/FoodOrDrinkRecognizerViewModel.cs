using Chapeau_Project_1._4.Models;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace Chapeau_Project_1._4.ViewModel
{
    public class FoodOrDrinkRecognizerViewModel
    {
        public bool IsDrink { get; set; }
        public string? FilterValue { get; set; }
    }
}

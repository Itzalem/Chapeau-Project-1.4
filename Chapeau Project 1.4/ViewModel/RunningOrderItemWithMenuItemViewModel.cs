using Chapeau_Project_1._4.Models;

namespace Chapeau_Project_1._4.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class RunningOrderItemWithMenuItemViewModel
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
        public EItemStatus ItemStatus { get; set; }


        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public ECategoryStatus MenuItemCategoryStatus { get; set; }
        public string OrderItemCategory { get; set; }
    }
}

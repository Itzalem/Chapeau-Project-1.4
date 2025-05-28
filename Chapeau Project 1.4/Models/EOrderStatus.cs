namespace Chapeau_Project_1._4.Models
{
    public enum EOrderStatus
    {
       
        onHold,
        
        pending,
        inProgress, 
        prepared,
        paid
            //paid an unpaid, other status will move to orderitem
    }
}

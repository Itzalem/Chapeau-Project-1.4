namespace Chapeau_Project_1._4.Models
{
    public enum EOrderStatus
    {
        draft,
        ordered,
        cancelled,
        pending,
        inProgress, 
        prepared,
        paid
            //paid an unpaid, other status will move to orderitem
    }
}

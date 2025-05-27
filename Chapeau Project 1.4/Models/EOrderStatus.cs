namespace Chapeau_Project_1._4.Models
{
    public enum EOrderStatus
    {
        draft,
        ordered,
        cancelled,
        pending,
        prepared,
        payed

            //paid an unpaid, other status will move to orderitem
    }
}

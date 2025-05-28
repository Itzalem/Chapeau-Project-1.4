namespace Chapeau_Project_1._4.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public Bill Bill { get; set; }
        public decimal Tip { get; set; }
        public decimal Total { get; set; }
        public int AlcoholDrinks { get; set; }
        public int NonAlcoholDrinks { get; set; }
        public EPaymentOptions PaymentType { get; set; }
        public string Feedback { get; set; }

        public Payment()
        {
             
        }

        public Payment(int paymentId, Bill bill, decimal tip, decimal total, int alcoholDrinks, int nonAlcoholDrinks, EPaymentOptions paymentType, string feedback)
        {
            PaymentId = paymentId;
            Bill = bill;
            Tip = tip;
            Total = total;
            AlcoholDrinks = alcoholDrinks;
            NonAlcoholDrinks = nonAlcoholDrinks;
            PaymentType = paymentType;
            Feedback = feedback;
        }
    }
}

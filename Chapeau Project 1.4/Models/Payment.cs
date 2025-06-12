namespace Chapeau_Project_1._4.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public Bill Bill { get; set; } = new Bill();
        public decimal Tip { get; set; }
        public decimal Total { get; set; }
        public EPaymentOptions PaymentType { get; set; }
        public string Feedback { get; set; }

        // calculate VAT
        public decimal VAT
        {
            get
            {
                decimal VAT = 0;
                foreach (OrderItem item in Bill.Order.OrderItems)
                {
                    if (item.MenuItem.IsAlcoholic)
                        VAT += (item.MenuItem.Price * (decimal)0.21);
                    else
                        VAT += (item.MenuItem.Price * (decimal)0.09);
                }
                return VAT;
            }
        }

        public Payment()
        {
             
        }

        public Payment(int paymentId, Bill bill, decimal tip, decimal total, EPaymentOptions paymentType, string feedback)
        {
            PaymentId = paymentId;
            Bill = bill;
            Tip = tip;
            Total = total;
            PaymentType = paymentType;
            Feedback = feedback;
        }

        public Payment(Bill bill, decimal total)
        {
            Bill = bill;
            Tip = 0;
            Total = total;
            Feedback = "";
        }
    }
}

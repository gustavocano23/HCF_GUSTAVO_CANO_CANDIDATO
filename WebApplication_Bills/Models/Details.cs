namespace WebApplication_Bills.Models
{
    public class Details
    {
        public int BillID { get; set; }
        public string BillDescription { get; set; }
        public decimal BillAmount { get; set; }
        public decimal BillUnitValue { get; set; }
        public decimal BillSubTotal { get; set; }
        public decimal BillPriceTotal { get; set; }
        public string BillCreatedBy { get; set; }
        public DateTime BillCreatedAt { get; set; }
        public int BillDetailID { get; set; }
        public string BillDetailProduct { get; set; }
        public string BillDetailProductDescription { get; set; }
        public decimal BillDetailAmount { get; set; }
        public decimal BillDetailUnitValue { get; set; }
        public decimal BillDetailSubTotal { get; set; }
    }
}

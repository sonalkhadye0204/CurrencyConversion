namespace CurrencyConversionAPI.Models
{
    public class ConversionRecord
    {

        public int Id { get; set; }
        public string FromCurrency { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

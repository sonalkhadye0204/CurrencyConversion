namespace CurrencyConversionAPI.DTO
{
    public class ConversionRequestDto
    {
        public string FromCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}

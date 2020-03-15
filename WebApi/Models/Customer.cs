namespace WebApi.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
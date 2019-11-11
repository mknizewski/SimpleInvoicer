using System.ComponentModel;

namespace SimpleInvoicer.Domain.Models
{
    public enum PaymentForm
    {
        [Description("Przelew bankowy")]
        Bank,

        [Description("Gotówka")]
        Cash
    }
}

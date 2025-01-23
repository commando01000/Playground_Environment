using Common.Layer;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<Response<object>> Pay()
        {
            // Set your secret key. Remember to switch to your live secret key in production.
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"]; // Replace with your Stripe secret key

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },

                LineItems = new List<SessionLineItemOptions>()
                {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = 2000, // $20.00
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "T-shirt",
                                    Description = "Comfortable and stylish t-shirt",
                                    Images = new List<string> { "https://example.com/t-shirt.png" },
                                },
                            },
                            Quantity = 2,
                        },
                },
                Mode = "payment",
                SuccessUrl = "https://www.youtube.com/watch?v=PVhbAcX4Ves&ab_channel=Code2Night",
                CancelUrl = "https://www.youtube.com/",
            };
            var service = new SessionService();
            var session = service.Create(options);

            // Redirect the customer to checkout.stripe.com to complete the payment
            return Task.FromResult(new Response<object> { Url = session.Url, Success = true, Message = "Success" });
        }
    }
}

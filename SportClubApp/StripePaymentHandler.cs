using Stripe;
using Stripe.Checkout;

namespace SportClubApp
{
    public static class StripePaymentHandler
    {
        // Inicializar Stripe con la clave secreta
        public static void InitializeStripe()
        {
            StripeConfiguration.ApiKey = Config.STRIPE_SECRET_KEY;
        }

        /// <summary>
        /// Crea una sesión de pago en Stripe para membresía
        /// </summary>
        public static Session CreatePaymentSession(int personaId, string nombre, string email)
        {
            try
            {
                InitializeStripe();

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = Config.MEMBERSHIP_AMOUNT_CENTS,
                                Currency = Config.CURRENCY,
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Membresía Club Deportivo",
                                    Description = $"Membresía anual para {nombre}"
                                }
                            },
                            Quantity = 1
                        }
                    },
                    Mode = "payment",
                    SuccessUrl = Config.SUCCESS_URL + $"?sessionId={{CHECKOUT_SESSION_ID}}&personaId={personaId}",
                    CancelUrl = Config.CANCEL_URL + $"?personaId={personaId}",
                    CustomerEmail = email
                };

                var service = new SessionService();
                Session session = service.Create(options);

                return session;
            }
            catch (StripeException ex)
            {
                throw new Exception($"Error al crear sesión de Stripe: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene los detalles de una sesión de pago
        /// </summary>
        public static Session GetSessionDetails(string sessionId)
        {
            try
            {
                InitializeStripe();

                var service = new SessionService();
                var options = new SessionGetOptions();
                options.AddExpand("payment_intent");

                Session session = service.Get(sessionId, options);
                return session;
            }
            catch (StripeException ex)
            {
                throw new Exception($"Error al obtener sesión: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica si el pago fue exitoso
        /// </summary>
        public static bool IsPaymentSuccessful(string sessionId)
        {
            try
            {
                Session session = GetSessionDetails(sessionId);
                return session.PaymentStatus == "paid";
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene el PaymentIntentId de una sesión
        /// </summary>
        public static string GetPaymentIntentId(string sessionId)
        {
            try
            {
                Session session = GetSessionDetails(sessionId);
                return session.PaymentIntentId;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Crea una sesión de pago en Stripe para No Socio (entrada diaria)
        /// </summary>
        public static Session CreateNoSocioPaymentSession(int noSocioId, string nombre, string email)
        {
            try
            {
                InitializeStripe();

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = Config.NOSOCIO_AMOUNT_CENTS,
                            Currency = Config.CURRENCY,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = Config.NOSOCIO_PRODUCT_NAME,
                                Description = $"Entrada diaria para {nombre}"
                            }
                        },
                        Quantity = 1
                    }
                },
                    Mode = "payment",
                    SuccessUrl = Config.NOSOCIO_SUCCESS_URL + $"?sessionId={{CHECKOUT_SESSION_ID}}&noSocioId={noSocioId}",
                    CancelUrl = Config.NOSOCIO_CANCEL_URL + $"?noSocioId={noSocioId}",
                    CustomerEmail = email
                };

                var service = new SessionService();
                Session session = service.Create(options);

                return session;
            }
            catch (StripeException ex)
            {
                throw new Exception($"Error al crear sesión de Stripe para No Socio: {ex.Message}");
            }
        }
    }
}


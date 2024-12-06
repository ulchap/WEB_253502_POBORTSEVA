using System.Text.Json.Serialization;
using WEB_253502_POBORTSEVA.Domain.Entities;
using WEB_253502_POBORTSEVA.UI.Extensions;

namespace WEB_253502_POBORTSEVA.UI.Services.CartService
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
            .HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddToCart(Product product)
        {
            base.AddToCart(product);
            Session?.Set("Cart", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set<SessionCart>("Cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }
}

using System.Text.Json.Serialization;
using WEB_153501_BYCHKO.Domain.Entities;
using WEB_153501_BYCHKO.Domain.Models;
using WEB_153501_BYCHKO.Extensions;

namespace WEB_153501_BYCHKO.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
            .HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.session = session;
            return cart;
        }

        public override void AddToCart(Airplane airplane)
        {
            base.AddToCart(airplane);
            session?.Set<Cart>("Cart", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            session?.Set<Cart>("Cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            session?.Set<Cart>("Cart", this);
        }

        [JsonIgnore]
        ISession? session;


    }
}

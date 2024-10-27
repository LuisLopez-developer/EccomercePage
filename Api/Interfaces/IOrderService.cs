namespace EccomercePage.Api.Interfaces
{
    public interface IOrderService
    {
        Task<bool> GenerateOrderThroughCart(int cartId, int paymentMethodId);
    }
}

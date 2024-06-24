using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MyGrpcService
{
    public class OrderServiceImpl : OrderService.OrderServiceBase
    {
        private readonly MyDbContext _context;

        public OrderServiceImpl(MyDbContext context)
        {
            _context = context;
        }

        public override async Task<OrderReply> GetOrder(OrderRequest request, ServerCallContext context)
        {
            var order = await _context.Orders
                                      .Include(o => o.Customer)
                                      .Include(o => o.Products)
                                      .FirstOrDefaultAsync(o => o.Id == request.Id);

            if (order == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Order not found"));
            }

            var reply = new OrderReply
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
            };

            foreach (var product in order.Products)
            {
                reply.Products.Add(new Product { Id = product.Id, Name = product.Name, Price = (float)product.Price });
            }

            return reply;
        }
    }
}

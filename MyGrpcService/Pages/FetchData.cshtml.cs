using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyGrpcService;
using System.Threading.Tasks;


namespace MyGrpcService.Pages
{
    public class FetchDataModel : PageModel
    {
        public OrderReply Order { get; set; }

        public async Task OnGetAsync()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
           // var client = new OrderService.OrderServiceClient(channel);
            var request = new OrderRequest { Id = 1 };
           // Order = await client.GetOrderAsync(request);
        }
    }
}
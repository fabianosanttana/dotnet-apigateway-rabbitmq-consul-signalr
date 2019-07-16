using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;
using ApiGateway.Models;
using System.Net.Http.Formatting;

namespace ApiGateway.Aggregators{
    public class CustomerWithProductAggregator : IDefinedAggregator{
        public Task<DownstreamResponse> Aggregate(List<DownstreamContext> responses)
        {
            var customerWithProduct = new CustomerWithProduct
            {
                Customer =  responses[0].DownstreamResponse.Content.ReadAsAsync<Customer>().Result,
                Product = responses[1].DownstreamResponse.Content.ReadAsAsync<Product>().Result
            };

            HttpResponseMessage response = new HttpResponseMessage();

            response.Content = new ObjectContent<CustomerWithProduct>(customerWithProduct, new JsonMediaTypeFormatter());

            return Task.FromResult(new DownstreamResponse(response));
        }
    }
}
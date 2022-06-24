using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderSaga.Contracts;
using OrderSaga.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderSaga.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRequestClient<CreateOrder> _createOrderRequestClient;
        private readonly IRequestClient<ChangeOrderStatus> _changeOrderStatusRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public OrderController(
            IRequestClient<CreateOrder> createOrderRequestClient,
            IRequestClient<ChangeOrderStatus> changeOrderStatusRequestClient,
            ISendEndpointProvider sendEndpointProvider)
        {
            _createOrderRequestClient = createOrderRequestClient;
            _changeOrderStatusRequestClient = changeOrderStatusRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            Guid orderId,
            int orderNumber,
            string customerName,
            string customerSurname,
            IList<OrderItemDto> items)
        {
            var (accepted, rejected) = await _createOrderRequestClient
                .GetResponse<OrderCreationAccepted, OrderCreationRejected>(new CreateOrder(
                    orderId, InVar.Timestamp, orderNumber, customerName, customerSurname, items));

            if(accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Ok(response.Message);
            }
            else
            {
                var response = await rejected;
                return BadRequest(response.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid orderId, OrderStatus status)
        {
            //var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:change-order-status"));

            //await endpoint.Send(new ChangeOrderStatus
            //{
            //    OrderId = orderId,
            //    Status = status
            //});

            //return Accepted();

            var response = await _changeOrderStatusRequestClient
                .GetResponse<OrderStatusChangingAccepted>(
                    new ChangeOrderStatus(orderId, status));

            return Ok(response.Message);
        }
    }
}

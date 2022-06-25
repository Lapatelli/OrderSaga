﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderSaga.Contracts;
using OrderSaga.Contracts.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderSaga.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IBusControl _bus;

        public OrderController(IBusControl bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            string customerName,
            string customerSurname,
            IList<OrderItemDto> items)
        {
            var message = CreateOrderCreatedMessage(customerName, customerSurname, items);
            await _bus.Publish(message);

            return Accepted(message);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int orderNumber, OrderStatus status)
        {
            var message = CreateOrderStatusChangedMessage(orderNumber, status);
            await _bus.Publish(message);

            return Accepted(message);
        }

        private static OrderCreated CreateOrderCreatedMessage(
            string customerName,
            string customerSurname,
            IList<OrderItemDto> items)
        {
            var orderId = InVar.CorrelationId;
            var orderDate = InVar.Timestamp;
            var orderNumber = (orderId.GetHashCode() & 0x7fffffff) % 100000000;

            return new OrderCreated(orderId, orderNumber, orderDate, customerName, customerSurname, items);
        }

        private static OrderStatusChanged CreateOrderStatusChangedMessage(int orderNumber, OrderStatus status)
        {
            var updatedDate = InVar.Timestamp;
            return new OrderStatusChanged(orderNumber, status, updatedDate);
        }
    }
}

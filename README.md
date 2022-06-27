# Order Saga

OrderSaga messaging solution with _RabbitMQ_, _MassTransit_, _NHibernate and \_SagaStateMachine_.

## Configuration and running steps:

1. Install _RabbitMQ_ locally.
2. Create `appsettings.json` files in _OrderSaga.DatabaseSchema_, _OrderSaga.Host_ and _OrderSaga.WebAPI_ projects based on samples which are located in the respective projects.
3. Apply necessary connection string to DB and configure RabbitMq connection with user credentials. Build all above mentioned projects for applying `appsettings.json` files.

_OrderSaga.DatabaseSchema_:

```cs
  "ConnectionStrings": {
    "OrderSagaDB": ""
  }
```

_OrderSaga.Host_:

```cs
  "ConnectionStrings": {
    "OrderSagaDB": ""
  },
  "RabbitMq": {
    "Host": "",
    "Credentials": {
      "Username": "",
      "Password": ""
    }
  }
```

4. Open cmd and follow the directory with the _OrderSaga.DatabaseSchema_ project. Run `dotnet run` command for applying migrations during which _Orders_ and _OrderItems_ tables are going to be created.

5. Open in new cmd windows _OrderSaga.Host_ and _OrderSaga.WebAPI_ projects. Run `dotnet run` command for starting OrderSaga messaging implementations.

In _OrderSaga.Host_ console the following information about OrderSagaStateMachine is logged:

- errors during consumption and receiving messages;
- info about completion of order creation event;
- info about status changing (is the status changed for order or not and the reason).

6. The following steps will provide the opportunity to check process flow of this App:
   - Send `POST` request from `OrderController` with filling all the necessary data (_CustomerName_, _CustomerSurname_, _OrderItems_). Generated `orderId` and `orderNumber` will be in response to this request. Please, apply the order number for checking the order info (`GET` request) and for updating the status of the order (`PUT` request) during next steps.
   - Send `GET` request from `OrderController` for receiving data about recently created order with correlated items.
   - Sned `PUT` request from `OrderControllewr` for updating order status. _Note: In case of successful status changing or impossibility of status changing, corresponding log could be found in debug console._

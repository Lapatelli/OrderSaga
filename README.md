# Order Saga

OrderSaga messaging solution with _RabbitMQ_, _MassTransit_, _NHibernate and \_SagaStateMachine_

## Configuration and running steps:

1. Create appsettings.json files in _OrderSaga.DatabaseSchema_, _OrderSaga.Host_ and _OrderSaga.WebAPI_ projects based on samples which are located in the respective projects.
2. Apply necessary connection string to DB and configure RabbitMq connection with user credentials.

_OrderSaga.DatabaseSchema_:

```cs
  "ConnectionStrings": {
    "OrderSagaDB": ""
  }
```

_cs OrderSaga.Host_:

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

3. Open cmd and follow the directory with the _OrderSaga.DatabaseSchema_ project. Run `dotnet run` command for applying migrations during which _Orders_ and _OrderItems_ tables are going to be created.

4. Open in new cmd windows _OrderSaga.Host_ and _OrderSaga.WebAPI_ projects. Run `dotnet run` command for starting OrderSaga messaging implementations.

In _OrderSaga.Host_ console the following information about OrderSagaStateMaching is logged:

- Errors during consumption and receiving messages;
- Info about completion of order creation event;
- Info about status changing (is the status changed for order or not and the reason).

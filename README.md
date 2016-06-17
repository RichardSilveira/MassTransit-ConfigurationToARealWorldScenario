
# MassTransit-ConfigurationToARealWorldScenario
A sample showing how to use and configure correctly MassTransit and Sagas state machine in a real world scenario


Scenario (Use case)
---
**Simulating online request for pizzas**

* The user sends an order.
* The order is received.
	- The order status could be either approved or rejected by the attendant.
	- If the order is approved, an estimated time should be informed.
	- If the order is rejected, an reason phrase should be informed.

* The order is done.

* Schedule messages to be sent to user when:
	- His order is either approved or rejected.
	- His order is near to be done.
	- His order is done (through order status update to _done_).

Frameworks & Libraries
---
* **RabbitMQ** - Very popular message broker that implements AMQP standart.
* **MassTransit** - Lightweight message bus for creating distributed applications using the .NET framework.
* **TopShelf** - Framework for hosting services written using the .NET framework.
* **Hangfire** - Provide an easy way to perform fire-and-forget, delayed and recurring tasks inside ASP.NET applications.
* **MongoDB** - Most popular nosql database nowadays.
* **NLog** - Advanced and high-performance logging solution for .NET.

ToDo
---
* Apply dependency injection and create unit tests.
* Configure Mongodb on Saga repository (waiting a moment for another 'Todo' on [MassTransit.MongoDb](https://github.com/LiberisLabs/MassTransit.MessageData.MongoDb)).

Suggestions
---
Please, criticize and give suggestions , this project was created for this, this is not about a library, it is just an implementation of how could be a real application using MassTransit as a message bus for RabbitMQ and exactly for it, whether any misconception is found, an issue will be very welcome to be opened.


===
_Special thanks to [Crhis Patterson](https://github.com/phatboyg) by your really fast support on stackoverflow over all my questions._

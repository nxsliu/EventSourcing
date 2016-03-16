# EventSourcing

This is a quick Eventsourcing POC consisting of some micro services, a process manager microservice, a projection service and WepAPIs. It requires RabbitMq and EventStore to run.

The concept is to open a bank account which happens in the following steps

1. User authenticates themselves using WebAPI
2. User submits application using WebAPI
3. Process manager picks up application and orchestrates it through the various checks and processes (micro services)
4. An event is added to the EventStore as the application progresses through these checks and processes
5. A projection service subscribes to the EventStore and projects the events into a relational DB.

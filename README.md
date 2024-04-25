![bITDevKit](src/bitDevKit_logo.png)
=====================================
Empowering developers with modular components for modern application development, centered around Domain-Driven Design principles.

Our goal is to empower developers by offering modular components that can be easily integrated into your projects. Whether you're working with repositories, commands, queries, or other components, the bITDevKit provides flexible solutions that can adapt to your specific needs.

This repository includes the complete source code for the bITDevKit, along with a variety of sample applications located in the ./examples folder within the solution. These samples serve as practical demonstrations of how to leverage the capabilities of the bITDevKit in real-world scenarios. All components are available as [nuget packages](https://www.nuget.org/packages?q=bitDevKit&packagetype=&prerel=true&sortby=relevance).

For the latest updates and release notes, please refer to the [RELEASES](./RELEASES.md).

Join us in advancing the world of software development with the bITDevKit!


Supported patterns, elements:
--------------------------------
- Entity
- AggregateRoot
- ValueObjects
  - TypedId
- DomainEvents
- BusinesRules, Check
- Repository
- Specifications
- Commands/Queries
- Outbox 
  - DomainEvents 
  - Messaging
- Decorator (Behavior)

Features (excerpt):
-------------------------------------
- EventStore (CQRS)
- Job Scheduling
- Storage
  - Documents
  - Files (TODO)
- Caching
- Messaging
- Queuing (TODO)
- Modules

Libraries used (excerpt):
-------------------------------------
- [Xunit](https://github.com/xunit/xunit)
- [MediatR](https://github.com/jbogard/MediatR)
- [EnsureThat.Core](https://github.com/danielwertheim/Ensure.That)
- [AutoMapper](https://github.com/AutoMapper/AutoMapper)
- [Mapster](https://github.com/MapsterMapper/Mapster)
- [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- [FluentAssertions](https://github.com/fluentassertions/fluentassertions)
- [Humanizer](https://github.com/Humanizr/Humanizer)
- [Polly](https://github.com/App-vNext/Polly)
- [Scrutor](https://github.com/khellang/Scrutor)
- [Serilog](https://github.com/serilog/serilog)
- [Quartz](https://github.com/quartz-scheduler/quartz)
- [Shouldly](https://github.com/shouldly/shouldly)
- [Testcontainer](https://github.com/testcontainers)
- EntityFramework Core
- Azure Storage
- Azure ServiceBus
- Azure CosmosDb
- RabbitMQ

Example projects
-----------------
- [EventStore](https://github.com/bridgingit/bitdevkit/examples)
- [WeatherForecast](https://github.com/bridgingit/bitdevkit/examples)
- [Shop](https://github.com/bridgingit/bitdevkit/examples)
- [DinnerFiesta](https://github.com/bridgingit/bitdevkit/examples)

Collaboration
---------
Simply create a pull request with your ideas or contact us.
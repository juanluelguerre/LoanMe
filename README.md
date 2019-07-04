## LoanMe Web
Is a SAMPLE ASP.NET Core Web App to show basic DevOps Steps, using Azure DevOps and Jenkins: 
 - [Azure DevOps](https://elguerre.com/2019/04/28/from-github-to-azure-app-service-through-azure-devops-pipelines)
 - [Jenkins](https://elguerre.com/2019/05/25/from-github-to-azure-app-service-through-jenkins-pipelines)

## LoanMe APIs
- Customers: Simple Data-Driven CRUD API for Customers.
- Loan. Sample API using CQRS + Event Sourcing and DDD.

**API Project**:
- Controller. Just a think controller/class to call the API logic. 
- Middlewares. ASP.NET MVC Middleware.
- Filters. ASP.NET MVC Filters.
- Application. API Logic

**Customers Application Folder**
- [Serilog](https://serilog.net/)
- [Swagger](https://docs.microsoft.com/es-es/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio)
- Entity Framework Core for Commands.

**LOAN Application Folder** by Domain (Customers and Accounts):
- [Serilog](https://serilog.net/)
- [Swagger](https://docs.microsoft.com/es-es/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio)
- Entity Framework Core for Commands.
- [Dapper](https://github.com/StackExchange/Dapper/tree/master/Dapper) for Queries
- [MediatR](https://github.com/jbogard/MediatR) as Mediator/Brocker
- [Marten](https://github.com/JasperFx/marten) to save events using PostgreSQL
- [Automapper](https://automapper.org/). Optional to map Model/Requests and Responses/ViewModels.
- [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql). MySQL using EF Core.


**Notes**:
- Services or Gateways class. Implementation from Domain interfaces to interact with Command and Query Handlers.
- Command, Query, Handlers and Event: CRQS and Event Sourcing Pattern.
- Models. Used by Controllers as Input requests/dtos.
- ViewModels. Used by Controlloers as output as responses/projections. Also know as QueryResults by QueryHandlers.



# Next steps
All those new Patterns, Tools and Platforms, will be Blog Post entries in [elGuerre.com](https://elguerre.com)
## Learning
1. Everything using Docker
2. Everything using Kubernetes
3. [RabitMQ](https://www.rabbitmq.com/) or
4. [Apache Kafka](https://kafka.apache.org/) as Message 
5. [Istio](https://istio.io/)Broker.
## Azure
6. Apply Azure Service BUS
7. Save Events to Azure  [Streamstone](https://github.com/yevhen/Streamstone)
8. [Terraform](https://www.terraform.io/)


# Runnng the App
1. Docker for MySQL:
`
.\deplo\mysql\start.cmd
`
2. Docker for PostgreSQL:
`
.\deploy\postgres\start.cmd
`
3. Run Entity Framework Migration: 
```
cd MyBudet\Api.Application.
dotnet ef migrations add Init --startup-project ..\MyBudget.Api\MyBudget.Api.csproj
dotnet ef database update --startup-project ..\MyBudget.Api\MyBudget.Api.csproj
``` 
# Bugs and Issues found during Development 
- [EF Core and MySQL](https://twitter.com/JuanluElGuerre/status/1140333496751284229)
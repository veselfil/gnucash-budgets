# GnuCashBudget.Adapter.Example

This project illustrates how to create an **adapter** and use `GnuCashBudget.Adapter.Abstractions`

Connector API documentation could be found in `CONNECTOR-API.md` docs

## How it works

Because various banks could have various APIs or not have API at all we need to be ready for that.

So the parsing of expenses from the banks will work this way:
1. Adapter.<bank> project is standalone and how it is implemented is up to the author of each project
2. All standalone projects have shared interface/abstraction and are exposing same
endpoints (more in `CONNECTOR-API.md` docs)
3. `GnuCashBudget.Api` project is communicating with all these project via REST api and getting all data it needs

### Example project limitations

This is only example project so many things aren't done correctly. Be aware of that if you copy this project.

For instance, you won't probably have data saved in `appsettings.Development.json` - you will either get them from 
bank's API or parse them from mail/somewhere. I am using `ExampleOptions` because it was fast and easy way to show you
how it works

## Docker-Compose

In this example project there is a `docker-compose.yml` file which has integration tests setup

That means we can use integration tests in the pipeline, see tests results and fail the build if tests are not correct

SpecFlow is generating TestResults by default (with NuGet package `SpecFlow.Plus.LivingDocPlugin` installed) and we as
a user just need to save the file (or make .trx file from it and give it to Azure for instance)

There are some plugins like `LivingDoc` which will make it nice-looking
// See https://aka.ms/new-console-template for more information
using DataAccess.Abstractions;
using Integrators.Core;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MoneyEntity.Logic;
using SqlLiteDataAccess;
using SqlLiteDataAccess.Model;

await TestDispatcher();
//await TestInit();
//await TestRead();
Console.WriteLine("in the end");
Console.ReadLine();

static async Task TestDispatcher()
{
    await new MoneyIntegrator()
        .RunDefaultIntegrator(new string[] { });
}




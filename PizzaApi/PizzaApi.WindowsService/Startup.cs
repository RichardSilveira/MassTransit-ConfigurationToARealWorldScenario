﻿using Hangfire;
using Hangfire.Mongo;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(PizzaApi.WindowsService.Startup))]
namespace PizzaApi.WindowsService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {            
            var storage = new MongoStorage("mongodb://localhost:27017", "hangfire-masstransit");

            //var storage = new SqlServerStorage(@"Data Source=.\SQLEXPRESS;Initial Catalog=hangfire-masstransit;Persist Security Info=True;User ID=sa;Password=123456");
            app.UseHangfireDashboard("/hangfire-masstransit", new DashboardOptions(), storage);
        }
    }
}
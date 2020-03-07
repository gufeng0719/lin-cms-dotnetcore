﻿using System;
using AutoMapper;
using FreeSql;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace LinCms.Test
{
    public abstract class BaseLinCmsTest : IDisposable
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IWebHostEnvironment HostingEnv;
        protected readonly IMapper Mapper;
        protected readonly IFreeSql FreeSql;
        protected readonly IUnitOfWork UnitOfWork;
        protected BaseLinCmsTest()
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseEnvironment("Development")
                .UseStartup<TestStartup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddConsole();
                }).UseNLog()
            ); ;
            ServiceProvider = server.Host.Services.CreateScope().ServiceProvider;

            HostingEnv = ServiceProvider.GetService<IWebHostEnvironment>();

            Mapper = ServiceProvider.GetService<IMapper>();
            FreeSql = ServiceProvider.GetService<IFreeSql>();
            UnitOfWork = ServiceProvider.GetService<IUnitOfWork>();

        }


        public void Dispose()
        {
            FreeSql?.Dispose();
            UnitOfWork?.Dispose();
        }

    }
}

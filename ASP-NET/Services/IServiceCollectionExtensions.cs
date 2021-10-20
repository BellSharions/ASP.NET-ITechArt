﻿using Business.Filters;
using Business.Interfaces;
using Business.Repositories;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ASP_NET.Services
{
    public static class IServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ActionFilters>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IRatingRepository, RatingRepository>();
        }
    }
}

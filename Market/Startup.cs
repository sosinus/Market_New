﻿using Market.App_Start;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace Market
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				});

			services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

			services.AddDbContext<MarketDBContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DbConnection")));


			services.RegisterUnitOfWork();
			services.RegisterServices();
			services.AddCors();
			services.RegisterIdentity(Configuration);
			services.AddLocalization(options => options.ResourcesPath = "Resources");

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseCors(builder =>
				builder
				.AllowAnyOrigin()
				//.WithOrigins(Configuration["ApplicationSettings:ClientURL"])
				.AllowAnyHeader()
				.AllowAnyMethod());
			app.UseAuthentication();
			app.UseStaticFiles();
			app.UseStaticFiles(new StaticFileOptions()
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
				RequestPath = new PathString("/Resources")
			});
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}

using Application;
using Application.Common.Interfaces;
using Infrastructure;
using Serilog;
using WebApi.Hubs;
using WebApi.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
	builder.Services.AddSignalR();

	// Add services to the container.
	builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.WebRootPath);


    

    builder.Services.AddScoped<IOrderHubService, OrderHubManager>();
    builder.Services.AddScoped<ICrawlerLogHubService, CrawlerLogHubManager>();
    builder.Services.AddScoped<INotificationHubService, NotificationHubManager>();

	// Configure CORS
	builder.Services.AddCors(options =>
	{
		options.AddPolicy("AllowAll",
			builder => builder
				.AllowAnyMethod()
				.AllowCredentials()
				.SetIsOriginAllowed((host) => true)
				.AllowAnyHeader());
	});


	var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    //app.UseCors(); // Enable CORS

	app.UseRouting();

	app.UseCors("AllowAll");

	app.MapControllers();

    app.MapHub<OrderHub>("/Hubs/OrderHub");
    app.MapHub<CrawlerLogHub>("/Hubs/CrawlerLogHub");
    app.MapHub<NotificationHub>("/Hubs/NotificationHub");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

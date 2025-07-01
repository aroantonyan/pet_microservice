    using PriceService.Services;

    var builder = WebApplication.CreateBuilder(args);



    builder.Services.AddGrpc();
    builder.Services.AddGrpcReflection();

    var app = builder.Build();

    app.MapGrpcService<PriceService.Services.gRPC.PriceGrpcService>();

    await StartupDbInitializer.CheckPriceTable(
        builder.Configuration.GetConnectionString("PriceDb")!,
        app.Lifetime.ApplicationStopping);


    app.Run();
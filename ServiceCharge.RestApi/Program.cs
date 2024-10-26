var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EfDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<BlockRepository, EFBlockRepository>();
builder.Services.AddScoped<BlockService, BlockAppService>();
builder.Services.AddScoped<BlockQuery, EFBlockQuery>();
builder.Services.AddScoped<FloorQuery, EFFloorQuery>();
builder.Services.AddScoped<FloorRepository, EFFloorRepository>();
builder.Services.AddScoped<FloorService, FloorService>();
builder.Services.AddScoped<UnitRepository, EFUnitRepository>();
builder.Services.AddScoped<UnitService, UnitService>();
builder.Services.AddScoped<UnitQuery, EFUnitQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
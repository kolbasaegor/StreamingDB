//using StreamingDB.Repositories;
//using StreamingDB.Classes;

var builder = WebApplication.CreateBuilder();

// Add services to the container.

//builder.Services.AddTransient<AlbumRepository>();
//builder.Services.AddSingleton<ArtistRepository>();
//builder.Services.AddSingleton<TrackRepository>();
//builder.Services.AddSingleton<ChartRepository>();
builder.Services.AddSingleton<StreamingDB.Services.Controller>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
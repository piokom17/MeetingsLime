using MeetingsLime.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

//TODO: Verify if I did it correctly, because I want to have this MeetingData to be loaded once per application lifetime
builder.Services.AddSingleton<IMeetingsService, MeetingsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

using MeetingsLime.Domain.Services;
using MeetingsLime.Infrastructure;
using MeetingsLime.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMeetingsService, MeetingsService>();
builder.Services.AddScoped<IMeetingSuggestionsValidator, MeetingSuggestionsValidator>();
builder.Services.AddScoped<IMeetingData, MeetingData>();

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
else
{
    app.UseHttpsRedirection();
}

//if (app.Environment.IsProduction())
//{
//    app.UseHttpsRedirection();
//}

//app.UseAuthorization();
app.MapControllers();

app.Run();

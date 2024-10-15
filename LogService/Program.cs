using LogService;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

void StartMessageBroker()
{
    TimeSpan[] retryIntervals = [TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(9), TimeSpan.FromSeconds(9), TimeSpan.FromSeconds(12), TimeSpan.FromSeconds(217)];

    AsyncRetryPolicy retryMessageBrokerPolicy = Policy.
    Handle<Exception>()
    .WaitAndRetryAsync(
    retryIntervals
    , (result, timeSpan, retryCount, context) =>
    {
        if (result != null) // If an exception occurred
        {
            Console.WriteLine($"Request failed with exception: {result.Message}. Retry count = {retryCount}. Waiting {timeSpan} before next retry.");
        }
    });
    retryMessageBrokerPolicy.ExecuteAsync(() =>  InitializeMessageBroker());
}

async Task InitializeMessageBroker()
{
    MessageBroker messageBroker = new MessageBroker();
}

//builder.Services.AddSingleton<MessageBroker>();
StartMessageBroker();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

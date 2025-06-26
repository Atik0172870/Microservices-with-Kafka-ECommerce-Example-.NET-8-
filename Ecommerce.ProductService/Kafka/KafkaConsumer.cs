
using Confluent.Kafka;
using Ecommerce.Model;
using Ecommerce.ProductService.Data;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Ecommerce.ProductService.Kafka
{
    public class KafkaConsumer(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
              {
                  _ = ConsumeAsync("order-topic", stoppingToken);
              }, stoppingToken);
        }
        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "product-service-group", // ← fixed group name
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                EnableAutoOffsetStore = true
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(stoppingToken);
                        if (consumeResult?.Message?.Value is null)
                            continue;

                        var order = JsonConvert.DeserializeObject<OrderModel>(consumeResult.Message.Value);

                        using var scope = serviceScopeFactory.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

                        var product = await dbContext.Products.FindAsync(order.ProductId);
                        if (product != null)
                        {
                            product.Quantity -= order.Quantity;
                            dbContext.Products.Update(product);
                            await dbContext.SaveChangesAsync();
                            Console.WriteLine($"Order processed: {order.Id}");
                        }
                        else
                        {
                            Console.WriteLine($"Product not found: {order.ProductId}");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.Error.WriteLine($"Invalid message format: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Unhandled error: {ex.Message}");
                    }

                    await Task.Delay(100, stoppingToken);
                }
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}

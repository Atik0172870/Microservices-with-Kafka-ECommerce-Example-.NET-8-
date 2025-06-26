var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.Ecommerce_OrderService>("apiservice-order");
builder.AddProject<Projects.Ecommerce_ProductService>("apiservice-product");

builder.Build().Run();

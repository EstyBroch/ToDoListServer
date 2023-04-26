using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt => opt.AddPolicy(MyAllowSpecificOrigins, policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ToDoDBContext>();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.MapGet("/", () => "to do list api is running");

app.MapGet("/items", (ToDoDBContext context) =>
{
    return context.Items.ToList();
});

app.MapPost("/items/{name}", async(ToDoDBContext context, string name)=>{
    var item=new Item(){Name=name};
    var newItem=context.Add(item);
    await context.SaveChangesAsync();
});

app.MapPut("/items/{id}",async(ToDoDBContext context,[FromBody] Item item,int id)=>{
var existsItem=await context.Items.FindAsync(id);
if(existsItem==null)
return Results.NotFound();

existsItem.Name=item.Name;
existsItem.IsComplete=item.IsComplete;

 await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async(ToDoDBContext context, int id)=>{
    var existItem = await context.Items.FindAsync(id);
    if(existItem is null) return Results.NotFound();

    context.Items.Remove(existItem);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();

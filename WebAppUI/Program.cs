using WebAppUI.Components;
using WebAppUI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://api:8080/");
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<StoreItemsService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<BaseService>();

builder.Services.AddRazorComponents();
builder.Services.AddRazorPages();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<App>();
app.MapRazorPages();


app.Run();

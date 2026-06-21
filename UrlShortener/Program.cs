using Microsoft.OpenApi;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "URL Shortener API", Version = "v1" });
    options.AddServer(new OpenApiServer { Url = "/" });
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "URL Shortener API v1");
    });
}

var urlShortener = new UrlShortenerService();

app.MapPost("/api/url", (ShortenRequest request, HttpContext context) =>
{
    if (request is null || string.IsNullOrWhiteSpace(request.Url))
    {
        return Results.BadRequest(new { error = "url is required." });
    }

    try
    {
        var shortCode = urlShortener.Shorten(request.Url.Trim());
        var shortUrl = $"{context.Request.Scheme}://{context.Request.Host}/{shortCode}";

        return Results.Ok(new ShortenResponse(shortCode, shortUrl, request.Url.Trim()));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.MapGet("/api/url/{shortCode}", (string shortCode) =>
{
    if (urlShortener.TryResolve(shortCode, out var longUrl))
    {
        return Results.Ok(new { shortCode, originalUrl = longUrl });
    }

    return Results.NotFound(new { error = "Short URL not found." });
});

app.MapGet("/{shortCode:regex(^[a-zA-Z0-9]+$)}", (string shortCode) =>
{
    if (urlShortener.TryResolve(shortCode, out var longUrl))
    {
        return Results.Redirect(longUrl, permanent: false);
    }

    return Results.NotFound(new { error = "Short URL not found." });
})
.ExcludeFromDescription();

app.Run();

record ShortenRequest(string Url);

record ShortenResponse(string ShortCode, string ShortUrl, string OriginalUrl);

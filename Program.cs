var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html>
<head>
  <title>Jenkins CI/CD Demo</title>
  <style>
    body { font-family: Arial, sans-serif; text-align: center; margin-top: 100px; background: #f4f6f8; }
    h1 { color: #2c3e50; }
    p { color: #555; font-size: 18px; }
    a { color: #0066cc; }
  </style>
</head>
<body>
  <h1>Welcome to my first .NET app</h1>
  <p>This project was created to practice Jenkins CI/CD.</p>
  <p><a href="/api/status">Check API status</a></p>
</body>
</html>
""", "text/html"));

app.MapGet("/api/status", () => new
{
    project = "HelloApp",
    purpose = "Practicing CI/CD with Jenkins",
    status = "running",
    time = DateTime.Now
});

app.Run();

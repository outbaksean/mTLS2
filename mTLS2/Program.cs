var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(listenOptions =>
    {
        var certPath = builder.Configuration["Kestrel:Certificates:Default:Path"];
        var certPassword = builder.Configuration["Kestrel:Certificates:Default:Password"];

        if (string.IsNullOrEmpty(certPath))
        {
            throw new InvalidOperationException("PFX certificate path is not configured in Kestrel:Certificates:Default:Path.");
        }

        if (string.IsNullOrEmpty(certPassword))
        {
            throw new InvalidOperationException("PFX certificate password is not configured. Set via User Secrets (Kestrel:Certificates:Default:Password) or Environment Variable (Kestrel__Certificates__Default__Password).");
        }

        listenOptions.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(
            certPath,
            certPassword
        );
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

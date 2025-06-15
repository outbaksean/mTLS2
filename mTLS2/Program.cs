using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Server.Kestrel.Https;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options => // Optional: configure certificate validation
    {
        options.AllowedCertificateTypes = CertificateTypes.All; // Example: Allow all cert types
        // Add more validation options here if needed, e.g.,
        // options.ValidateCertificateUse = true;
        // options.ValidateValidityPeriod = true;
        // options.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck; // Example
    });

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
        listenOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

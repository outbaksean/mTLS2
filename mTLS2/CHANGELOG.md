# Changelog

## Server-Side SSL/TLS Configuration

- Configured the Kestrel web server in `Program.cs` to use a specific server certificate for HTTPS.
- The certificate is loaded from a PFX file, with the path specified in `appsettings.json` under `Kestrel:Certificates:Default:Path`.
- The PFX certificate password is to be provided via User Secrets (key: `Kestrel:Certificates:Default:Password`) for local development or an environment variable (name: `Kestrel__Certificates__Default__Password`) for deployed environments.
- Removed the hardcoded password from `appsettings.json`.
- Added a placeholder configuration for the PFX certificate path in `appsettings.json`.
- Added null/empty checks in `Program.cs` for both the PFX certificate path and password retrieved from configuration, throwing an `InvalidOperationException` if either is not set.

## Git Configuration
- Added `Keys/` to the `.gitignore` file to prevent the `Keys` directory (intended for storing sensitive files like PFX certificates) from being committed to the repository.

## Mutual TLS (mTLS) Configuration
- Configured Kestrel in `Program.cs` to require client certificates by setting `ClientCertificateMode = ClientCertificateMode.RequireCertificate` within `ConfigureHttpsDefaults`.
- Added certificate authentication services using `builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate()`. Basic certificate validation options can be configured here.
- Added the authentication middleware `app.UseAuthentication()` to the HTTP request pipeline in `Program.cs`, placed before `app.UseAuthorization()`.
- Added the `Microsoft.AspNetCore.Authentication.Certificate` NuGet package to the project to support certificate authentication.
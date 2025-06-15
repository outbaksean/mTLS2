# Changelog

## Server-Side SSL/TLS Configuration

- Configured the Kestrel web server in `Program.cs` to use a specific server certificate for HTTPS.
- The certificate is loaded from a PFX file, with the path specified in `appsettings.json` under `Kestrel:Certificates:Default:Path`.
- The PFX certificate password is to be provided via User Secrets
# Security Policy

## Supported versions

Only the latest version on the default branch is supported.

## Reporting a vulnerability

Report suspected vulnerabilities privately through GitHub Security Advisories or
to `[maintainer security contact]`. Do not publish passwords, personal data,
tokens, exploit details, or other sensitive information in a public issue.

## Security considerations

- Keep SQL Server connection strings and credentials outside the repository.
- Use a dedicated database account with the minimum required permissions.
- Keep authentication, authorization, password hashing, and reset-token logic
  enabled when running the application.
- Do not deploy with detailed development errors enabled.
- Review NuGet dependencies regularly.

# Introduction

This project is an early-stage PayPal RESTful WebHook Listener built on .NET Core. This was made with the idea in mind that it could serve as a generic implementation of a barebones listener, with domain-specific implementations to be included by the user (you) [in this project](./PayPalWebHookListener.EventServices). Then, you need only register these services/middleware in [Startup.cs](./PayPalWebHookListener.Api/Startup.cs) and you're good to go. 

## Web Hook Validation

Per PayPal's web hook validation scheme, you'd need to include the following:

- If deploying behind the provided [Nginx reverse proxy](./PayPalWebHookListener.ReverseProxy), then you'd need to include a PayPal.Resources.DigiCertSHA2ExtendedValidationServerCA.crt and PayPal.Resources.DigiCertSHA2ExtendedValidationServerCA.key in that directory, before building the image.

- The sample [Api](./PayPalWebHookListener.Api) does not support HTTPS, and so an assumption is in place that a reverse proxy is used to support and enforce it.
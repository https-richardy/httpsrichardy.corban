global using System.Diagnostics.CodeAnalysis;
global using System.Security.Claims;
global using Microsoft.Extensions.Primitives;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi;

global using Corban.Simulations.WebApi.Extensions;
global using Corban.Simulations.WebApi.Constants;
global using Corban.Simulations.WebApi.Middlewares;

global using Corban.Simulations.Infrastructure.IoC.Extensions;
global using Corban.Simulations.CrossCutting.Configurations;

global using HttpsRichardy.Federation.Sdk.Extensions;
global using HttpsRichardy.Federation.Sdk.Contracts.Errors;

global using Serilog;
global using Serilog.Context;

global using Scalar.AspNetCore;
global using FluentValidation.AspNetCore;

global using Idempwanna.Core.Configuration;
global using Idempwanna.Core.Attributes;

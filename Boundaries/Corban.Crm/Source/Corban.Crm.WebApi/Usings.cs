global using System.Diagnostics.CodeAnalysis;
global using System.Security.Claims;
global using Microsoft.Extensions.Primitives;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.OpenApi.Models;

global using Corban.Crm.WebApi.Extensions;
global using Corban.Crm.WebApi.Constants;
global using Corban.Crm.WebApi.Middlewares;

global using Corban.Crm.Infrastructure.IoC.Extensions;
global using Corban.Crm.CrossCutting.Configurations;

global using HttpsRichardy.Federation.Sdk.Extensions;
global using HttpsRichardy.Federation.Sdk.Contracts.Errors;

global using Serilog;
global using Serilog.Context;

global using Scalar.AspNetCore;
global using FluentValidation.AspNetCore;

global using Idempwanna.Core.Configuration;
global using Idempwanna.Core.Attributes;

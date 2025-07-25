﻿global using System.IdentityModel.Tokens.Jwt;
global using System.Linq.Expressions;
global using System.Net;
global using System.Security.Claims;
global using System.Text;
global using System.Text.RegularExpressions;
global using Consul;
global using EventBus.Abstractions.Abstractions;
global using EventBus.Abstractions.Events;
global using EventBus.RabbitMq;
global using EventBus.RabbitMq.Abstractions;
global using EventBus.RabbitMq.Connections;
global using EventBus.RabbitMq.Subscriptions;
global using FluentValidation;
global using FluentValidation.AspNetCore;
global using IP2LocationIOComponent;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Nest.Shared;
global using Nest.Shared.Abstractions;
global using Nest.Shared.Entities;
global using Nest.Shared.Enums;
global using Nest.Shared.Exceptions;
global using Nest.Shared.Utils;
global using NestAuth.API.Abstractions.Repositories;
global using NestAuth.API.Abstractions.Servisec;
global using NestAuth.API.ConfigurationModels;
global using NestAuth.API.Consts;
global using NestAuth.API.Context;
global using NestAuth.API.DTOs;
global using NestAuth.API.DTOs.AuthDtos;
global using NestAuth.API.Entities;
global using NestAuth.API.Extensions;
global using NestAuth.API.Middlewares;
global using NestAuth.API.Repositories;
global using NestAuth.API.Services;
global using NestAuth.API.Tools;
global using NestAuth.API.Validation;
global using Newtonsoft.Json;
global using RabbitMQ.Client;
global using StackExchange.Redis;
global using UAParser;
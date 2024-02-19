using System;
using Azure.Core;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using CloudRepublic.BenchMark.API.V2.Interfaces;
using CloudRepublic.BenchMark.API.V2.Services;
using CloudRepublic.BenchMark.Application.Statics;
using CloudRepublic.BenchMark.Data;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudRepublic.BenchMark.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, string sectionKey, TokenCredential tokenCredential)
    {
        services.AddScoped<IResponseCacheService>(s =>
        {
            var configuration = s.GetRequiredService<IConfiguration>();
            var storageSection = configuration.GetSection(sectionKey);
            var blobServiceClient = new BlobServiceClient(new Uri(storageSection["containerEndpoint"]), tokenCredential);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(storageSection["containerName"]);


            return new ResponseCacheService(blobContainerClient);
        });

        return services;
    }
}
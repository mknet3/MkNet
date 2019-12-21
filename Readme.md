# MkNet

[![Build Status](https://dev.azure.com/manuelcanete/Cross/_apis/build/status/manuelcaub.MkNet?branchName=master)](https://dev.azure.com/manuelcanete/Cross/_build/latest?definitionId=1&branchName=master)

This project is a simple workaround to add AAD authentication to our docs built with mkdocs.

## How to build

```console
mkdocs build -t readthedocs
docker build -f ./src/MkNet/Dockerfile ./src -t mknet:local
docker run -p 5001:80 --env AzureAd__TenantId=YourTenantId --env AzureAd__ClientId=YourClientId mknet:local

```

## Repository

* [mcanete/mknet](https://hub.docker.com/r/mcanete/mknet)

## Test it with docker

All you need are a ClientId (Application Id) and a TenantId from Azure Active Directory.

```console
docker run -p 5001:80 --env AzureAd__TenantId=YourTenantId --env AzureAd__ClientId=YourClientId mcanete/mknet
```

Enjoy it!

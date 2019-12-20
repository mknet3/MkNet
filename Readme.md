# MkNet

This project is a simple workaround to add AAD authentication to our docs built with mkdocs.

## How to build

```bash
mkdocs build -t readthedocs
docker build -f ./src/MkNet/Dockerfile ./src -t mknet:latest
docker run -p 5001:80 --env AzureAd__TenantId=YourTenantId --env AzureAd__ClientId=YourClientId mknet:latest

```

Enjoy it!

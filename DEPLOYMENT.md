# Deployment Guide

## Overview

This app can be deployed to Azure App Service with automatic CI/CD via GitHub Actions.

## Prerequisites

1. Azure account (free at azure.microsoft.com)
2. GitHub repository with your code
3. Gemini API key

---

## Step 1: Push Code to GitHub

```bash
git add .
git commit -m "Add deployment configs"
git push origin main
```

---

## Step 2: Create Azure Resources

### Option A: Using Azure Portal (Web UI)

1. Go to portal.azure.com
2. Click "Create a resource"
3. Search "Web App"
4. Click "Create"
5. Fill in:
   - **Resource Group**: Create new (e.g., "thesignal-rg")
   - **Name**: Choose unique (e.g., "thesignal-app-123")
   - **Runtime stack**: .NET 8 (LTS)
   - **Operating System**: Linux
   - **Region**: Choose closest to users
   - **Pricing Plan**: Free F1 (for testing) or Basic B1
6. Click "Review + Create" then "Create"

### Option B: Using Azure CLI

```bash
# Login to Azure
az login

# Create resource group
az group create --name thesignal-rg --location eastus

# Create App Service plan (FREE tier)
az appservice plan create \
  --name thesignal-plan \
  --resource-group thesignal-rg \
  --sku F1 \
  --is-linux

# Create Web App
az webapp create \
  --name thesignal-app-123 \
  --resource-group thesignal-rg \
  --plan thesignal-plan \
  --runtime "DOTNETCORE:8.0"
```

---

## Step 3: Get Publish Profile

1. In Azure Portal, go to your Web App
2. Click "Overview" → "Get publish profile"
3. Download the file
4. Open it and copy the entire content

---

## Step 4: Configure GitHub Secrets

1. Go to your GitHub repo
2. Click "Settings" → "Secrets and variables" → "Actions"
3. Click "New repository secret"
4. Name: `AZURE_WEBAPP_PUBLISH_PROFILE`
5. Value: Paste the entire publish profile content from Step 3
6. Click "Add secret"

### Add Gemini API Key

1. Create another secret
2. Name: `GEMINI_API_KEY`
3. Value: Your Gemini API key
4. Click "Add secret"

---

## Step 5: Update Workflow

Edit `.github/workflows/azure-deploy.yml`:

```yaml
env:
  AZURE_WEBAPP_NAME: thesignal-app-123  # <-- Change to your app name
```

---

## Step 6: Deploy

Push any change to trigger deployment:

```bash
git commit --allow-empty -m "Trigger deployment"
git push
```

Or manually trigger:
- Go to GitHub repo → Actions → "Deploy to Azure App Service" → "Run workflow"

---

## Step 7: Verify Deployment

1. Check GitHub Actions tab for build status
2. Once green, visit: `https://thesignal-app-123.azurewebsites.net`
3. Check health: `https://thesignal-app-123.azurewebsites.net/health`

---

## Configuration

### Environment Variables

Set in Azure Portal → Configuration → Application settings:

| Name | Value | Description |
|------|-------|-------------|
| `Gemini__ApiKey` | your-key | Gemini API key |
| `ASPNETCORE_ENVIRONMENT` | Production | Environment mode |

Note: Use double underscore `__` for nested config (e.g., `Gemini__ApiKey` = `Gemini:ApiKey`)

### Database

- SQLite file (`app.db`) is created on first run
- Stored in `/home/site/wwwroot/` on Azure
- Consider Azure SQL Database for production scale

---

## Docker Deployment (Alternative)

Build and run locally:

```bash
# Build
docker build -t thesignal .

# Run
docker run -p 8080:8080 \
  -e Gemini__ApiKey=your-key \
  thesignal
```

---

## Troubleshooting

### Build Fails

Check GitHub Actions logs for errors.

### App Won't Start

Check Azure logs:
```bash
az webapp log tail --name your-app --resource-group your-rg
```

Or in Portal: Monitoring → Log stream

### Database Issues

SQLite on Azure (Linux):
- File permissions: Ensure `/home/site/wwwroot/` is writable
- Persistent storage: Enable in Configuration → Path mappings

### 500 Errors

1. Enable detailed errors in Azure Configuration
2. Set `ASPNETCORE_DETAILEDERRORS=true` temporarily

---

## Cost Estimates

| Resource | Free Tier | Basic Tier |
|----------|-----------|------------|
| Azure App Service | $0/mo | ~$13/mo |
| Azure SQL (optional) | - | ~$5/mo |
| Gemini API | - | Pay per use (~$0.001/request) |

---

## Next Steps

- [ ] Set up custom domain
- [ ] Add HTTPS certificate
- [ ] Configure Application Insights for monitoring
- [ ] Set up staging environment
- [ ] Migrate to Azure SQL Database

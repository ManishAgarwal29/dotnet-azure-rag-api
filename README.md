# dotnet-azure-rag-api
A basic .NET API for implementing Retrieval-Augmented Generation (RAG) using Azure OpenAI and Azure AI Search. Includes endpoints for document upload and chat using indexed data.

It allows users to:
- Upload PDF documents, process them into vector embeddings, and index them in Azure AI Search.
- Query against these documents using a chatbot interface powered by Azure OpenAI.

## NuGet Package Dependencies

Install all required packages using:

dotnet add package Azure.AI.OpenAI  
dotnet add package Azure.Search.Documents  
dotnet add package Azure.Core  
dotnet add package Azure.Identity  
dotnet add package PdfPig  

## Required Environment Variables

### Azure OpenAI
[System.Environment]::SetEnvironmentVariable("AzureOpenAI__ApiKey", "<your-openai-api-key>", "Machine")
[System.Environment]::SetEnvironmentVariable("AzureOpenAI__Endpoint", "https://<your-openai-resource>.openai.azure.com/", "Machine")

### Azure AI Search
[System.Environment]::SetEnvironmentVariable("AzureSearch__ApiKey", "<your-search-api-key>", "Machine")
[System.Environment]::SetEnvironmentVariable("AzureSearch__Endpoint", "https://<your-search-resource>.search.windows.net", "Machine")

## Optional Environment Variable

### Upload PDF API Endpoint
[System.Environment]::SetEnvironmentVariable("UPLOAD_API_KEY", "<your-secure-api-key>", "Machine")

## Azure Resources Required

Azure OpenAI Resource  
Azure AI Search Resource  
GPT-4o Chat Completion and text-embedding-ada-002 Embedding Models  

## Features

API Key-based authorization for secured endpoint (e.g. PDF upload)  
PDF ingestion    
GPT-4o-powered chat completion  
Vector embedding and similarity search via Azure AI Search  

from openai import AzureOpenAI
from app.config import AZURE_OPENAI_ENDPOINT, AZURE_OPENAI_KEY, API_VERSION

def get_openai_client():
    return AzureOpenAI(
        api_key=AZURE_OPENAI_KEY,
        api_version=API_VERSION,
        azure_endpoint=AZURE_OPENAI_ENDPOINT,
    )
import os
from dotenv import load_dotenv

load_dotenv()

AZURE_OPENAI_ENDPOINT = os.getenv("AZURE_OPENAI_ENDPOINT")
AZURE_OPENAI_KEY = os.getenv("AZURE_OPENAI_KEY")
GPT_ENGINE = os.getenv("GPT_ENGINE")
API_VERSION = os.getenv("API_VERSION", "2024-12-01-preview")


_required = [ AZURE_OPENAI_ENDPOINT, AZURE_OPENAI_KEY, GPT_ENGINE, API_VERSION]
if not all(_required):
    missing = [name for name, val in [
        ("AZURE_OPENAI_ENDPOINT", AZURE_OPENAI_ENDPOINT),
        ("AZURE_OPENAI_KEY", AZURE_OPENAI_KEY),        
        ("GPT_ENGINE", GPT_ENGINE),
        ("API_VERSION", API_VERSION)
    ] if not val]
    raise ValueError(f"Missing required environment variables: {missing}")
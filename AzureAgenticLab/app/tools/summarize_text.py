from app.openai_client import get_openai_client
from app.config import GPT_ENGINE

def summarize_text(text: str) -> str:
    client = get_openai_client()
    response = client.chat.completions.create(
        model=GPT_ENGINE,
        messages=[
            {"role": "system", "content": "You are a helpful assistant that summarizes text."},
            {"role": "user", "content": f"Summarize this: {text}"},
        ]
    )
    return response.choices[0].message.content.strip()
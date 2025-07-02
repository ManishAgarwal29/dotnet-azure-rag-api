import requests
import json

def call_rag_chat_api(query: str):
    base_url = "https://localhost:7215"  # Change this to your actual base URL
    url = f"{base_url.rstrip('/')}/api/Chat"
    headers = {
        "Accept": "application/json",
        "Content-Type": "application/json"
    }
    
    payload = {"question": query}

    resp = requests.post(url, headers=headers, json=payload, verify=False, timeout=30)
    try:
        resp.raise_for_status()
    except requests.HTTPError as e:
        # You can inspect resp.status_code and resp.text here
        print(f"Request failed: {resp.status_code} - {resp.text}")
        raise

    # parse JSON response
    try:
        return resp.json()
    except json.JSONDecodeError:
        # response was not JSON
        return resp.text

import logging
import json
from typing import Callable, Dict
from app.openai_client import get_openai_client
from app.tools.summarize_text import summarize_text
from app.tools.calculate_bmi import calculate_bmi
from app.tools.get_financial_advice import get_financial_advice
from app.tools.call_rag_chat_api import call_rag_chat_api
from app.config import GPT_ENGINE

# Set up logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger("AIAgent")

# Tool registry
TOOLS: Dict[str, Callable] = {
    "calculate_bmi": calculate_bmi,
    "get_financial_advice": get_financial_advice,
    "summarize_text": summarize_text,
    "call_rag_chat_api": call_rag_chat_api
}

def ai_agent(user_input: str) -> str:
    try:
        decision = decide_action(user_input)
        tool_name = decision["tool"]
        args = decision["args"]

        if tool_name == "no_tool":
            return "Sorry, I couldn ’t find a suitable tool to help with that request."

        if tool_name in TOOLS:
            result = TOOLS[tool_name](**args)
            return result
        else:
            return f"Unknown tool requested: {tool_name}"
    except Exception as e:
        logger.exception("Error handling user input")
        return f"An error occurred: {str(e)}"
    
# LLM control flow decision maker
def decide_action(user_input: str) -> Dict:
    prompt = """
You are a decision-making controller. Based on the user input below, decide which tool to invoke.

TOOLS:
1. calculate_bmi(weight: float, height: float) - Use only for health, fitness, or BMI-related queries.
2. get_financial_advice(income: float, expenses: float) - Use only for money, finance, or budgeting queries.
3. summarize_text(text: str) - Use for requests asking to summarize content or text.
4. call_rag_chat_api(query: str) - Use ONLY if the query clearly relates to Microsoft Azure, cloud services, internal IT support topics like VPN, Outlook setup, password reset, service desk, or organization policies. DO NOT use this tool for general questions.
5. no_tool() - Use if none of the tools apply to the user input.

Respond with a JSON in the following format:
{"tool": "tool_name", "args": {"arg1": value1, "arg2": value2}}

If none of the tools match the user's query, respond with:
{"tool": "no_tool", "args": {}}

User Input:
""" + user_input
    
    client = get_openai_client()
    response = client.chat.completions.create(
        model=GPT_ENGINE,
        messages=[
            {"role": "system", "content": "You are an expert agent controller for routing requests to tools."},
            {"role": "user", "content": prompt},
        ],
        temperature=0.2
    )

    #raw_response = response["choices"][0]["message"]["content"].strip()
    raw_response = response.choices[0].message.content.strip()
    logger.info(f"LLM control flow decision: {raw_response}")

    # Clean Markdown code block if present
    if raw_response.startswith("```"):
        raw_response = raw_response.strip("`").strip()
        if raw_response.startswith("json"):
            raw_response = raw_response[4:].strip()

    return json.loads(raw_response)
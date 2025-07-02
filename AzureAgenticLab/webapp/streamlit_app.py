import streamlit as st
import os
import sys
sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
from app.agent_chat import ai_agent

st.set_page_config(page_title="AI Multi Agent Control Flow", layout="wide")
st.title("AI Multi Agent Control Flow")

query = st.text_input("Enter your query:", "")
if st.button("Submit") and query:
    with st.spinner("Processing..."):
        try:
            response = ai_agent(query)
            st.success("Response from AI Agent:")
            st.write(response)
        except Exception as e:
            st.error(f"An error occurred: {e}")
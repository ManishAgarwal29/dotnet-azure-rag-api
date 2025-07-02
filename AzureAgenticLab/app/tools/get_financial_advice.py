def get_financial_advice(income: float, expenses: float) -> str:
    savings = income - expenses
    if savings < 0:
        return "You are spending more than you earn. Consider reducing expenses."
    elif savings < 5000:
        return "Try to increase your savings for better financial health."
    else:
        return "You are doing well financially. Consider investing your savings."
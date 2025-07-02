def calculate_bmi(weight: float, height: float) -> str:
    bmi = weight / (height ** 2)
    category = (
        "Underweight" if bmi < 18.5 else
        "Normal" if bmi < 24.9 else
        "Overweight" if bmi < 29.9 else
        "Obese"
    )
    return f"Your BMI is {bmi:.2f}, which is considered {category}."
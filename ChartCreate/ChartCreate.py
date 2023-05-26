# -*- coding: cp1251 -*-
import pandas as pd
from matplotlib import pyplot as plt
plt.rcParams["figure.figsize"] = [7.00, 3.50]
plt.rcParams["figure.autolayout"] = True
columns = ["Фамилия", "Зарплата"]
df = pd.read_csv(r"C:\Users\anton\Desktop\otchet.csv", usecols=columns, encoding='utf8')
plt.plot(df.Фамилия, df.Зарплата)
plt.savefig(r'C:\Users\anton\Desktop\otchet.png')

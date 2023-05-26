# -*- coding: cp1251 -*-
import pandas as pd
from matplotlib import pyplot as plt
plt.rcParams["figure.figsize"] = [7.00, 3.50]
plt.rcParams["figure.autolayout"] = True
columns = ["�������", "�������������"]
df = pd.read_csv(r"C:\Users\anton\Desktop\otchet.csv", usecols=columns, encoding='utf8')
df.loc[ len(df.index )] = ['',0]
plt.plot(df.�������[::-1], df.�������������[::-1])
plt.savefig(r'C:\Users\anton\Desktop\otchet.png')

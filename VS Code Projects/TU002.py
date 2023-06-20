from numpy import NaN
import pandas as pd
pd.set_option('display.max_columns', None)
pd.set_option('display.max_row',None)
read = pd.read_csv("vacancies.csv")
read = read.sort_values("salary_to")
read = read[read.salary_to > 1]
print(read.loc[9])
import csv
from os import remove
import re
import pandas as pd
def clear(x):
    return ' '.join(re.sub(r"<[^>]+>", '', x).split())
name_of_file = "vacancies.csv"
with open(name_of_file,encoding='utf-8-sig') as fp:
    reader = csv.reader(fp)
    head = next(reader)
    data_lines = []
    for row in reader:
        flag = True
        if (len(row) < len(head)):
            continue
        for i in row:
            if (i == ""):
                flag = False
        if (flag):
            data_lines.append(row)
output_dict = list()
for line in data_lines:
    data_dict = dict()
    for i in range(len(head)):
        temp_line = line[i]
        temp_line = temp_line.split('\n')
        if (len(temp_line) == 1):
            data_dict[head[i]] = clear(temp_line[0])
        else:
            for g in range(len(temp_line)):
                temp_mass = temp_line[g].split()
                temp_line[g] = ' '.join(temp_mass)
            data_dict[head[i]] = ', '.join(temp_line)
    output_dict.append(data_dict)
for item in output_dict:
    if item["salary_currency"] == "RUR":
        0+0
    else: output_dict.remove(item);
output_dict.sort(key=lambda x: x["salary_to"])
for i in range(len(output_dict)):
    for key, item in output_dict[i].items():
        print(key + ": " + item)
    if (i<len(output_dict)-1): print()

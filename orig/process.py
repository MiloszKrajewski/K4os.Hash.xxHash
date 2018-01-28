#!/usr/bin/python3

import re
import subprocess

def preprocess():
    return subprocess.check_output([
        'mcpp.exe', 
        '-Iinclude', 
        '-DXXH_FORCE_MEMORY_ACCESS=2',
        '-DXXH_FORCE_ALIGN_CHECK=0',
        'xxhash.c'
    ]).decode("utf-8")

def sreplace(result, before, after):
    return result.replace(before, after)

result = preprocess()
result = sreplace(result, '\r\n', '\n')

with open("xxhash.cs", "w") as f:
    f.write(result)

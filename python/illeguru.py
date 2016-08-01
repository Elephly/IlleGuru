import sys
from os import *
from os.path import *

from stringpattern import StringPattern
from stringpatternutil import ParsePatterns

def Parsic(path_str):
    file_list = []
    match_patterns = ParsePatterns("shelly")

    if isfile(path_str):
        print("Illeguru operating on file '{0}'".format(path_str))
        file_list.append(path_str);
    elif isdir(path_str):
        print("Illeguru operating on directory '{0}'".format(path_str))
        file_list.extend([join(path_str, f) for f in listdir(path_str) if isfile(join(path_str, f))])
    else:
        print("Invalid path argument provided '{0}'".format(path_str))

    for f in file_list:
        file_name = file_name_match = split(f)[1]
        for pat in match_patterns:
            file_name_match = pat.Match(file_name_match)

if __name__ == "__main__":
    path_str = ""
    if len(sys.argv) > 1:
        path_str = sys.argv[1]
    Parsic(path_str)

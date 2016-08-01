import sys

from stringpattern import *

pattern_type = {
    "a": AlbumPattern,
    "p": ArtistPattern,
    "/": DelimPattern,
    "g": GenrePattern,
    "[": IgnoreInPattern,
    "<": IgnorePrePattern,
    ">": IgnorePostPattern,
    "t": TitlePattern,
    "n": TrackNumberPattern,
}

def ParsePatterns(string):
    assert type(string) is str
    in_str = string.lower()
    patterns = []
    # Parse string for parsec patterns and add to the patterns array
    while len(in_str) > 0:
        patterns += [pattern_type[in_str[0]]()]
        in_str = in_str[1:]
    return patterns

if __name__ == "__main__":
    string = ""
    if len(sys.argv) > 1:
        string = sys.argv[1]
    print(ParsePatterns(string))

from abc import ABC, abstractmethod

class StringPattern(ABC):
    @abstractmethod
    def Match(self, string):
        pass

class AlbumPattern(StringPattern):
    def Match(self, string):
        pass

class ArtistPattern(StringPattern):
    def Match(self, string):
        pass

class DelimPattern(StringPattern):
    def Match(self, string):
        pass

class GenrePattern(StringPattern):
    def Match(self, string):
        pass

class IgnoreInPattern(StringPattern):
    def Match(self, string):
        pass

class IgnorePrePattern(StringPattern):
    def Match(self, string):
        pass

class IgnorePostPattern(StringPattern):
    def Match(self, string):
        pass

class TitlePattern(StringPattern):
    def Match(self, string):
        pass

class TrackNumberPattern(StringPattern):
    def Match(self, string):
        pass

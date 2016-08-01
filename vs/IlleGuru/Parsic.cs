using System;
using System.Collections.Generic;
using System.IO;

namespace IlleGuru
{
    class Parsic
    {
        public static void Run(string directory, bool clear = false)
        {
            TagLib.Id3v2.Tag.DefaultVersion = 3;
            TagLib.Id3v2.Tag.ForceDefaultVersion = true;

            string songDirectory = (new DirectoryInfo(directory)).FullName;
            string songDirectoryName = (new DirectoryInfo(directory)).Name;
            string comment = String.Format("Media retrieved from \"{0}\".\nMetadata filled by IlleGuru Parsic.", songDirectory);
            string[] files = Directory.GetFiles(songDirectory);
            List<string> failedFiles = new List<string>();

            int count = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Progress: {0} of {1}.", count, files.Length);
            Console.ResetColor();

            foreach (string path in files)
            {
                string songName = Path.GetFileNameWithoutExtension(path);
                try
                {
                    if (clear)
                    {
                        using (TagLib.File f = TagLib.File.Create(path))
                        {
                            f.Tag.Clear();
                            f.Save();
                        }
                    }
                    else
                    {
                        string[] songNameTokens = songName.Split(' ');

                        int trackNumber = 0;
                        if (songNameTokens[0].EndsWith(".") && Int32.TryParse(songNameTokens[0].TrimEnd('.'), out trackNumber))
                        {
                            songName = songName.Substring(songName.IndexOf(' ')).Trim();
                        }

                        songName = songName.Replace("Electronic -- ", "");
                        songName = songName.Replace("Electro Funk -- ", "");
                        songName = songName.Replace("Electro House -- ", "");
                        songName = songName.Replace("Electro Pop -- ", "");
                        songName = songName.Replace("FrenchELETRO -- ", "");
                        songName = songName.Replace("FutureBASS -- ", "");
                        songName = songName.Replace("FutureFUNK -- ", "");
                        songName = songName.Replace("GlitchHOP -- ", "");
                        songName = songName.Replace("House -- ", "");
                        songName = songName.Replace("IndieDANCE -- ", "");
                        songName = songName.Replace("Indie Dance -- ", "");
                        songName = songName.Replace("IndiePOP -- ", "");
                        songName = songName.Replace("Melodic House -- ", "");
                        songName = songName.Replace("NuDISCO -- ", "");
                        songName = songName.Replace("Nu Disco -- ", "");
                        songName = songName.Replace("Nu Funk -- ", "");
                        songName = songName.Replace("NuFUNK -- ", "");
                        songName = songName.Replace("R&B -- ", "");
                        songName = songName.Replace("SynthPOP -- ", "");

                        songName = songName.Replace(" - Diversity Release", "");
                        songName = songName.Replace(" (HQ)", "");
                        songName = songName.Replace(" (Official Lyric Video)", "");
                        songName = songName.Replace(" (Official Video)", "");
                        songName = songName.Replace(" (Original Mix)", "");

                        int openBracket = songName.IndexOf(" [");
                        while (openBracket != -1)
                        {
                            int closedBracket = songName.IndexOf("]");
                            if (closedBracket != -1 && closedBracket > openBracket)
                            {
                                songName = songName.Remove(openBracket, closedBracket - openBracket + 1);
                            }
                            else break;
                            openBracket = songName.IndexOf(" [");
                        }

                        int songComment = songName.IndexOf(" --");
                        if (songComment > songName.IndexOf(" - "))
                        {
                            songName = songName.Substring(0, songComment);
                        }

                        string songArtist = songName.Substring(0, songName.IndexOf(" - "));
                        string songTitle = songName.Substring(songName.IndexOf(" - ") + 3);

                        List<string> songArtists = new List<string>();

                        int seperatorIndex = songArtist.ToLower().IndexOf(" x ");
                        while (seperatorIndex != -1)
                        {
                            songArtists.Add(songArtist.Substring(0, seperatorIndex));
                            songArtist = songArtist.Substring(seperatorIndex + 3);
                            seperatorIndex = songArtist.ToLower().IndexOf(" x ");
                        }
                        songArtists.Add(songArtist);

                        List<string> temp = songArtists;
                        songArtists = new List<string>();
                        foreach (string s in temp)
                        {
                            string artist = s;
                            seperatorIndex = artist.ToLower().IndexOf(" & ");
                            while (seperatorIndex != -1)
                            {
                                songArtists.Add(artist.Substring(0, seperatorIndex));
                                artist = artist.Substring(seperatorIndex + 3);
                                seperatorIndex = artist.ToLower().IndexOf(" & ");
                            }
                            songArtists.Add(artist);
                        }

                        temp = songArtists;
                        songArtists = new List<string>();
                        foreach (string s in temp)
                        {
                            string artist = s;
                            songArtists.AddRange(artist.Split(','));
                        }

                        for (int i = 0; i < songArtists.Count; i++)
                        {
                            songArtists[i] = songArtists[i].Trim();
                        }

                        using (TagLib.File f = TagLib.File.Create(path))
                        {
                            if (trackNumber > 0)
                            {
                                f.Tag.Track = (uint)trackNumber;
                            }
                            f.Tag.Title = songTitle.Trim();
                            f.Tag.Comment = comment.Trim();
                            f.Tag.Performers = songArtists.ToArray();
                            f.Tag.Album = songDirectoryName.Trim();
                            f.Save();
                        }
                    }
                }
                catch (Exception e)
                {
                    ConsoleBackspace(string.Format("Progress: {0} of {1}.", count, files.Length).Length);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0}: {1}", songName, e.Message);
                    Console.ResetColor();
                    failedFiles.Add(path);
                    continue;
                }

                ConsoleBackspace(string.Format("Progress: {0} of {1}.", count, files.Length).Length);
                Console.WriteLine(songName);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Progress: {0} of {1}.", ++count, files.Length);
                Console.ResetColor();
            }

            ConsoleBackspace(string.Format("Progress: {0} of {1}.", count, files.Length).Length);

            if (failedFiles.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n{0} file{1} unable to be properly parsed:", failedFiles.Count, (failedFiles.Count > 1 ? "s were" : " was"));
                foreach (string s in failedFiles)
                {
                    Console.WriteLine("\t{0}", s);
                }
                Console.ResetColor();
            }
            Console.Write("\nParsic: Done! Press any key to continue.");
            Console.ReadKey();
        }

        public static void ConsoleBackspace(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Console.Write("\b \b");
            }
        }
    }
}
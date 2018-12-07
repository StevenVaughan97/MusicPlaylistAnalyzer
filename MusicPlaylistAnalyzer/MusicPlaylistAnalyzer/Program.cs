using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MusicPlaylistAnalyzer
{
    public class Song
    {
        public string Name;
        public string Artist;
        public string Album;
        public string Genre;
        public int Size;
        public int Time;
        public int Year;
        public int Plays;

        public Song(string name, string artist, string album, string genre, int size, int time, int year, int plays)
        {
            Name = name;
            Artist = artist;
            Album = album;
            Genre = genre;
            Size = size;
            Time = time;
            Year = year;
            Plays = plays;
        }


        override public string ToString()
        {
            return String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", Name, Artist, Album, Genre, Size, Time, Year, Plays);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            string report = null;
            int i;
            List<Song> RowCol = new List<Song>();
            try
            {
                if (File.Exists($"SampleMusicPlaylist.txt") == false)
                {
                    Console.WriteLine("SampleMusicPlaylist text File does not exist...");
                }
                else
                {
                    StreamReader sr = new StreamReader($"SampleMusicPlaylist.txt");
                    i = 0;
                    string line = sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        i = i + 1;
                        try
                        {
                            string[] strings = line.Split('\t');

                            if (strings.Length < 8)
                            {
                                Console.Write("Record doesnʼt contain the correct number of elements");
                                Console.WriteLine($"Row {i} contains {strings.Length}  values. It should contain 8.");
                                break;
                            }
                            else
                            {
                                Song dataTemp = new Song((strings[0]), (strings[1]), (strings[2]), (strings[3]), Int32.Parse(strings[4]), Int32.Parse(strings[5]), Int32.Parse(strings[6]), Int32.Parse(strings[7]));
                                RowCol.Add(dataTemp);
                            }
                        }
                        catch
                        {
                            Console.Write("Error occurs reading lines from playlist data file...");
                            break;
                        }
                    }
                    sr.Close();
                }
            }


            catch (Exception e)
            {
                Console.WriteLine("Playlist data file canʼt be opened...");
            }


            try
            {
                Song[] songs = RowCol.ToArray();
                using (StreamWriter write = new StreamWriter("MusicPlaylistReport.txt"))
                {
                    write.WriteLine("Music Playlist Report\n");

                    //1. How many songs received 200 or more plays?   
                    var SongsPlays = from song in songs where song.Plays >= 200 select song;
                    report += "\nSongs that received 200 or more plays:\n \n";

                    foreach (Song song in SongsPlays)
                    {
                        report += song + "\n";
                    }

                    //2.How many songs are in the playlist with the Genre of “Alternative”?
                    var SongsGenreAlternative = from song in songs where song.Genre == "Alternative" select song;
                    i = 0;
                    foreach (Song song in SongsGenreAlternative)
                    {
                        i++;
                    }
                    report += $"\nSongs are in the playlist with the Genre of Alternative: {i}\n \n";

                    //3.How many songs are in the playlist with the Genre of “Hip - Hop / Rap”?
                    var SongsGenreHipHopRap = from song in songs where song.Genre == "Hip-Hop/Rap" select song;
                    i = 0;
                    foreach (Song song in SongsGenreHipHopRap)
                    {
                        i++;
                    }
                    report += $"Number of songs Hip-Hop/Rap: {i}\n \n";

                    //4.What songs are in the playlist from the album “Welcome to the Fishbowl?”
                    var SongsAlbumFishbowl = from song in songs where song.Album == "Welcome to the Fishbowl" select song;
                    report += "Songs from the album Welcome to the Fishbowl:\n \n";
                    foreach (Song song in SongsAlbumFishbowl)
                    {
                        report += song + "\n";
                    }

                    //5.What are the songs in the playlist from before 1970 ?
                    var Songs1970 = from song in songs where song.Year < 1970 select song;
                    report += "\nSongs from before 1970:\n \n";
                    foreach (Song song in Songs1970)
                    {
                        report += song + "\n";
                    }

                    //6.What are the song names that are more than 85 characters long?
                    var Names85Characters = from song in songs where song.Name.Length > 85 select song.Name;
                    report += "\nSong names longer than 85 characters:\n \n";
                    foreach (string name in Names85Characters)
                    {
                        report += name + "\n";
                    }

                    //7.What is the longest song ? (longest in Time)
                    var LongestSong = from song in songs orderby song.Time descending select song;
                    report += "\nLongest song:\n \n";
                    report += LongestSong.First();

                    write.Write(report);
                    write.Close();
                }
                Console.WriteLine("Music Playlist Report file has be created...");
            }


            catch (Exception ex)
            {
                Console.WriteLine("Report file canʼt be opened or written to...");
            }

            Console.ReadLine();
        }
    }
}
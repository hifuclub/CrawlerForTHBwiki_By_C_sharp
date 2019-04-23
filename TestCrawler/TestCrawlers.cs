using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;


namespace TestCrawler
{
    public class TestCrawlers
    {
        public string crawler(string uri)
        {
            ArrayList albumInfoArray = new ArrayList();
            albumInfoArray.Clear();
            Console.Out.WriteLine(uri);
            int previousTrack = 0;
            try
            {
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(uri);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                //Console.WriteLine(response.StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                // Cleanup the streams and the response.
                string[] songInfo;
                string knife01 = "曲目列表</span></h2>";
                string knife02 = "评论</span></h2>";
                string tuneInfo;
                try
                {
                    tuneInfo = cutStrings(responseFromServer, knife01, knife02)[0];

                    Regex knifeForSong = new Regex("<tr><td id=\"\\d{0,3}\" class=\"info\\w{1,3}\"><b>");
                    songInfo = knifeForSong.Split(tuneInfo);

                }
                catch (Exception)
                {


                    tuneInfo = cutStrings(responseFromServer, knife01, 0);

                    Regex knifeForSong = new Regex("<tr><td id=\"\\d{0,3}\" class=\"info\\w{1,3}\"><b>");
                    songInfo = knifeForSong.Split(tuneInfo);

                }

                //Console.Out.WriteLine("1234567890".Remove(0,5));
                foreach (string s in songInfo)
                {
                    bool isTurn = true;
                    bool isOrigin = true;
                    bool isArtist = true;
                    bool isOnlyComposer = false;
                    bool isVoice = false;
                    string tune = s;
                    int trackInt = 0;
                    string title = "";
                    string artist = "";
                    string arrange = "";
                    string composer = "";
                    string voice = "";
                    ArrayList origin = new ArrayList();

                    //track
                    Regex trackReg = new Regex(@"^\d{1,3}");
                    string trackString = trackReg.Match(tune).ToString();
                    try
                    {
                        trackInt = int.Parse(trackString);
                        Console.Out.WriteLine("轨道:" + trackInt);
                    }
                    catch (Exception)
                    {
                        Console.Out.WriteLine("没有获取到序号");
                        isTurn = false;

                    }

                    //title
                    try
                    {
                        tune = cutStrings(tune, "<td", 0);
                        title = cutStrings(tune, "", "</td>")[0];

                        Regex cut = new Regex(">[^<]{1,}<");
                        Match titleMatch = cut.Match(title);
                        title = titleMatch.ToString();
                        title = cutStrings(title, ">", "<",1,0)[0];
                        //{
                        //    string[] lsSs = cutString(tune, "action=edit\">", "</a>", 13, 0);
                        //    title = lsSs[0];
                        //    tune = lsSs[1];
                        //}
                        //else
                        //{
                        //    string[] lsSs = cutString(tune, "class=\"title\">", "<span class", 14, 0);
                        //    title = lsSs[0];
                        //    tune = lsSs[1];
                        //}
                        Console.Out.WriteLine("曲名:" + title);

                    }
                    catch (Exception)
                    {

                        Console.Out.WriteLine("没有获取到标题");
                        isTurn = false;

                    }
                    if (isTurn)
                    {
                        if (Regex.IsMatch(tune, "配音"))
                        {
                            tune = cutStrings(tune, "配音", 0);
                            voice = cutStrings(tune, "", "</td></tr>")[0];
                            tune = cutStrings(tune, "配音", "</td></tr>")[1];
                            voice = cutTags(voice);
                            voice = voice.Replace("（页面不存在）", "");
                            Console.Out.WriteLine("配音:" + voice);
                            string ls = "";
                            for (; Regex.IsMatch(voice, "（"); ls+= ";")
                            {
                                ls += cutStrings(voice, "（", "）",1,0)[0];
                                voice = cutStrings(voice, "）", 1);
                                Console.Out.WriteLine(voice);
                            }
                            voice = ls;
                            voice = voice.Remove(voice.Length - 1);
                            isVoice = true;
                        }
                        else if(Regex.IsMatch(tune, "演唱"))
                        {
                            tune = cutStrings(tune, "演唱", 0);
                            artist = cutStrings(tune, "", "</td></tr>")[0];
                            tune = cutStrings(tune, "演唱", "</td></tr>")[1];
                            artist = cutTags(artist);
                            artist = artist.Replace("（页面不存在）", "");
                            Console.Out.WriteLine("演唱:" + artist);

                        }
                        else if (Regex.IsMatch(tune, "编曲"))
                        {
                            tune = cutStrings(tune, "编曲", 0);
                            arrange = cutStrings(tune, "", "</td></tr>")[0];
                            tune = cutStrings(tune, "编曲", "</td></tr>")[1];
                            arrange = cutTags(arrange);
                            arrange = arrange.Replace("（页面不存在）", "");
                            Console.Out.WriteLine("编曲:" + arrange);
                            isArtist = false;
                        }
                        else if (Regex.IsMatch(tune, "作曲"))
                        {
                            tune = cutStrings(tune, "作曲", 0);
                            composer = cutStrings(tune, "", "</td></tr>")[0];
                            tune = cutStrings(tune, "作曲", "</td></tr>")[1];
                            composer = cutTags(composer);
                            composer = composer.Replace("（页面不存在）", "");
                            Console.Out.WriteLine("作曲:" + composer);
                            isArtist = false;
                            isOnlyComposer = true;
                        }
                        if (Regex.IsMatch(tune, "原曲"))
                        {
                            Console.Out.WriteLine("原曲:");
                            for (; Regex.IsMatch(tune, "ogmusic");)
                            {
                                tune = cutStrings(tune, "ogmusic", 0);
                                tune = cutStrings(tune, "title=\"", 7);
                                string onriginTurn = Regex.Split(tune, "\">")[0];
                                origin.Add(onriginTurn);
                                Console.Out.WriteLine(onriginTurn);
                            }
                        }
                        else
                        {
                            isOrigin = false;
                        }
                    }

                    Console.Out.WriteLine("next\n");

                    if (isTurn)
                    {
                        if (previousTrack > trackInt)
                        {
                            tune = "\r\n\r\n\r\n";
                        }
                        else
                        {
                            tune = "";
                        }
                        if (isVoice)
                        {
                            
                            tune = tune + trackInt + "." + title + "【歌手】" + voice;
                        }
                        else if(isArtist)
                        {
                            tune = tune + trackInt + "." + title + "【歌手】" + artist;
                        }
                        else if (isOnlyComposer)
                        {
                            tune = tune + trackInt + "." + title + "【作曲】" + composer;
                        }
                        else
                        {
                            tune = tune + trackInt + "." + title + "【编曲】" + arrange;
                        }
                        if (isOrigin)
                        {
                            tune = tune + "【副标题】原曲：";
                            foreach (string og in origin)
                            {
                                tune = tune + og + " / ";
                            }
                            tune = tune.Remove(tune.Length - 3);
                        }

                        //字符修正，
                        tune = tune.Replace("&#39;", "'");
                        tune = tune.Replace("&#91;", "[");
                        tune = tune.Replace("&#93;", "]");
                        tune = tune.Replace(" ～", "　～");

                        albumInfoArray.Add(tune);
                        previousTrack = trackInt;
                    }

                }


                reader.Close();
                dataStream.Close();
                response.Close();

                string[] albumInfoString = new string[albumInfoArray.Count];
                for(int i = 0; i < albumInfoArray.Count; i++)
                {
                    albumInfoString[i] = (string)albumInfoArray[i];
                }
                File.WriteAllLines("专辑信息.txt", albumInfoString);
                File.WriteAllText("爬取.html", tuneInfo);

                Form2 form2 = new Form2(albumInfoArray);
                form2.Show();

                return DateTime.Now+"  爬取成功";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return DateTime.Now + "  爬取失败";
            }
        }
        public string[] cutStrings(string s, string cut1, string cut2)
        {
            string[] returnString=new string[2];
            int cutLocation01 = s.IndexOf(cut1);
            int cutLocation02 = s.IndexOf(cut2);

            string cutString = s.Remove(0, cutLocation01);
            cutString = cutString.Remove(cutLocation02 - cutLocation01);
            returnString[0] = cutString;
            returnString[1] = s.Remove(0, cutLocation02);
            return returnString;
        }
        public string[] cutStrings(string s, string cut1, string cut2, int prefixNum, int stemNum)
        {
            string[] returnString = new string[2];
            int cutLocation01 = s.IndexOf(cut1);
            int cutLocation02 = s.IndexOf(cut2);

            string cutString = s.Remove(0, cutLocation01 + prefixNum);
            cutString = cutString.Remove(cutLocation02 - cutLocation01 - stemNum - prefixNum);
            returnString[0] = cutString;
            returnString[1] = s.Remove(0, cutLocation02);
            return returnString;
        }
        public string cutStrings(string s, string cut1, int prefixNum)
        {
            int cutLocation01 = s.IndexOf(cut1);

            string cutString = s.Remove(0, cutLocation01 + prefixNum);
            return cutString;
        }
        public string cutTags(string s)
        {
            string returnString = "";
            Regex cut = new Regex(">[^<]{1,}<");
            MatchCollection tagMatch = cut.Matches(s);
            foreach (Match ss in tagMatch)
            {
                string ls = ss.ToString();
                if (ss.ToString().Equals(">，<"))
                {
                    ls = ls.Replace(">，<", ";");
                    returnString = returnString + ls;
                }
                else
                {
                    returnString = returnString + ss.ToString().Remove(0, 1).Remove(ss.ToString().Length - 2);
                }
            }

            return returnString;

        }
    }
}
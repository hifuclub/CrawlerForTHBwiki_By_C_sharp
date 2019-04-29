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
        int previousTrack = 0;
        ArrayList albumInfoArray = new ArrayList();
        string tuneInfo;
        bool isSuccess = true;
        //主逻辑
        public string crawler(string url)
        {
            previousTrack = 1;
            albumInfoArray.Clear();
            Console.Out.WriteLine(url);
            try
            {
                string[] htmlStr = GetWikiHTML(url);
                foreach (string s in htmlStr)
                {
                    TuneBean tuneBean = StrAssemblyLine(s);
                    PrintTuneInfo(tuneBean);
                }
                Form2 form2 = new Form2(albumInfoArray);
                form2.Show();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                isSuccess = false;
            }
            if (isSuccess)
            {
                return DateTime.Now + "  爬取成功";
            }
            return DateTime.Now + "  爬取失败";

        }

        //获取网页代码string并提取成曲代码
        private string[] GetWikiHTML(string url) {
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(url);
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


            reader.Close();
            dataStream.Close();
            response.Close();

            return songInfo;
        
    }
        //对字符串进行处理
        private TuneBean StrAssemblyLine(string s)
        {
            TuneBean tuneBean = new TuneBean();

            
            string tune = s;


            //track
            Regex trackReg = new Regex(@"^\d{1,3}");
            string trackString = trackReg.Match(tune).ToString();
            try
            {
                tuneBean.trackInt = int.Parse(trackString);
                Console.Out.WriteLine("轨道:" + tuneBean.trackInt);
            }
            catch (Exception)
            {
                Console.Out.WriteLine("没有获取到序号");
                tuneBean.isTurn = false;

            }

            //title
            try
            {
                tune = cutStrings(tune, "<td", 0);
                tuneBean.title = cutStrings(tune, "", "</td>")[0];

                Regex cut = new Regex(">[^<]{1,}<");
                Match titleMatch = cut.Match(tuneBean.title);
                tuneBean.title = titleMatch.ToString();
                tuneBean.title = cutStrings(tuneBean.title, ">", "<", 1, 0)[0];
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
                Console.Out.WriteLine("曲名:" + tuneBean.title);

            }
            catch (Exception)
            {

                Console.Out.WriteLine("没有获取到标题");
                tuneBean.isTurn = false;

            }
            if (tuneBean.isTurn)
            {
                if (Regex.IsMatch(tune, "配音"))
                {
                    tune = cutStrings(tune, "配音", 0);
                    tuneBean.voice = cutStrings(tune, "", "</td></tr>")[0];
                    tune = cutStrings(tune, "配音", "</td></tr>")[1];
                    tuneBean.voice = cutTags(tuneBean.voice);
                    tuneBean.voice = tuneBean.voice.Replace("（页面不存在）", "");
                    Console.Out.WriteLine("配音:" + tuneBean.voice);
                    string ls = "";
                    for (; Regex.IsMatch(tuneBean.voice, "（"); ls += ";")
                    {
                        ls += cutStrings(tuneBean.voice, "（", "）", 1, 0)[0];
                        tuneBean.voice = cutStrings(tuneBean.voice, "）", 1);
                        Console.Out.WriteLine(tuneBean.voice);
                    }
                    tuneBean.voice = ls;
                    tuneBean.voice = tuneBean.voice.Remove(tuneBean.voice.Length - 1);
                    tuneBean.isVoice = true;
                }
                else if (Regex.IsMatch(tune, "演唱"))
                {
                    tune = cutStrings(tune, "演唱", 0);
                    tuneBean.artist = cutStrings(tune, "", "</td></tr>")[0];
                    tune = cutStrings(tune, "演唱", "</td></tr>")[1];
                    tuneBean.artist = cutTags(tuneBean.artist);
                    tuneBean.artist = tuneBean.artist.Replace("（页面不存在）", "");
                    Console.Out.WriteLine("演唱:" + tuneBean.artist);

                }
                else if (Regex.IsMatch(tune, "编曲"))
                {
                    tune = cutStrings(tune, "编曲", 0);
                    tuneBean.arrange = cutStrings(tune, "", "</td></tr>")[0];
                    tune = cutStrings(tune, "编曲", "</td></tr>")[1];
                    tuneBean.arrange = cutTags(tuneBean.arrange);
                    tuneBean.arrange = tuneBean.arrange.Replace("（页面不存在）", "");
                    Console.Out.WriteLine("编曲:" + tuneBean.arrange);
                    tuneBean.isArtist = false;
                }
                else if (Regex.IsMatch(tune, "作曲"))
                {
                    tune = cutStrings(tune, "作曲", 0);
                    tuneBean.composer = cutStrings(tune, "", "</td></tr>")[0];
                    tune = cutStrings(tune, "作曲", "</td></tr>")[1];
                    tuneBean.composer = cutTags(tuneBean.composer);
                    tuneBean.composer = tuneBean.composer.Replace("（页面不存在）", "");
                    Console.Out.WriteLine("作曲:" + tuneBean.composer);
                    tuneBean.isArtist = false;
                    tuneBean.isOnlyComposer = true;
                }
                if (Regex.IsMatch(tune, "原曲"))
                {
                    Console.Out.WriteLine("原曲:");
                    string onriginTurn;
                    for (; Regex.IsMatch(tune, "ogmusic");)
                    {
                        tune = cutStrings(tune, "ogmusic", 0);
                        tune = cutStrings(tune, "title=\"", 7);
                        if (Regex.IsMatch(tune, "<div class=\"ogmusic\">"))
                        {
                            Console.Out.WriteLine(tune);
                            tune = cutStrings(tune, "<div class=\"ogmusic\">", 21);
                            onriginTurn = Regex.Split(tune, "</")[0];
                        }
                        else {
                            onriginTurn = Regex.Split(tune, "\">")[0];
                        }
                        
                        tuneBean.origin.Add(onriginTurn);
                        Console.Out.WriteLine(onriginTurn);
                    }
                }
                else
                {
                    tuneBean.isOrigin = false;
                }


                Console.Out.WriteLine("next\n");


            }
            return tuneBean;
        }
        //打印歌曲信息
        private void PrintTuneInfo(TuneBean tuneBean) {
            if (tuneBean.isTurn)
            {
                string tune;
                if (previousTrack > tuneBean.trackInt)
                {
                    tune = "\r\n\r\n\r\n";
                }
                else
                {
                    tune = "";
                }
                if (tuneBean.isVoice)
                {

                    tune = tune + tuneBean.trackInt + "." + tuneBean.title + "【歌手】" + tuneBean.voice;
                }
                else if (tuneBean.isArtist)
                {
                    tune = tune + tuneBean.trackInt + "." + tuneBean.title + "【歌手】" + tuneBean.artist;
                }
                else if (tuneBean.isOnlyComposer)
                {
                    tune = tune + tuneBean.trackInt + "." + tuneBean.title + "【作曲】" + tuneBean.composer;
                }
                else
                {
                    tune = tune + tuneBean.trackInt + "." + tuneBean.title + "【编曲】" + tuneBean.arrange;
                }
                if (tuneBean.isOrigin)
                {
                    tune = tune + "【副标题】原曲：";
                    foreach (string og in tuneBean.origin)
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
                previousTrack = tuneBean.trackInt;
            }
            string[] albumInfoString = new string[albumInfoArray.Count];
            for (int i = 0; i < albumInfoArray.Count; i++)
            {
                albumInfoString[i] = (string)albumInfoArray[i];
            }
            File.WriteAllLines("专辑信息.txt", albumInfoString);
            File.WriteAllText("爬取.html", tuneInfo);


        }
        

        //裁剪函数
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
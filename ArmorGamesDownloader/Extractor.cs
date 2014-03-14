using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using MoreLinq;

namespace ArmorGamesDownloader
{
    public static class Extractor
    {
        private static Regex swfRegex = new Regex(@"[\[\(\)\]a-zA-Z0-9%:_/\.=-]+\.swf");
        private static Regex tempRegex1, tempRegex2;
        private static Uri tempUri;
        public static Boolean UsingProxy = false;
        private static Boolean FindCorrectName = true;
        private static String tempName = "";
      


        public static Boolean Extract(ref GameData gd, String stringHtml, String stringURL = "")
        {
            MatchCollection regexMatches;
            String stringRegEx;
            FindCorrectName = true;
            switch (gd.Target)
            {
                case SiteNameEnum.Common:
                    #region Common
                    {
                        regexMatches = tempRegex1.Matches(stringHtml);
                        if (regexMatches.Count > 0)
                        {
                            foreach (Match M in regexMatches)
                            {
                                if (gd.Target == SiteNameEnum.Armorgames)
                                    stringURL = stringURL.Replace("armor.ag", "armorgames.com");
                                tempUri = UriAbsolute(M.Value, stringURL);
                                gd.FileList.Add(new GameData.FileData()
                                    {
                                        Link = tempUri.AbsoluteUri,
                                        OriginalName = tempUri.Segments.Last(), //Извлекаем оригинальное имя файла
                                        CorrectName = NameFix((FindCorrectName) ? tempRegex2.Match(stringHtml).Value : tempName) //Правильное имя файла игры
                                    });                          
                            }
                            gd.Reset();
                            break;
                        }
                        else return false;
                    }
                    #endregion
                case SiteNameEnum.Armorgames:
                    #region Armorgames
                    {
                        tempRegex1 = new Regex("(?<=<param name=\"movie\" value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>)[^\\|]+(?= \\|)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Maxgames:
                    #region MaxGames
                    {
                        /****Шаблон MaxGames****/
                        tempRegex1 = new Regex("(?<=param name=\"movie\" value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>Play )[^,]+(?=,)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Swartag:
                    #region Swartag
                    {
                        /****Шаблон Swartag****/
                        tempRegex1 = new Regex("(?<=shockwave-flash\" data=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>)[^\\|]+(?= \\|)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Kongregate:
                    #region Kongregate
                    {
                        /****Шаблон Kongregate****/
                        tempRegex1 = new Regex(@"http[/23AF%:]+[a-z]+\.kongregate\.com[2F/%]+gamez[a-zA-Z0-9./_%-]+\.swf");
                        tempRegex2 = new Regex("(?<=title>Play )[^,]+(?=,)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Newgrounds:
                    #region Newgrounds
                    {
                        /****Шаблон NewGrounds****/
                        tempRegex1 = new Regex(@"http[/23AFs%:\\]+uploads\.ungrounded(?!\.net/apiassets)[a-zA-Z0-9./_%\\-]+\.swf");
                        tempRegex2 = new Regex("(?<=title>)[^<]+(?=<)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Thepodge:
                    #region The Podge
                    {
                        tempRegex1 = new Regex("(?<=embed src=\")uploadedfiles/[a-zA-Z0-9_\\.-]+");
                        tempRegex2 = new Regex("(?<=title>)[^<]+(?= by)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Flashgames247:
                    #region flashgames247
                    {
                        #region Checking for internal link
                        //Переход от страницы с кнопкой "играть" на страницу с, собственно, игрой
                        if (!stringURL.Contains("/play/"))
                        {
                            Regex r = new Regex(@"play/[0-9]+\.html");
                            if (r.IsMatch(stringHtml))
                            {
                                gd.ExternalLink = true;
                                gd.CurrentLink = "http://www.flashgames247.com/" + r.Match(stringHtml).Value;
                                return false;
                            }
                        }
                        #endregion

                        stringRegEx = "(?<=<!-- Game -->.*src=\")[^\"]+";
                        regexMatches = new Regex(stringRegEx, RegexOptions.Singleline).Matches(stringHtml);
                        tempRegex2 = new Regex("(?<=title>)[^-]+(?= -)");
                        if (regexMatches.Count > 0)
                        {
                            Match regexMatch2 = swfRegex.Match(regexMatches[0].Value);
                            if (regexMatch2.Success)
                            {
                                tempUri = UriAbsolute(regexMatch2.Value, stringURL);
                                
                                gd.FileList.Add(new GameData.FileData()
                                {
                                    Link = tempUri.AbsoluteUri,
                                    OriginalName = tempUri.Segments.Last(), //Извлекаем оригинальное имя файла
                                    CorrectName = NameFix(tempRegex2.Match(stringHtml).Value) //Правильное имя файла игры
                                });



                                /*gd.LinkList.Add(tempUri.AbsoluteUri);
                                gd.OriginalNamesList.Add(tempUri.Segments.Last()); //Извлекаем оригинальное имя файла
                                gd.CorrectName = new Regex("(?<=title>)[^-]+(?= -)").Match(stringHtml).Value; //Правильное имя файла игры
                                gd.CorrectNamesList.Add(gd.CorrectName.Replace(": ", " - ")); */
                                gd.Reset();
                                break;
                            }
                            else //Игра расположена на внешнем домене
                            {
                                gd.ExternalLink = true;
                                gd.CurrentLink = regexMatches[0].Value;
                                return false;
                            }
                        }
                        else return false;
                    }
                    #endregion         
                case SiteNameEnum.Jayisgames:
                    #region jayisgames
                    {
                        stringRegEx = "(?<=<iframe [^>]+src=\")[^\"']+";
                        regexMatches = new Regex(stringRegEx, RegexOptions.Singleline).Matches(stringHtml);
                        if (regexMatches.Count > 0)
                        {
                            if (Uri.TryCreate(regexMatches[0].Value, UriKind.Absolute, out tempUri))
                            {
                                gd.ExternalLink = true;
                                gd.CurrentLink = regexMatches[0].Value;
                                return false;
                            }
                        }

                        stringRegEx = "(?<=var swf = \")[^\"]+";
                        regexMatches = new Regex(stringRegEx).Matches(stringHtml);
                        tempRegex2 = new Regex("(?<=<title>[^<]+Play )[^<]+(?=, a FREE online game)", RegexOptions.Singleline);
                        if (regexMatches.Count > 0)
                        {
                            tempUri = UriAbsolute(regexMatches[0].Value, stringURL);
                            gd.FileList.Add(new GameData.FileData()
                            {
                                Link = tempUri.AbsoluteUri,
                                OriginalName = tempUri.Segments.Last(), //Извлекаем оригинальное имя файла
                                CorrectName = NameFix(tempRegex2.Match(stringHtml).Value) //Правильное имя файла игры
                            });
                            /*
                            gd.LinkList.Add(tempUri.AbsoluteUri);
                            gd.OriginalNamesList.Add(tempUri.Segments.Last()); //Извлекаем оригинальное имя файла
                            gd.CorrectName = tempRegex2.Match(stringHtml).Value; //Правильное имя файла игры
                            gd.CorrectNamesList.Add(gd.CorrectName.Replace(": ", " - "));*/
                            gd.Reset();
                        }
                        break;
                    }
                    #endregion
                case SiteNameEnum.Gamesfreecom:
                    #region gamesfree.com
                    {
                        tempRegex1 = new Regex("(?<=<!-- Flash Game -->[^!]+<param name=\"movie\" value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>)[^<]+(?=, a free online)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Box10:
                    #region box10.com
                    {
                        tempRegex1 = new Regex("(?<=<!-- Game -->[^!]+<param name=\"movie\" value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>)[^<]+(?= - Free Games)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Belugerin:
                    #region belugerinstudios.com
                    {
                        tempRegex1 = new Regex("(?<=id=\"flashplay\" src=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>)[^\\|]+(?= \\|)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Funnygames:
                    #region funnygames.ru
                    {
                        tempRegex1 = new Regex("(?<=<param name=\"movie\" value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>&#1048;&#1075;&#1088;&#1072; )[^<]+(?= - FunnyGames)");
                        tempName = ConvertFromCodePoint(tempRegex2.Match(stringHtml).Value);
                        FindCorrectName = false;
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Arcadetown:
                    #region arcadetown.com
                    {
                        tempRegex1 = new Regex("(?<=<param name=[\"\' ]*movie[\"\' ]* value=[\"\' ]*)" + swfRegex, RegexOptions.IgnoreCase);
                        tempRegex2 = new Regex("(?<=title>)[^<]+(?= - Arcade Town)");
                        goto case SiteNameEnum.Common;
                    }
                #endregion
                case SiteNameEnum.Arcadebomb:
                    #region arcadebomb.com
                    {
                        tempRegex1 = new Regex("(?<=id=\"flashgame\"[^!]+<param name=\"movie\" value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>)[^<]+(?= , free online)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Crazymonkeygames:
                    #region crazymonkeygames.com
                    {
                        tempRegex1 = new Regex("(?<=<param name=\"movie\" value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>)[^<]+(?= and Other Free)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Notdoppler:
                    #region notdoppler.com
                    {
                        tempRegex1 = new Regex("(?<=<param name=movie value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>)[^<]+(?= - Play it on)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Gamesfreeca:
                    #region gamesfree.ca
                    {
                        tempRegex1 = new Regex("(?<=id=\"gamecontent\"[^>]+data=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>Play[ ]+)[^<]+(?= - Game - Free)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Likwidgames:
                    #region likwidgames.com
                    {
                        tempRegex1 = new Regex("(?<=swfobject.embedSWF\\(\")[^\"]+");
                        tempRegex2 = new Regex("(?<=title>Play[ ]+)[^<]+(?= at Likwid)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion                
                case SiteNameEnum.Barbariangames:
                    #region barbarian-games.com
                    {
                        tempRegex1 = new Regex("(?<=<param name=\"movie\" value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>)[^<]+(?= - Barbarian)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Twotowersgames:
                    #region Twotowersgames
                    {
                        tempRegex1 = new Regex("(?<=twotowersgames.com/games/)[^/]+");
                        tempRegex2 = new Regex("(?<=h1[^>]*>)[^<]+");
                        regexMatches = tempRegex2.Matches(stringHtml);
                        if(!tempRegex1.IsMatch(stringURL) | regexMatches.Count == 0) return false;
                        String temp = tempRegex1.Match(stringURL).Value;
                        gd.FileList.Add(new GameData.FileData()
                        {
                            Link = "http://twotowersgames.com/game/" + temp + "/" + temp + ".swf",
                            OriginalName = temp + ".swf", //Извлекаем оригинальное имя файла
                            CorrectName = NameFix(regexMatches[0].Value) //Правильное имя файла игры
                        });
                        gd.Reset();
                        break;
                    }
                    #endregion
                case SiteNameEnum.Stickpage:
                    #region stickpage.com
                    {
                        tempRegex1 = new Regex("(?<=<param name=\"movie\" value=\")" + swfRegex);
                        tempRegex2 = new Regex("(?<=title>Play )[^<]+(?= - Stick)");
                        goto case SiteNameEnum.Common;
                    }
                    #endregion
                case SiteNameEnum.Other:
                    #region Other
                    {
                        /****Поиск файла иным способом****/
                        stringRegEx = "(?<=<param name=[\"\' ]*movie[\"\' ]* value=[\"\' ]*)" + swfRegex;
                        regexMatches = new Regex(stringRegEx, RegexOptions.IgnoreCase).Matches(stringHtml);
                        if (regexMatches.Count > 0)
                        {
                            foreach (Match M in regexMatches)
                            {
                                tempUri = UriAbsolute(M.Value, stringURL);
                                gd.FileList.Add(new GameData.FileData()
                                {
                                    Link = tempUri.AbsoluteUri,
                                    OriginalName = tempUri.Segments.Last(), //Извлекаем оригинальное имя файла
                                    CorrectName = NameFix(tempUri.Segments.Last(), true) //Правильное имя файла игры
                                });
                            }
                        }
                        stringRegEx = "(?<=(embedSWF|new SWFObject)\\(\")[^\"]+";
                        regexMatches = new Regex(stringRegEx, RegexOptions.IgnoreCase).Matches(stringHtml);
                        if (regexMatches.Count > 0)
                        {
                            foreach (Match M in regexMatches)
                            {
                                tempUri = UriAbsolute(M.Value, stringURL);
                                gd.FileList.Add(new GameData.FileData()
                                {
                                    Link = tempUri.AbsoluteUri,
                                    OriginalName = tempUri.Segments.Last(), //Извлекаем оригинальное имя файла
                                    CorrectName = NameFix(tempUri.Segments.Last(), true) //Правильное имя файла игры
                                });
                            }
                        }
                        stringRegEx = "(?<=<script type[ ]*=[\"\' ]*text/javascript[\"\' ]*>[^<>]+)" + swfRegex + "(?=[^<>]+</script>)";
                        regexMatches = new Regex(stringRegEx, RegexOptions.Singleline).Matches(stringHtml);
                        if (regexMatches.Count > 0)
                        {
                            foreach (Match M in regexMatches)
                            {
                                tempUri = UriAbsolute(M.Value, stringURL);
                                gd.FileList.Add(new GameData.FileData()
                                {
                                    Link = tempUri.AbsoluteUri,
                                    OriginalName = tempUri.Segments.Last(), //Извлекаем оригинальное имя файла
                                    CorrectName = NameFix(tempUri.Segments.Last(), true) //Правильное имя файла игры
                                });
                            }
                        }
                        stringRegEx = "(?<=\")http" + swfRegex + "(?=\")";
                        regexMatches = new Regex(stringRegEx, RegexOptions.Singleline).Matches(stringHtml);
                        if (regexMatches.Count > 0)
                        {
                            foreach (Match M in regexMatches)
                            {
                                tempUri = UriAbsolute(M.Value, stringURL);
                                gd.FileList.Add(new GameData.FileData()
                                {
                                    Link = tempUri.AbsoluteUri,
                                    OriginalName = tempUri.Segments.Last(), //Извлекаем оригинальное имя файла
                                    CorrectName = NameFix(tempUri.Segments.Last(), true) //Правильное имя файла игры
                                });
                            }
                        }
                        gd.Reset();
                        if (gd.FileList.Count == 0) return false;
                        else break;
                    }
                    #endregion
            }
            return true;
        }

        public static String ConvertFromCodePoint(String str)
        {
            return Regex.Replace(str, "&#[0-9]{4};", Evaluator);
        }
        private static String Evaluator(Match match)
        {
            return ((char)int.Parse(match.Value.Substring(2,4), System.Globalization.NumberStyles.Integer)).ToString();
        }
        public static String NameFix(String str, Boolean correctName = false)
        {
            if (str == null) return null;
            if (str.Length != 0)
            {
                if (correctName)
                {
                    str = str.Replace("_", " ").Replace("-", " ").Replace(": ", " - ");
                    Char[] temp = str.ToCharArray();
                    temp[0] = Char.ToUpper(temp[0]);
                    for (int i = 1; i < str.Length; i++)
                        if (temp[i - 1] == ' ') temp[i] = Char.ToUpper(temp[i]);
                    return new String(temp);
                }
                else return str.Replace(": ", " - ");
            }
            else return str;
        }
        public static String UrlConvert(String str)
        {
            if (str == null) return str;
            return str.Replace("%2F", "/").Replace("%3A", ":").Replace("\\", "");
        }
        public static String ConvertAllLinksToAbsolute(String originalHtml, String Host)
        {
            var baseUri = new Uri(Host);
            var pattern = "(?<name>src|href|value)=\"(?<value>[^\"]*)\"";
            var matchEvaluator = new MatchEvaluator(
                match =>
                {
                    var value = match.Groups["value"].Value;
                    Uri uri;

                    if (Uri.TryCreate(baseUri, value, out uri))
                    {
                        var name = match.Groups["name"].Value;
                        return string.Format("{0}=\"{1}\"", name, uri.AbsoluteUri);
                    }

                    return null;
                });
            return Regex.Replace(originalHtml, pattern, matchEvaluator);
        }
        public static Uri UriAbsolute(String Link, String Host, Boolean fromHtml = true)
        {
            Link = UrlConvert(Link); Host = UrlConvert(Host);
            Uri baseUri, uriResult;
            Boolean result;
            if (!Link.Contains("http://") & !Link.Contains("https://") & !Link.Contains("ftp://"))
            {
                if (fromHtml)
                {
                    if (Host == null) return null;
                    else
                    {
                        if (!Host.Contains("http://") & !Host.Contains("https://") & !Host.Contains("ftp://"))
                            Host = "http://" + Host;
                        result = Uri.TryCreate(Host, UriKind.Absolute, out baseUri);
                        if (!result) return null;
                        result = Uri.TryCreate(new Uri(baseUri.Scheme + "://" + baseUri.Host, UriKind.Absolute), Link, out uriResult);
                        if (result) return uriResult;
                        return null;
                    }
                }
                Link = "http://" + Link;
            }
            result = Uri.TryCreate(Regex.Replace(Link, "^[/]{2,}", "http://"), UriKind.Absolute, out uriResult);
            if (result) return uriResult;
            return null;
        }        
        

        public static String FileSizeToString(long byteCount)
        {
            string[] suf = { " Б", " КБ", " МБ", " ГБ", " ТБ", " ПБ"};
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        public static void AppendIframe(ref String stringHtml)
        {
            String stringRegEx = "(?<=<iframe[^>]+src=\")[^\"]+";
            MatchCollection iframeMatches = new Regex(stringRegEx).Matches(stringHtml);
            HttpWebRequest request = null;
            Uri requestUri;
            StringBuilder sb = new StringBuilder(stringHtml);
            if (iframeMatches.Count > 0)
            {
                String[] iframes = new String[iframeMatches.Count];
                for (int i = 0; i < iframeMatches.Count; i++)
                {
                    requestUri = UriAbsolute(iframeMatches[i].Value, null, false);
                    if(!Uri.TryCreate(Regex.Replace(iframeMatches[i].Value, "^//", "http://"), UriKind.Absolute, out requestUri)) 
                        continue;
                    request = (HttpWebRequest)WebRequest.Create(requestUri);
                    try
                    {
                        if (!UsingProxy) request.Proxy = null;
                        request.Method = "Get";
                        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36";
                        iframes[i] = (new StreamReader(((HttpWebResponse)request.GetResponse()).GetResponseStream())).ReadToEnd();
                    }
                    catch (WebException g)
                    {
                        //MessageBox.Show(g.Message);
                        continue;
                    }
                    sb.Append(ConvertAllLinksToAbsolute(iframes[i], iframeMatches[i].Value));
                }
            }
            stringHtml = sb.ToString();
        }
    }
    
    public class GameData
    {
        public class FileData
        {
            public String Link, OriginalName, CorrectName;
            public Nullable<Int32> FileSize;
            public FileData()
            {
                Link = OriginalName = CorrectName = "";
                FileSize = null;
            }
        }
        //public List<Nullable<UInt64>> FileSizes;
        public Int32 CurrentIndex = 0;
        public String CurrentLink
        {
            get { return FileList[CurrentIndex].Link; }
            set { FileList[CurrentIndex].Link = value; }
        }
        public String CurrentOriginalName
        {
            get { return FileList[CurrentIndex].OriginalName; }
            set { FileList[CurrentIndex].OriginalName = value; }
        }
        public String CurrentCorrectName
        {
            get { return FileList[CurrentIndex].CorrectName; }
            set { FileList[CurrentIndex].CorrectName = value; }
        }
        public Int32? CurrentFileSize
        {
            get { return FileList[CurrentIndex].FileSize; }
            set { FileList[CurrentIndex].FileSize = value; }
        }
        public SiteNameEnum Target;
        public Boolean ExternalLink;
        //public List<String> LinkList;
        //public List<String> OriginalNamesList;
        //public List<String> CorrectNamesList;
        public List<FileData> FileList;
        public GameData()
        {
            /*LinkList = new List<String>();
            FileSizes = new List<Nullable<UInt64>>();
            OriginalNamesList = new List<string>();
            CorrectNamesList = new List<string>();*/
            FileList = new List<FileData>();
            //Link = OriginalName = CorrectName = "";
            Target = SiteNameEnum.Other;
            ExternalLink = false;
            CurrentIndex = 0;
        }
        /*public void ChangeTo(Int32 Index)
        {
            if (FileList.Count < Index + 1 | Index < 0) return;
            CurrentLink = FileList[Index].Link;
            CorrectName = CorrectNamesList[Index];
            OriginalName = OriginalNamesList[Index];

        }*/
        public FileData this[int i]
        {
            get
            {
                if (i < 0 | i >= FileList.Count | FileList.Count == 0) return null;
                return FileList[i];
            }
            set
            {
                if (i >= 0 & i < FileList.Count)
                    FileList[i] = value;
            }
        }
        public void Reset()
        {
            /*LinkList = LinkList.Distinct().ToList();
            CorrectNamesList = CorrectNamesList.Distinct().ToList();
            OriginalNamesList = OriginalNamesList.Distinct().ToList();*/
            //ChangeTo(0);
            FileList = MoreEnumerable.DistinctBy(FileList, x => x.Link).ToList();
            CurrentIndex = 0;
        }
    }
    
}

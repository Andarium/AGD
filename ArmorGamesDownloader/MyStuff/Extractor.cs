using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using MoreLinq;
using ArmorGamesDownloader.Properties;
using ArmorGamesDownloader.Extensions;

namespace ArmorGamesDownloader
{
    public enum SiteNameEnum
    {
        [EnumExtension.StringValue("armorgames.com")]
        Armorgames,
        [EnumExtension.StringValue("kongregate.com")]
        Kongregate,
        [EnumExtension.StringValue("newgrounds.com")]
        Newgrounds,
        [EnumExtension.StringValue("twotowersgames.com")]
        Twotowersgames,
        [EnumExtension.StringValue("thepodge.co.uk")]
        Thepodge,
        [EnumExtension.StringValue("stickpage.com")]
        Stickpage,
        [EnumExtension.StringValue("likwidgames.com")]
        Likwidgames,
        [EnumExtension.StringValue("swartag.com")]
        Swartag,
        [EnumExtension.StringValue("jayisgames.com")]
        Jayisgames,
        [EnumExtension.StringValue("barbarian-games.com")]
        Barbariangames,
        [EnumExtension.StringValue("notdoppler.com")]
        Notdoppler,
        [EnumExtension.StringValue("gamesfree.ca")]
        Gamesfreeca,
        [EnumExtension.StringValue("crazymonkeygames.com")]
        Crazymonkeygames,
        [EnumExtension.StringValue("maxgames.com")]
        Maxgames,
        [EnumExtension.StringValue("arcadebomb.com")]
        Arcadebomb,
        [EnumExtension.StringValue("arcadetown.com")]
        Arcadetown,
        [EnumExtension.StringValue("flashgames247")]
        Flashgames247,
        [EnumExtension.StringValue("belugerinstudios.com")]
        Belugerin,
        [EnumExtension.StringValue("box10.com")]
        Box10,
        [EnumExtension.StringValue("gamesfree.com")]
        Gamesfreecom,
        [EnumExtension.StringValue("funnygames.ru")]
        Funnygames,
        [EnumExtension.StringValue("!@#$%^&*()_+=-")]
        Other,
        [EnumExtension.StringValue("!@#$%^&*()_+=-")]
        Common
    };
    
    public static class Extractor
    {
        #region Variables
        public static Boolean UsingProxy = false;
        private static Regex swfRegex = new Regex(Resources.strRegexSwf);
        private static Regex tempRegex1, tempRegex2;
        private static Uri tempUri;        
        private static Boolean FindCorrectName = true;
        private static String tempName = "";
        #endregion

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
                                        CorrectName = FixGameName((FindCorrectName) ? tempRegex2.Match(stringHtml).Value : tempName) //Правильное имя файла игры
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
                                gd.FileList.Add(new GameData.FileData()
                                    {
                                        Link = "http://www.flashgames247.com/" + r.Match(stringHtml).Value
                                    });
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
                                    CorrectName = FixGameName(tempRegex2.Match(stringHtml).Value) //Правильное имя файла игры
                                });
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
                                CorrectName = FixGameName(tempRegex2.Match(stringHtml).Value) //Правильное имя файла игры
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
                            CorrectName = FixGameName(regexMatches[0].Value) //Правильное имя файла игры
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
                                    CorrectName = FixGameName(tempUri.Segments.Last(), true) //Правильное имя файла игры
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
                                    CorrectName = FixGameName(tempUri.Segments.Last(), true) //Правильное имя файла игры
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
                                    CorrectName = FixGameName(tempUri.Segments.Last(), true) //Правильное имя файла игры
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
                                    CorrectName = FixGameName(tempUri.Segments.Last(), true) //Правильное имя файла игры
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

        private static String Evaluator(Match match)
        {
            //Ужас, не правда ли? =)
            return ((char)int.Parse(match.Value.Substring(2, 4), System.Globalization.NumberStyles.Integer)).ToString();
        }
        public static String ConvertFromCodePoint(String str)
        {
            return Regex.Replace(str, "&#[0-9]{4};", Evaluator);
        }
        
        public static String FixGameName(String str, Boolean correctName = false)
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
        public static String PrepareUrl(String stringUrl)
        {
            //if (stringUrl.Contains("twotowersgames")) stringUrl = stringUrl.Replace("/games/", "/embed/").Replace("preplay", "");

            if (stringUrl.Contains("stickpage.com") & !stringUrl.Contains("gameplay"))
                stringUrl = stringUrl.Replace("game", "gameplay");

            else if (stringUrl.Contains("belugerinstudios"))
                stringUrl = stringUrl.Replace("playgame&", "playgames&").Replace("&name=", "&game=");

            else if (stringUrl.Contains("maxgames.com/game/")) 
                stringUrl = stringUrl.Replace("/game/", "/play/");

            else if (stringUrl.Contains("arcadetown.com") & !stringUrl.Contains("playiframe")) 
                stringUrl = Regex.Replace(stringUrl, "(?<=arcadetown\\.com/[^/]+/).*", "playiframe.asp");

            else if (stringUrl.Contains("gamesfree.ca") & !stringUrl.Contains("/play/")) 
                stringUrl = Regex.Replace(stringUrl, "gamesfree.ca/game/[0-9]+/", x => x + "play/");

            return stringUrl;
        }
        public static String ConvertUrl(String str)
        {
            if (str == null) return str;
            return str.Replace("%2F", "/").Replace("%3A", ":").Replace("\\", "");
        }
        public static String MakeLinksAbsolute(String originalHtml, String Host)
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
            Link = ConvertUrl(Link); 
            Host = ConvertUrl(Host);
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
        

        public static String GetSizeString(long byteCount)
        {
            string[] suf = { " Б", " КБ", " МБ", " ГБ", " ТБ", " ПБ"};
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
        public static String GetSizeString(long byteCount, int suffix, bool showSuffix = true)
        {
            string[] suf = { " Б", " КБ", " МБ", " ГБ", " ТБ", " ПБ" };
            if (byteCount == 0)
                return (showSuffix) ? "0" + suf[suffix] : "0";
            long bytes = Math.Abs(byteCount);
            int place = suffix;// Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 2);
            return (Math.Sign(byteCount) * num).ToString() + (showSuffix ? suf[place] : "");
        }

        public static void AppendIframes(ref String stringHtml)
        {
            MatchCollection iframeMatches = new Regex(Resources.strRegexIframe).Matches(stringHtml);
            if (iframeMatches.Count == 0) return;

            Uri requestUri;
            HttpWebRequest request;            
            StringBuilder sb = new StringBuilder();
            String[] iframes = new String[iframeMatches.Count];

            for (int i = 0; i < iframeMatches.Count; i++)
            {
                requestUri = UriAbsolute(iframeMatches[i].Value, null, false);
                if (!Uri.TryCreate(Regex.Replace(iframeMatches[i].Value, "^//", "http://"), UriKind.Absolute, out requestUri))
                    continue;
                request = (HttpWebRequest)WebRequest.Create(requestUri);
                try
                {
                    if (!UsingProxy) request.Proxy = null;
                    request.Method = "Get";
                    request.UserAgent = Resources.strUserAgent;
                    iframes[i] = new StreamReader((request.GetResponse()).GetResponseStream()).ReadToEnd();
                }
                catch (WebException g)
                {
                    //MessageBox.Show(g.Message);
                    continue;
                }
                sb.Append(MakeLinksAbsolute(iframes[i], iframeMatches[i].Value));
            }
            stringHtml += sb.ToString();
        }
    }
    
    
    
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Forms;

namespace ArmorGamesDownloader
{
    public enum ErrorStateEnum { NoError, UrlFormatError, ConnectionError, FileNotFound, Inception };
    public enum SiteNameEnum {
        Armorgames, Kongregate, Newgrounds, Twotowersgames,
        Thepodge, Stickpage, Likwidgames, Swartag, Jayisgames, Barbariangames, Notdoppler, Gamesfreeca, Crazymonkeygames, Maxgames,
        Arcadebomb, Arcadetown, Flashgames247, Box10, Belugerin, Gamesfreecom, Funnygames, 
        Other, Common 
    };

	public partial class frmMain : Form
	{
        private void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = null;
            HttpWebRequest request = null;
            String stringHtml = "";
            Uri tempUri;
            GD = new GameData();
            ErrorState = ErrorStateEnum.NoError;
            bgWorker.ReportProgress(10);

            if (stringUrl.EndsWith(".swf", StringComparison.Ordinal))
            {
                tempUri = Extractor.UriAbsolute(stringUrl, stringUrl, false);
                bgWorker.ReportProgress(100);
                e.Result = stringUrl;
                GD.FileList.Add(new GameData.FileData()
                {
                    Link = tempUri.AbsoluteUri,
                    OriginalName = tempUri.Segments.Last(), //Извлекаем оригинальное имя файла
                    CorrectName = Extractor.NameFix(tempUri.Segments.Last(), true) //Правильное имя файла игры
                });
                return;
            }
            else
            {
                //if (stringUrl.Contains("twotowersgames")) stringUrl = stringUrl.Replace("/games/", "/embed/").Replace("preplay", "");
                /*else*/ 
                if (stringUrl.Contains("stickpage.com") & !stringUrl.Contains("gameplay")) stringUrl = stringUrl.Replace("game", "gameplay");
                else if (stringUrl.Contains("belugerinstudios")) stringUrl = stringUrl.Replace("playgame&", "playgames&").Replace("&name=", "&game=");
                else if (stringUrl.Contains("maxgames.com/game/")) stringUrl = stringUrl.Replace("/game/", "/play/");
                else if (stringUrl.Contains("arcadetown.com") & !stringUrl.Contains("playiframe")) stringUrl = Regex.Replace(stringUrl, "(?<=arcadetown\\.com/[^/]+/).*", "playiframe.asp");
                else if (stringUrl.Contains("gamesfree.ca") & !stringUrl.Contains("/play/")) stringUrl = Regex.Replace(stringUrl, "gamesfree.ca/game/[0-9]+/", x => x + "play/"); 
                /*************************Шаг первый - создание запроса*************************/
                #region Querry Create
                tempUri = Extractor.UriAbsolute(stringUrl, stringUrl, false);
                if(tempUri == null)
                {
                    ErrorState = ErrorStateEnum.UrlFormatError;
                    bgWorker.ReportProgress(100);
                    bgWorker.CancelAsync();
                    return;
                }
                tbUrl.Invoke((Action)(() => tbUrl.Text = tempUri.AbsoluteUri));
                request = (HttpWebRequest)WebRequest.Create(tempUri);
                bgWorker.ReportProgress(20);
                #endregion
                /*************************Шаг второй - попытка соединения***********************/
                #region Connection
                Boolean google = stringUrl.Contains("google");
                do
                {
                    try
                    {
                        if (!Extractor.UsingProxy) request.Proxy = null;
                        request.Method = "Get";
                        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36";
                        stringHtml = (new StreamReader(((HttpWebResponse)request.GetResponse()).GetResponseStream())).ReadToEnd();
                    }
                    catch (WebException g)
                    {
                        MessageBox.Show(g.Message);
                        ErrorState = ErrorStateEnum.ConnectionError;
                        bgWorker.ReportProgress(100);
                        bgWorker.CancelAsync();
                        return;
                    }
                    google = stringUrl.Contains("google");
                    if (google)
                    {
                        if (stringHtml.Contains("URL='http://"))
                        {
                            //stringUrl = stringHtml.Remove(stringHtml.IndexOf("'\"></noscript>")).Substring(stringHtml.IndexOf("URL='") + 5);
                            stringUrl = new Regex("(?<=URL=')[^']+").Match(stringHtml).Value;
                            request = (HttpWebRequest)WebRequest.Create(stringUrl);
                        }
                    }

                } while (google);

                bgWorker.ReportProgress(60);
                #endregion
                /*************************Шаг третий - определение типа**************************/
                #region Type detection
                if (stringUrl.Contains("armorgames.com") | stringUrl.Contains("armor.ag")) GD.Target = SiteNameEnum.Armorgames;
                else if (stringUrl.Contains("kongregate.com")) GD.Target = SiteNameEnum.Kongregate;
                else if (stringUrl.Contains("newgrounds.com")) GD.Target = SiteNameEnum.Newgrounds;
                else if (stringUrl.Contains("twotowersgames.com")) GD.Target = SiteNameEnum.Twotowersgames;
                else if (stringUrl.Contains("thepodge.co.uk")) GD.Target = SiteNameEnum.Thepodge;
                else if (stringUrl.Contains("stickpage.com")) GD.Target = SiteNameEnum.Stickpage;
                else if (stringUrl.Contains("likwidgames.com")) GD.Target = SiteNameEnum.Likwidgames;
                else if (stringUrl.Contains("swartag.com")) GD.Target = SiteNameEnum.Swartag;
                else if (stringUrl.Contains("jayisgames.com")) GD.Target = SiteNameEnum.Jayisgames;
                else if (stringUrl.Contains("barbarian-games.com")) GD.Target = SiteNameEnum.Barbariangames;
                else if (stringUrl.Contains("notdoppler.com")) GD.Target = SiteNameEnum.Notdoppler;
                else if (stringUrl.Contains("gamesfree.ca")) GD.Target = SiteNameEnum.Gamesfreeca;
                else if (stringUrl.Contains("crazymonkeygames.com")) GD.Target = SiteNameEnum.Crazymonkeygames;
                else if (stringUrl.Contains("maxgames.com")) GD.Target = SiteNameEnum.Maxgames;
                else if (stringUrl.Contains("arcadebomb.com")) GD.Target = SiteNameEnum.Arcadebomb;
                else if (stringUrl.Contains("arcadetown.com")) GD.Target = SiteNameEnum.Arcadetown;
                else if (stringUrl.Contains("flashgames247")) GD.Target = SiteNameEnum.Flashgames247;
                else if (stringUrl.Contains("belugerinstudios.com")) GD.Target = SiteNameEnum.Belugerin;
                else if (stringUrl.Contains("box10.com")) GD.Target = SiteNameEnum.Box10;
                else if (stringUrl.Contains("gamesfree.com")) GD.Target = SiteNameEnum.Gamesfreecom;
                else if (stringUrl.Contains("funnygames.ru")) GD.Target = SiteNameEnum.Funnygames;
                else GD.Target = SiteNameEnum.Other;

                /*************************Доп. шаг - добавить iframe, если есть*****************/
                Extractor.AppendIframe(ref stringHtml);
                /*************************Последний шаг, он легкий самый =)*********************/
                if (Extractor.Extract(ref GD, stringHtml, stringUrl))
                {
                    #region Запрос размера файла
                    foreach(GameData.FileData f in GD.FileList)
                    {
                        try
                        {
                            request = (HttpWebRequest)WebRequest.Create(f.Link);
                            request.Method = "HEAD";
                            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36";
                            using (System.Net.WebResponse response = request.GetResponse())
                            {
                                int ContentLength;
                                if (int.TryParse(response.Headers.Get("Content-Length"), out ContentLength))
                                {
                                    f.FileSize = ContentLength;
                                }
                                else f.FileSize = null;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    #endregion
                    bgWorker.ReportProgress(100);
                    e.Result = GD;
                }
                else
                {
                    if (GD.ExternalLink)
                        ErrorState = ErrorStateEnum.Inception;
                    else 
                        ErrorState = ErrorStateEnum.FileNotFound;
                    bgWorker.ReportProgress(100);
                }
                #endregion
            }
        }

	}

}
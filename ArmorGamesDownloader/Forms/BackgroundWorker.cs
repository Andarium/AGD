using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using ArmorGamesDownloader.Properties;
using ArmorGamesDownloader.Extensions;

namespace ArmorGamesDownloader
{
    public enum ErrorStateEnum 
    { 
        NoError, UrlFormatError, ConnectionError, FileNotFound, Inception 
    };

    public partial class frmMain : System.Windows.Forms.Form
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
                    CorrectName = Extractor.FixGameName(tempUri.Segments.Last(), true) //Правильное имя файла игры
                });
                return;
            }
            else
            {
                /*************************Шаг нулевой - обработка ссылки************************/
                stringUrl = Extractor.PrepareUrl(stringUrl);
                /*************************Шаг первый - создание запроса*************************/
                #region Querry Create
                tempUri = Extractor.UriAbsolute(stringUrl, stringUrl, false);
                if (tempUri == null)
                {
                    ErrorState = ErrorStateEnum.UrlFormatError;
                    bgWorker.ReportProgress(100);
                    bgWorker.CancelAsync();
                    return;
                }
                tbUrl.Invoke((Action)(() => tbUrl.Text = tempUri.AbsoluteUri)); //Надеюсь, никто этого не увидит :D
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
                        request.UserAgent = Resources.strUserAgent;
                        stringHtml = (new StreamReader(((HttpWebResponse)request.GetResponse()).GetResponseStream())).ReadToEnd();
                    }
                    catch (WebException g)
                    {
                        System.Windows.Forms.MessageBox.Show(g.Message);
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
                            stringUrl = new Regex("(?<=URL=')[^']+").Match(stringHtml).Value;
                            request = (HttpWebRequest)WebRequest.Create(stringUrl);
                        }
                    }

                } while (google);

                bgWorker.ReportProgress(60);
                #endregion
                /*************************Шаг третий - определение типа**************************/
                #region Type detection

                if (stringUrl.Contains("armor.ag")) GD.Target = SiteNameEnum.Armorgames;
                else
                {
                    GD.Target = SiteNameEnum.Other;
                    foreach (SiteNameEnum site in (SiteNameEnum[])Enum.GetValues(typeof(SiteNameEnum)))
                    {
                        if(stringUrl.Contains(site.GetStringValue()))
                        {
                            GD.Target = site;
                            break;
                        }
                    }                    
                }
                #endregion
                /*************************Доп. шаг - добавить iframe, если есть*****************/
                Extractor.AppendIframes(ref stringHtml);
                /*************************Последний шаг, он легкий самый =)*********************/
                if (Extractor.Extract(ref GD, stringHtml, stringUrl))
                {
                    #region Запрос размера файла
                    foreach (GameData.FileData f in GD.FileList)
                    {
                        try
                        {
                            request = (HttpWebRequest)WebRequest.Create(f.Link);
                            request.Method = "HEAD";
                            request.UserAgent = Resources.strUserAgent;
                            using (System.Net.WebResponse response = request.GetResponse())
                            {
                                Int64 ContentLength;
                                f.FileSize = (Int64.TryParse(response.Headers.Get("Content-Length"), out ContentLength))
                                    ? ContentLength
                                    : (Int64?)null;
                            }
                        }
                        catch
                        {
                            continue; //Ага, просто переходим дальше xD
                        }
                    }
                    #endregion
                    bgWorker.ReportProgress(100);
                    e.Result = GD;
                }
                else
                {
                    ErrorState = (GD.ExternalLink) 
                        ? ErrorStateEnum.Inception 
                        : ErrorStateEnum.FileNotFound;
                    bgWorker.ReportProgress(100);
                }
            }
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            if (e.Result != null)
            {
                /*if (stringUrl.Contains("twotowersgames")) tbOutput.Text = (e.Result.ToString()).Replace("-exclusive", "");
                else tbOutput.Text = e.Result.ToString();*/
                cbOutput.Items.Clear();
                if (GD.FileList.Count > 0)
                {
                    cbOutput.Items.AddRange(GD.FileList.Select(x => x.Link).ToArray());
                    cbOutput.SelectedIndex = 0;
                    saveDialog.FileName = GD.CurrentOriginalName;
                }
            }
            SetEnableState();
            
            if (GD.ExternalLink)
            {
                tbUrl.Text = GD.CurrentLink;
                btnStart.PerformClick();
            }
        }
        private void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 10:
                    lblStatus.Text = "Соединение...";
                    break;
                case 60:
                    lblStatus.Text = "Соединение успешно. Поиск файла...";
                    break;
                case 80:
                    lblStatus.Text = "Файл найден.";
                    break;
                case 100:
                    {
                        switch(ErrorState)
                        {
                            default:
                            case ErrorStateEnum.NoError:
                                lblStatus.Text = "Файл найден.";
                                break;
                            case ErrorStateEnum.ConnectionError:
                                lblStatus.Text = "Ошибка соединения.";
                                break;                            
                            case ErrorStateEnum.UrlFormatError:
                                lblStatus.Text = "Ошибка. Неверный формат URL.";
                                break;
                            case ErrorStateEnum.Inception:
                                lblStatus.Text = "Найдена внешняя ссылка.";
                                break;
                            case ErrorStateEnum.FileNotFound:
                                {
                                    if (GD.Target == SiteNameEnum.Kongregate)
                                        lblStatus.Text = "Файл игры не найден. Вероятно, что это MMO или 3D игра (Kongregate).";
                                    else if (GD.Target == SiteNameEnum.Arcadetown)
                                        lblStatus.Text = "Файл игры не найден. Вероятно, что игра платная (Arcadetown)";
                                    else
                                        lblStatus.Text = "На данной странице .swf файл не найден, проверьте введенный адрес.";
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    lblStatus.Text = "";
                    break;
            }
            StatusStrip.Update();
        }

    }


}
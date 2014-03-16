using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
//using System.Text;

namespace ArmorGamesDownloader
{
    public class GameData
    {
        public SiteNameEnum Target;
        public Boolean ExternalLink;
        public List<FileData> FileList;

        public class FileData
        {
            public String Link, OriginalName, CorrectName;
            public Nullable<Int64> FileSize;
            public FileData()
            {
                Link = OriginalName = CorrectName = "";
                FileSize = null;
            }
        }
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
        public Int64? CurrentFileSize
        {
            get { return FileList[CurrentIndex].FileSize; }
            set { FileList[CurrentIndex].FileSize = value; }
        }
        
        public GameData()
        {
            FileList = new List<FileData>();
            Target = SiteNameEnum.Other;
            ExternalLink = false;
            CurrentIndex = 0;
        }
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
            FileList = MoreEnumerable.DistinctBy(FileList, x => x.Link).ToList();
            CurrentIndex = 0;
        }
    }
}

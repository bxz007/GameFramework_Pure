//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public partial class ProcedureMenu : ProcedureBase
    {
        #region Test 下载
        private void InitTest()
        {
            InitDownLoad();
            //测试DR数据
            IDataTable<DRTestDataTable> dt = GameEntry.DataTable.GetDataTable<DRTestDataTable>();
            DRTestDataTable dr = dt.GetDataRow(10000);

            //测试定时器
            GameEntry.TimerManager.AddTimer<int>(TestTimer, 10, 0.5f, 1, 10);
        }

        private void TestTimer(int TestInt)
        {
            Log.Info("定时器"+ TestInt);
        }

        private void TestUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        private void ShutdownTest()
        {
            ShutdownDownLoad();
        }

        #region 下载
        private void InitDownLoad()
        {
            GameEntry.Event.Subscribe(DownloadUpdateEventArgs.EventId, OnDownLoadUpdate);
            GameEntry.Event.Subscribe(DownloadSuccessEventArgs.EventId, OnDownloadSuccess);
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebrequestSuc);
            StartDownLoad();
        }

        private void ShutdownDownLoad()
        {
            GameEntry.Event.Unsubscribe(DownloadUpdateEventArgs.EventId, OnDownLoadUpdate);
            GameEntry.Event.Unsubscribe(DownloadSuccessEventArgs.EventId, OnDownloadSuccess);
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebrequestSuc);
        }

        private List<string> waitLoadFiles = new List<string>();
        private void OnWebrequestSuc(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = e as WebRequestSuccessEventArgs;
            byte[] bytes = ne.GetWebResponseBytes();
            string text = Encoding.UTF8.GetString(bytes);
            waitLoadFiles = GetFileList(text);
            //开始下载文件
            if (waitLoadFiles.Count > 0)
                DownLoadFile(waitLoadFiles[0]);
        }
        private void OnDownloadSuccess(object sender, GameEventArgs e)
        {
            DownloadSuccessEventArgs ne = e as DownloadSuccessEventArgs;
            if (ne.UserData == null)
                return;
            Log.Debug("下载了"+ ne.UserData.ToString());
            waitLoadFiles.Remove(ne.UserData.ToString());
            if (waitLoadFiles.Count == 0)
            {
                Log.Debug("下载完成了");
            }
            else
            {
                DownLoadFile(waitLoadFiles[0]);
            }
        }

        //Test 下载东西

        private const string downPath = "http://10.10.3.212/Platform_Windows/DownLoadTest/";
        private const string downLoadPath = "E:/TestDownLoad/";
        private void StartDownLoad()
        {
            if (!Directory.Exists(downLoadPath))
            {
                Directory.CreateDirectory(downLoadPath);
            }
            //1,如果上次没下载完 会自动续传
            //2,单文件下载 
            GameEntry.Download.AddDownload(GameFramework.Utility.Path.GetRegularPath(downLoadPath + "abc.mp4"), GameFramework.Utility.Path.GetRegularPath(downPath + "abc.mp4"),"TestDownLoad");
            //文件夹下载 最好有个数据表存起来  先拉取这个数据表   FileList.xml
            Log.Debug("开始下载");  
            GameEntry.WebRequest.AddWebRequest(GameFramework.Utility.Path.GetRegularPath(downPath +"/FileList.xml"));           
        }

        private void DownLoadFile(string filePath)
        {
            GameEntry.Download.AddDownload(GameFramework.Utility.Path.GetRegularPath(downLoadPath+filePath), GameFramework.Utility.Path.GetRegularPath(downPath+filePath),"fileList",filePath);
        }

        private void OnDownLoadUpdate(object sender, GameEventArgs e)
        {
            DownloadUpdateEventArgs ne = e as DownloadUpdateEventArgs;
            Log.Debug( "序号" + ne.SerialId + "  大小"+ne.CurrentLength/1024f);            
        }
        #endregion
        #region XML   仅仅为了测试      
        public List<string> GetFileList(string xmlText)
        {
            List<string> filePaths = new List<string>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlText);           
            XmlNode xmlRoot = xmlDocument.SelectSingleNode("FileList");
            XmlNode child = xmlRoot.FirstChild;
            while (child != null)
            {
                XmlElement xe = child as XmlElement;
                XmlNode cchild = xe.FirstChild;
                while (cchild != null)
                {
                    string value = cchild.Attributes.GetNamedItem("Name").Value;
                    filePaths.Add(value);
                    cchild = cchild.NextSibling;
                }
                child = child.NextSibling;
            }
            return filePaths;
        }

        #endregion
        #endregion
    }
}

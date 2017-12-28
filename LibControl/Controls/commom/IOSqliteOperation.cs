﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommLibrary;

/************************************************************************************
 * Copyright (c) 2016 All Rights Reserved.
 * CLR版本： 4.0.30319.18408
 *机器名称：PC120
 *公司名称：
 *命名空间：MatrixSystemEditor.commom
 *文件名：  IOSqliteOperation
 *版本号：  V1.0.0.0
 *唯一标识：3095a0c4-643d-485a-98cb-b4f4303c51bf
 *当前的用户域：SEIKAKU
 *创建人：  williamxia
 *电子邮箱：XXXX@sina.cn
 *创建时间：11/19/2016 2:18:02 PM
 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：11/19/2016 2:18:02 PM
 *修改人： williamxia
 *版本号： V1.0.0.0
 *描述：
 *
/************************************************************************************/
/*
        1:when close table ,first clear the database,then do save all date to datable
        2:when load read the data from database         
        */
namespace Lib.Controls
{
    class IOSqliteOperation
    {

        private string uDataBase = "sysdb.db"; //default is in the app file directory
        public SQLiteConnection m_dbConnection;


        public IOSqliteOperation(string dbName)
        {
            uDataBase = dbName;
            createDataBase();
            connectToDataBase();
            // createTables();
        }

        public IOSqliteOperation()
        {

            createDataBase();
            connectToDataBase();

        }

        public bool FileInUse(string strpath)
        {
            bool res = false;
            try
            {
                using (FileStream fs = new FileStream(strpath, FileMode.OpenOrCreate))
                {
                    res = !fs.CanWrite;
                }
                return false;
            }
            catch (IOException ex)
            {
                return true;

            }

        }

        private void createDataBase()
        {
            if (File.Exists(uDataBase))
            {
                //  Debug.WriteLine("database is exist ");
                if (FileInUse(uDataBase))
                {
                    //  MessageBox.Show("File is in used or has opened,you must close the file firstly!");
                    Environment.Exit(-1);
                }
            }
            else
            {
                //  Debug.WriteLine("database is not exist....");
                SQLiteConnection.CreateFile(uDataBase);
            }

        }
        private static string DBPwsd = "IOWilliamxia2016";
        private void connectToDataBase()
        {
            try
            {
                string strDBC = string.Format("Data Source={0};Version=3;Password={1}", uDataBase, DBPwsd);
                // string strDBC = string.Format("Data Source={0};Version=3;", uDataBase);
                m_dbConnection = new SQLiteConnection(strDBC);
                m_dbConnection.Open();
                m_dbConnection.ChangePassword(DBPwsd);
            }
            catch (Exception ec)
            {
                //   MessageBox.Show("Open dtabase error");
                Debug.WriteLine("connect to database error...............");
            }
        }

        private const string CDeviceTable = "DeviceModTable";

        private void createTable(string msql, string strTable)
        {
            //first check if exist
            //string.Format("create table if not exists {0}(name varchar(20),score int)"
            string sql = string.Format(msql, strTable);
            SQLiteCommand comand = new SQLiteCommand(sql, m_dbConnection);
            comand.ExecuteNonQuery();//insert or create table ,it is the non return type   

        }

        private const string ModuleInfoPosTb = "ModuleInfoTable";

        public void renewSaveModuleInfo(List<CDeviceInfo> devList)
        {
            createModuleInfoTable();
            saveToDevList(devList);
        }

        public void createModuleInfoTable()  //for save and renew table now
        {
            try
            {
                //create module position table

                //drop table first
                string sql = string.Format("drop table if exists {0}", ModuleInfoPosTb);
                SQLiteCommand comand = new SQLiteCommand(sql, m_dbConnection);
                comand.ExecuteNonQuery();//insert or create table ,it is the non return type 
                ///execute for create table
                sql = string.Format("create table if not exists {0}(moduleType int,devName varchar(20),noteTxt varchar(20),machineID int," +
                "devID int,devPtX float,devPtY float,linindex int )", ModuleInfoPosTb);
                comand = new SQLiteCommand(sql, m_dbConnection);
                comand.ExecuteNonQuery();//insert or create table ,it is the non return type 
            }
            catch (Exception e)
            {

            }

        }

        // Inserts some values in the highscores table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.
        public void saveToDevList(List<CDeviceInfo> devList)
        {
            if (devList == null) return;
            string sql = "";
            SQLiteCommand comnd = null;
            foreach (CDeviceInfo dev in devList)
            {

                sql = string.Format("insert into {0}(moduleType, devName,noteTxt,machineID,devID,devPtX,devPtY,linindex) values (?,?,?,?,?,?,?,?)", ModuleInfoPosTb);
                comnd = new SQLiteCommand(sql, m_dbConnection);
                comnd.Parameters.Add("moduleType", System.Data.DbType.Int16);
                comnd.Parameters["moduleType"].Value = dev.devModuleType;

                comnd.Parameters.Add("devName", System.Data.DbType.String);

                comnd.Parameters["devName"].Value = dev.devProv.strDevName;  //dev.devProv.pDeviceName;
                comnd.Parameters.Add("noteTxt", System.Data.DbType.String);
                comnd.Parameters["noteTxt"].Value = dev.noteTxt;

                comnd.Parameters.Add("machineID", System.Data.DbType.Int16);
                comnd.Parameters["machineID"].Value = dev.devProv.pMachineID;

                comnd.Parameters.Add("devID", System.Data.DbType.Int16);
                comnd.Parameters["devID"].Value = dev.devProv.pDeviceID;

                comnd.Parameters.Add("devPtX", System.Data.DbType.Double);
                comnd.Parameters["devPtX"].Value = dev.DevPoint.X;

                comnd.Parameters.Add("devPtY", System.Data.DbType.Double);
                comnd.Parameters["devPtY"].Value = dev.DevPoint.Y;

                comnd.Parameters.Add("linindex", System.Data.DbType.Int16);
                comnd.Parameters["linindex"].Value = dev.lineIndex;


                comnd.ExecuteNonQuery();
            }

        }

        public void readToDevList(List<CDeviceInfo> devList)
        {
            if (devList == null) return;
            devList.Clear(); //renew

            try
            {
                // connectToDataBase();
                string sql = string.Format("select * from {0}", ModuleInfoPosTb);
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                CDeviceInfo devi = null;
                while (reader.Read())
                {

                    //devicelist
                    devi = new CDeviceInfo();
                    devi.devModuleType = (Module_Type)reader["moduleType"];
                    int tmp = (int)reader["machineID"];
                    devi.devProv.pMachineID = (UInt16)tmp;
                    tmp = (int)reader["devID"];
                    devi.devProv.pDeviceID = (UInt16)tmp;

                    string strTmpDev = reader["devName"].ToString();
                    Debug.WriteLine("---------read from database devname is : " + strTmpDev);


                    devi.devProv.setStrDevName(strTmpDev);
                    devi.noteTxt = reader["noteTxt"].ToString();

                    devi.DevPoint.X = (double)reader["devPtX"];
                    devi.DevPoint.Y = (double)reader["devPtY"];
                    devi.lineIndex = (int)reader["linindex"];
                    devList.Add(devi);
                    devi.printDevInfo();
                }
            }
            catch (SQLiteException e)
            {


            }

        }



#if _refcence

        private void refreshTable()
        {
            string sql = string.Format("select * from {0} ", CLineTable);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            int rowNum = 0;
            /*
            if (reader.HasRows)
            {
                while (reader.Read())
                {                
                    Console.WriteLine("Name = ", reader["Name"]);
                    Console.WriteLine("Address = ", reader["score"]);
                }
            }
            */
            using (DataTable dt = new DataTable())
            {
                dt.Load(reader);
                rowNum = dt.Rows.Count;
            }
            reader.Close();

            Debug.WriteLine("table rows count is  {0}", rowNum);
            if (rowNum == 0) //use insert model
            {
                sql = string.Format("INSERT INTO {0}(name,score) VALUES(?,?)", CLineTable);
                command = new SQLiteCommand(sql, m_dbConnection);

                command.Parameters.Add("name", System.Data.DbType.String);
                command.Parameters["name"].Value = muserList[0].Name;

                command.Parameters.Add("score", System.Data.DbType.Int16);
                command.Parameters["score"].Value = muserList[0].Score;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            else //use modify update model
            {
                //modify
                sql = string.Format("UPDATE {0} set name=?,score=?", CLineTable);
                command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.Add("name", System.Data.DbType.String);
                command.Parameters["name"].Value = muserList[0].Name;
                //
                command.Parameters.Add("score", System.Data.DbType.Int16);
                command.Parameters["score"].Value = muserList[0].Score;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }


            }



        }
#endif

    }
}
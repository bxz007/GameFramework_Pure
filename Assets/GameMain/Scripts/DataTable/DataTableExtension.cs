﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class DataTableExtension
    {
        private const string DataRowClassPrefixName = "GameMain.DR";
        internal static readonly char[] DataSplitSeparators = new char[] { '\t' };
        internal static readonly char[] DataTrimSeparators = new char[] { '\"' };
        internal static readonly char[] DataArraySeparators = new char[] { ';' };
        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName, string dataTableAssetName, object userData)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            //string[] splitedNames = dataTableName.Split('_');
            //if (splitedNames.Length > 2)
            //{
            //    Log.Warning("Data table name is invalid.");
            //    return;
            //}

            //string dataRowClassName = DataRowClassPrefixName + splitedNames[0];
            //Type dataRowType = Type.GetType(dataRowClassName);
            //if (dataRowType == null)
            //{
            //    Log.Warning("Can not get data row type with class name '{0}'.", dataRowClassName);
            //    return;
            //}

            //判断是否带者路径 如果带了只取最后一个
            string[] splitNames = dataTableName.Split('/');

            string dataRowClassName = DataRowClassPrefixName + splitNames[splitNames.Length - 1];

            Type dataRowType = Type.GetType(dataRowClassName);
            if (dataRowType == null)
            {
                Log.Warning("Can not get data row type with class name '{0}'.", dataRowClassName);
                return;
            }

            splitNames = dataTableName.Split('_');
            if (splitNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string name = splitNames.Length > 1 ? splitNames[1] : null;
            DataTableBase dataTable = dataTableComponent.CreateDataTable(dataRowType, name);
            dataTable.ReadData(dataTableAssetName, Constant.AssetPriority.DataTableAsset, userData);
        }

        public static Color32 ParseColor32(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Color32(byte.Parse(splitedValue[0]), byte.Parse(splitedValue[1]), byte.Parse(splitedValue[2]), byte.Parse(splitedValue[3]));
        }

        public static Color ParseColor(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Color(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Quaternion ParseQuaternion(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Quaternion(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Rect ParseRect(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Rect(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Vector2 ParseVector2(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector2(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]));
        }

        public static Vector3 ParseVector3(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]));
        }

        public static Vector4 ParseVector4(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector4(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }



        #region Array

        public static string[] ParseStringArray(string value)
        {
            return value.Split(DataArraySeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 转换int数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int[] ParseInt32Array(string value)
        {
            string[] ses = value.Split(DataArraySeparators, StringSplitOptions.RemoveEmptyEntries);
            int[] array = new int[ses.Length];
            for (int i = 0; i < ses.Length; i++)
            {
                array[i] = int.Parse(ses[i]);
            }
            return array;
        }

        /// <summary>
        /// 单精度 也就是Float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float[] ParseSingleArray(string value)
        {
            string[] ses = value.Split(DataArraySeparators, StringSplitOptions.RemoveEmptyEntries);
            float[] array = new float[ses.Length];
            for (int i = 0; i < ses.Length; i++)
            {
                array[i] = float.Parse(ses[i]);
            }
            return array;
        }
        #endregion

        public static CampType ParseCampType(string value)
        {
            return (CampType)Enum.Parse(typeof(CampType), value);
        }
    }
}

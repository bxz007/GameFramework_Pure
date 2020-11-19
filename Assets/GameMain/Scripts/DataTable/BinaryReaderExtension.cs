//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GameMain
{
    public static class BinaryReaderExtension
    {
        public static Color32 ReadColor32(this BinaryReader binaryReader)
        {
            return new Color32(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
        }

        public static Color ReadColor(this BinaryReader binaryReader)
        {
            return new Color(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }

        public static DateTime ReadDateTime(this BinaryReader binaryReader)
        {
            return new DateTime(binaryReader.ReadInt64());
        }

        public static Quaternion ReadQuaternion(this BinaryReader binaryReader)
        {
            return new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }

        public static Rect ReadRect(this BinaryReader binaryReader)
        {
            return new Rect(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }

        public static Vector2 ReadVector2(this BinaryReader binaryReader)
        {
            return new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }

        public static Vector3 ReadVector3(this BinaryReader binaryReader)
        {
            return new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }

        public static Vector4 ReadVector4(this BinaryReader binaryReader)
        {
            return new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
        }

        static BinaryFormatter serializer = new BinaryFormatter();
        #region Array
        public static int[] ReadInt32Array(this BinaryReader binaryReader)
        {
            //先读长度
            int len = binaryReader.ReadInt32();
            int[] array = new int[len];
            for (int i = 0; i < len; i++)
                array[i] = binaryReader.ReadInt32();
            return array;
        }

        public static float[] ReadSingleArray(this BinaryReader binaryReader)
        {
            //先读长度
            int len = binaryReader.ReadInt32();
            float[] array = new float[len];
            for (int i = 0; i < len; i++)
                array[i] = binaryReader.ReadSingle();
            return array;
        }

        public static string[] ReadStringArray(this BinaryReader binaryReader)
        {
            //先读长度
            int len = binaryReader.ReadInt32();
            string[] array = new string[len];
            for (int i = 0; i < len; i++)
                array[i] = binaryReader.ReadString();
            return array;
        }


        #endregion
        
        public static GameMain.CampType ReadCampType(this BinaryReader binaryReader)
        {
            return (GameMain.CampType)(serializer.Deserialize(binaryReader.BaseStream));
        }
    }
}

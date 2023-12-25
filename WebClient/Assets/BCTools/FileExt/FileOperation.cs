using System.IO;
using System.Security.Cryptography;  
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
//created by WANGBENCHONG,almost for editor
public class FileOperation
{
	public enum ELineEnd
	{
		Windows,
		linux,
		Mac
	}
	//the path format should be A/B/C ,not A\B\C, call me when OnGUI should be OK
	public static bool showIOPath(string titlename, ref string path, bool isfolder,bool savemode = false,string defaultname = "",string extension = "")
	{
		bool cansave = false;
		#if UNITY_EDITOR
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(titlename, path);
		if (savemode && GUILayout.Button("save", GUILayout.Width(40)) || !savemode && GUILayout.Button("...", GUILayout.Width(35)))
		{
			string tempStr = path;
			if (isfolder)
				tempStr = EditorUtility.OpenFolderPanel("Choose the folder", tempStr, defaultname);
			else if (savemode)
			{
				tempStr = EditorUtility.SaveFilePanel("choose the path you save", tempStr.Substring(0, tempStr.LastIndexOf("/")), defaultname, extension);
			}
			else
			{
				tempStr = EditorUtility.OpenFilePanel("Choose the file", tempStr, extension);
			}
			if (!string.IsNullOrEmpty(tempStr))
			{
				path = tempStr;
				if (savemode) cansave = true;
			}
		}
		EditorGUILayout.EndHorizontal();
		#endif
		return cansave;
	}

	public static void Save(string fileName, string saveString)
	{
		Save(fileName,saveString,System.Text.Encoding.UTF8);
	}
	public static void Save(string fileName, string saveString, Encoding encoding)
	{
		FileStream fileStream = null;
		StreamWriter writer = null;
		try
		{
			fileStream = new FileStream(fileName, FileMode.Create);
			writer = new StreamWriter(fileStream,encoding);
			writer.Write(saveString);
			writer.Close();
			fileStream.Close();
		}
		catch (Exception e)
		{
			#if UNITY_EDITOR
			EditorUtility.DisplayDialog("File Save Error!",fileName+"\n"+e.Message, "OK");
			#endif
		}
		finally
		{
			if(fileStream != null)
				fileStream.Close();
			if(writer != null)
				writer.Close();
		}
	}
	public static string Load(string fileName)
	{
		return Load (fileName,System.Text.Encoding.UTF8);
	}
	public static string Load(string fileName,Encoding encoding)
	{
		string rt = "";
		if (string.IsNullOrEmpty(fileName))
			return rt;
		if (!File.Exists(fileName))
			return rt;
		else
		{
			StreamReader reader = new StreamReader(fileName, encoding);
			rt = reader.ReadToEnd();
		}
		return rt;
	}
	/// <summary>
	/// switch file path mode
	/// </summary>
	/// <param name="path">path you want to change</param>
	/// <param name="mode">mode is true-->A\B\C;else-->A/B/C</param>
	/// <returns>return path string</returns>
	public static string SetPathMode(string path,bool mode)
	{
		string rt = "" ;
		if (mode)
		{
			rt = path.Replace("/","\\");
		}
		else
		{
			rt = path.Replace("\\", "/");
		}
		return rt;
	}

	public static List<FileInfo> GetFilesIncludeChildFold(string path,string searchPattern = "*.*")
	{
		List<FileInfo> rt = new List<FileInfo>();
		DirectoryInfo dinfo = new DirectoryInfo(path);
		AddFileResort(dinfo, searchPattern, rt);
		return rt;
	}

	private static void AddFileResort(DirectoryInfo dinfo,string searchPattern,List<FileInfo> filelist)
	{
        //DirectoryInfo ��GetFiles��GetDirectories������web�²�����
#if !UNITY_WEBPLAYER && !UNITY_STANDALONE
		if (dinfo.Exists)
		{
			FileInfo[] finfo = dinfo.GetFiles(searchPattern);
			for (int i = 0; i < finfo.Length; i++)
			{
				filelist.Add(finfo[i]);
			}
			DirectoryInfo[] dinfoarray = dinfo.GetDirectories();
			for (int j = 0; j < dinfoarray.Length; j++)
			{
				AddFileResort(dinfoarray[j], searchPattern, filelist);
			}
		}
#endif
    }

	//�����ı��ļ�����β����
	public static void SetFileLineEnding(string filePath,FileOperation.ELineEnd endType)
	{
		StreamReader sr = null;
		string lineEndStr = "\r\n";
		if(endType == ELineEnd.linux)
			lineEndStr = "\n";
		else if(endType == ELineEnd.Mac)
			lineEndStr = "\r";
		try
		{
			FileStream fileStream = new FileStream(filePath, FileMode.Open);
			Encoding encoding = GetEncoding(fileStream);
			fileStream.Close();
			sr = new StreamReader(filePath,encoding);
			String text = "";
			String line;
			//--------------�жϣ��粻��Ҫת�����������ļ�-----------------
			if (true)//���ٱ���������
			{
				char[] buff = new char[251];
				string strbuff = "";
				if (sr.BaseStream.Length > 251)
				{
					sr.Read(buff, 0, 250);
					strbuff = new string(buff);
				}
				else
					strbuff = sr.ReadToEnd();

				if (strbuff.Contains("\r\n"))
				{
					if (endType == ELineEnd.Windows)
					{
						//Debug.Log("Already Windows:\n"+filePath);
						return;
					}
				}
				else if (strbuff.Contains("\r"))
				{
					if (endType == ELineEnd.Mac)
					{
						//Debug.Log("Already Mac:\n" + filePath);
						return;
					}
				}
				else if (strbuff.Contains("\n"))
				{
					if (endType == ELineEnd.linux)
					{
						//Debug.Log("Already Linux:\n" + filePath);
						return;
					}
				}
			}
			sr.BaseStream.Seek(0, SeekOrigin.Begin);
			sr.DiscardBufferedData();
			//-------------------------------------------------------------
			bool first = true;
			while ((line = sr.ReadLine()) != null) 
			{
				if(!first)
				{
					text += lineEndStr;
				}
				else
				{
					first = false;
				}

				text += line;
			}
			sr.BaseStream.Seek(-1, SeekOrigin.Current);
			string checkFileEnd = sr.ReadToEnd();//���ڼ���ļ���β��û�л��з�
			if (checkFileEnd.Contains("\r") || checkFileEnd.Contains("\n"))//�ļ�ȷʵ���û��з�����β��
				text += lineEndStr;
			sr.Close();
			Save (filePath,text,encoding);
		}
		catch (Exception e)
		{
			#if UNITY_EDITOR
			EditorUtility.DisplayDialog("File Save Error!", filePath +"\n"+e.Message, "OK");
			#endif
		}
		finally
		{
			if(sr != null)
				sr.Close();
		}
	}

	#region �������
	/// <summary>   
	/// ͨ���������ļ������ж��ļ��ı�������   
	/// </summary>   
	/// <param name="fs">�ļ���</param>   
	/// <returns>�ļ��ı�������</returns>   
	public static System.Text.Encoding GetEncoding(Stream fs)
	{
		//byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
		//byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
		//byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //��BOM   
		Encoding reVal = Encoding.Default;
		
		BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
		byte[] ss = r.ReadBytes(4);
		if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
		{
			reVal = Encoding.BigEndianUnicode;
		}
		else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
		{
			reVal = Encoding.Unicode;
		}
		else
		{
			if (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF)
			{
				reVal = Encoding.UTF8;
			}
			else
			{
				int i;
				int.TryParse(fs.Length.ToString(), out i);
				ss = r.ReadBytes(i);
				
				if (IsUTF8Bytes(ss))
					reVal = Encoding.UTF8;
			}
		}
		r.Close();
		return reVal;
	}
	 
	// �ж��Ƿ��ǲ��� BOM �� UTF8 ��ʽ   
	private static bool IsUTF8Bytes(byte[] data)
	{
		int charByteCounter = 1; //���㵱ǰ���������ַ�Ӧ���е��ֽ���
		byte curByte; //��ǰ�������ֽ�
		for (int i = 0; i < data.Length; i++)
		{
			curByte = data[i];
			if (charByteCounter == 1)
			{
				if (curByte >= 0x80)
				{
					//�жϵ�ǰ   
					while (((curByte <<= 1) & 0x80) != 0)
					{
						charByteCounter++;
					}
					//���λ��λ��Ϊ��0 ��������2��1��ʼ ��:110XXXXX...........1111110X��   
					if (charByteCounter == 1 || charByteCounter > 6)
					{
						return false;
					}
				}
			}
			else
			{
				//����UTF-8 ��ʱ��һλ����Ϊ1   
				if ((curByte & 0xC0) != 0x80)
				{
					return false;
				}
				charByteCounter--;
			}
		}
		if (charByteCounter > 1)
		{
			throw new Exception("��Ԥ�ڵ�byte��ʽ!");
		}
		return true;
	}
	#endregion �������
	
	#region MD5
	public static string GetMD5(string fileName)
	{
		try  
		{  
			FileStream file = new FileStream(fileName, System.IO.FileMode.Open);  
			MD5 md5 = new MD5CryptoServiceProvider();  
			byte[] retVal = md5.ComputeHash(file);  
			file.Close();  
			StringBuilder sb = new StringBuilder();  
			for (int i = 0; i < retVal.Length; i++)  
			{  
				sb.Append(retVal[i].ToString("x2"));  
			}  
			return sb.ToString();  
		}  
		catch (Exception ex)  
		{  
			throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);  
		}
		return "";
	}
	#endregion MD5
}

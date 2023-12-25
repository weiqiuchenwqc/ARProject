using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

/// <summary>
/// Helper class containing generic functions used throughout the UI library.
/// </summary>

static public class BCTools
{
	/// <summary>
	/// Convenience function that converts Class + Function combo into Class.Function representation.
	/// </summary>

	static public string GetFuncName (object obj, string method)
	{
		if (obj == null) return "<null>";
		string type = obj.GetType().ToString();
		int period = type.LastIndexOf('/');
		if (period > 0) type = type.Substring(period + 1);
		return string.IsNullOrEmpty(method) ? type : type + "/" + method;
	}


	/// <summary>
	/// Returns the hierarchy of the object in a human-readable format.
	/// </summary>

	static public string GetHierarchy (GameObject obj)
	{
		if (obj == null) return "";
		string path = obj.name;

		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			path = obj.name + "\\" + path;
		}
		return path;
	}

	static public void ConvertToMoneyMode(ref string numStr, int nFenWei, string insertChar)
	{
		if (nFenWei != 3 && nFenWei != 4) return;
		int dotIndex = numStr.LastIndexOf('.');
		if (dotIndex < 0)dotIndex = numStr.Length;
		for (int i = dotIndex - nFenWei; i > 0; i -= nFenWei)
		{
			numStr = numStr.Insert(i, insertChar);
		}
	}
	static public string ConvertToNormalNumStr(string moneyStr, string insertChar)
	{
		return moneyStr.Replace(insertChar, "");
	}
	static public void SortChildSibling(Transform fathertran, bool small2Big)
	{
		List<Transform> tranList = new List<Transform>();
		int childCount = fathertran.childCount;
		for (int i = 0; i < childCount; i++)
		{
			tranList.Add(fathertran.GetChild(i));
		}
		tranList.Sort(SortByName);
		for (int i = 0; i < childCount; i++)
		{
			if (small2Big)
			{
				tranList[i].SetAsLastSibling();
			}
			else
				tranList[i].SetAsFirstSibling();
		}
	}
	static public int SortByName(Transform a, Transform b) { return string.Compare(a.name, b.name); }

	public static int GetIndexOfActiveChildTran(Transform child)//��������ʾ�ĺ����У���ȡ��0��ʼ�ģ�ĳ�����ӵġ�����ʾ���С��е�Index,�Ҳ�������-1
	{
		int rt = -1;
		if (child == null || !child.gameObject.activeSelf) return rt;
		Transform father = child.parent;
		if (father == null) return rt;

		int childIndex = child.GetSiblingIndex();
		rt = childIndex;
		for (int i = 0; i < father.childCount; i++)
		{
			Transform tran = father.GetChild(i);
			if (tran == child) continue;
			int tranIndex = tran.GetSiblingIndex();
			if (tranIndex < childIndex && !tran.gameObject.activeSelf)
			{
				rt--;
			}
		}
		return rt;
	}

	public static int GetCountOfActiveChild(Transform father)//��ȡ���д�����ʾ״̬��������ĸ���
	{
		int i = 0, j = 0;
		while (i < father.childCount)
		{
			if (father.GetChild(i).gameObject.activeSelf)
				j++;
			i++;
		}
		return j;
	}

	public static string[] DivideNumAndString(string strmix)//�ֽ⡾�����ַ���+���֡������ַ���Ϊ��������
	{
		string[] rt = new string[] { strmix, "0" };
		for (int i = 0; i < strmix.Length; i++)
		{
			if (strmix[i] <= '9' && strmix[i] >= '0')
			{
				rt[0] = strmix.Substring(0, i);
				rt[1] = strmix.Substring(i);
				break;
			}
		}
		return rt;
	}
}

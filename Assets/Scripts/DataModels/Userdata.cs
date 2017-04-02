using System;
using Unity3dAzure.AppServices;

[Serializable]
public class Userdata: DataModel 
{
	public string username;
	public string gender = "";
	public string height = "";
	public string weight = "";
	public int birthDay;
	public int birthMonth;
	public int birthYear;
}
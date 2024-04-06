using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

[System.Serializable]
public class ResponseDto
{
    public string code;
    public string message;
    public Data data;
    public bool isSuccess;
}

[System.Serializable]
public class Data
{
    public double b;
    public double r;
    public double g;
}

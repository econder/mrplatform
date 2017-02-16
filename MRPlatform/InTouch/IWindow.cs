using System;
using System.Data;
using System.Runtime.InteropServices;


namespace MRPlatform.InTouch
{
    [Guid("B1F1FF23-8758-4978-8BE2-E65FBB07C8DA")]
    public interface IWindow
    {
        DataSet Windows(string windowIndexFileName);
    }


    [Guid("1625A459-8F31-4F62-8F0D-1D48F64BDF67")]
    public interface IWindowEvents
    {

    }
}

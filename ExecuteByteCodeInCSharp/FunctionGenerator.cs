using System.Runtime.InteropServices;

namespace ExecuteByteCodeInCSharp;
public class FunctionGenerator
{
    public static T Generate<T>(byte[] data)
    {
        var addressOfAssembledByteCodeInMemory = ((int)Pin(data)) + 8; //skip array type(4 bytes) and array size(4 bytes)
        UnlockPage(addressOfAssembledByteCodeInMemory, (uint)data.Length);
        Console.WriteLine(addressOfAssembledByteCodeInMemory.ToString("X"));

        //returns delegate of correct type
        return (T)(object)Marshal.GetDelegateForFunctionPointer<T>((IntPtr)addressOfAssembledByteCodeInMemory);
    }


    static IntPtr Pin(object data) //pins our bytecode array to memory
    {
        List<object> memory = new();
        List<GCHandle> handles = new();

        memory.Add(data);
        var handle = GCHandle.Alloc(data);
        handles.Add(handle);

        return Marshal.ReadIntPtr(GCHandle.ToIntPtr(handle)); //returns array base address
    }

    static void UnlockPage(int address, uint size)
    {
        uint old;
        VirtualProtect((IntPtr)address, size, (uint)PageProtection.PAGE_EXECUTE_READWRITE, out old);
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtext, out uint lpflOldProtext);
}
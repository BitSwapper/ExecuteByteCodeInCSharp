namespace ExecuteByteCodeInCSharp;
public class BytesToFunction
{
    public delegate int FunctionInt(int x, int y);
    public static FunctionInt AddTwoNumsInByteCode = FunctionGenerator.Generate<FunctionInt>(new byte[] { 0x8B, 0x44, 0x24, 0x04, 0x8B, 0x5C, 0x24, 0x08, 0x01, 0xD8, 0xC3 });
}

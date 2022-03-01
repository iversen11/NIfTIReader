
using System.Runtime.InteropServices;
namespace NifTIReader
{
    [StructLayout(LayoutKind.Sequential, Size = 3)]
    public class RgbTriple
    {
        byte Red { get; set; }
        byte Green { get; set; }
        byte Blue { get; set; }
        
        
        public RgbTriple()
        {

        }
        public RgbTriple(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public class RgbalphaTriple
    {
        byte Red { get; set; }
        byte Green { get; set; }
        byte Blue { get; set; }
        byte Alpha { get; set; }
        public RgbalphaTriple()
        {

        }
        public RgbalphaTriple(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }
}

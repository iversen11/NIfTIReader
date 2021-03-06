using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Linq;
using System.Reflection;
using System.Numerics;
namespace NifTIReader
{
    class Program
    {

        static void Main()
        {
	
            // insert file path here string file = ;


            Nifti1Header header = NiftiHeaderMethods.ReadNifti1Header(file);
            NiftiHeaderMethods.PrintHeader(header);


            Array unScaledImage = NiftiImageMethods.ReadNifti1Image(file, header);

            
            float[] Data = NiftiImageMethods.ScaleImage(unScaledImage, header.dim[1], header.dim[2], header.dim[3], header.dim[4]); //TODO ask Sebastian do we scale functional images?
            Console.WriteLine(Data.Length);
            Data[16384] = 16384;

            /* ImageSlice imageSlice = new ImageSlice(header, Data, 0, 0); //NiftiImageMethods.NiftiToBitmap(file, header);
             Console.WriteLine(imageSlice);
             Console.WriteLine(imageSlice[0,0]);*/

            FunctionalImageFullScan fScan = new FunctionalImageFullScan(header, Data);

            FunctionalImageVolume testVol = fScan[0];

            ImageSlice testSlice = testVol[1];
            Console.WriteLine(testSlice[0,0]);
            //NiftiImageMethods.ScaledArrayToBitmap(Data.Skip(0).Take(65536).ToArray(), "structuralslicetest.bmp", header);
            //StructuralImageVolume StructuralVolume = new StructuralImageVolume(header, Data);
        }
        
    }
}


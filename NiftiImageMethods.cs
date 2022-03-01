using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
namespace NifTIReader
{
    public static class NiftiImageMethods
    {

        /// <summary>
        /// Converts Nifti Image Array to Bitmap, TODO currently does not support greyscale(not built in =(. Will be writing custom pallete soon). For now make sure output string contains .bmp
        /// </summary>
        /// <param name="fileName"></param>
        public static void NiftiToBitmap(string fileName, string outputFileName, Nifti1Header header) //TODO change input parameter from file name to image slice, make own pallete for grayscale
            {
                int width = header.dim[1]; //width dimension from header
                int height = header.dim[2]; // height dimension from header
                int depth = header.dim[3];
                Array originalImage = ReadNifti1Image(fileName,header); //array from file in type of datatype code

                var ScaledImage = ScaleImage(originalImage, width, height, depth); //scales image between 0 and 255 and returns bytes
                var byteArray = ScaledImage.Select(scaledValue => Convert.ToByte(scaledValue)).ToArray();
                
                CreateSaveBitmap(width, height, byteArray, outputFileName); //TODO change this, depth needs to be fixed to take a slice number
            }

        public static void ScaledArrayToBitmap(float[] scaledData, string outputFileName, Nifti1Header header)
        {
            int width = header.dim[1]; //width dimension from header
            int height = header.dim[2]; // height dimension from header

            var byteArray = scaledData.Select(scaledValue => Convert.ToByte(scaledValue)).ToArray();

            CreateSaveBitmap(width, height, byteArray, outputFileName); //TODO change this, depth needs to be fixed to take a slice number

        }
        /// <summary>
        /// overload to allow for single slice
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float[] ScaleImage(Array originalImage, int width, int height) //TODO this needs to be refactored to scale all of the image array and then print a certain slice
        {
            var bufferImage = originalImage.Cast<object>().ToArray().Take(width * height).Select(Convert.ToSingle); //TODO extending the .Take to include z may make the volume work
            var imagemax = bufferImage.Max();
            var ScaledImage = bufferImage.Select(x => Convert.ToSingle(Math.Round(x / imagemax * 255))).ToArray(); //scales buffer image to range 0-255 (greyscale), rounds and converts to bytes 
            return ScaledImage;
        }
        /// <summary>
        /// scales image between 0 and 255, rounds, and converts to bytes
        /// </summary>
        /// <param name="originalImage"></param> 
        /// <returns></returns>
        public static float[] ScaleImage(Array originalImage, int width, int height, int depth) //TODO this needs to be refactored to scale all of the image array and then print a certain slice
        {
            var bufferImage =  originalImage.Cast<object>().ToArray().Take(width * height * depth).Select(Convert.ToSingle); //TODO extending the .Take to include z may make the volume work
            var imagemax = bufferImage.Max();
            var ScaledImage = bufferImage.Select(x => Convert.ToSingle(Math.Round(x / imagemax * 255))).ToArray(); //scales buffer image to range 0-255 (greyscale), rounds and converts to bytes 
            return ScaledImage;
        }

        public static float[] ScaleImage(Array originalImage, int width, int height, int depth, int runNumber) //TODO this needs to be refactored to scale all of the image array and then print a certain slice
        {
            var bufferImage = originalImage.Cast<object>().ToArray().Take(width * height * depth * runNumber).Select(Convert.ToSingle); //TODO extending the .Take to include z may make the volume work
            var imagemax = bufferImage.Max();
            var ScaledImage = bufferImage.Select(x => Convert.ToSingle(Math.Round(x / imagemax * 255))).ToArray(); //scales buffer image to range 0-255 (greyscale), rounds and converts to bytes 
            return ScaledImage;
        }

        public static float[] ScaleFullImage(Array originalImage)
        {
            var bufferImage = originalImage.Cast<object>().ToArray().Select(Convert.ToSingle); //TODO extending the .Take to include z may make the volume work
            var imagemax = bufferImage.Max();
            var ScaledImage = bufferImage.Select(x => Convert.ToSingle(Math.Round(x / imagemax * 255))).ToArray(); //scales buffer image to range 0-255 (greyscale), rounds and converts to bytes 
            return ScaledImage;
        }

        /// <summary>
        /// creates and saves a bitmap with the given output file name
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="scaledImage"></param>
        /// <param name="outputFileName"></param>
        public static void CreateSaveBitmap(int width, int height, byte[] scaledImage, string outputFileName)
        {

            using var bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed); //creates bitmap from byte array, FORMAT MUST BE SPECIFIED HERE AND BMPDATA FORMAT MUST BE BMP.PIXELFORMAT TO WORK
            //printing in color, needs to be greyscale
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);

            IntPtr pNative = bmpData.Scan0;
            Marshal.Copy(scaledImage, 0, pNative, scaledImage.Length);
            bmp.UnlockBits(bmpData);

            string path = Path.Combine(Directory.GetCurrentDirectory(), outputFileName); //TODO check if file name contains.bmp, if not add it in- perhaps we could async this 
            bmp.Save(path);
        }



        /// <summary>
        /// reads in a Nifti1Image using information from the header and a dicionary of Nifti Codes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Array ReadNifti1Image(string file, Nifti1Header header) ///has not been tested with nulls, needs to be tested that the correct image is being returned
        {
            //Nifti1Header header = NiftiHeaderMethods.ReadNifti1Header(file);

            var lookup = new Dictionary<short, Type> //creates dictionary with defined nifti and analyze lookup codes, im guessing there's gonna be some problems with the bigintegers so it will need to be tested , https://nifti.nimh.nih.gov/pub/dist/src/niftilib/nifti1.h
            {
                { 0, null },
                { 1, null },
                { 2, typeof(byte) },
                { 4, typeof(short) },
                { 8, typeof(int) },
                { 16, typeof(float) },
                { 32, typeof(long) }, //this is supposed to be complex type, check and adjust 
                { 64, typeof(double) },
                { 128, typeof(RgbTriple) },
                { 255, null},
                { 256, typeof(sbyte) },
                { 512, typeof(ushort) },
                { 768, typeof(uint) },
                { 1024, typeof(long) }, //these two are supposed to be long long type, check and adjust
                { 1280, typeof(ulong)},
                { 1536, typeof(BigInteger) }, //long double
                { 1792, typeof(BigInteger) }, //double pair
                { 2048, typeof(BigInteger) }, //long double pair 
                { 2305, typeof(RgbalphaTriple) } //RGBA
            };
            Array imageArray = MatchNifti1DataType(header, file, lookup);
            return imageArray;
        }
        /// <summary>
        /// matches on the Nifti Code data type and returns an array adjusted for size 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="fileName"></param>
        /// <param name="lookup"></param>
        /// <returns></returns>
        public static Array MatchNifti1DataType(Nifti1Header header, string fileName, Dictionary<short, Type> lookup) //pairs with ReadNifti1Image
        {
            var info = new FileInfo(fileName);
            long offset = (long)header.vox_offset; //find offset of image 
            Type dataType = MatchNifti1Dictionary(lookup, header);
            int sizeOfDataType = Marshal.SizeOf(dataType); 
            var buf = new byte[info.Length - (int)offset]; // size of image 

            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                stream.Seek(offset, SeekOrigin.Begin); //read image from offset, any slice can be found by multiplying (256*256) * desired slice out of 150 + offset
                stream.Read(buf, 0, buf.Length);
            }
            var block = Array.CreateInstance(dataType, buf.Length / sizeOfDataType); //conversion to floats can be made here by changing datatype for typeof(float)
            Buffer.BlockCopy(buf, 0, block, 0, buf.Length);
            return block;
        }

        /// <summary>
        /// matches header datatype field on Nifti code dictionary and returns the typeof(datatype)
        /// </summary>
        /// <param name="lookup"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static Type MatchNifti1Dictionary(Dictionary<short, Type> lookup, Nifti1Header header) //TODO this needs to be tested with custom types once we find a way to implement size. this pairs with matchnifti1datatype
        {
            Type dataType;
            short headerDataTypeCode = (short)header.GetType().GetField("datatype").GetValue(header); //find Nifti Code specifiying data type from dictionary 
            lookup.TryGetValue(headerDataTypeCode, out dataType); //finds dataType associated with code 
            return dataType;
        }
    }
}

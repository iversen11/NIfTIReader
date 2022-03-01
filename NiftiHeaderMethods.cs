using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;


namespace NifTIReader
{
    //TODO handle intent codes
    //TODO all of the methods exclusive to Nifti1Headers will need to be extended to analyze, nifti2, etc. Maybe try to make it generic if possible
    //TODO there's a bug that if the image is read before the header is printed, the datatype field is printed at the beginning , maybe it's a reference issue? if i dont clear the terminal after running print before the image stuff it works for a few times
    public static class NiftiHeaderMethods
    {
        public enum Endian { BigEndian, LittleEndian, BigEndianLittleSystem, LittleEndianBigSystem, Unknown };
        
        /// <summary>
        /// reads in a binary nifti file and converts to byte array
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Nifti1Header ReadNifti1Header(string fileName)
        {
            Nifti1Header nifti1 = new Nifti1Header(); //instantiate new Nifti1Header class 
            byte[] Nifti1File = File.ReadAllBytes(fileName).ToArray(); //read bytes in from file path to byte array
            EndianDecision(Nifti1File, nifti1); //find the endianness of the byte array and flip if it doesn't match the endianness of the system 
            Nifti1Header header = ByteArrayToStruct<Nifti1Header>(Nifti1File); //create header struct from byte array 
            return header;
        }

        

        /// <summary>
        /// takes a byte array and casts it to the desired header-struc, current support is only for Nifti1Headers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteFile"></param>
        /// <returns></returns>
        public static T ByteArrayToStruct<T>(byte[] byteFile) where T : struct //T signifies header type 
        {
            GCHandle handle = GCHandle.Alloc(byteFile, GCHandleType.Pinned); //pin byte file to protect from garbage collector
            T header = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T)); //create struct from Ptr
            return header;
        }

        /// <summary>
        /// Prints out all of the header names and values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="header"></param>
        public static void PrintHeader<T>(T header) where T:struct //T signifies header type 
        {
            foreach (var field in header.GetType().GetFields()) //gets every field of the specified header type 
            {

                if (field.FieldType.IsArray) 
                {
                    Console.Write(field.Name + ":");
                    Console.Write("[");
                    Array arr = (Array)field.GetValue(header); //allows me to find the length to print with commas on every value but the last
                    int index = 0;
                    foreach (var value in arr)
                    { 
                        if (index < arr.Length - 1)
                        {
                            Console.Write(value + ",");
                            index++;
                        }
                        else
                            Console.Write(value);
                    }
                    Console.Write("]");
                    index = 0;
                    Console.WriteLine();
                }
                else Console.WriteLine(field.Name + ":" + field.GetValue(header)); //prints non-array values 

            }
        }

        /// <summary>
        /// finds the endianness of the file, maches with endainness of system, and returns enumeration defined in NiftiHeader.cs
        /// </summary>
        /// <param name="niftiFile"></param>
        /// <returns></returns>
        public static Endian GetFileEndianness(byte[] niftiFile)
        {
            byte byte1 = niftiFile[40]; // takes first value of dim[] to check endianness
            byte byte2 = niftiFile[41];
            short endianByteChecker = BitConverter.ToInt16(new byte[] { byte1, byte2 }); //converts to short to check endianness
            if (BitConverter.IsLittleEndian)
            {
                if (endianByteChecker >= 1 && endianByteChecker <= 7) //values specified by nifti to determine endianness, could use a refactor to not have magic numbers 
                    return Endian.LittleEndian;
                else
                    return Endian.BigEndianLittleSystem;

            }
            else if (!BitConverter.IsLittleEndian)
            {
                if (endianByteChecker >= 1 && endianByteChecker <= 7)
                    return Endian.BigEndian;
                else
                    return Endian.LittleEndianBigSystem;
            }
            else
                return Endian.Unknown;
        }

        /// <summary>
        /// Flips the endianness for a byte file based on the desired struct, handles arrays by flipping the indvidual byte arrays and ignores strings (immune to endianness)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="niftiFile"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static byte[] FlipEndian<T>(byte[] niftiFile,T header) where T:struct
        {
           
            foreach (var field in header.GetType().GetFields())
            {
                if (field.FieldType.IsArray) 
                {
                    
                    int offset = Marshal.OffsetOf(header.GetType(), header.GetType().GetField(field.Name).Name).ToInt32(); // see if we can trim this down
                    Type type = header.GetType().GetField(field.Name).FieldType.GetElementType();
                    int typeSize = Marshal.SizeOf(type); //size of each element 

                    Type headerType = header.GetType(); // finds the size constant of the field 
                    FieldInfo fieldInfo = headerType.GetField(field.Name); //retrieves current field
                    object[] attributeArray = fieldInfo.GetCustomAttributes(typeof(MarshalAsAttribute), false); //create object array of ll the attributes 
                    MarshalAsAttribute attributeMarshal = (MarshalAsAttribute) attributeArray[0]; //takes object at first index which corresponds to object associated with field
                    int sizeConst = attributeMarshal.SizeConst;
                    
                    for (int seekOffset = offset; seekOffset < offset+(sizeConst*typeSize); seekOffset += typeSize) //for each value stored inside of the array, flips the byte to reverse endianness. offset is specified in header, size of array is sizeConst*size
                        Array.Reverse(niftiFile, seekOffset, typeSize); 

                }
                else if (field.FieldType.IsEquivalentTo(typeof(string))) 
                {
                    continue;
                }
                else
                {
                    int offset = Marshal.OffsetOf(typeof(Nifti1Header), header.GetType().GetField(field.Name).Name).ToInt32(); //see if we can trim this down
                    int size = Marshal.SizeOf(field.FieldType);
                    Array.Reverse(niftiFile, offset, size);
                }
            }
            return niftiFile;
        }

        /// <summary>
        /// finds endianess based on [40] byte and makes decison whether to flip the arrays or not 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static byte[] EndianDecision<T>(byte[] file, T header ) where T:struct
        {
            Endian endianness = GetFileEndianness(file);
            if(endianness == Endian.BigEndianLittleSystem || endianness == Endian.LittleEndianBigSystem)
            {
                 FlipEndian<T>(file,header);
                 return file;
                
            }
            return file;
        }  
    }
}

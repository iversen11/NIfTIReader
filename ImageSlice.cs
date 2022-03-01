using System;
using System.IO;
using System.Linq;
namespace NifTIReader
{
    public class ImageSlice
    {
        internal float[,] Slice {get;}

        public float this[int row, int column]
        {
            get => Slice[row, column];
        }

        public ImageSlice(Nifti1Header header, float[] data, int sliceNumber, int volumeNumber) 
        {
            int x = header.dim[1]; //x dimension from header
            int y = header.dim[2]; //y dimension from header
            int z = header.dim[3];

            int sliceOffset = (x * y * sliceNumber);  //finds offset of desired slice
            int volumeOffset;

            switch(header.dim[4]) //switches on dim 4 (number of volumes) to account for differences in indexing
            {
                case 1:
                    volumeOffset = 0;
                    break;

                default:
                    volumeOffset = (x * y * z * volumeNumber);
                    break;

            }
            
            float[] singleSliceByte = data.Skip(sliceOffset + volumeOffset).Take(x * y).ToArray(); //takes single slice - slice number is designated by offset
            int bytePosition = 0; //iterator to track byte position in singleSliceByte array 
            float[,] slice = new float[x, y]; //linqs to input 2d, encaspulate slices within volume

            for (int row = 0; row < x; row++) //Proper index  = row#*256+col#
            {
                for (int column = 0; column < y; column++) //TODO these were changed aware from being -1, needs to test
                {
                    slice[row, column] = singleSliceByte[bytePosition]; //adds correct byte to position 
                    bytePosition++;
                }
            }
            Slice = slice;
        }

    }
}

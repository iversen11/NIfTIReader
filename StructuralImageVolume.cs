using System;
using System.Linq;
namespace NifTIReader
{
    public class StructuralImageVolume
    {
        

        internal ImageSlice[] StructuralVolume;

        public StructuralImageVolume(Nifti1Header header, float[] data)
        {
            {

                int slicesInVolume = header.dim[3];
                ImageSlice[] structuralVolume = new ImageSlice[slicesInVolume];

                for (int sliceIndex = 0; sliceIndex < slicesInVolume; sliceIndex++)
                {
                    structuralVolume[sliceIndex] = new ImageSlice(header, data, sliceIndex, header.dim[4]);
                }

                StructuralVolume = structuralVolume;
            }
        }





       /* internal float[,,] StructuralImageVolumeCreator(Nifti1Header header, float[] data)
        {
            {
                //TODO this is returning 65536 from data.Length so we need to get the whole volume - we need to refactor the image reader to get the whole volume and then use the slicer 
                int x = header.dim[1]; //x dimension from header
                int y = header.dim[2]; //y dimension from header
                int z = header.dim[3];

                int bytePosition = 0; //iterator to track byte position in singleSliceByte array 
                float[,,] volume = new float[z, x, y]; //linqs to input 2d, encaspulate slices within volume
                for (int height = 0; height < z - 1; height++)
                {
                    for (int row = 0; row < x - 1; row++)
                    {
                        for (int column = 0; column < y - 1; column++)
                        {
                            volume[height, row, column] = data[bytePosition]; //adds correct byte to position 
                            bytePosition++;
                        }
                    }
                }
                return volume;
            }

        }*/
    }
}

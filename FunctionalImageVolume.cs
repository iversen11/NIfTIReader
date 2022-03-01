using System;
namespace NifTIReader
{
    public class FunctionalImageVolume
    {
        internal ImageSlice[] FunctionalVolume; //creates volume

        public ImageSlice this[int index]
        {
            get => FunctionalVolume[index];
        }

        public FunctionalImageVolume(Nifti1Header header, float[] data, int volumeNumber)
        {
            {

                int slicesInVolume = header.dim[3];
                ImageSlice[] functionalVolume = new ImageSlice[slicesInVolume];

                for (int sliceIndex = 0; sliceIndex < slicesInVolume; sliceIndex++)
                {
                    functionalVolume[sliceIndex] = new ImageSlice(header, data, sliceIndex, volumeNumber); // creates array of slices depedning on the number of slices in volume 
                }

                FunctionalVolume = functionalVolume;
            }
        }
    }
}

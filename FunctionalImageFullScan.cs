using System;
namespace NifTIReader
{
    public class FunctionalImageFullScan
    {
        internal FunctionalImageVolume[] FunctionalFull;

        public FunctionalImageVolume this[int index]
        {
            get => FunctionalFull[index];
        }

        public FunctionalImageFullScan(Nifti1Header header, float[] data)
        {
            int volumesInScan = header.dim[4];
            FunctionalImageVolume[] functionalFull = new FunctionalImageVolume[volumesInScan];

            for (int volumeIndex = 0; volumeIndex < volumesInScan; volumeIndex++)
            {
                functionalFull[volumeIndex] = new FunctionalImageVolume(header, data, volumeIndex); //creates array of volumes based on number of runs 
            }

            FunctionalFull = functionalFull;
        }
    }
}

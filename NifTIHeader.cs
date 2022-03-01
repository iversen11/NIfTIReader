
using System.Runtime.InteropServices;

namespace NifTIReader
{
    

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi ,Size = 348)]
    public struct Nifti1Header

    {
        // when doing endianness reverse array elements not array itself
        //create and then we can use refelection along with method
        //[FieldOffset(0)]
        [MarshalAs(UnmanagedType.I4, SizeConst = 1)]
        public int sizeof_hdr;

        //[FieldOffset(8)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string data_type; // Analyze compatibility  - unused

        //[FieldOffset(14)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
        public string db_name;  // Analyze compatibility  - unused

        //[FieldOffset(32)]
        [MarshalAs(UnmanagedType.I4, SizeConst = 1)]
        public int extents;  // Analyze compatibility  - unused

        //[FieldOffset(36)]
        [MarshalAs(UnmanagedType.I2, SizeConst = 1)]
        public short session_error;  // Analyze compatibility  - unused

        //[FieldOffset(38)]
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte regular; // Analyze compatibility  - unused

        //[FieldOffset(39)]
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte dim_info;

        //[FieldOffset(40)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 8)]
        public short[] dim;

        //[FieldOffset(56)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float intent_p1;

        //[FieldOffset(60)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float intent_p2;

        //[FieldOffset(64)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float intent_p3;

        //[FieldOffset(68)]
        [MarshalAs(UnmanagedType.I2, SizeConst = 1)]
        public short intent_code;

        //[FieldOffset(70)]
        [MarshalAs(UnmanagedType.I2, SizeConst = 1)]
        public short datatype;

        //[FieldOffset(72)]
        [MarshalAs(UnmanagedType.I2, SizeConst = 1)]
        public short bitpix;

        //[FieldOffset(74)]
        [MarshalAs(UnmanagedType.I2, SizeConst = 1)]
        public short slice_start;

        //[FieldOffset(76)] returns a different value from matlab, I think that they round while I don't
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 8)]
        public float[] pixdim;

        //[FieldOffset(108)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float vox_offset;

        //[FieldOffset(112)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float scl_slope;

        //[FieldOffset(116)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float scl_inter;

        //[FieldOffset(120)]
        [MarshalAs(UnmanagedType.I2, SizeConst = 1)]
        public short slice_end;

        //[FieldOffset(122)]
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte slice_code;

        //[FieldOffset(123)]
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte xytz_units;

        //[FieldOffset(124)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float cal_max;

        //[FieldOffset(128)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float cal_min;

        //[FieldOffset(132)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float slice_duration;

        //[FieldOffset(136)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float toffset;

        //[FieldOffset(140)]
        [MarshalAs(UnmanagedType.I4, SizeConst = 1)]
        public int glmax; // Analyze compatibility  - unused

        //[FieldOffset(144)]
        [MarshalAs(UnmanagedType.I4, SizeConst = 1)]
        public int glmin;

        //[FieldOffset(148)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string descrip;

        //[FieldOffset(228)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string aux_file;

        //[FieldOffset(252)]
        [MarshalAs(UnmanagedType.I2, SizeConst = 1)]
        public short qform_code;

        //[FieldOffset(254)]
        [MarshalAs(UnmanagedType.I2, SizeConst = 1)]
        public short sform_code;

        //[FieldOffset(256)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float quatern_b;

        //[FieldOffset(260)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float quatern_c;

        //[FieldOffset(264)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float quatern_d;

        //[FieldOffset(268)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float qoffset_x;

        //[FieldOffset(272)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float qoffset_y;

        //[FieldOffset(276)]
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float qoffset_z;

        //[FieldOffset(280)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] srow_x;

        //[FieldOffset(296)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] srow_y;

        //[FieldOffset(312)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] srow_z;

        //[FieldOffset(328)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string intent_name;

        //[FieldOffset(344)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string magic;

    }

}

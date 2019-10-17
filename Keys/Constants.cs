using System.Collections.Generic;

namespace Keys
{
    public class Constants
    {
        #region Triad

        #region MajorTriad
        public static List<int> TriadMajorRootPos =new List<int>(){ 0,  4, 7 };
        public static List<int> TriadMajor1stInv = new List<int>(){ -8, -5, 0 };
        public static List<int> TriadMajor2ndInv = new List<int>(){ -5,  0, 4 };
        #endregion

        #region MinorTriad
        public static List<int> TriadMinorRootPos = new List<int>{  0,  3, 7 };
        public static List<int> TriadMinor1stInv =  new List<int>{ -9, -5, 0 };
        public static List<int> TriadMinor2ndInv =  new List<int>{ -5,  0, 3 };
        #endregion

        #endregion

        #region Seventh

        #region MajorSeventh
        public static List<int> SeventhMajorRootPos =new List<int>{  0,  4,  7, 11 };
        public static List<int> SeventhMajor1stInv = new List<int>{ -8, -5, -1, 0 };
        public static List<int> SeventhMajor2ndInv = new List<int>{ -5, -1,  0, 4 };
        public static List<int> SeventhMajor3rdInv = new List<int>{ -1,  0,  4, 7 };
        #endregion

        #region MinorSeventh
        public static List<int> SeventhMinorRootPos =new List<int> {  0,  3,  7, 10 };
        public static List<int> SeventhMinor1stInv = new List<int> { -9, -5, -2, 0 };
        public static List<int> SeventhMinor2ndInv = new List<int> { -5, -2,  0, 3 };
        public static List<int> SeventhMinor3rdInv = new List<int> { -2,  0,  3, 7 };
        #endregion                 

        #endregion

        #region Scales

        public static List<int> MajorScale =         new List<int> { 0, 2, 4, 5, 7, 9, 11, 12, 11, 9, 7, 5, 4, 2, 0 };
        public static List<int> MinorNaturalScale =  new List<int> { 0, 2, 3, 5, 7, 8, 10, 12, 10, 8, 7, 5, 3, 2, 0 };
        public static List<int> MinorHarmonicScale = new List<int> { 0, 2, 3, 5, 7, 8, 11, 12, 11, 8, 7, 5, 3, 2, 0 };
        public static List<int> MinorMelodicScale =  new List<int> { 0, 2, 3, 5, 7, 9, 11, 12, 10, 8, 7, 5, 3, 2, 0 };

        #endregion
    }
}

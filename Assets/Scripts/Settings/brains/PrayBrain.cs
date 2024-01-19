namespace Settings.brains
{
    public class PrayBrain : BrainBase
    {
        public PrayBrain()
        {
            matrix[0, 15] = 4; matrix[0, 19] = 0.1; matrix[0, 20] = -1; matrix[0, 21] = 1;
            matrix[1, 15] = -4; matrix[1, 20] = 0.5; matrix[1, 21] = -0.5;
            matrix[4, 16] = 0.5; matrix[4, 19] = -0.1; matrix[4, 20] = 0.1; matrix[4, 21] = 0.5; matrix[4, 21] = -0.5;
            matrix[5, 17] = -0.4; matrix[5, 18] = 0.2; matrix[5, 19] = -0.2; matrix[5, 20] = -0.7; matrix[5, 21] = 0.7;
            matrix[6, 17] = 0.5; matrix[6, 19] = -0.1; matrix[6, 20] = 0.1; matrix[6, 21] = 0.5; matrix[6, 21] = -0.5;
            matrix[7, 17] = -4; matrix[7, 18] = 0.2; matrix[7, 19] = -0.2; matrix[7, 20] = -0.5; matrix[7, 21] = 0.5;
            matrix[8, 15] = 0.4; matrix[8, 16] = 4; matrix[8, 17] = -1.5; matrix[8, 20] = -2.2; matrix[8, 21] = 2.2;
            matrix[9, 16] = -1; matrix[9, 17] = 1.5; matrix[9, 18] = 0.2; matrix[9, 19] = -0.2; matrix[9, 20] = 1.5; matrix[9, 21] = -1.5;
            matrix[10, 15] = -0.2; matrix[10, 18] = -0.3; matrix[10, 19] = 0.3; matrix[10, 20] = 1.1; matrix[10, 21] = -1.1; matrix[10, 27] = 2.6;
            matrix[11, 15] = 0.2; matrix[11, 18] = 1; matrix[11, 19] = -1; matrix[11, 20] = -1.1; matrix[11, 21] = 1.1; matrix[11, 27] = -4;
            matrix[12, 18] = -0.4; matrix[12, 19] = 0.4; matrix[12, 20] = 0.5; matrix[12, 21] = -0.5; matrix[12, 28] = 1.5;
            matrix[13, 17] = 0.5; matrix[13, 18] = 0.3; matrix[13, 19] = -0.3; matrix[13, 20] = -8; matrix[13, 21] = 0.8; matrix[13, 28] = -4;
            matrix[15, 15] = 0.5; matrix[15, 22] = 3.5; matrix[15, 23] = -0.8; matrix[15, 24] = -1; matrix[15, 25] = 0.3; matrix[15, 26] = -1; matrix[15, 27] = -1; matrix[15, 28] = -1;
            matrix[16, 16] = 0.3; matrix[16, 23] = -0.8; matrix[16, 24] = 2.1; matrix[16, 25] = -0.7; matrix[16, 26] = 0.7; matrix[16, 27] = -0.5; matrix[16, 28] = 4; matrix[16, 29] = -1.8;
            matrix[17, 17] = 0.2; matrix[17, 23] = -0.2; matrix[17, 24] = 0; matrix[17, 25] = 1.5; matrix[17, 26] = 0.5; matrix[17, 27] = -0.3; matrix[17, 28] = -0.4; matrix[17, 29] = 3;
            matrix[18, 18] = 0.1; matrix[18, 23] = -0.1; matrix[18, 24] = 0.5; matrix[18, 25] = 0.3; matrix[18, 26] = 1.5; matrix[18, 27] = -2; matrix[18, 28] = -0.3; matrix[18, 29] = -2;
            matrix[19, 19] = 0.1; matrix[19, 24] = -0.5; matrix[19, 25] = -0.3; matrix[19, 26] = -1.2; matrix[19, 27] = 0.2; matrix[19, 28] = 0.3; matrix[19, 29] = 0.2;
            matrix[20, 23] = -0.1; matrix[20, 24] = -0.8; matrix[20, 25] = -0.2; matrix[20, 26] = -2; matrix[20, 27] = 1.5; matrix[20, 28] = 0.8; matrix[20, 29] = 0.7;
            matrix[21, 23] = 0.4; matrix[21, 24] = 1; matrix[21, 25] = 0.2; matrix[21, 26] = 2; matrix[21, 27] = -1.2; matrix[21, 28] = -0.7; matrix[21, 29] = -7;
            matrix[27, 26] = 0.2;
        }
    }
}
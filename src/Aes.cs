using System;

namespace SimpleAes
{
    public class Aes
    {
        private const int Bits = 128;
        private const int BlockRowSize = 4;
        private const int BlockSize = 16;
        private const int Rounds = 10;
        private readonly AesBlock[] _subKeys;
        public Aes(byte[] key)
        {
           _subKeys = AesKeys.CreateSubKeys(key);
        }

        public byte[] Encrypt(byte[] plain)
        {
            int extrabytes = plain.Length % BlockSize;
            int blocks = plain.Length / BlockSize + (extrabytes > 0 ? 1 : 0);
            byte[] cipherText = new byte[blocks * BlockSize];

            byte[] toEncrypt = new byte[blocks * BlockSize];
            Array.Copy(plain, toEncrypt, plain.Length);

            for (int i = 0; i < blocks; i++)
            {
                byte[] tmp = new byte[BlockSize];
                Array.Copy(toEncrypt, i * BlockSize, tmp, 0, BlockSize);
                var block = new AesBlock(tmp, false); // new byte[BlockRowSize][];
                
                var result = EncryptBlock(block);
                
                // Add the encrypted block to the result array
                Array.Copy(result.AsArray(), 0, cipherText, i * BlockSize, BlockSize);
            }

            return cipherText;
        }

        public byte[] Decrypt(byte[] cipherText)
        {

            return null;
        }

        private AesBlock EncryptBlock(AesBlock block)
        {
            // XOR block with the first round key
            for (int i = 0; i < BlockRowSize; i++)
            for (int j = 0; j < BlockRowSize; j++)
                block[j,i] ^= _subKeys[0][j,i];
            
            for (int round = 0; round < Rounds; round++)
            {
                // Byte substitution with sBox
                for (int row = 0; row < AesBlock.BlockRowLength; row++)
                for (int col = 0; col < AesBlock.BlockColLength; col++)
                {
                    byte tmp = block[row, col];
                    block[row, col] = AesConstants.SBox[tmp];
                }
                 
                // Shift the block
                for (int i = 0; i < block.Length(); i++)
                {
                    block.ShiftHorizontal(i, i);
                }

                // Mix the columns but skip the last round
                if(round != Rounds - 1) MixColumns(ref block);

                for (int row = 0; row < BlockRowSize; row++)
                for (int col = 0; col < BlockRowSize; col++)
                {
                    block[row, col] ^= _subKeys[round + 1][row,col];
                    
                }
            }
            
            return block;
        }

        private static byte GaloisMult2(byte val, byte polyRed = 0x1b) { 
            return val >= 128 ? (byte)((val << 1) ^ polyRed) : (byte)(val << 1);
        }
        
        private static void MixColumns(ref AesBlock block)
        {
            byte[] temp = new byte[BlockRowSize];
            for (int i = 0; i < BlockRowSize; i++)
            {
                temp[0] = (byte) (GaloisMult2(block[0][i]) ^ GaloisMult2(block[1][i]) ^ block[1][i] ^ block[2][i] ^ block[3][i]);
                temp[1] = (byte) (GaloisMult2(block[1][i]) ^ GaloisMult2(block[2][i]) ^ block[2][i] ^ block[3][i] ^ block[0][i]);
                temp[2] = (byte) (GaloisMult2(block[2][i]) ^ GaloisMult2(block[3][i]) ^ block[3][i] ^ block[0][i] ^ block[1][i]);
                temp[3] = (byte) (GaloisMult2(block[3][i]) ^ GaloisMult2(block[0][i]) ^ block[0][i] ^ block[1][i] ^ block[2][i]);

                for (int j = 0; j < BlockRowSize; j++) block[j][i] = temp[j];
            }
        }

    }
}
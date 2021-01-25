namespace SimpleAes
{
    public static class AesKeys
    {
        private const int Rounds = 10;

        public static AesBlock[] CreateSubKeys(byte[] key)
        {
            var subKeys = new AesBlock[Rounds + 1];
            for (int i = 0; i < subKeys.Length; i++) subKeys[i] = new AesBlock();

            // Copy the key into the first block
            subKeys[0] = new AesBlock(key, false);

            for (int blockIndex = 1; blockIndex < subKeys.Length; blockIndex++)
            {
                for (int col = 0; col < AesBlock.BlockColLength; col++)
                {
                        
                    // Rotword
                    if (col == 0)
                    {
                        // Copy first col from prev block
                        subKeys[blockIndex].SetCol(subKeys[blockIndex - 1].getCol(AesBlock.BlockColLength - 1), col);

                        // Shift col
                        subKeys[blockIndex].ShiftVertical(col, 1);
                            
                        // Byte Substitution with sBox
                        for (int i = 0; i < AesBlock.BlockRowLength; i++)
                        {
                            byte tmp = subKeys[blockIndex][i, col];
                            subKeys[blockIndex][i, col] = AesConstants.SBox[tmp];
                        }
                            
                        // Add round constant
                        byte[] constant = AesConstants.RoundConstants[blockIndex - 1];
                        for (int i = 0; i < AesBlock.BlockRowLength; i++)
                        {
                            subKeys[blockIndex][col, i] ^= constant[i];
                        }
                    }
                    else
                    {
                        subKeys[blockIndex].SetCol(subKeys[blockIndex].getCol(col - 1), col);
                    }

                    // XOR with subkey from prev block 
                    for (int i = 0; i < AesBlock.BlockRowLength; i++)
                    {
                        subKeys[blockIndex][i, col] ^= subKeys[blockIndex - 1][i, col];
                    }
                }
            }

            return subKeys;
        }
    }
}
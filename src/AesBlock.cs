using System;

namespace SimpleAes
{
    public class AesBlock
    {
        public const int BlockRowLength = 4;
        public const int BlockColLength = 4;
        private byte[][] _block;

        public AesBlock()
        {
            _block = new byte[BlockColLength][];
            Reset();
        }

        public AesBlock(byte[] array, bool horizontal = true)
        {
            // Init the array
            _block = new byte[BlockColLength][];
            Reset();

            if (horizontal)
            {
                for (int i = 0; i < BlockRowLength; i++)
                {
                    Array.Copy(array, BlockRowLength * i, _block[i], 0, BlockRowLength);
                }
            }
            else
            {
                int arrayIndex = 0;
                for (int i = 0; i < BlockRowLength; i++)
                {
                    for (int j = 0; j < BlockColLength; j++)
                    {
                        _block[j][i] = array[arrayIndex];
                        arrayIndex += 1;
                        if (arrayIndex >= array.Length) return;
                    }
                }
            }
        }

        public byte this[int row, int col]
        {
            get => _block[row][col];
            set => _block[row][col] = value;
        }
        public byte[] this[int row]
        {
            get => _block[row];
            set => _block[row] = value;
        }

        public byte[] AsArray()
        {
            byte[] array = new byte[BlockRowLength * BlockColLength];

            int arrIndex = 0;
            for (int row = 0; row < BlockRowLength; row++)
            {
                for (int col = 0; col < BlockColLength; col++)
                {
                    array[arrIndex] = _block[col][row];
                    arrIndex += 1;
                }
            }

            return array;
        }

        public int Length() => _block.Length;

        public byte[] getCol(int index)
        {
            byte[] col = new byte[BlockColLength];
            for (int i = 0; i < BlockColLength; i++)
            {
                col[i] = _block[i][index];
            }

            return col;
        }

        public void SetCol(byte[] array, int colIndex)
        {
            for (int i = 0; i < BlockColLength; i++)
            {
                _block[i][colIndex] = array[i];
            }
        }

        public void ShiftHorizontal(int index, int shifts)
        {
            ShiftArray(ref _block[index], shifts);
        }

        public void ShiftVertical(int index, int shifts)
        {
            // Lazy way of shifting vertically
            // But doing this is, is stupid anyway ¯\_(ツ)_/¯
            int size = _block.Length;
            byte[] temp = new byte[BlockRowLength];

            for (int row = 0; row < BlockRowLength; row++)
            {
                temp[row] = _block[row][index];
            }
            
            ShiftArray(ref temp, shifts);
            
            
            for (int row = 0; row < BlockRowLength; row++)
            {
                _block[row][index] = temp[row];
            }
        }

        private static void ShiftArray(ref byte[] array, int shifts)
        {
            byte[] tmp = new byte[shifts];
            Array.Copy(array, 0, tmp, 0, shifts);
            Array.Copy(array, shifts, array, 0, array.Length - shifts);

            for (int i = tmp.Length; i-- > 0;)
            {
                int ind = array.Length - (tmp.Length - i);
                array[ind] = tmp[i];
            }
        }

        public void Reset()
        {
            for (int i = 0; i < _block.Length; i++)
            {
                _block[i] = new byte[BlockRowLength];
            }
        }

        public void PrintBlock()
        {
            foreach (byte[] t in _block) Console.WriteLine(BitConverter.ToString(t));
            Console.WriteLine();
        }
    }
}
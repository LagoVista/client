using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Models
{
    public class DFUProgress : ModelBase
    {
        private int _progress;
        public int Progress 
        { 
            get { return _progress; }
            set { Set(ref _progress, value); }
        }

        private int _blockIndex;
        public int BlockIndex
        {
            get { return _blockIndex; }
            set { Set(ref _blockIndex, value); }
        }

        private int _totalBlockCount;
        public int TotalBlockCount
        {
            get { return _totalBlockCount; }
            set { Set(ref _totalBlockCount, value); }
        }

        private int _checkSum;
        public int CheckSum
        {
            get { return _checkSum; }
            set { Set(ref _checkSum, value); }
        }
    }
}

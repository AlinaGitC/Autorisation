using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorisation.Models
{
    public class BlockState
    {
        private static readonly string BlockFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "auth_block.txt");

        public static DateTime? GetBlockTime()
        {
            if (File.Exists(BlockFilePath))
            {
                var text = File.ReadAllText(BlockFilePath);
                if (DateTime.TryParse(text, out var blockTime))
                {
                    return blockTime;
                }
            }
            return null;
        }

        public static void SetBlockTime(DateTime blockTime)
        {
            File.WriteAllText(BlockFilePath, blockTime.ToString("o"));
        }

        public static void ClearBlockTime()
        {
            if (File.Exists(BlockFilePath))
            {
                File.Delete(BlockFilePath);
            }
        }
    }
}

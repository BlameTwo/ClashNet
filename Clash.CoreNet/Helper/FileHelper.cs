using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clash.CoreNet.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// 检查文件夹是否存在并创建
        /// </summary>
        /// <param name="path"></param>
        public static void FolderExits(string path)
        {
            if (!System.IO.Directory.Exists(path))
                Directory.CreateDirectory(path);
        }


        /// <summary>
        /// 检查文件是否存在并创建
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="remove">创建时是否移除</param>
        /// <returns></returns>
        public async static Task<bool> FileExits(string path,bool remove)
        {
            if (File.Exists(path) && remove)
            {
                File.Delete(path);
                return true;
            }
            else if(!File.Exists(path) && !remove)
            {
                await File.CreateText(path).DisposeAsync();
                return true;
            }
            return false;
        }
    }
}

﻿/*Copyright(C) 2018 Sergi4UA

This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see <https://www.gnu.org/licenses/>.
*/

using System;

namespace EQUINE
{
    public class FileSizeHelper
    {
        public static string getFormattedFileSize(long bytes)
        {
            if(bytes > 1024)
            {
                long kb = bytes / 1024;

                if(kb > 1024)
                {
                    float mb = kb / 1024f;
                    return String.Format("{0:0.##} MiB", mb);
                }

                return String.Format("{0:0.##} KiB", kb);
            }
            else
            {
                return bytes + " bytes";
            }
        }
    }
}

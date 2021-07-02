using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CELD.FileEncoder
{
    class Program
    {
        static void Main(string[] args)
        {
			if(args.Length<2){
				   Console.WriteLine("Usage: encoder sourceFolderPath destFolderPath");
				  return;
			}
            string source = args[0];
            string target = args[1];

            Encoding fromEncoding = Encoding.GetEncoding(866);
            Encoding toEncoding = Encoding.GetEncoding(1251);

            var files = Directory.EnumerateFiles(source, "*", SearchOption.TopDirectoryOnly);


            foreach (string fileName in files)
            {

                String contents = File.ReadAllText(fileName, fromEncoding);
                String[] sp = {"\r\n"}	;			
				String[] lines = contents.Split(sp,  StringSplitOptions.None);
				
				if(lines[0].StartsWith("TAG") || lines[0].StartsWith("t_")){
					string tagName  =lines[0].ToLower();
					if(!tagName.EndsWith(".txt")){
							tagName+=".txt";
					}
					
					string newFileName = Path.Combine(target, tagName);
					File.WriteAllLines(newFileName, lines.Skip(1), toEncoding);
					Console.WriteLine(newFileName);
				}else{
					string fileNameRelative = fileName.Substring(source.Length);
					string newFileName = Path.Combine(target, fileNameRelative);
					string newFileDirectory = Path.GetDirectoryName(newFileName);

					Directory.CreateDirectory(newFileDirectory);
					File.WriteAllText(newFileName, contents, toEncoding);
					Console.WriteLine(newFileName);
				}
                
            }

            Console.WriteLine("Done");
        }
    }
}

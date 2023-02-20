using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FrequancyChiper
{

    internal static class Program
    {
        public static void Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }
        static char[] Encrypt(char[] text, int height, int text_length)
        {
            int currentIndex = 0;
            char[] encrypted_text = new char[text_length];
            for (int i = height - 1; i < text_length; i += (height - 1) * 2, currentIndex++)
                encrypted_text[currentIndex] = text[i];

            for (int step = 1; step + 1 < height; step++)
                for (int i = height - step - 1; i < text_length; i += (height - step - 1) * 2)
                {
                    encrypted_text[currentIndex] = text[i];
                    currentIndex++;
                    i += step * 2;
                    /*Console.WriteLine($"{new string(encrypted_text)} {currentIndex}");*/
                    if (i < text_length)
                    {
                        encrypted_text[currentIndex] = text[i];
                        currentIndex++;
                    }
                }
            for (int i = 0; i < text_length; i += (height - 1) * 2, currentIndex++)
                encrypted_text[currentIndex] = text[i];

            return encrypted_text;
        }

        static char[] Decrypt(char[] text, int height, int text_length)
        {
            int currentIndex = 0;

            char[] decrypted_text = new char[text_length];
            /*Populate<char>(decrypted_text, '0');*/

            for (int i = height - 1, step = 2 * (height - 1); i < text_length; i += step, currentIndex++)
                decrypted_text[i] = text[currentIndex];//First row

            for (int i = height - 1; i > 1; i--)//Rows between
                for (int j = i - 1; j < text_length; j += 2 * (i - 1))
                {
                    decrypted_text[j] = text[currentIndex++];
                    j += 2 * (height - i);
                    if (j < text_length)
                        decrypted_text[j] = text[currentIndex++];
                }
            for (int i = 0, step = 2 * (height - 1); i < text_length; i += step, currentIndex++)
                decrypted_text[i] = text[currentIndex];//Last row


            return decrypted_text;
        }

        static void Main(string[] args)
        {

            Console.Write(Encrypt("Cryptogrhapy".ToCharArray(), 3, "Cryptogrhapy".Length));
            Console.Write("Enter the height(>1): ");
            int height = int.Parse(Console.ReadLine());
            
            Console.Write("The block size(>1): ");
            int blockSize = int.Parse(Console.ReadLine());

            Console.Write("Input file path(valid): ");
            string inputFilePath = Console.ReadLine();

            Console.Write("Output file path(if does not exist will be created): ");
            string outputFilePath = Console.ReadLine();

            Console.Write("Enter 1(to crypt the text) or 2 (to decrypt the text): ");
            string mode = Console.ReadLine();

            char[] textBlock = new char[blockSize];
            int textLenght = 0;
            DateTime startTime = DateTime.Now;

            using (StreamReader reader = new StreamReader(inputFilePath))
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                if (mode == "1")
                    while (reader.Peek() > 0)
                    {
                        textLenght = reader.Read(textBlock, 0, textBlock.Length);//reading in chunks
                        writer.Write(Encrypt(textBlock, height, textLenght));
                    }
                else if (mode == "2")
                    while (reader.Peek() > 0)
                    {
                        textLenght = reader.Read(textBlock, 0, textBlock.Length);//reading in chunks
                        writer.Write(Decrypt(textBlock, height, textLenght));
                    }
                else
                    Console.WriteLine("Wrong input!");
                Console.WriteLine("Memory usage: {0} bytes", GC.GetTotalMemory(false));
            }

            DateTime endTime = DateTime.Now;
            TimeSpan exTime = endTime - startTime;

            Console.WriteLine("\nTime of execution: {0}", exTime);
            Console.WriteLine("Memory usage: {0} bytes", GC.GetTotalMemory(false));

        }
        static void Main2(string[] args )//without block size. Each line is encrypted in full lenght 
        {
            DateTime startTime = DateTime.Now;
            int height = 18;

            using (StreamReader reader = new StreamReader("D:\\Visual Studio Projects\\C#\\FrequancyChiper\\big.txt"))
            using (StreamWriter writer = new StreamWriter("D:\\Visual Studio Projects\\C#\\FrequancyChiper\\output.txt"))
            {
                char[] line;
                /*while ((line = reader.ReadLine().ToCharArray()) != null)*/
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().ToCharArray();
                    writer.WriteLine(Encrypt(line, height, line.Length));
                }
            }

            using (StreamReader reader = new StreamReader("D:\\Visual Studio Projects\\C#\\FrequancyChiper\\output.txt"))
            using (StreamWriter writer = new StreamWriter("D:\\Visual Studio Projects\\C#\\FrequancyChiper\\decode.txt"))
            {
                char[] line;
                /*while ((line = reader.ReadLine().ToCharArray()) != null)*/
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().ToCharArray();
                    writer.WriteLine(Decrypt(line, height, line.Length));
                }
            }

            DateTime endTime = DateTime.Now;
            TimeSpan exTime = endTime - startTime;

            Console.WriteLine("Time of execution: {0}", exTime);
            Console.WriteLine("Memory usage: {0} bytes", GC.GetTotalMemory(false));

        }
    }
}

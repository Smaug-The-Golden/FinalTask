using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask

{
    [Serializable]
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Student(string name, string group, DateTime dateOfBirth)
        {
            Name = name;
            Group = group;
            DateOfBirth = dateOfBirth;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Укажите полный путь до файла ""Students.dat"": ");  //C:\Users\Professional\Desktop\Students.dat
            string dir_for_bin_file = Console.ReadLine();

            Console.WriteLine(@"Программа запишет каждую группу студетов в отдельный файл и поместит новые файлы в папке ""Students"" на рабочем столе.");
            string dir_for_record = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Students";
            CreatingDirectory(dir_for_record);

            ReadBinnaryFile(dir_for_bin_file, dir_for_record);
            Console.WriteLine("\n Программа завершила работу!");
            Console.ReadKey();
        }

        static void ReadBinnaryFile(string pathFile, string path)
        {
            if (File.Exists(pathFile))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (var fs = new FileStream(pathFile, FileMode.OpenOrCreate))
                    {
                        Student[] newStudent = (Student[])formatter.Deserialize(fs);
                        Console.WriteLine($"\n{pathFile}");
                        foreach (Student st in newStudent)
                        {
                            Console.WriteLine($"Имя: {st.Name},  Группа: {st.Group},  День рождения {st.DateOfBirth.ToString("dd.MM.yyyy")}");
                            GroupRecording(path + @"\Группа_" + st.Group + ".txt", st);
                        }
                        Console.WriteLine("\n Программа успешно распределила студентов по группам!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка: {0}", ex.Message);
                }
            }
            else { Console.WriteLine($"Файла {pathFile} не существует по уазанному пути"); }
        }

        static void GroupRecording(string filePath, Student stud)
        {
            var fileInfo = new FileInfo(filePath);
            try
            {
                if (!fileInfo.Exists)  // Проверим, существует ли файл по данному пути
                {
                    //   Если не существует - создаём и записываем в строку
                    using (StreamWriter sw = fileInfo.CreateText())
                    {
                        sw.WriteLine(stud.Name + ", " + stud.DateOfBirth.ToString("dd.MM.yyyy"));
                    }
                }
                else
                {
                    // Если существует, то добавляем в него новую строку
                    using (StreamWriter sw = fileInfo.AppendText())
                    {
                        sw.WriteLine(stud.Name + ", " + stud.DateOfBirth);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при записи студента в файл группы: {0}", ex.Message);
            }
        }

        static void CreatingDirectory(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                try

                {
                    dirInfo.Create();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка при создании папки: {0}", ex.Message);
                }
            }

        }

    }

}

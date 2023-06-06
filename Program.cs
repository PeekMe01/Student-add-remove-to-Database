using System.Data.SqlClient; // this is the name of the provider i wrote in app.config
using System.Data.Entity; // DBContext class and other related stuff are in this namespace
using System.Xml.Linq;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;

namespace EF_Config
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using DataContext context = new DataContext("name = AppConnectionString"); // we put "using" so the garbage collector can dispose of it once its out of scope.
            List<Student> students = new List<Student>(); // students list.

            int option = 0;
            Console.WriteLine("Choose an option:\n" + "1- Get all students\n" + "2- Insert Student\n" + "3- Delete Student by ID\n");

            while (true)
            {
                do
                {
                    Console.WriteLine("Insert option:");
                    option = int.Parse(Console.ReadLine());
                } while (option <= 0 || option > 3);

                if(option == 1)
                {
                    students = context.Students.ToList();
                    Console.WriteLine($"{"Student ID:",-20}{"Student Name:",-20}{"Student Address:",-20}{"Student GPA:",-20}\n");
                    for (int i =0;i<students.Count;i++)
                    {
                        Console.WriteLine($"{students[i].StudentId,-20} {students[i].Name,-20} {students[i].Address,-20} {students[i].GPA,-20}");
                    }
                }

                if(option == 2)
                {
                    string name="", address="";
                    int gpa=0;
                    do
                    {
                        Console.Write("Insert Student Name: ");
                        name = Console.ReadLine();
                    } while (name == "");

                    do
                    {
                        Console.Write("Insert Student Address: ");
                        address = Console.ReadLine();
                    } while (address == "");
                    
                    do
                    {
                        Console.Write("Insert Student GPA: ");
                        gpa = int.Parse(Console.ReadLine());
                    } while (gpa < 0 || gpa > 5);

                    Student st = new Student { Name= name, Address = address , GPA = gpa };
                    context.Students.Add(st);
                    context.SaveChanges();
                }

                if(option == 3)
                {
                    int id = 0;
                    do
                    {
                        Console.Write("Enter a valid ID: ");
                        id = int.Parse(Console.ReadLine());
                    } while (id < 0 || context.Students.FirstOrDefault(s => s.StudentId == id)==null);

                    context.Students.Remove(context.Students.Find(id));
                    context.SaveChanges();
                }

            }
        }
    }


    class DataContext: DbContext // ":" is for inheritance
    {
        public DbSet<Student> Students { get; set; }

        public DataContext(string name):base(name) // here we are passing the name to the super class constructor that has a constructor that take a string and makes it the name of the db.
        {
            // constructor
        }


    }
    [Table("StudentsTable",Schema ="University")]
    class Student
    {
        public int StudentId { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        
        public int? GPA { get; set; } // "?" makes it nullable

    }
}
using lambda_practice.data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace lambda_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                 .UseInMemoryDatabase(databaseName: "Sample_Data")
                 .Options;

            // Insert test data in the DB
            using (var context = new DatabaseContext(options))
            {
                var intiliazer = new DatabaseInitializer();
                intiliazer.Initialize(context);
            }

            using (var context = new DatabaseContext(options))
            {
                //You can erase the following 3 lines if needed
                //context.Cities.ForEachAsync(c => Console.WriteLine($"{c.Id}, {c.Name}"));
                //context.Departments.ForEachAsync(d => Console.WriteLine(d.Name));
                //context.Employees.ForEachAsync(e => Console.WriteLine($"{e.FirstName} {e.LastName}"));

                //1. List all employees whose departament has an office in Chihuahua
                context.Employees
                    .Where(e => e.Department.Cities.Any(c => c.Name == "Chihuahua"))
                    .ForEachAsync(e => Console.WriteLine($"{e.FirstName} {e.LastName}"));



                //2. List all departments and the number of employees that belong to each department.
                context.Departments
                    .ForEachAsync(d => Console.WriteLine($"{d.Name} {context.Employees.Where(e => e.DepartmentId == d.Id).Count()} "));


                //3. List all remote employees. That is all employees whose living city is not the same one as their department's.
                context.Employees
                    .Where(e => !(e.Department.Cities.Any(c => c.Name == e.City.Name)))
                    .ForEachAsync(e => Console.WriteLine($"{e.FirstName} {e.LastName}"));


                //4. List all employees whose hire aniversary is next month.
                context.Employees
                    .Where(e => e.HireDate.Month == DateTime.Today.AddMonths(1).Month)
                    .ForEachAsync(e => Console.WriteLine($"{e.FirstName} {e.LastName}"));

                //5. List all 12 months of the year and the number of employees hired on each month.
                string[] names = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                for(int i = 0; i < 12; i++)
                {
                    Console.WriteLine($"{names[i]} {context.Employees.Where(e => e.HireDate.Month == i + 1).Count()}");
                    
                }
                
            }

            Console.ReadLine();
        }
    }
}

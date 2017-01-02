using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoAssignment.Generators;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoAssignment {
  class Program {
    private static IMongoClient _client;
    private static IMongoDatabase _database;
    private static IMongoCollection<BsonDocument> employees_doc;
    private static IMongoCollection<BsonDocument> projects_doc;
    private static int seed;

    private const string Menu = 
      "-------------------------------------------------\n" + 
      "Select an action:\n" + 
      "1 - Insert a number of random employees and projects into the database.\n" + 
      "2 - Print employee and project data from the database.\n" + 
      "3 - Clear the database.\n" + 
      "help - Print this message again.\n" + 
      "exit - Exit.\n" + 
      "-------------------------------------------------";

    public static void Main(string[] args) {
      _client = new MongoClient();
      _database = _client.GetDatabase("assignment");

      try {
        _database.RunCommandAsync((Command<BsonDocument>) "{ping:1}").Wait();
      }
      catch (Exception) {
        Console.WriteLine("Mongo server (mongod.exe) was not detected,\nplease make sure it is running before you run\nthis program.");
        Console.Write("Press any key to exit...");
        Console.ReadKey();
        return;
      }

      employees_doc = _database.GetCollection<BsonDocument>("employees");
      projects_doc = _database.GetCollection<BsonDocument>("projects");



      Console.WriteLine(Menu);
      mainmenu:
      var input = QueryUserOption(new []{"help","exit","1","2","3"});
      switch (input) {
        case "1":
          InsertRandom();
          goto mainmenu;
        case "2":
          PrintCollections();
          goto mainmenu;
        case "3":
          ClearDB(true);
          goto mainmenu;
        case "exit":
          break;
        case "help":
          Console.WriteLine(Menu);
          goto mainmenu;
        default:
          break;
      }
    }

    private static void PrintCollections() {
      var employeeMap = new BsonJavaScript(@"
        function() {
          var employee = this;
          emit(employee.bsn, { name: employee.name , surname: employee.surname });
        }
        ");
      var employeeReduce = new BsonJavaScript(@"
        function(key, values) {
          var result = {name: '', surname: ''};
          
          values.forEach(function(value) {
            result.name = value.name
            result.surname = value.surname
          });
          
          return result;
        }");
      var projectMap = new BsonJavaScript(@"
        function() {
          var project = this;
          emit(project.projectid, { projectname: project.projectname , budget: project.budget , allocatedhours: project.allocatedhours , buildingname: project.buildingname});
        }
        ");
      var projectReduce = new BsonJavaScript(@"
        function(key, values) {
          var result = {projectname: '', budget: 0, allocatedhours: 0, buildingname: ''};
          
          values.forEach(function(value) {
            result.projectname = value.projectname
            result.budget = value.budget
            result.allocatedhours = value.allocatedhours
            result.buildingname = value.buildingname
          });
          
          return result;
          }");

      var options = new MapReduceOptions<BsonDocument, BsonDocument>();

      try {
        var employeeResults = employees_doc.MapReduce(employeeMap, employeeReduce, options).ToListAsync().Result;

        var isFancy = QueryUserBool("Would you like to have the results formatted in a more readable fashion? y/n",
          new[] {"y", "yes"}, new[] {"n", "no"});

        Console.WriteLine("Employees: ");
        foreach (var employeeResult in employeeResults) {
          if (!isFancy) {
            Console.WriteLine(employeeResult);
            continue;
          }
          var str = $"{employeeResult["_id"]} - {employeeResult["value"]["name"]} {employeeResult["value"]["surname"]}";
          Console.WriteLine(str);
        }

        Console.WriteLine("\nProjects: ");
        var projectResults = projects_doc.MapReduce(projectMap, projectReduce, options).ToListAsync().Result;
        foreach (var projectResult in projectResults) {
          if (!isFancy) {
            Console.WriteLine(projectResult);
          }
          var str = $"{projectResult["_id"]} - {projectResult["value"]["projectname"]}  has a budget of {projectResult["value"]["budget"]}\nit has been given a timespan of {projectResult["value"]["allocatedhours"]} hours\nand will be worked on at {projectResult["value"]["buildingname"]}";
          Console.WriteLine(str);
        }
      }
      catch (MongoCommandException e) {
        Console.WriteLine("Database is not initialised, please add elements first.");
        ClearDB(false);
      }
    }

    private static async void InsertRandom() {
      seed = QueryUserNumber("Enter a number to use as seed for the generator:", int.MinValue, int.MaxValue);
      var en = QueryUserNumber("How many employees would you like to generate?", 0, 5000);
      var pn = QueryUserNumber("And how many projects?", 0, 5000);

      for (int i = 0; i < en; i++) {
        var doc = new EmployeeGenerator(seed).Generate();
        seed = i%2 == 1 ? seed + (i+1) : seed - (i+1);
        await employees_doc.InsertOneAsync(doc.ToBsonDocument());
      }

      for (int i = 0; i < pn; i++) {
        var doc = new ProjectGenerator(seed).Generate();
        seed = i % 2 == 1 ? seed + (i + 1) : seed - (i + 1);
        await projects_doc.InsertOneAsync(doc.ToBsonDocument());
      }

      Console.WriteLine("-= Added Documents =-");
    }

    private static void ClearDB(bool showmsg) {
      _client.DropDatabase("assignment");
      if (showmsg) {
        Console.WriteLine("-= Database Dropped =-");
      }
    }

    private static int QueryUserNumber(string question, int min, int max) {
      Console.WriteLine(question);
      while (true) {
        var input = Console.ReadLine();
        try {
          if (input != null) {
            var validated_input = int.Parse(input);
            if (validated_input < min || validated_input > max) {
              throw new IndexOutOfRangeException("Input did not fall between minimum and maximum values");
            }
            return validated_input;
          }
        }
        catch (Exception ex) {
          Console.WriteLine(ex.Message);
        }
      }
    }

    private static string QueryUserOption(string question, string[] options) {
      Console.WriteLine(question);
      return QueryUserOption(options);
    }

    private static string QueryUserOption(string[] options) {
      while (true) {
        var input = Console.ReadLine();
        try {
          if (input != null) {
            if (!options.Contains(input.ToLower())) {
              throw new ArgumentOutOfRangeException();
            }
            return input.ToLower();
          }
        }
        catch (Exception) {
          Console.WriteLine("Input was not one of the given options. Please try again");
        }
      }
    }

    private static bool QueryUserBool(string question, string[] trueAnswers, string[] falseAnswers) {
      Console.WriteLine(question);
      while (true) {
        var input = Console.ReadLine();
        try {
          if (input != null) {
            if (trueAnswers.Contains(input.ToLower())) {
              return true;
            }
            if (falseAnswers.Contains(input.ToLower())) {
              return false;
            }
            throw new InvalidOperationException();
          }
        }
        catch (Exception) {
          Console.WriteLine("Input was not one of the given options. Please try again");
        }
      }
    } 
  }
}

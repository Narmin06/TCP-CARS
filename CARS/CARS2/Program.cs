//CLIENT
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using CARS2;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;

var client = new TcpClient();
client.Connect(ip, port);

var stream = client.GetStream();
var bw = new BinaryWriter(stream);
var br = new BinaryReader(stream);

Command command = null;
string response = null;
string str = null;

while (true)
{
    Console.WriteLine("----- Command list -----");
    Console.WriteLine("GET - Show all cars");
    Console.WriteLine("POST - Add new car");
    Console.WriteLine("PUT - Update car");
    Console.WriteLine("DELETE - Delete car");
    Console.Write("Enter command name:");
    str = Console.ReadLine()!.ToUpper();
    switch (str)
    {
        case Command.Get:
            command = new Command { Text = Command.Get };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            var cars = JsonSerializer.Deserialize<List<Car>>(response);
            Console.WriteLine("Cars in database:");
            Console.WriteLine();
            cars!.ForEach(c => Console.WriteLine($@"ID: {c.ID}
Model: {c.Model}
Year: {c.Year}"));
            break;

        case Command.Post:
            Console.Write("Enter model: ");
            var model = Console.ReadLine();
            Console.Write("Enter year: ");
            var year = int.Parse(Console.ReadLine());
            var newCar = new Car { Model = model, Year = year };
            command = new Command
            {
                Text = Command.Post,
                Value = newCar
            };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.WriteLine(response);
            break;

        case Command.Put:
            Console.Write("Enter ID to update: ");
            var idToUpdate = int.Parse(Console.ReadLine());
            Console.Write("Enter new model: ");
            var updatedModel = Console.ReadLine();
            Console.Write("Enter new year: ");
            var updatedYear = int.Parse(Console.ReadLine());
            var updatedCar = new Car { ID = idToUpdate, Model = updatedModel, Year = updatedYear };
            command = new Command
            {
                Text = Command.Put,
                Value = updatedCar
            };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.WriteLine(response);
            break;

        case Command.Delete:
            Console.Write("Enter ID: ");
            var idToDelete = int.Parse(Console.ReadLine());
            var deletedCar = new Car { ID = idToDelete };
            command = new Command
            {
                Text = Command.Delete,
                Value = deletedCar
            };
            bw.Write(JsonSerializer.Serialize(command));
            response = br.ReadString();
            Console.WriteLine(response);
            break;

        default:
            Console.WriteLine("Unknown command. Try again.");
            break;
    }
    Console.WriteLine("Press ENTER to continue...");
    Console.ReadLine();
    Console.Clear();
}
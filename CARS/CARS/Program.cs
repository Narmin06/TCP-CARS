using CARS;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;
var listener = new TcpListener(ip, port);
listener.Start();

Console.WriteLine("Server Started...");

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var br = new BinaryReader(stream);
    var bw = new BinaryWriter(stream);

    using var context = new CarDbContext();
    while (true)
    {
        var input = br.ReadString();
        var command = JsonSerializer.Deserialize<Command>(input);
        switch (command.Text)
        {
            case Command.Get:
                var allCars = context.Cars.ToList();
                bw.Write(JsonSerializer.Serialize(allCars));
                break;
            case Command.Post:
                context.Cars.Add(command.Value);
                context.SaveChanges();
                bw.Write("Car added successfully.");
                break;
            case Command.Put:
                var updatedCar = command.Value;
                var updatedCarID = context.Cars.FirstOrDefault(c => c.ID == updatedCar.ID);
                if (updatedCarID != null)
                {
                    updatedCarID.Model = updatedCar.Model;
                    updatedCarID.Year = updatedCar.Year;
                    context.SaveChanges();
                    bw.Write("Car updated successfully.");
                }
                else { bw.Write("Car not found."); }
                break;
            case Command.Delete:
                var idToDelete = command.Value.ID;
                var carToDelete = context.Cars.FirstOrDefault(c => c.ID == idToDelete);
                if (carToDelete != null)
                {
                    context.Cars.Remove(carToDelete);
                    context.SaveChanges();
                    bw.Write("Car deleted successfully.");
                }
                else
                {
                    bw.Write("Car not found.");
                }
                break;
            default:
                break;
        }
    }
}
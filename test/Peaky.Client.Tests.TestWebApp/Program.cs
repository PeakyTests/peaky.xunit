namespace Peaky.Client.Tests.TestWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddPeakySensors();
        builder.Services.AddPeakyTests(tests =>
        {
            tests.Add("dependencies", "testuri", new("http://testuri.org"));
            
            tests.Add("staging", "TestWebApp", new("http://localhost:5158"));

            tests.Add("production", "TestWebApp", new("http://localhost:5158"));
        });

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UsePeaky();

        app.Run();
    }
}
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApp97
{
    internal class Program
    {
        static async Task Main()
        {
            TcpListener server = null;

            try
            {
                // Встановлюємо порт для слухання
                int port = 8888;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // Створюємо TcpListener для слухання вказаного порту
                server = new TcpListener(localAddr, port);

                // Запускаємо слухання
                server.Start();

                Console.WriteLine("Сервер запущено...");

                while (true)
                {
                    Console.Write("Очікування клієнта... ");

                    // Блокуючий виклик, очікуємо підключення клієнта
                    TcpClient client = await server.AcceptTcpClientAsync();
                    Console.WriteLine("Клієнт підключений!");

                    // Обробка підключеного клієнта асинхронно
                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            finally
            {
                // Зупиняємо TcpListener
                server.Stop();
            }
        }

        static async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                // Отримуємо NetworkStream для читання та запису даних
                NetworkStream stream = client.GetStream();

                byte[] data = new byte[256];

                // Отримуємо дані від клієнта
                int bytesRead = await stream.ReadAsync(data, 0, data.Length);
                string request = Encoding.ASCII.GetString(data, 0, bytesRead);
                Console.WriteLine($"Отримано запит: {request}");

                // Відправляємо відповідь на запит
                string response = DateTime.Now.ToString();
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                await stream.WriteAsync(responseData, 0, responseData.Length);

                // Закриваємо з'єднання з клієнтом
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка обробки клієнта: {ex.Message}");
            }
        }

    }
}
namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //var url = "https://google.com/search?q=Hello+world";
            //var url = "http://localhost:8080/?message=HelloServer&name=Petya&age=23";//get
            var url = "http://localhost:8080/";//post
            try
            {
                //var message = await GetRequestAsync(url);
                var message = await PostRequestAsync(url);
                Console.WriteLine($"Message: {message}");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        static async Task<string> PostRequestAsync(string url)
        {
            using var client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                {"message","Hello, Server" },
                {"name","Vasya" },
                {"age","23" }
            };

            var content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Headers:");
                foreach (var item in response.Headers)
                {
                    Console.Write($"{item.Key}: ");
                    foreach (var value in item.Value)
                    {
                        Console.WriteLine($"{value}, ");
                    }
                    Console.WriteLine();
                }
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                throw new HttpRequestException("Something went wrong");
            }


        }
        static async Task<string> GetRequestAsync(string url)
        {
            using var client = new HttpClient();


            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Headers:");
                foreach (var item in response.Headers)
                {
                    Console.Write($"{item.Key}: ");
                    foreach (var value in item.Value)
                    {
                        Console.WriteLine($"{value}, ");
                    }
                    Console.WriteLine();
                }
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                throw new HttpRequestException("Something went wrong");
            }
        }
    }
}
namespace DatabaseRestoration.Display
{
    using System;

    public static class Prompts
    {
        public static int Menu()
        {
            int oUserChoice = 0;
            Console.WriteLine("1.) Create basic CRUD stored procedures");
            Console.WriteLine("2.) Generate Layers");
            Console.WriteLine("3.) Generate Models");
            Console.WriteLine("4.) Exit");

            oUserChoice = ObtainFromUser<int>("Looks like you didn't give me a number.");
            return oUserChoice;
        }

        private static T ObtainFromUser<T>(string iQuestion = "", string iError = "")
        {
            T result = (typeof(T).IsValueType || typeof(T) == typeof(string))
                ? default(T) : Activator.CreateInstance<T>();

            bool conversionSuccessful = false;
            if (string.IsNullOrWhiteSpace(iQuestion))
            {
                Console.WriteLine(iQuestion);
            }
            while (!conversionSuccessful)
            {
                string response = Console.ReadLine();
                try
                {
                    result = (T)Convert.ChangeType(response, typeof(T));
                    conversionSuccessful = true;
                }
                catch (FormatException formatEx)
                {
                    if (!string.IsNullOrWhiteSpace(iError))
                    {
                        Console.WriteLine(iError);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Cast, unable to convert {0} to {1} due to a formatting issue: {2}",
                               response, typeof(T).ToString(), formatEx.Message);
                    }
                }
                catch (InvalidCastException invCastEx)
                {
                    if (!string.IsNullOrWhiteSpace(iError))
                    {
                        Console.WriteLine(iError);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Cast, unable to convert {0} to {1}: {2}",
                            response, typeof(T).ToString(), invCastEx.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(iError);
                }
            }
            return result;
        }
    }
}

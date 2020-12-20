namespace Composer
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var input = "this is my input";
            var validateNotNull = new ValidateNotNull<string>();
            var replaceMy = (Func<string, string>)(i => i.Replace("my", "some"));

            var result = input
                .Then(validateNotNull)
                .Then(replaceMy)
                .Then(i => i + " after change");

            Console.WriteLine(result);
        }
    }

    public class ValidateNotNull<T> : IOperation<T, T>
    {
        public T Execute(T input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return input;
        }
    }

    public static class ParameterExtensions
    {
        public static TOutput Then<TInput, TOutput>(this TInput input, Func<TInput, TOutput> function)
        {
            return function(input);
        }

        public static Task<TOutput> ThenAsync<TInput, TOutput>(this TInput input, Func<TInput, Task<TOutput>> function)
        {
            return function(input);
        }

        public static async Task<TOutput> ThenAsync<TInput, TOutput>(this Task<TInput> input, Func<TInput, TOutput> function)
        {
            return function(await input);
        }

        public static async Task<TOutput> ThenAsync<TInput, TOutput>(this Task<TInput> input, Func<TInput, Task<TOutput>> function)
        {
            return await function(await input);
        }
        
        public static TOutput Then<TInput, TOutput>(this TInput input, IOperation<TInput, TOutput> operation)
        {
            return input.Then(operation.Execute);
        }
    }

    public interface IOperation<TInput, TOutput>
    {
        TOutput Execute(TInput input);
    }
}

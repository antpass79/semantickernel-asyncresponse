using System.Text;
using System.Text.Json;

namespace AsyncSemanticKernelResponse.Services
{
    public class JsonAsyncStreamingService
    {
        private StringBuilder _buffer = new();

        public string ReadNextJson(string chunck)
        {
            _buffer.Append(chunck);

            var firstIndex = _buffer.IndexOf("{", 0, true);

            if (firstIndex != -1)
            {
                var stack = new Stack<int>();
                stack.Push(firstIndex);

                for (int i = firstIndex + 1; i < _buffer.Length; i++)
                {
                    if (_buffer[i] == '{')
                    {
                        stack.Push(i);
                    }
                    else if (_buffer[i] == '}')
                    {
                        stack.Pop();
                        if (stack.Count == 0)
                        {
                            var json = _buffer.ToString(firstIndex, i - firstIndex + 1);
                            if (IsValidJson(json))
                            {
                                _buffer.Remove(firstIndex, i - firstIndex + 1);
                                return json;
                            }
                        }
                    }
                }
            }
            else
            {
                _buffer.Clear();
            }

            return default!;
        }

        public static bool IsValidJson(string json)
        {
            if (json == null)
                return false;

            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch (JsonException exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }
    }
}
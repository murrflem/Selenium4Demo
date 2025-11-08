using System;
using System.Linq;

namespace S4Demo.Framework.Helpers
{
  public class CommonHelper
  {
    public string RandomString(int numOfChars)
    {
      const string chars = "abcdefghijklmnopqrstuvwxyz";
      var random = new Random();
      var randomString =
      new string(Enumerable.Repeat(chars, numOfChars).Select(set => set[random.Next(set.Length)]).ToArray());
      return randomString;
    }

    public double RandomNumber(int numOfChars)
    {
      const string numbers = "0123456789";
      var random = new Random();
      var randomNum = new string(Enumerable.Repeat(numbers, numOfChars).Select(set => set[random.Next(set.Length)]).ToArray());
      return Convert.ToDouble(randomNum);
    }

    public string AlpahNumericString(int numOfChars)
    {
      const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
      var random = new Random();
      var randomString =
      new string(Enumerable.Repeat(chars, numOfChars).Select(set => set[random.Next(set.Length)]).ToArray());
      return randomString;
    }
  }
}
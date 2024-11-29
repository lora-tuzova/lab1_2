using System.Text.RegularExpressions;

namespace lab1_2.Services
{
    public class CustomValidator
    {
        public bool ValidateNameForPath(string name)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                if (name.Contains(c)) return false;
            }
            return true;
        }

        public bool ValidateTime(string time)
        {
            string timePattern = @"^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$"; //перевірка коректності запису момента часу
            Regex regex = new Regex(timePattern);
            return regex.IsMatch(time);
        }

        public bool ValidateTimeInterval(string time)
        {
            string timePattern = @"^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)-([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$"; //перевірка коректності запису інтервалу часу
            Regex regex = new Regex(timePattern);
            return regex.IsMatch(time);
        }
    }

}

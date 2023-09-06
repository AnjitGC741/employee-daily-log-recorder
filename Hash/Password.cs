using System.Security.Cryptography;
using System.Text;
namespace employeeDailyTaskRecorder.Hash
{
    public class Password
    {
        private string _password;
        public Password(string password)
        {
            this._password = password;  
        }
        public string HashPassword()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(this._password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}

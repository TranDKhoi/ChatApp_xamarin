using ChatApp_xamarin.Models;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Linq;
using ChatApp_xamarin.Resources;
using System.Net.Mail;
using System.Net;

namespace ChatApp_xamarin.Services
{
    public class AuthService
    {
        static private AuthService _ins;
        static public AuthService ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new AuthService();
                }
                return _ins;
            }
            private set => _ins = value;
        }

        private FirebaseClient _client;
        public FirebaseClient Client
        {
            get { return _client; }
            set { _client = value; }
        }

        private const string baseurl = "https://chatapp-xamarin-default-rtdb.asia-southeast1.firebasedatabase.app/";


        AuthService()
        {
            Client = new FirebaseClient(baseurl);
        }

        public async Task<(String, User)> SignUp(string email, string pass)
        {

            try
            {
                var listUser = await Client.Child("users").OnceAsync<User>();
                if (listUser.Count != 0)
                {
                    foreach (var user in listUser)
                    {
                        if (user.Object.email == email)
                        {
                            return (AppResources.emailexisted, null);
                        }
                    }
                }

                User u = new User
                {
                    email = email,
                    password = MD5Hash(pass),
                };
                var json = JsonConvert.SerializeObject(u);
                var res = await Client.Child("users").PostAsync(json);
                if (res != null)
                {
                    var trueUser = JsonConvert.DeserializeObject<User>(res.Object);
                    trueUser.id = res.Key;
                    return (null, trueUser);
                }
                return (AppResources.Failed, null);
            }
            catch (Exception e)
            {
                return (e.Message, null);
            }

        }

        public async Task<(String, User)> Login(string email, string pass)
        {
            try
            {
                var listUser = await Client.Child("users").OnceAsync<User>();

                if (listUser.Count == 0)
                {
                    return (AppResources.emaildoesnotexist, null);
                }
                else
                {
                    var doc = listUser.Where(x => x.Object.email == email).FirstOrDefault();
                    if (doc == null)
                    {
                        return (AppResources.emaildoesnotexist, null);
                    }

                    var trueUser = doc.Object;
                    if (trueUser.password != MD5Hash(pass))
                    {
                        return (AppResources.wrongemailorpassword, null);
                    }

                    trueUser.id = doc.Key;
                    return (null, trueUser);

                }
            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }

        public async Task<(String, bool)> ResetPassword(string userId, string password)
        {
            try
            {
                User u = await GetUserById(userId);
                if (u == null)
                    return (AppResources.emaildoesnotexist, false);

                u.id = null;
                u.password = MD5Hash(password);
                await Client.Child($"users/{userId}").PatchAsync(JsonConvert.SerializeObject(u));

                return (AppResources.resetpasswordsuccessfully, true);
            }
            catch (Exception e)
            {
                return (e.Message, false);
            }
        }

        public async Task<(String, User)> GetUserByEmail(string email)
        {
            try
            {
                var listUser = await Client.Child("users").OnceAsync<User>();

                if (listUser.Count == 0)
                {
                    return (AppResources.emaildoesnotexist, null);
                }
                else
                {
                    var doc = listUser.Where(x => x.Object.email == email).FirstOrDefault();
                    if (doc == null)
                    {
                        return (AppResources.emaildoesnotexist, null);
                    }

                    var trueUser = doc.Object;

                    trueUser.id = doc.Key;
                    return (null, trueUser);

                }
            }
            catch (Exception e)
            {
                return (e.Message, null);
            }
        }

        private async Task<User> GetUserById(string id)
        {
            try
            {
                var user = await Client.Child($"users/{id}").OnceSingleAsync<User>();


                if (user == null)
                {
                    return null;
                }
                else
                {
                    user.id = id;
                    return user;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string MD5Hash(string str)
        {
            StringBuilder hash = new StringBuilder();
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytes = md5.ComputeHash(new UTF8Encoding().GetBytes(str));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("X2"));
            }
            return hash.ToString();
        }

        public async Task<int> SendVerifyCode(string email)
        {
            Random rnd = new Random();
            int randomCode = rnd.Next(10000, 99999);
            await Task.Run(() =>
            {

                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("gahulla2002@gmail.com");
                mail.Subject = "AppChat - Verification mail";
                mail.Body = "This is your verification code: " + randomCode;


                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;

                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new NetworkCredential("gahulla2002@gmail.com", "tiyjilkneqakfxkh");
                smtp.Send(mail);

            });
            return (randomCode);

        }
    }


}
